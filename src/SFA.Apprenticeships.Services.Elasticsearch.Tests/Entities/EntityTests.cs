using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Common.Entities.Elasticsearch;

namespace SFA.Apprenticeships.Services.Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class EntityTests
    {
        [TestCase]
        public void RangeHasValueShouldInitializeToFalse()
        {
            var test = new SortableRange<int>();

            test.HasValue.Should().BeFalse();
        }

        [TestCase]
        public void DoesSettingRangeFromValueSetHasValue()
        {
            var test = new SortableRange<int> {RangeFrom = 1};

            test.HasValue.Should().BeTrue();
        }

        [TestCase]
        public void DoesSettingRangeToValueSetHasValue()
        {
            var test = new SortableRange<int> { RangeTo = 1 };

            test.HasValue.Should().BeTrue();
        }

        [TestCase]
        public void DoesSettingRangeFromValueWithIncorrectTypeThrowException()
        {
            Action test = () => new SortableRange<int> { RangeFrom = 1d };

            test.ShouldThrow<ArgumentException>();
        }

        [TestCase]
        public void DoesSettingRangeToValueWithIncorrectTypeThrowException()
        {
            Action test = () => new SortableRange<int> {RangeTo = 1d};

            test.ShouldThrow<ArgumentException>();
        }
    }
}
