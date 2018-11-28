using System;
using NSubstitute;
using TddXt.NScan.Domain;
using TddXt.NScan.Xml;
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
      
      //WHEN
      file.EvaluateNamespacesCorrectness(report);

      //THEN
      report.Received(1).ProjectScopedViolation(
        xmlSourceCodeFile.ParentProjectName + " has root namespace " + 
        xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
        + xmlSourceCodeFile.Name + " located in project root folde has incorrect namespace "
        + xmlSourceCodeFile.Namespace
        );
    }
    //bug multiple namespaces
    //bug non-root files
    //bug happy path as well!!!
  }
}
