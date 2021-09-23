using System.Collections.Generic;
using FluentAssertions;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.ProjectScopedRulesSpecification
{
  public class CSharpMethodSpecification
  {
    [Theory]
    [InlineData("Should*", "ShouldLol", true)]
    [InlineData("Should*", "TestLol", false)]
    public void ShouldBeAbleToSayWhetherItsNameMatchesAPattern(string pattern, string methodName, bool expectedResult)
    {
      //GIVEN
      var declaration = new MethodDeclarationInfo(methodName, Any.ReadOnlyList<string>());

      var method = new CSharpMethod(declaration, Any.Instance<IProjectScopedRuleViolationFactory>());

      //WHEN
      var nameMatches = method.NameMatches(Pattern.WithoutExclusion(pattern));

      //THEN
      nameMatches.Should().Be(expectedResult);
    }

    [Fact]
    public void ShouldReportErrorFromDecorationEvaluationWhenIsNotDecorated()
    {
      //GIVEN
      var declaration = new MethodDeclarationInfo(Any.String(), new List<string>());
      var violationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
      var cSharpMethod = new CSharpMethod(declaration, violationFactory);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var parentClassName = Any.String();
      var description = Any.Instance<RuleDescription>();
      var violation = Any.Instance<RuleViolation>();

      violationFactory.ProjectScopedRuleViolation(description, $"Method {declaration.Name} in class {parentClassName} does not have any attribute").Returns(violation);

      //WHEN
      cSharpMethod.EvaluateMethodsHavingCorrectAttributes(report, parentClassName, description);

      //THEN
      report.Received(1).Add(violation);
    }

    [Fact]
    public void ShouldNotReportErrorFromDecorationEvaluationWhenIsDecorated()
    {
      //GIVEN
      var declaration = new MethodDeclarationInfo(Any.String(), new List<string> { Any.String() });
      var violationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
      var report = Substitute.For<IAnalysisReportInProgress>();
      var parentClassName = Any.String();
      var description = Any.Instance<RuleDescription>();
      var violation = Any.Instance<RuleViolation>();

      var cSharpMethod = new CSharpMethod(declaration, violationFactory);

      violationFactory.ProjectScopedRuleViolation(
        description, 
        $"Method {declaration.Name} in class {parentClassName} does not have any attribute")
        .Returns(violation);

      //WHEN
      cSharpMethod.EvaluateMethodsHavingCorrectAttributes(report, parentClassName, description);

      //THEN
      report.DidNotReceive().Add(Arg.Any<RuleViolation>());
    }
  }
}
