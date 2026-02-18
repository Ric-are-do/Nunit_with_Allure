using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace Allurre_with_Nunit.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("Mixed Results Tests")]
[AllureFeature("Mixed Scenarios")]
[Parallelizable(ParallelScope.Self)]
public class MixedResultsTests
{
    // --- PASSING TESTS ---

    [Test]
    [AllureStory("Passing - String Operations")]
    [AllureTag("mixed", "pass", "string")]
    public void StringReplaceWorks()
    {
        Assert.That("Hello World".Replace("World", "NUnit"), Is.EqualTo("Hello NUnit"));
    }

    [Test]
    [AllureStory("Passing - Math Operations")]
    [AllureTag("mixed", "pass", "math")]
    public void SquareRootIsCorrect()
    {
        Assert.That(Math.Sqrt(144), Is.EqualTo(12));
    }

    [Test]
    [AllureStory("Passing - Collection Operations")]
    [AllureTag("mixed", "pass", "collection")]
    public void ListRemoveWorks()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        list.Remove(3);
        Assert.That(list, Has.Count.EqualTo(4));
        Assert.That(list, Does.Not.Contain(3));
    }

    [Test]
    [AllureStory("Passing - Type Checks")]
    [AllureTag("mixed", "pass", "type")]
    public void DateTimeIsCorrectType()
    {
        object now = DateTime.Now;
        Assert.That(now, Is.InstanceOf<DateTime>());
    }

    [Test]
    [AllureStory("Passing - Range Checks")]
    [AllureTag("mixed", "pass", "range")]
    public void ValueIsInRange()
    {
        Assert.That(50, Is.InRange(1, 100));
    }

    [Test]
    [AllureStory("Passing - String Operations")]
    [AllureTag("mixed", "pass", "string")]
    public void StringSplitWorks()
    {
        var parts = "one,two,three".Split(',');
        Assert.That(parts, Has.Length.EqualTo(3));
        Assert.That(parts[1], Is.EqualTo("two"));
    }

    [Test]
    [AllureStory("Passing - Math Operations")]
    [AllureTag("mixed", "pass", "math")]
    public void MaxReturnsLargest()
    {
        Assert.That(Math.Max(10, 20), Is.EqualTo(20));
    }

    [Test]
    [AllureStory("Passing - Collection Operations")]
    [AllureTag("mixed", "pass", "collection")]
    public void DictionaryValueLookup()
    {
        var dict = new Dictionary<string, int> { { "x", 10 }, { "y", 20 } };
        Assert.That(dict["y"], Is.EqualTo(20));
    }

    [Test]
    [AllureStory("Passing - Exception Handling")]
    [AllureTag("mixed", "pass", "exception")]
    public void DivideByZeroThrowsException()
    {
        Assert.Throws<DivideByZeroException>(() =>
        {
            var result = 10 / int.Parse("0");
        });
    }

    [Test]
    [AllureStory("Passing - Boolean Logic")]
    [AllureTag("mixed", "pass", "logic")]
    public void AndOperatorWorks()
    {
        Assert.That(true && true, Is.True);
        Assert.That(true && false, Is.False);
    }

    // --- FAILING TESTS ---

    [Test]
    [AllureStory("Failing - String Operations")]
    [AllureTag("mixed", "fail", "string")]
    public void StringLengthIsWrong()
    {
        Assert.That("Hello".Length, Is.EqualTo(10));
    }

    [Test]
    [AllureStory("Failing - Math Operations")]
    [AllureTag("mixed", "fail", "math")]
    public void NegativeSquareRootFails()
    {
        var result = Math.Sqrt(16);
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    [AllureStory("Failing - Collection Operations")]
    [AllureTag("mixed", "fail", "collection")]
    public void ListIsNotInExpectedOrder()
    {
        var list = new List<int> { 9, 1, 5, 3, 7 };
        Assert.That(list, Is.Ordered);
    }

    [Test]
    [AllureStory("Failing - Type Checks")]
    [AllureTag("mixed", "fail", "type")]
    public void StringIsNotInt()
    {
        object value = "not a number";
        Assert.That(value, Is.InstanceOf<int>());
    }

    [Test]
    [AllureStory("Failing - Range Checks")]
    [AllureTag("mixed", "fail", "range")]
    public void ValueIsOutOfRange()
    {
        Assert.That(200, Is.InRange(1, 100));
    }

    [Test]
    [AllureStory("Failing - String Operations")]
    [AllureTag("mixed", "fail", "string")]
    public void StringEndsWithFails()
    {
        Assert.That("Hello World", Does.EndWith("Hello"));
    }

    [Test]
    [AllureStory("Failing - Math Operations")]
    [AllureTag("mixed", "fail", "math")]
    public void RoundingFails()
    {
        Assert.That(Math.Round(2.5), Is.EqualTo(3));
    }

    [Test]
    [AllureStory("Failing - Collection Operations")]
    [AllureTag("mixed", "fail", "collection")]
    public void ArraysAreNotEqual()
    {
        var expected = new[] { "a", "b", "c" };
        var actual = new[] { "a", "b", "d" };
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [AllureStory("Failing - Equality")]
    [AllureTag("mixed", "fail", "equality")]
    public void DatesAreNotEqual()
    {
        var date1 = new DateTime(2026, 1, 1);
        var date2 = new DateTime(2026, 12, 31);
        Assert.That(date1, Is.EqualTo(date2));
    }

    [Test]
    [AllureStory("Failing - Boolean Logic")]
    [AllureTag("mixed", "fail", "logic")]
    public void OrOperatorCheckFails()
    {
        Assert.That(false || false, Is.True);
    }
}
