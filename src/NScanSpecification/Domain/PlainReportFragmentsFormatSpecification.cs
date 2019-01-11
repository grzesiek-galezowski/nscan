using System.Collections.Generic;
using FluentAssertions;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using Xunit;
using static System.Environment;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class PlainReportFragmentsFormatSpecification
  {
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