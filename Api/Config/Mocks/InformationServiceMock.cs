using System;
using AlgoBacktesterBackend.Api.Services;
using FakeItEasy;

namespace AlgoBacktesterBackend.Api.Config.Mocks;

public static class InformationServiceMock {
    public static IInformationService Get() {
        var fakeService = A.Fake<IInformationService>();
        return fakeService;
    }
}