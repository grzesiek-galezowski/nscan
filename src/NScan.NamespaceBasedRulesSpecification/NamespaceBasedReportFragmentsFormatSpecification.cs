using NScan.NamespaceBasedRules;

namespace NScan.NamespaceBasedRulesSpecification;

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
      $"  {namespace1}{Environment.NewLine}" +
      $"    {namespace2}{Environment.NewLine}" +
      $"      {namespace3}{Environment.NewLine}" +
      $"{header} 2:{Environment.NewLine}" +
      $"  {namespace4}{Environment.NewLine}" +
      $"    {namespace5}{Environment.NewLine}" +
      $"      {namespace6}{Environment.NewLine}");
  }

}
