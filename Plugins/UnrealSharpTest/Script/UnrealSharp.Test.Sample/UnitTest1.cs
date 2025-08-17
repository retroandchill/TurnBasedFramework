using UnrealSharp.Test.Asserts;
using UnrealSharp.Test.Attributes;

namespace UnrealSharp.Test.Sample;

public class Tests
{
    [AutomationSetup]
    public void Setup()
    {
    }

    [AutomationTest]
    public void Test1()
    {
        Assert.Pass();
    }

    [AutomationTest]
    public void Test2()
    {
        Assert.Fail("This test is failing");
    }
    
    /*
    [TestCase(12, 3, 4)]
    [TestCase(12, 2, 6)]
    [TestCase(12, 4, 3)]
    public void DivideTest(int n, int d, int q)
    {
        Assert.That(n / d, Is.EqualTo(q));
    }
    */
}