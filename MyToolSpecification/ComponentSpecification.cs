using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyTool
{
  public class ComponentSpecification
  {
    [Fact]
    public void ShouldReportAllSatisfiedRules()
    {
      //GIVEN
      var context = new ApplicationContext();
      context.HasProject("A");
      context.HasProject("B");

      context.AddIndependentOfRule("A", "B");

      //WHEN
      context.StartAnalysis();

      //THEN
      context.ReportShouldContainLine("[A] independentOf [B]: [OK]");
    }

    [Fact]
    public void ShouldDetectDirectRuleBreak()
    {
      //GIVEN
      var context = new ApplicationContext();
      context.HasProject("A").WithReferences("B", "C", "D");
      context.HasProject("B");
      context.HasProject("C");
      context.HasProject("D");

      context.AddIndependentOfRule("A", "B");

      //WHEN

      context.StartAnalysis();

      //THEN
      context.ReportShouldContainLine("Expected A to be independent of B, but found otherwise");
    }
  }
}