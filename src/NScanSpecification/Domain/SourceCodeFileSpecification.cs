using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.NScan.Xml;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
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
          new List<string>(/* bug */));
      }

      public string ParentProjectAssemblyName { get; set; } = Any.Instance<string>();
      public string ParentProjectRootNamespace { get; set; } = Any.Instance<string>();
      public List<string> DeclaredNamespaces { get; set; } = new List<string> { Any.String() };
      public string FileName { get; set; } = Any.Instance<string>();
    }

    [Fact]
    public void ShouldReportIncorrectNamespaceWhenIsInRootFolderAndItsOnlyNamespaceDoesNotMatchRootNamespace()
    {
      //GIVEN
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder();
      xmlSourceCodeFile.DeclaredNamespaces = new List<string> {Any.OtherThan(xmlSourceCodeFile.ParentProjectRootNamespace)};
      var file = new SourceCodeFile(xmlSourceCodeFile.Build());
      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();


      //WHEN
      file.EvaluateNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.ProjectScopedViolation(
        ruleDescription,
        xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " + 
        xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
        + xmlSourceCodeFile.FileName + " has incorrect namespace "
        + xmlSourceCodeFile.DeclaredNamespaces.Single()
        ));
    }

    [Fact]
    public void ShouldReportOkWhenIsInRootFolderAndItsOnlyNamespaceMatchesRootNamespace()
    {
      //GIVEN
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder();
      xmlSourceCodeFile.DeclaredNamespaces = new List<string>() { xmlSourceCodeFile.ParentProjectRootNamespace };
      var file = new SourceCodeFile(xmlSourceCodeFile.Build());
      var report = Substitute.For<IAnalysisReportInProgress>();

      //WHEN
      file.EvaluateNamespacesCorrectness(report, Any.String());

      //THEN
      report.ReceivedNothing();
    }
    
    [Fact]
    public void ShouldReportOkWhenIsInNestedFolderAndItsOnlyNamespaceMatchesRootNamespaceSuffixedWithFolderPath()
    {
      //GIVEN
      var folder = Any.String();
      var subfolder = Any.String();
      var fileName = Path.Combine(folder, subfolder, Any.String());
      var xmlSourceCodeFile = new XmlSourceCodeFileBuilder();
      xmlSourceCodeFile.FileName = fileName;
      xmlSourceCodeFile.DeclaredNamespaces =
        new List<string> {$"{xmlSourceCodeFile.ParentProjectRootNamespace}.{folder}.{subfolder}"};

      var file = new SourceCodeFile(xmlSourceCodeFile.Build());
      var report = Substitute.For<IAnalysisReportInProgress>();


      //WHEN
      file.EvaluateNamespacesCorrectness(report, Any.String());

      //THEN
      report.ReceivedNothing();
    }

  }
}
