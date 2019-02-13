using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class SourceCodeFileSpecification
  {
    public class XmlSourceCodeFileBuilder
    {

      public XmlSourceCodeFile Build()
      {
        return new XmlSourceCodeFile(
          FileName, 
          DeclaredNamespaces, 
          ParentProjectRootNamespace, 
          ParentProjectAssemblyName, 
          Usings);
      }

      public List<string> Usings { get; set; } = Any.List<string>();
      public string ParentProjectAssemblyName { get; set; } = Any.Instance<string>();
      public string ParentProjectRootNamespace { get; set; } = Any.Instance<string>();
      public List<string> DeclaredNamespaces { get; set; } = Any.String().AsList();
      public string FileName { get; set; } = Any.Instance<string>();
    }

    [Fact]
    public void ShouldReportIncorrectNamespaceWhenIsInRootFolderAndItsOnlyNamespaceDoesNotMatchRootNamespace()
    {
      //GIVEN
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder();
      xmlSourceCodeFile.DeclaredNamespaces = Any.OtherThan(xmlSourceCodeFile.ParentProjectRootNamespace).AsList();
      var ruleViolationFactory = Substitute.For<IRuleViolationFactory>();
      var file = new SourceCodeFile(xmlSourceCodeFile.Build(), ruleViolationFactory);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();
      var violation = Any.Instance<RuleViolation>();

      ruleViolationFactory.ProjectScopedRuleViolation(
        ruleDescription, 
        xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
        xmlSourceCodeFile.ParentProjectRootNamespace + " but the file " +
        xmlSourceCodeFile.FileName + " has incorrect namespace " +
        xmlSourceCodeFile.DeclaredNamespaces.Single()).Returns(violation);


      //WHEN
      file.EvaluateNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.Add(violation));
    }
    
    [Fact]
    public void ShouldReportErrorWhenFileDeclaresNoNamespaces()
    {
      //GIVEN
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder();
      xmlSourceCodeFile.DeclaredNamespaces = new List<string>();
      var ruleViolationFactory = Substitute.For<IRuleViolationFactory>();
      var file = new SourceCodeFile(xmlSourceCodeFile.Build(), ruleViolationFactory);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();
      var violation = Any.Instance<RuleViolation>();

      ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription,
        xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
        xmlSourceCodeFile.ParentProjectRootNamespace + " but the file " +
        xmlSourceCodeFile.FileName + " has no namespace declared").Returns(violation);

      //WHEN
      file.EvaluateNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.Add(violation));
    }    
    
    [Fact]
    public void ShouldReportErrorWhenFileDeclaresMoreThanOneNamespace()
    {
      //GIVEN
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder();
      var namespace1 = Any.String();
      var namespace2 = Any.String();
      xmlSourceCodeFile.DeclaredNamespaces = new List<string> {namespace1, namespace2};
      var ruleViolationFactory = Substitute.For<IRuleViolationFactory>();
      var file = new SourceCodeFile(xmlSourceCodeFile.Build(), ruleViolationFactory);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();
      var violation = Any.Instance<RuleViolation>();

      ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription,
        $"{xmlSourceCodeFile.ParentProjectAssemblyName} " +
        $"has root namespace {xmlSourceCodeFile.ParentProjectRootNamespace} " +
        $"but the file {xmlSourceCodeFile.FileName} " +
        $"declares multiple namespaces: {namespace1}, {namespace2}").Returns(violation);

      //WHEN
      file.EvaluateNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.Add(violation));
    }

    [Fact]
    public void ShouldReportOkWhenIsInRootFolderAndItsOnlyNamespaceMatchesRootNamespace()
    {
      //GIVEN
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder();
      xmlSourceCodeFile.DeclaredNamespaces = xmlSourceCodeFile.ParentProjectRootNamespace.AsList();
      var file = new SourceCodeFile(xmlSourceCodeFile.Build(), Any.Instance<IRuleViolationFactory>());
      var report = Substitute.For<IAnalysisReportInProgress>();

      //WHEN
      file.EvaluateNamespacesCorrectness(report, Any.String());

      //THEN
      report.ReceivedNothing();
    }
    
    [Fact]
    public void ShouldAddMappingBetweenNamespaceAndUsingsToCacheWhenAsked()
    {
      //GIVEN
      var namespace1 = Any.String();
      var namespace2 = Any.String();
      var namespace3 = Any.String();
      var using1 = Any.String();
      var using2 = Any.String();
      var using3 = Any.String();
      var cache = Substitute.For<INamespacesDependenciesCache>();
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder
      {
        DeclaredNamespaces = new List<string>() {namespace1, namespace2, namespace3},
        Usings = new List<string>() { using1, using2, using3}
      };
      var file = new SourceCodeFile(xmlSourceCodeFile.Build(), Any.Instance<IRuleViolationFactory>());

      //WHEN
      file.AddNamespaceMappingTo(cache);

      //THEN
      cache.Received(1).AddMapping(namespace1, using1);
      cache.Received(1).AddMapping(namespace1, using2);
      cache.Received(1).AddMapping(namespace1, using3);
      cache.Received(1).AddMapping(namespace2, using1);
      cache.Received(1).AddMapping(namespace2, using2);
      cache.Received(1).AddMapping(namespace2, using3);
      cache.Received(1).AddMapping(namespace3, using1);
      cache.Received(1).AddMapping(namespace3, using2);
      cache.Received(1).AddMapping(namespace3, using3);
    }

  }

  public static class ToCollectionExtensions
  {
    public static List<T> AsList<T>(this T item)
    {
      return new List<T> { item };
    }
  }
}
