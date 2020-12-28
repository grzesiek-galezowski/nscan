using System;
using System.Collections.Generic;
using FluentAssertions;
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
      var namespace1 = Any.Instance<NamespaceName>();
      var namespace2 = Any.Instance<NamespaceName>();
      var namespace3 = Any.Instance<NamespaceName>();
      var namespace4 = Any.Instance<NamespaceName>();
      var namespace5 = Any.Instance<NamespaceName>();
      var namespace6 = Any.Instance<NamespaceName>();
      var header = Any.String();
      var cycles = new List<NamespaceDependencyPath>
      {
        NamespaceDependencyPath.With(namespace1, namespace2, namespace3),
        NamespaceDependencyPath.With(namespace4, namespace5, namespace6),
      };

      //WHEN
      var result = format.ApplyTo(cycles, header);

      //THEN
      result.Should().Be(
        $"{header} 1:{Environment.NewLine}" +
        $"  {namespace1.Value}{Environment.NewLine}" +
        $"    {namespace2.Value}{Environment.NewLine}" +
        $"      {namespace3.Value}{Environment.NewLine}" +
        $"{header} 2:{Environment.NewLine}" +
        $"  {namespace4.Value}{Environment.NewLine}" +
        $"    {namespace5.Value}{Environment.NewLine}" +
        $"      {namespace6.Value}{Environment.NewLine}");
    }

  }
}
