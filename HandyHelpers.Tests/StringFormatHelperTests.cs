using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace HandyHelpers.Tests
{
    using HandyHelpers;

    using ArgsDictionary = Dictionary<string, object>;
    using TupleWithDictionary = Tuple<string, Dictionary<string, object>, string>;
    using TupleWithObject = Tuple<string, object, string>;

    public class StringFormatHelperTests
    {
        [Fact]
        public void TestStringFormatWithDictionarySimple()
        {
            var testData = new List<TupleWithDictionary>()
            {
                new TupleWithDictionary("{foo}", new ArgsDictionary() { { "foo", "bar" } }, "bar")
            };

            testData.Should().NotBeEmpty();

            foreach (var testCase in testData)
            {
                // run the test case
                testCase.Item1.Format(testCase.Item2).Should().Be(testCase.Item3);
            }
        }

        [Fact]
        public void TestStringFormatWithObjectSimple()
        {
            var testData = new List<TupleWithObject>()
            {
                new TupleWithObject("{foo}", new { foo = "bar" }, "bar")
            };

            testData.Should().NotBeEmpty();

            foreach (var testCase in testData)
            {
                // run the test case
                testCase.Item1.Format(testCase.Item2).Should().Be(testCase.Item3);
            }
        }

        [Theory]
        [InlineData("", "", new string[] { })]
        [InlineData("abc", "abc", new string[] { })]
        [InlineData("{{foo}}", "{{foo}}", new string[] { })]
        [InlineData("{foo}", "{0}", new[] { "foo" })]
        [InlineData("a{foo}b", "a{0}b", new[] { "foo" })]
        [InlineData("a{foo}b{bar}c", "a{0}b{1}c", new[] { "foo", "bar" })]
        public void TestTryParseAndConvertTemplate(string template, string expectedConvertedTemplate, ICollection<string> expectedKeysArray)
        {
            var actualTemplate = string.Empty;
            ICollection<string> actualKeysArray = null;

            StringFormatHelpers.TryParseAndConvertTemplate(template, out actualTemplate, out actualKeysArray);

            actualTemplate.Should().Be(expectedConvertedTemplate);
            actualKeysArray.Should().Equal(expectedKeysArray);
        }

        [Fact]
        public void TestExtractObjectFromPropertyPathSimple()
        {
            var testObj = new
            {
                foo = "bar",
                nested = new
                {
                    foo = "bar"
                }
            };

            StringFormatHelpers.ExtractObjectFromPropertyPath("foo", testObj).Should().Be(testObj.foo);
            StringFormatHelpers.ExtractObjectFromPropertyPath("nested.foo", testObj).Should().Be(testObj.nested.foo);

            Action negative = () => StringFormatHelpers.ExtractObjectFromPropertyPath("bar", testObj);
            negative.ShouldThrow<InvalidOperationException>();
        }

        private class A
        {
            public B B { get; set; } 

            public int Value { get; set; }
        }

        private class B
        {
            public A A { get; set; }

            public int Value { get; set; }
        }

        [Fact]
        public void TestExtractObjectFromPropertyPathCircular()
        {
            var a = new A() { Value = 1 };
            var b = new B() { Value = 2, A = a };
            a.B = b;

            StringFormatHelpers.ExtractObjectFromPropertyPath("Value", a).Should().Be(a.Value);
            StringFormatHelpers.ExtractObjectFromPropertyPath("Value", b).Should().Be(b.Value);

            StringFormatHelpers.ExtractObjectFromPropertyPath("B.Value", a).Should().Be(a.B.Value);
            StringFormatHelpers.ExtractObjectFromPropertyPath("A.Value", b).Should().Be(b.A.Value);

            StringFormatHelpers.ExtractObjectFromPropertyPath("A.B.A.B.A.Value", b).Should().Be(a.Value);
        }

        [Theory]
        [InlineData("foo", "foo")]
        [InlineData("foo:abc", "foo")]
        [InlineData("foo,100", "foo")]
        [InlineData("foo,100:abc", "foo")]
        public void TestExtractIndexFromFormat(string template, string expectedIndex)
        {
            StringFormatHelpers.ExtractIndexFromFormat(template).Should().Be(expectedIndex);
        }
    }
}
