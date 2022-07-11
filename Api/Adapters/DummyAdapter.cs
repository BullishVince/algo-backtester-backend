using System;

namespace AlgoBacktesterBackend.Api.Adapters;
public interface IDummyAdapter {
    public void Test();
}
public class DummyAdapter: IDummyAdapter {
    public DummyAdapter(string test){}
    public void Test() {

    }
}