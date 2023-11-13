using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using TimeServer.Core.Models;
using TimeServer.Core.Ports;
using TimeServer.Core.Providers;
using TimeServer.Core.Strategies;
using Xunit;

namespace TimeServer.Core.Tests
{
    public class TimeStrategyTests
    {
        private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        private readonly ITimeLoggingPort _loggingPort = Substitute.For<ITimeLoggingPort>();

        private readonly TimeStrategy _sut;

        public TimeStrategyTests()
        {
            _sut = new TimeStrategy(_dateTimeProvider, _loggingPort);
        }

        [Fact]
        public async Task GetCurrentDateTimeAsync_ReturnsDataFromProvider()
        {
            var currentDateTime = new DateTime(2020, 2, 3, 4, 5, 6, DateTimeKind.Utc);

            _dateTimeProvider.GetCurrentDateTime().Returns(currentDateTime);

            var result = await _sut.GetCurrentDateTimeAsync();

            result.Should().Be(currentDateTime);
        }

        [Fact]
        public async Task GetCurrentDateTimeAsync_LogsDataToPort()
        {
            var currentDateTime = new DateTime(2020, 2, 3, 4, 5, 6, DateTimeKind.Utc);

            _dateTimeProvider.GetCurrentDateTime().Returns(currentDateTime);

            await _sut.GetCurrentDateTimeAsync();

            await _loggingPort.Received().CreateLogAsync(Arg.Is<TimeRequestLog>(p => p.Time == currentDateTime));
        }

        [Fact]
        public async Task GetTimeLogsAsync_PassesCorrectParametersToPort()
        {
            var page = 3;
            var size = 10;

            await _sut.GetTimeLogsAsync(page, size);

            await _loggingPort.Received().GetLogsAsync(Arg.Is(20), Arg.Is(10));
        }
    }
}