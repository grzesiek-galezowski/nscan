using FluentAssertions;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Domain.Root
{
  public class CSharpClassSpecification
  {
    [Theory]
    [InlineData("*Specification", "LolSpecification", true)]
    [InlineData("*Specification", "TestLol", false)]
    public void ShouldBeAbleToSayWhetherItsNameMatchesAPattern(string pattern, string className, bool expectedResult)
    {
      //GIVEN
      var declaration = new ClassDeclarationInfo(className, Any.String());
      
      var @class = new CSharpClass(declaration, Any.Array<ICSharpMethod>());
      
      //WHEN
      var nameMatches = @class.NameMatches(Pattern.WithoutExclusion(pattern));

      //THEN
      nameMatches.Should().Be(expectedResult);
    }
    
    [Fact]
    public void ShouldEvaluateDecorationWithAttributesForAllMethodsThatMatchInclusionPattern()
    {
      //GIVEN
      var report = Any.Instance<IAnalysisReportInProgress>();
      var methodNameInclusionPattern = Any.Pattern();
      var ruleDescription = Any.String();
      var method1 = Substitute.For<ICSharpMethod>();
      var method2 = Substitute.For<ICSharpMethod>();
      var method3 = Substitute.For<ICSharpMethod>();
      var methods = new [] {method1, method2, method3};
      var declaration = Any.Instance<ClassDeclarationInfo>();
      var @class = new CSharpClass(declaration, methods);

      method1.NameMatches(methodNameInclusionPattern).Returns(true);
      method2.NameMatches(methodNameInclusionPattern).Returns(false);
      method3.NameMatches(methodNameInclusionPattern).Returns(true);

      //WHEN
      @class.EvaluateDecorationWithAttributes(report, methodNameInclusionPattern, new RuleDescription(ruleDescription));

      //THEN
      method1.Received(1).EvaluateMethodsHavingCorrectAttributes(report, declaration.Name, new RuleDescription(ruleDescription));
      method2.DidNotReceive().EvaluateMethodsHavingCorrectAttributes(
        Arg.Any<IAnalysisReportInProgress>(), 
        Arg.Any<string>(), 
        Arg.Any<RuleDescription>());
      method3.Received(1).EvaluateMethodsHavingCorrectAttributes(report, declaration.Name, new RuleDescription(ruleDescription));
    }
  }
}
