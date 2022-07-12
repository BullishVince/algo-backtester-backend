using System;
using AlgoBacktesterBackend.Api.Models;
using AlgoBacktesterBackend.Api.Services;
using AlgoBacktesterBackend.Domain.Models;
using Bogus;
using FakeItEasy;

namespace AlgoBacktesterBackend.Api.Config.Mocks;

public static class BacktestingServiceMock {
    public static IBacktestingService Get() {
        var faker = new Faker();
        var fakeService = A.Fake<IBacktestingService>();
        // A.CallTo(() => fakeService.RunBacktest(A<BacktestingRequest>._))
        //     .ReturnsLazily(() => new BacktestingResult(){
        //         NumberOfTrades = faker.Random.Number(500),
        //         Profitability = faker.Random.Decimal()
        //     });

        return fakeService;
    }
}