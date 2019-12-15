using System;
using System.Collections.Generic;
using FluentAssertions;
using NScan.Domain.Root;
using NScan.NamespaceBasedRules;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class NamespaceBasedReportFragmentsFormatSpecification
  {
    [Fact]
    public void ShouldFormatCycles()
    {
      //GIVEN
      var format = new NamespaceBasedReportFragmentsFormat();
      var namespace1 = Any.String();
      var namespace2 = Any.String();
      var namespace3 = Any.String();
      var namespace4 = Any.String();
      var namespace5 = Any.String();
      var namespace6 = Any.String();
      var header = Any.String();
      var cycles = new List<IReadOnlyList<string>>
      {
        new List<string> {namespace1, namespace2, namespace3},
        new List<string> {namespace4, namespace5, namespace6},
      };

      //WHEN
      var result = format.ApplyTo(cycles, header);

      //THEN
      result.Should().Be(
        $"{header} 1:{Environment.NewLine}" +
        $"  {namespace1}{Environment.NewLine}" +
        $"    {namespace2}{Environment.NewLine}" +
        $"      {namespace3}{Environment.NewLine}" +
        $"{header} 2:{Environment.NewLine}" +
        $"  {namespace4}{Environment.NewLine}" +
        $"    {namespace5}{Environment.NewLine}" +
        $"      {namespace6}{Environment.NewLine}");
    }

  }
}