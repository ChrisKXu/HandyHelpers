using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace HandyHelpers.Tests
{
    using HandyHelpers;

    public class IntHelpersTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10000)]
        [InlineData(int.MinValue)]
        public void TestTimesNegativeOrZero(int value)
        {
            var list = new List<int>();

            var iterations = value.Times(i =>
            {
                list.Add(i);
                return true;
            });

            iterations.Should().Be(0);
            list.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        [InlineData(10000)]
        public void TestTimesPositiveSmallData(int value)
        {
            var list = new List<int>();

            var iterations = value.Times(i =>
            {
                list.Add(i);
                return true;
            });

            iterations.Should().Be(value);
            list.Should().NotBeEmpty();
            list.Count().Should().Be(value);

            for(var i = 0; i < value; ++i)
            {
                list[i].Should().Be(i);
            }
        }

        [Fact]
        public void TestTimesPositiveBigData()
        {
            var list = new List<int>();
            var stopPoint = 2000000;
            var iterations = int.MaxValue.Times(i =>
            {
                list.Add(i);
                return i < stopPoint - 1;
            });

            iterations.Should().Be(stopPoint);
            list.Should().NotBeEmpty();
            list.Count().Should().Be(stopPoint);

            for (var i = 0; i < stopPoint; ++i)
            {
                list[i].Should().Be(i);
            }
        }

        [Fact]
        public void TestTimesPositiveWithException()
        {
            var list = new List<int>();
            var stopPoint = 20;
            var exceptionText = "Stop point exception!";

            try
            {
                var iterations = 100.Times(i =>
                {
                    list.Add(i);

                    if (i == stopPoint - 1)
                    {
                        throw new Exception(exceptionText);
                    }

                    return true;
                });
            }
            catch(Exception exception)
            {
                exception.Message.Should().Be(exceptionText);
                list.Count().Should().Be(stopPoint);

                for (var i = 0; i < stopPoint; ++i)
                {
                    list[i].Should().Be(i);
                }
            }
        }
    }
}
