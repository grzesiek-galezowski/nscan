using System.IO;
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
    [Fact]
    public void ShouldReportIncorrectNamespaceWhenIsInRootFolderAndItsOnlyNamespaceDoesNotMatchRootNamespace()
    {
      //GIVEN
      var xmlSourceCodeFile = Any.Instance<XmlSourceCodeFile>();
      var file = new SourceCodeFile(xmlSourceCodeFile);
      var report = Substitute.For<IAnalysisReportInProgress>();
      var ruleDescription = Any.String();

      xmlSourceCodeFile.ParentProjectRootNamespace = Any.String();
      xmlSourceCodeFile.DeclaredNamespace = Any.OtherThan(xmlSourceCodeFile.ParentProjectRootNamespace);

      //WHEN
      file.EvaluateNamespacesCorrectness(report, ruleDescription);

      //THEN
      XReceived.Only(() => report.ProjectScopedViolation(
        ruleDescription,
        xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " + 
        xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
        + xmlSourceCodeFile.Name + " has incorrect namespace "
        + xmlSourceCodeFile.DeclaredNamespace));
    }

    [Fact]
    public void ShouldReportOkWhenIsInRootFolderAndItsOnlyNamespaceMatchesRootNamespace()
    {
      //GIVEN
      var xmlSourceCodeFile = Any.Instance<XmlSourceCodeFile>();
      var file = new SourceCodeFile(xmlSourceCodeFile);
      var report = Substitute.For<IAnalysisReportInProgress>();

      xmlSourceCodeFile.ParentProjectRootNamespace = Any.String();
      xmlSourceCodeFile.DeclaredNamespace = xmlSourceCodeFile.ParentProjectRootNamespace;

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
      var xmlSourceCodeFile = Any.Instance<XmlSourceCodeFile>();

      var file = new SourceCodeFile(xmlSourceCodeFile);
      var report = Substitute.For<IAnalysisReportInProgress>();

      xmlSourceCodeFile.Name = fileName;
      xmlSourceCodeFile.ParentProjectRootNamespace = Any.String();
      xmlSourceCodeFile.DeclaredNamespace = $"{xmlSourceCodeFile.ParentProjectRootNamespace}.{folder}.{subfolder}";

      //WHEN
      file.EvaluateNamespacesCorrectness(report, Any.String());

      //THEN
      report.ReceivedNothing();
    }

  }
}
