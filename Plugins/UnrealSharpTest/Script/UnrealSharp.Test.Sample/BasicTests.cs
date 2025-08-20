using NUnit.Framework;

namespace UnrealSharp.Test.Sample;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void PassingTest()
    {
        Assert.Pass("This is an explicit pass");
    }

    [Test]
    public void FailingTest()
    {
        Assert.Fail("This test is failing");
    }
}