using System.Collections.Generic;
using FluentAssertions;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;
using static System.Environment;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class PlainReportFragmentsFormatSpecification
  {
    [Fact]
    public void ShouldCreateStringWithConcatenatedStringRepresentationOfProjectPathWhenApplied()
    {
      //GIVEN
      var format = new PlainReportFragmentsFormat();
      var p1 = Any.Instance<IReferencedProject>();
      var p2 = Any.Instance<IReferencedProject>();
      var p3 = Any.Instance<IReferencedProject>();

      //WHEN
      var result = format.ApplyToPath(new List<IDependencyPathBasedRuleTarget>() {p1, p2, p3});

      //THEN
      result.Should().Be($"[{p1.ToString()}]->[{p2.ToString()}]->[{p3.ToString()}]");
    }

    [Fact]
    public void ShouldCreateStringWithSingleProjectWhenViolationPathConsistsOfASingleProject()
    {
      //GIVEN
      var format = new PlainReportFragmentsFormat();
      var p1 = Any.Instance<IDependencyPathBasedRuleTarget>();

      //WHEN
      var result = format.ApplyToPath(new List<IDependencyPathBasedRuleTarget>() {p1});

      //THEN
      result.Should().Be($"[{p1.ToString()}]");
    }

    [Fact]
    public void ShouldFormatCycles()
    {
      //GIVEN
      var format = new PlainReportFragmentsFormat();
      var namespace1 = Any.String();
      var namespace2 = Any.String();
      var namespace3 = Any.String();
      var namespace4 = Any.String();
      var namespace5 = Any.String();
      var namespace6 = Any.String();
      var cycles = new List<IReadOnlyList<string>>
      {
        new List<string> {namespace1, namespace2, namespace3},
        new List<string> {namespace4, namespace5, namespace6},
      };

      //WHEN
      var result = format.ApplyToCycles(cycles);

      //THEN
      result.Should().Be(
        $"Cycle 1:{NewLine}" +
        $"  {namespace1}{NewLine}" + 
        $"    {namespace2}{NewLine}" + 
        $"      {namespace3}{NewLine}" + 
        $"Cycle 2:{NewLine}" +
        $"  {namespace4}{NewLine}" + 
        $"    {namespace5}{NewLine}" + 
        $"      {namespace6}{NewLine}");
    }

  }
}