using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Vertical.SpectreLogger.Templates;
using Xunit;

namespace Vertical.SpectreLogger.Tests.Templates
{
    public class TemplateStringTests
    {
        [Fact]
        public void SplitReturnsNoSegments()
        {
            GetSegments(string.Empty).ShouldBeEmpty();
        }

        [Fact]
        public void SplitIgnoresEscapedTemplate()
        {
            var segments = GetSegments("{template}{{not-template}}");
            segments.Count.ShouldBe(2);
            segments[0].Value.ShouldBe("{template}");
            segments[0].IsTemplate.ShouldBeTrue();
            segments[1].Value.ShouldBe("{{not-template}}");
        }
        
        [Fact]
        public void SplitFindsFirstSegment()
        {
            var segment = GetSegments("{Template}").Single();
            
            segment.Length.ShouldBe("{Template}".Length);
            segment.Match.ShouldNotBeNull();
            segment.InnerTemplate.ShouldBe("Template");
            segment.Value.ShouldBe("{Template}");
            segment.StartIndex.ShouldBe(0);
        }

        [Fact]
        public void SplitFindsFirstNonTemplateSegment()
        {
            var segments = GetSegments("Not a template").Single();
            
            segments.Length.ShouldBe("Not a template".Length);
            segments.Match.ShouldBeNull();
            segments.StartIndex.ShouldBe(0);
            segments.Value.ShouldBe("Not a template");
            segments.InnerTemplate.ShouldBeNull();
        }

        [Fact]
        public void SplitFindsLastTemplateSegment()
        {
            var segments = GetSegments("Hi {name}");

            segments.Count.ShouldBe(2);
            segments[0].Value.ShouldBe("Hi ");
            segments[0].Length.ShouldBe(3);
            segments[0].IsTemplate.ShouldBeFalse();
            segments[0].InnerTemplate.ShouldBeNull();
            segments[0].Match.ShouldBeNull();
            segments[1].Value.ShouldBe("{name}");
            segments[1].Length.ShouldBe("{name}".Length);
            segments[1].Match.ShouldNotBeNull();
            segments[1].InnerTemplate.ShouldBe("name");
            segments[1].StartIndex.ShouldBe(3);
        }

        [Fact]
        public void SplitFindsLastNotTemplateSegment()
        {
            var segments = GetSegments("{name} says hi");

            segments.Count.ShouldBe(2);
            segments[0].Value.ShouldBe("{name}");
            segments[0].IsTemplate.ShouldBeTrue();
            segments[1].Value.ShouldBe(" says hi");
            segments[1].IsTemplate.ShouldBeFalse();
        }

        private static List<TemplateSegment> GetSegments(string str)
        {
            var list = new List<TemplateSegment>();
            TemplateString.Split(str, (in TemplateSegment segment) => list.Add(segment));
            return list;
        }
    }
}