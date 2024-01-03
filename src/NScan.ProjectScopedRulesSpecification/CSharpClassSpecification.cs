using LanguageExt;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScanSpecification.Lib;

namespace NScan.ProjectScopedRulesSpecification;

public class CSharpClassSpecification
{
  [Theory]
  [InlineData("*Specification", "LolSpecification", true)]
  [InlineData("*Specification", "TestLol", false)]
  public void ShouldBeAbleToSayWhetherItsNameMatchesAPattern(string pattern, string className, bool expectedResult)
  {
    //GIVEN
    var declaration = new ClassDeclarationInfo(className, Any.String());
      
    var @class = new CSharpClass(declaration, Any.Seq<ICSharpMethod>());
      
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
    var description = Any.Instance<RuleDescription>();
    var method1 = Substitute.For<ICSharpMethod>();
    var method2 = Substitute.For<ICSharpMethod>();
    var method3 = Substitute.For<ICSharpMethod>();
    var methods = Seq.create(method1, method2, method3);
    var declaration = Any.Instance<ClassDeclarationInfo>();
    var @class = new CSharpClass(declaration, methods);

    method1.NameMatches(methodNameInclusionPattern).Returns(true);
    method2.NameMatches(methodNameInclusionPattern).Returns(false);
    method3.NameMatches(methodNameInclusionPattern).Returns(true);

    //WHEN
    @class.EvaluateDecorationWithAttributes(report, methodNameInclusionPattern, description);

    //THEN
    method1.Received(1).EvaluateMethodsHavingCorrectAttributes(report, declaration.Name, description);
    method2.DidNotReceive().EvaluateMethodsHavingCorrectAttributes(
      Arg.Any<IAnalysisReportInProgress>(), 
      Arg.Any<string>(), 
      Arg.Any<RuleDescription>());
    method3.Received(1).EvaluateMethodsHavingCorrectAttributes(report, declaration.Name, description);
  }
}
