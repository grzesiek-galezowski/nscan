using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using FluentAssertions;
using NScan.Lib;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class SourceCodeFileSpecification
  {
    [Fact]
    public void ShouldReportIncorrectNamespaceWhenIsInRootFolderAndItsOnlyNamespaceDoesNotMatchRootNamespace()
    {
      //GIVEN
      var ruleViolationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
      var parentProjectAssemblyName = Any.String();
      var parentProjectRootNamespace = Any.String();
      var pathRelativeToProjectRoot = Any.Instance<RelativeFilePath>();
      var fileBuilder = new SourceCodeFileBuilder
      {
        RuleViolationFactory = ruleViolationFactory,
        ParentProjectAssemblyName = parentProjectAssemblyName,
        PathRelativeToProjectRoot = pathRelativeToProjectRoot,
        ParentProjectRootNamespace = parentProjectRootNamespace
      };
      fileBuilder.DeclaredNamespaces = Any.OtherThan(fileBuilder.ParentProjectRootNamespace).AsList();
      var file = fileBuilder.Build();  

      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();
      var violation = Any.Instance<RuleViolation>();

      ruleViolationFactory.ProjectScopedRuleViolation(
        ruleDescription, 
        parentProjectAssemblyName + " has root namespace " +
        parentProjectRootNamespace + " but the file " +
        pathRelativeToProjectRoot + " has incorrect namespace " +
        fileBuilder.DeclaredNamespaces.Single()).Returns(violation);

      //WHEN
      file.CheckNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.Add(violation));
    }
    
    [Fact]
    public void ShouldReportErrorWhenFileDeclaresNoNamespaces()
    {
      //GIVEN
      var ruleViolationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
      var parentProjectAssemblyName = Any.String();
      var parentProjectRootNamespace = Any.String();
      var pathRelativeToProjectRoot = Any.Instance<RelativeFilePath>();
      var fileBuilder = new SourceCodeFileBuilder
      {
        RuleViolationFactory = ruleViolationFactory,
        DeclaredNamespaces = new List<string>(),
        ParentProjectAssemblyName = parentProjectAssemblyName,
        ParentProjectRootNamespace = parentProjectRootNamespace,
        PathRelativeToProjectRoot = pathRelativeToProjectRoot
      };
      var file = fileBuilder.Build();
      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();
      var violation = Any.Instance<RuleViolation>();

      ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription,
        parentProjectAssemblyName + " has root namespace " +
        parentProjectRootNamespace + " but the file " +
        pathRelativeToProjectRoot + " has no namespace declared").Returns(violation);

      //WHEN
      file.CheckNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.Add(violation));
    }    
    
    [Fact]
    public void ShouldReportErrorWhenFileDeclaresMoreThanOneNamespace()
    {
      //GIVEN
      var namespace1 = Any.String();
      var namespace2 = Any.String();
      var ruleViolationFactory = Substitute.For<IProjectScopedRuleViolationFactory>();
      var parentProjectAssemblyName = Any.String();
      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();
      var violation = Any.Instance<RuleViolation>();
      var projectRootNamespace = Any.String();
      var fileName = Any.Instance<RelativeFilePath>();

      var file = new SourceCodeFileBuilder
      {
        DeclaredNamespaces = new List<string> { namespace1, namespace2 },
        RuleViolationFactory = ruleViolationFactory,
        ParentProjectAssemblyName = parentProjectAssemblyName,
        ParentProjectRootNamespace = projectRootNamespace,
        PathRelativeToProjectRoot = fileName
      }.Build();

      ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription,
        $"{parentProjectAssemblyName} " +
        $"has root namespace {projectRootNamespace} " +
        $"but the file {fileName} " +
        $"declares multiple namespaces: {namespace1}, {namespace2}").Returns(violation);

      //WHEN
      file.CheckNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.Add(violation));
    }

    [Fact]
    public void ShouldReportOkWhenIsInRootFolderAndItsOnlyNamespaceMatchesRootNamespace()
    {
      //GIVEN
      var fileBuilder = new SourceCodeFileBuilder();
      fileBuilder.DeclaredNamespaces = fileBuilder.ParentProjectRootNamespace.AsList();
      var file = fileBuilder.Build();
      var report = Substitute.For<IAnalysisReportInProgress>();

      //WHEN
      file.CheckNamespacesCorrectness(report, Any.String());

      //THEN
      report.ReceivedNothing();
    }
    
    [Fact]
    public void ShouldReportFileNameWhenConvertedToString()
    {
      //GIVEN
      var pathRelativeToProjectRoot = Any.Instance<RelativeFilePath>();
      var file = new SourceCodeFileBuilder
      {
        PathRelativeToProjectRoot = pathRelativeToProjectRoot
      }.Build();

      //WHEN
      var stringRepresentation = file.ToString();

      //THEN
      stringRepresentation.Should().Be(pathRelativeToProjectRoot.ToString());
    }


    [Fact]
    public void ShouldNotReportAnythingWhenThereAreNoClasses()
    {
      //GIVEN
      var report = Substitute.For<IAnalysisReportInProgress>();
      var class1 = Substitute.For<ICSharpClass>();
      var class2 = Substitute.For<ICSharpClass>();
      var class3 = Substitute.For<ICSharpClass>();
      var classNameInclusionPattern = Any.Pattern();
      var methodNameInclusionPattern = Any.Pattern();
      var ruleDescription = Any.String();
      var sourceCodeFile = new SourceCodeFileBuilder
      {
        Classes = new [] {class1, class2, class3}
      }.Build();

      class1.NameMatches(classNameInclusionPattern).Returns(true);
      class2.NameMatches(classNameInclusionPattern).Returns(false);
      class3.NameMatches(classNameInclusionPattern).Returns(true);

      //WHEN
      sourceCodeFile.CheckMethodsHavingCorrectAttributes(report, classNameInclusionPattern, methodNameInclusionPattern, ruleDescription);

      //THEN
      class1.Received(1).EvaluateDecorationWithAttributes(report, methodNameInclusionPattern, ruleDescription);
      class2.DidNotReceive()
        .EvaluateDecorationWithAttributes(Arg.Any<IAnalysisReportInProgress>(), Arg.Any<Pattern>(), Arg.Any<string>());
      class3.Received(1).EvaluateDecorationWithAttributes(report, methodNameInclusionPattern, ruleDescription);
    }

  }

  public class SourceCodeFileBuilder
  {
    public SourceCodeFile Build()
    {
      return new SourceCodeFile(RuleViolationFactory, DeclaredNamespaces,
        ParentProjectAssemblyName, ParentProjectRootNamespace, PathRelativeToProjectRoot,
        Classes);
    }

    public IReadOnlyList<string> Usings { get; set; } = Any.Instance<IReadOnlyList<string>>();

    public RelativeFilePath PathRelativeToProjectRoot { get; set; } = Any.Instance<RelativeFilePath>();

    public string ParentProjectRootNamespace { get; set; } = Any.Instance<string>();

    public string ParentProjectAssemblyName { get; set; } = Any.Instance<string>();

    public IReadOnlyList<string> DeclaredNamespaces { get; set; } = Any.Instance<IReadOnlyList<string>>();

    public IProjectScopedRuleViolationFactory RuleViolationFactory { get; set; } = Any.Instance<IProjectScopedRuleViolationFactory>();
    public ICSharpClass[] Classes { get; set; } = Any.Array<ICSharpClass>();
  }

  public static class ToCollectionExtensions
  {
    public static List<T> AsList<T>(this T item)
    {
      return new List<T> { item };
    }
  }
}
