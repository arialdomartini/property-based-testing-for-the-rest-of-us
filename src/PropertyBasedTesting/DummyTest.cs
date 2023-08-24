using FsCheck.Xunit;

namespace PropertyBasedTesting;

public class DummyTest
{
    [Property]
    bool always_pass(int n) => 
        n - n == 0;
}
