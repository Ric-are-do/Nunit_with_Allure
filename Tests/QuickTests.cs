using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;

namespace Allurre_with_Nunit.Tests;

[TestFixture]
[AllureNUnit]
[AllureSuite("Quick Tests")]
[AllureFeature("Unit Tests")]
[Parallelizable(ParallelScope.Self)]
public class QuickTests
{
    [Test]
    [AllureStory("String Operations")]
    [AllureTag("quick", "string")]
    public void StringContainsSubstring()
    {
        Assert.That("Hello World", Does.Contain("World"));
    }

    [Test]
    [AllureStory("String Operations")]
    [AllureTag("quick", "string")]
    public void StringStartsWith()
    {
        Assert.That("Hello World", Does.StartWith("Hello"));
    }

    [Test]
    [AllureStory("String Operations")]
    [AllureTag("quick", "string")]
    public void StringToUpperConvertsCorrectly()
    {
        Assert.That("hello".ToUpper(), Is.EqualTo("HELLO"));
    }

    [Test]
    [AllureStory("String Operations")]
    [AllureTag("quick", "string")]
    public void EmptyStringHasZeroLength()
    {
        Assert.That(string.Empty, Has.Length.EqualTo(0));
    }

    [Test]
    [AllureStory("String Operations")]
    [AllureTag("quick", "string")]
    public void StringTrimRemovesWhitespace()
    {
        Assert.That("  hello  ".Trim(), Is.EqualTo("hello"));
    }

    [Test]
    [AllureStory("Math Operations")]
    [AllureTag("quick", "math")]
    public void AdditionIsCorrect()
    {
        Assert.That(2 + 3, Is.EqualTo(5));
    }

    [Test]
    [AllureStory("Math Operations")]
    [AllureTag("quick", "math")]
    public void MultiplicationIsCorrect()
    {
        Assert.That(4 * 5, Is.EqualTo(20));
    }

    [Test]
    [AllureStory("Math Operations")]
    [AllureTag("quick", "math")]
    public void DivisionIsCorrect()
    {
        Assert.That(10 / 2, Is.EqualTo(5));
    }

    [Test]
    [AllureStory("Math Operations")]
    [AllureTag("quick", "math")]
    public void ModuloReturnsRemainder()
    {
        Assert.That(10 % 3, Is.EqualTo(1));
    }

    [Test]
    [AllureStory("Math Operations")]
    [AllureTag("quick", "math")]
    public void AbsoluteValueOfNegative()
    {
        Assert.That(Math.Abs(-42), Is.EqualTo(42));
    }

    [Test]
    [AllureStory("Collection Operations")]
    [AllureTag("quick", "collection")]
    public void ListContainsAddedItem()
    {
        var list = new List<string> { "apple", "banana", "cherry" };
        Assert.That(list, Does.Contain("banana"));
    }

    [Test]
    [AllureStory("Collection Operations")]
    [AllureTag("quick", "collection")]
    public void ListCountIsCorrect()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        Assert.That(list, Has.Count.EqualTo(5));
    }

    [Test]
    [AllureStory("Collection Operations")]
    [AllureTag("quick", "collection")]
    public void ArrayIsSorted()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        Assert.That(array, Is.Ordered);
    }

    [Test]
    [AllureStory("Collection Operations")]
    [AllureTag("quick", "collection")]
    public void DictionaryContainsKey()
    {
        var dict = new Dictionary<string, int> { { "one", 1 }, { "two", 2 } };
        Assert.That(dict, Does.ContainKey("one"));
    }

    [Test]
    [AllureStory("Collection Operations")]
    [AllureTag("quick", "collection")]
    public void EmptyListIsEmpty()
    {
        var list = new List<int>();
        Assert.That(list, Is.Empty);
    }

    [Test]
    [AllureStory("Boolean Logic")]
    [AllureTag("quick", "logic")]
    public void TrueIsTrue()
    {
        Assert.That(true, Is.True);
    }

    [Test]
    [AllureStory("Boolean Logic")]
    [AllureTag("quick", "logic")]
    public void NotFalseIsTrue()
    {
        Assert.That(!false, Is.True);
    }

    [Test]
    [AllureStory("Boolean Logic")]
    [AllureTag("quick", "logic")]
    public void NullIsNull()
    {
        string? value = null;
        Assert.That(value, Is.Null);
    }

    [Test]
    [AllureStory("Type Checks")]
    [AllureTag("quick", "type")]
    public void IntegerIsValueType()
    {
        int number = 42;
        Assert.That(number, Is.InstanceOf<int>());
    }

    [Test]
    [AllureStory("Type Checks")]
    [AllureTag("quick", "type")]
    public void StringIsReferenceType()
    {
        object value = "hello";
        Assert.That(value, Is.InstanceOf<string>());
    }
}
