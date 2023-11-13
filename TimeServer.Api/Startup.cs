using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeServer.Api.Helpers;
using TimeServer.Api.Services;
using TimeServer.Core.Ports;
using TimeServer.Core.Providers;
using TimeServer.Core.Strategies;
using TimeServer.DAL;

namespace TimeServer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.ConfigureCertificateAuthorization();

            services.AddDbContext<TimeServerContext>();

            services.AddTransient<ITimeStrategy, TimeStrategy>();
            services.AddTransient<ITimeLoggingPort, TimeRequestRepository>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddWindowsService();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Will create the database if not existing
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<TimeServerContext>();
                context.Database.EnsureCreated();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure gRPC
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<TimeService>();
                endpoints.MapGrpcService<TimeLogService>();

                endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
        }
    }

    static file class AuthorizationExtension
    {
        public static IServiceCollection ConfigureCertificateAuthorization(this IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.ConfigureHttpsDefaults(options =>
                {
                    options.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                    options.AllowAnyClientCertificate();
                });
            });

            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {
                    options.AllowedCertificateTypes = CertificateTypes.All;
                    options.Events = new CertificateAuthenticationEvents
                    {
                        OnCertificateValidated = context =>
                        {
                            var claims = new[]
                            {
                                new Claim(
                                    ClaimTypes.NameIdentifier,
                                    context.ClientCertificate.Subject,
                                    ClaimValueTypes.String, context.Options.ClaimsIssuer),
                                new Claim(
                                    ClaimTypes.Name,
                                    context.ClientCertificate.Subject,
                                    ClaimValueTypes.String, context.Options.ClaimsIssuer)
                            };

                            context.Principal = new ClaimsPrincipal(
                                new ClaimsIdentity(claims, context.Scheme.Name));
                            context.Success();

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.AuthorizationPolicyCertificate, policy =>
                {
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                    policy.RequireClaim(ClaimTypes.Name);
                });
            });

            return services;
        }
    }
}
