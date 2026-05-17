using Xunit;

namespace TaskTracker.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        Assert.Equal(2, 1 + 1);
    }

    [Fact]
    public void Test3()
    {
        Assert.Contains("задача", "это задача");
    }
}
