using System.Collections.Generic;
using FluentAssertions;
using NScan.DependencyPathBasedRules;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.DependencyPathBasedRulesSpecification
{
  public class DependencyPathReportFragmentsFormatSpecification
  {
    [Fact]
    public void ShouldCreateStringWithConcatenatedStringRepresentationOfProjectPathWhenApplied()
    {
      //GIVEN
      var format = new DependencyPathReportFragmentsFormat();
      var p1 = Any.Instance<IDependencyPathBasedRuleTarget>();
      var p2 = Any.Instance<IDependencyPathBasedRuleTarget>();
      var p3 = Any.Instance<IDependencyPathBasedRuleTarget>();

      //WHEN
      var result = format.ApplyToPath(new List<IDependencyPathBasedRuleTarget> {p1, p2, p3});

      //THEN
      result.Should().Be($"[{p1.ToString()}]->[{p2.ToString()}]->[{p3.ToString()}]");
    }

    [Fact]
    public void ShouldCreateStringWithSingleProjectWhenViolationPathConsistsOfASingleProject()
    {
      //GIVEN
      var format = new DependencyPathReportFragmentsFormat();
      var p1 = Any.Instance<IDependencyPathBasedRuleTarget>();

      //WHEN
      var result = format.ApplyToPath(new List<IDependencyPathBasedRuleTarget> {p1});

      //THEN
      result.Should().Be($"[{p1.ToString()}]");
    }
  }
}
