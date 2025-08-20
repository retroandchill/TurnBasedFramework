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
    
    [TestCase(12, 3, 4)]
    [TestCase(12, 2, 6)]
    [TestCase(12, 4, 3)]
    public void DivideTest(int n, int d, int q)
    {
        Assert.That(n / d, Is.EqualTo(q));
    }
    
    [Test]
    public void BooleanValueTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(true, Is.True);
            Assert.That(false, Is.False);
        }
    }

    [Test]
    public void StringValueTest()
    {
        Assert.That("Hello", Is.EqualTo("Hello"));
        
        Assert.That("Hello", Is.Not.EqualTo("World"));
    }

    [Test]
    public void NullValueTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That((object?)null, Is.Null);
            Assert.That("", Is.Not.Null);
        }
    }
}