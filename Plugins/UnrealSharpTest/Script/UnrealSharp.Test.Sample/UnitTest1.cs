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
    
    [AutomationTest]
    public async Task Test3()
    {
        await Task.Delay(1000);
        Assert.Pass();
    }
    
    [AutomationTest]
    public async ValueTask Test4()
    {
        await Task.Delay(1000);
        Assert.Pass();
    }
    
    [AutomationTest]
    public async Task<bool> Test5()
    {
        await Task.Delay(1000);
        Assert.Pass();
        return true;
    }
    
    [AutomationTest]
    public async ValueTask<bool> Test6()
    {
        await Task.Delay(1000);
        Assert.Pass();
        return true;
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