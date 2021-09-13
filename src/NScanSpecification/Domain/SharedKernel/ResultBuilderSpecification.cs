using FluentAssertions;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.XFluentAssert.Api;
using Xunit;
using static System.Environment;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Domain.SharedKernel
{
  public class ResultBuilderSpecification 
  {
    //bug !!! nothing for AppendRuleSeparator
    //These tests should tell exactly how the output looks like!
    [Fact]
    public void ShouldPrintAllOksInTheSameOrderTheyWereReceived()
    {
      //GIVEN
      var report = new ResultBuilder();
      var anyDescription1 = Any.Instance<RuleDescription>();
      var anyDescription2 = Any.Instance<RuleDescription>();
      var anyDescription3 = Any.Instance<RuleDescription>();

      report.AppendOk(anyDescription1);
      report.AppendRuleSeparator();
      report.AppendOk(anyDescription2);
      report.AppendRuleSeparator();
      report.AppendOk(anyDescription3);

      //WHEN
      var output = report.Text();

      //THEN
      output.Should().Be(
        $"{anyDescription1}: [OK]{NewLine}" +
        $"{anyDescription2}: [OK]{NewLine}" +
        $"{anyDescription3}: [OK]");
    }

    [Fact]
    public void ShouldPrintAllViolationsInTheSameOrderTheyWereReceived()
    {
      //GIVEN
      var report = new ResultBuilder();
      var ruleDescription1 = Any.Instance<RuleDescription>();
      var ruleDescription2 = Any.Instance<RuleDescription>();
      var ruleDescription3 = Any.Instance<RuleDescription>();
      var violations1 = Any.String();
      var violations2 = Any.String();
      var violations3 = Any.String();

      report.AppendViolations(ruleDescription1, violations1);
      report.AppendRuleSeparator();
      report.AppendViolations(ruleDescription2, violations2);
      report.AppendRuleSeparator();
      report.AppendViolations(ruleDescription3, violations3);

      //WHEN
      var output = report.Text();

      //THEN
      output.Should().Be(
        $"{ruleDescription1}: [ERROR]{NewLine}" +
        $"{violations1}{NewLine}" +
        $"{ruleDescription2}: [ERROR]{NewLine}" +
        $"{violations2}{NewLine}" +
        $"{ruleDescription3}: [ERROR]{NewLine}" +
        $"{violations3}");
    }
  }
}
