using System;
using System.Collections.Generic;
using FluentAssertions;
using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.Domain.Domain.Root;
using TddXt.AnyRoot.Strings;
using Xunit;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class PlainReportFragmentsFormatSpecification
  {
    [Fact]
    public void ShouldCreateStringWithConcatenatedStringRepresentationOfProjectPathWhenApplied()
    {
      //GIVEN
      var format = new PlainReportFragmentsFormat();
      var p1 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var p2 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var p3 = AnyRoot.Root.Any.Instance<IReferencedProject>();

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
      var p1 = AnyRoot.Root.Any.Instance<IDependencyPathBasedRuleTarget>();

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
      var namespace1 = AnyRoot.Root.Any.String();
      var namespace2 = AnyRoot.Root.Any.String();
      var namespace3 = AnyRoot.Root.Any.String();
      var namespace4 = AnyRoot.Root.Any.String();
      var namespace5 = AnyRoot.Root.Any.String();
      var namespace6 = AnyRoot.Root.Any.String();
      var cycles = new List<IReadOnlyList<string>>
      {
        new List<string> {namespace1, namespace2, namespace3},
        new List<string> {namespace4, namespace5, namespace6},
      };

      //WHEN
      var result = format.ApplyToCycles(cycles);

      //THEN
      result.Should().Be(
        $"Cycle 1:{Environment.NewLine}" +
        $"  {namespace1}{Environment.NewLine}" + 
        $"    {namespace2}{Environment.NewLine}" + 
        $"      {namespace3}{Environment.NewLine}" + 
        $"Cycle 2:{Environment.NewLine}" +
        $"  {namespace4}{Environment.NewLine}" + 
        $"    {namespace5}{Environment.NewLine}" + 
        $"      {namespace6}{Environment.NewLine}");
    }

  }
}