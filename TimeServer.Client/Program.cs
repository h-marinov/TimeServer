using System.Security.Cryptography.X509Certificates;
using Grpc.Net.Client;
using TimeServer.Api;


string address = HandleInput();

await TestGetTimeMethod();
await TestGetTimeMethod();

await TestGetLogsMethod();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

TimeLog.TimeLogClient CreateClientWithCert(
    string baseAddress,
    X509Certificate2 certificate)
{
    // Add client cert to the handler
    var handler = new HttpClientHandler();
    handler.ClientCertificates.Add(certificate);

    // Create the gRPC channel
    var channel = GrpcChannel.ForAddress(baseAddress, new GrpcChannelOptions
    {
        HttpHandler = handler
    });

    return new TimeLog.TimeLogClient(channel);
}

X509Certificate2 GetClientTestingCertificate()
{
    string certificateString = "MIIKkQIBAzCCCk0GCSqGSIb3DQEHAaCCCj4Eggo6MIIKNjCCBg8GCSqGSIb3DQEHAaCCBgAEggX8MIIF+DCCBfQGCyqGSIb3DQEMCgECoIIE/jCCBPowHAYKKoZIhvcNAQwBAzAOBAg0TVY6f7OAJQICB9AEggTYxSGxhdr5+siCKOF4wwNR1fFZD6MgSrdNJdtVvhGH4di0cN/ebBCZE0LndiowCI3eeuh9+NZF2IPGBrCgz53KelH67LF4ViJcWUIku8trtPlLYvPNqlFt9N7Cg2sIY7RqseFop2Tk56YRF5vlivQC0YyOwULT5smgGl//ZVmWOpMssxsp2z2+uhg7oOgxgODEiYxyytSyAti+lSStj/ZV2MMnPmQgwSozFDf21ZlEAonuu0ATDMBGNyM18kJeCtEdcMLeq3VAjPF3Ai6qbESj1E9/hRKZP3965fSfKfQAM9T6bRzO/uFJSwBa5R4J4f6uU+JiFySkSHvvqeyQ5WP+DQTkRSC/bYtzaIEzzaHqG/N5HytRXCyGyyA3qF95nyra6au99He8tIrEd9VP9O/zmDU28siauFqkwt8uXY0bg/FSXccKvtxqoaCVJNK7UriVnQ4y/Ykt78lVbjpddLF8lh3tznGzuFm8PhSggrsyBUiQ2YNqyBlOYHZAHTeJ8wrIuKeK+zHhNBoGRrUSLVxxGJWuqCFyGdAGijdgC4bGhBJbtMyovqDndgt8OnPy9ysigz7eRYc2FqGtQgDC/D5lVDPJZP7kaGX33o8DNrVWSkR/rYZT8O3Eu9SNBEfh12UtFVNq3QHpGi3NR6pGV2QSanE5HHBfVPjA7mKA0b0v8ydd6uo6mIMsI6F0S5S3L4pufmTusoSrIF7k56gYIHLhMIrLJA8GA+gXXx+eueJZX+AA7sC9M0vWGc4YxNebItjs/2R5ZS1f5mfO7OEnLlV9MGnMmtokn7lCUoUykT9PYG85h6MyVPvrkWIMfJ4p8YJCadgwG9Lfa+km8HGunH/CsAD/xmuSGpVyTbDPxQNCnuQ/kkw3LWMYJBEA+QMHZncSrZ9zQGdGE9sHAz7yuUPtg/KHONtTiHaK4mNL8bPO+z/+PRHi8eW4d24KsRcBx1J9VpiYb9EQz/YLl2SjrJh/qk9QrvMdNfMzepTN8ivkbKDp2OmPKhwydMxEDsX4Te7PbXUiG+RX/lRr5FjL6gcB33/xkOv3KbBp9NAvWYTw9zxzi94iSck3bCzzWO2D2eVC669d6HpE0hSCMOeJmjeiND7rjIHUFpVR9/hs+BgfwBVge4ZQuJEY1n0MrA0JWxYa8urOxaYV4//i2uJsW5HLgU9TsWTaRf+DIG1u8v4wgkaa1V/6hha7zW/Br+blUmycRkT42XXXG0jwKbOmuT8DyFiWUjXbq316ihCtDr9VGlY3wYjys5aev4GKX4uSrzaB+9cvOchIszqdQIhY/6XzHWdPBoahbdnHf1vR7WDgYWMaCftJKLzIjbXuHZXYONvgLukKMR61BU67WBleCXalNsFnUh05MAmto3BSjqfGLKA8jl1MV0MLD6Sl3mTNRzcdIUELjyzK+CnFd17+7kdudb2AqgM1sCEyQP2vZPvgU8PVmi5KM8/r7fHYiPyneLKqTctiJ59UeLNTeb8Pm8P37aBHQssHoImB1KRpl1r03s95+++svGcIDXDhOQu2xoHuGuJUM29atnblZOCtkl/SJiNIoxWrM/+GXKJKRjg4e9NxSaHW3QoYbbED04RXJYDUYO18lkeL0uxWtguDRJ50sWnpihuCYIVdSLoZb1KyxAKF9ynr6T6JdTGB4jANBgkrBgEEAYI3EQIxADATBgkqhkiG9w0BCRUxBgQEAQAAADBdBgkqhkiG9w0BCRQxUB5OAHQAZQAtAGMAZQBkADMAOAAzAGYAMgAtADYAYwAyADcALQA0AGUANgAzAC0AYgBkADUAOQAtAGYANAA3ADEAMgBkADEANQA0ADEAMgAzMF0GCSsGAQQBgjcRATFQHk4ATQBpAGMAcgBvAHMAbwBmAHQAIABTAG8AZgB0AHcAYQByAGUAIABLAGUAeQAgAFMAdABvAHIAYQBnAGUAIABQAHIAbwB2AGkAZABlAHIwggQfBgkqhkiG9w0BBwagggQQMIIEDAIBADCCBAUGCSqGSIb3DQEHATAcBgoqhkiG9w0BDAEDMA4ECAjz/ijptbHRAgIH0ICCA9g0oeB8O6DLAdr7knh488twpdZFOy5ZVds9dqbuALqq9jHrvr8Ng4fT+oUucJjKf1B0SDGpA3hlAYrHFbRwa1AvaUXLvQEUaEXRAWaCPfxo5LHaYjBTufEFW7ENDHJKjW5VUwUB9Qut9wv0EFpfVHsU7ZglxpplYapErsSXd75rLQJr/bgElBaMlvFmJ/G/1asJ+vnJvnTsaCXvO3MFte2DoGD4IanwfPNXzQYh8uyJaG+DBNnm1ULaaprcjK1xP8kBA4AJ3X+np0urct/ZKwciOG2+KL1SSEjsnimwKVi7CcI8y8lISkWwR3dqTNcEBmuZSZnY9V9kQQLLoFzbL1PntvA5BC6sSujayXnPRV94M7vDkvy/yhhFwZgT4yaM6WwQMVymXota2YKs2M7BdfNYRLzx2gCLxloFls1j5D3Wx60j7qoMl4Sd+ilTm/xlt6O3M8PYmO1aa7yFALvM/iWL+hnaGTOqLhA2VL//BvQI50a1d0Hm/RdIK4crZcPHeBW39sMMJlHUDzUHSQ5Zriv7a0kuihwsU68zRHjHiO8wnt9LklbACsBC6ggp10Pdlrh++1Ugj77HUUBfzLd1Rjn9jYd6pKkpeMIssaJaVzfbNtOC4yDmuxbfhqeTSNDzhHAr4Nw9xJ3BrMgcrjSdoF4Yt0f8t0TUfpV5qWO7RWoqmV5bho0aRYFVa5h1QgL5HSX532Ky1C4I2DVHupef6ruJ4hVldoFn/7rsYRUS2iGfgQ+/Y2sA4/Rnc/zkeEm2fRZECiN+R7dhB2MtNN+5T8SXMC5OaApCN9f9D5o06SDa5LWhkmX0Dq6jlELREfZ6IxM2VhB1DmqK8zALYVDqExmCiJp/97guc3OnAgCXA090gZJ5/l7Mtutg0oS8X8C+wEhUbFXVEYEpqTvZRoQ830hVKPhK/IhDn4P8Q+N/GlCLasWO0sQVqlW3HFQgdjcSV5qqzxkYGxgX/20THj4QMNqFs/LAySgQ231dMCSJ4Xz0VrNWLLYDXFbUMesP9yEXYIpM+WCelX+hp0cCzYB55awcKJ7TeOXfcVL8KAkMgW5ylqMF1hahznOC6SxVAiHlbxxYNlD+EsHOV7y0com5EIiBOZKa2avWz/1adINAb1n4bxr3G5OAD1FCM8MpKJSqhoT88C6W7CMYhfLizXM+pjaVCX/d+6uRofvt1G7AAIrKp9u8ZPZ7fWI2EJ4t51eXhE/Xd7WQ/TEpPJeYgVr/Lput8W6Tj1jqP5OKZPsxfPcfB5PrXPsV+4aHg2vpAyMxjMFiys30DkKD9RAb+htFgfhbS6r9y+pmLLQwOzAfMAcGBSsOAwIaBBQ1HxIk53kOwgmAq+m4okPVzcdO+AQUPqkuTAt6u6lecUiHLXPwozzR/UECAgfQ";
    var privateKeyBytes = Convert.FromBase64String(certificateString);
    var pfxPassword = "Password";
    return new X509Certificate2(privateKeyBytes, pfxPassword);
}

async Task TestGetTimeMethod()
{
    using var channel = GrpcChannel.ForAddress(address);
    var timeClient = new Time.TimeClient(channel);
    var timeReply = await timeClient.GetCurrentAsync(new CurrentTimeRequestDto { });
    Console.WriteLine("Time client reply: " + timeReply.Time);
}

async Task TestGetLogsMethod()
{
    var timeLogClient = CreateClientWithCert(address, GetClientTestingCertificate());
    var timeLogReply = await timeLogClient.GetLogsAsync(new LogRequestDto { Page = 1, Size = 10 });
    Console.WriteLine("Time log client reply: ");
    if (timeLogReply?.Logs is null)
    {
        Console.WriteLine("Empty response.");
    }
    else
    {
        foreach (var log in timeLogReply.Logs)
        {
            Console.WriteLine(log?.Time);
        }
    }
}

string HandleInput()
{
    Console.WriteLine("Enter target system:");
    Console.WriteLine("1. Windows service");
    Console.WriteLine("2. Development environment");
    var input = Console.ReadLine();

    return input switch
    {
        "1" => "http://localhost:5000",
        "2" => "https://localhost:7285",
        _ => throw new ArgumentOutOfRangeException(input, nameof(input)),
    };
}