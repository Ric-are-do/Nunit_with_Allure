using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace Allurre_with_Nunit.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("Failing Tests")]
[AllureFeature("Failure Scenarios")]
[Parallelizable(ParallelScope.Self)]
public class FailingTests
{
    [Test]
    [AllureStory("Assertion Failures")]
    [AllureTag("failing", "string")]
    public void StringEqualityFails()
    {
        Assert.That("hello", Is.EqualTo("world"));
    }

    [Test]
    [AllureStory("Assertion Failures")]
    [AllureTag("failing", "math")]
    public void MathAssertionFails()
    {
        Assert.That(2 + 2, Is.EqualTo(5));
    }

    [Test]
    [AllureStory("Assertion Failures")]
    [AllureTag("failing", "collection")]
    public void ListDoesNotContainExpectedItem()
    {
        var list = new List<string> { "apple", "banana" };
        Assert.That(list, Does.Contain("cherry"));
    }

    [Test]
    [AllureStory("Assertion Failures")]
    [AllureTag("failing", "boolean")]
    public void FalseIsNotTrue()
    {
        Assert.That(false, Is.True);
    }

    [Test]
    [AllureStory("Assertion Failures")]
    [AllureTag("failing", "comparison")]
    public void GreaterThanFails()
    {
        Assert.That(1, Is.GreaterThan(10));
    }

    [Test]
    [AllureStory("Null and Empty Failures")]
    [AllureTag("failing", "null")]
    public void NotNullFails()
    {
        string? value = null;
        Assert.That(value, Is.Not.Null);
    }

    [Test]
    [AllureStory("Null and Empty Failures")]
    [AllureTag("failing", "empty")]
    public void EmptyCollectionIsNotEmpty()
    {
        var list = new List<int>();
        Assert.That(list, Is.Not.Empty);
    }

    [Test]
    [AllureStory("Null and Empty Failures")]
    [AllureTag("failing", "string")]
    public void EmptyStringIsNotEmpty()
    {
        Assert.That(string.Empty, Is.Not.Empty);
    }

    [Test]
    [AllureStory("Type and Range Failures")]
    [AllureTag("failing", "type")]
    public void WrongTypeFails()
    {
        object value = 42;
        Assert.That(value, Is.InstanceOf<string>());
    }

    [Test]
    [AllureStory("Type and Range Failures")]
    [AllureTag("failing", "range")]
    public void OutOfRangeFails()
    {
        Assert.That(100, Is.InRange(1, 10));
    }

    [Test]
    [AllureStory("Collection Order Failures")]
    [AllureTag("failing", "collection")]
    public void UnsortedArrayFailsOrderCheck()
    {
        var array = new[] { 5, 3, 1, 4, 2 };
        Assert.That(array, Is.Ordered);
    }

    [Test]
    [AllureStory("Collection Order Failures")]
    [AllureTag("failing", "collection")]
    public void WrongCountFails()
    {
        var list = new List<int> { 1, 2, 3 };
        Assert.That(list, Has.Count.EqualTo(5));
    }

    [Test]
    [AllureStory("String Pattern Failures")]
    [AllureTag("failing", "string")]
    public void StartsWithFails()
    {
        Assert.That("Hello World", Does.StartWith("World"));
    }

    [Test]
    [AllureStory("String Pattern Failures")]
    [AllureTag("failing", "string")]
    public void ContainsFails()
    {
        Assert.That("Hello World", Does.Contain("Goodbye"));
    }

    [Test]
    [AllureStory("String Pattern Failures")]
    [AllureTag("failing", "string")]
    public void RegexMatchFails()
    {
        Assert.That("hello123", Does.Match(@"^\d+$"));
    }

    [Test]
    [AllureStory("Exception Failures")]
    [AllureTag("failing", "exception")]
    public void ExpectedExceptionNotThrown()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var x = 1 + 1;
        });
    }

    [Test]
    [AllureStory("Exception Failures")]
    [AllureTag("failing", "exception")]
    public void WrongExceptionType()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            throw new InvalidOperationException("wrong type");
        });
    }

    [Test]
    [AllureStory("Equality Failures")]
    [AllureTag("failing", "equality")]
    public void ObjectEqualityFails()
    {
        var expected = new { Name = "Alice", Age = 30 };
        var actual = new { Name = "Bob", Age = 25 };
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [AllureStory("Equality Failures")]
    [AllureTag("failing", "equality")]
    public void ArrayEqualityFails()
    {
        var expected = new[] { 1, 2, 3 };
        var actual = new[] { 1, 2, 4 };
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [AllureStory("Equality Failures")]
    [AllureTag("failing", "equality")]
    public void DoubleToleranceFails()
    {
        Assert.That(3.14, Is.EqualTo(2.71).Within(0.01));
    }
}
