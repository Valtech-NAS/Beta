using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Elasticsearch.Entities;

namespace SFA.Apprenticeships.Services.Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class EntityTests
    {
        [TestCase]
        public void RangeHasValueShouldInitializeToFalse()
        {
            var test = new Range<int>();

            test.HasValue.Should().BeFalse();
        }

        [TestCase]
        public void DoesSettingRangeFromValueSetHasValue()
        {
            var test = new Range<int> {RangeFrom = 1};

            test.HasValue.Should().BeTrue();
        }

        [TestCase]
        public void DoesSettingRangeToValueSetHasValue()
        {
            var test = new Range<int> { RangeTo = 1 };

            test.HasValue.Should().BeTrue();
        }

        [TestCase]
        public void DoesSettingRangeFromValueWithIncorrectTypeThrowException()
        {
            Action test = () => new Range<int> { RangeFrom = 1d };

            test.ShouldThrow<ArgumentException>();
        }

        [TestCase]
        public void DoesSettingRangeToValueWithIncorrectTypeThrowException()
        {
            Action test = () => new Range<int> {RangeTo = 1d};

            test.ShouldThrow<ArgumentException>();
        }
    }
}
