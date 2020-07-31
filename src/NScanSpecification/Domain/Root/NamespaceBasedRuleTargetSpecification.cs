using System.Collections.Generic;
using NScan.Domain;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Strings;
using Xunit;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class NamespaceBasedRuleTargetSpecification
  {
    [Fact]
    public void ShouldEvaluateRuleWithItsNamespaceDependenciesMapping()
    {
      //GIVEN
      var namespacesCache = AnyRoot.Root.Any.Instance<INamespacesDependenciesCache>();
      var rule = Substitute.For<INamespacesBasedRule>();
      var report = AnyRoot.Root.Any.Instance<IAnalysisReportInProgress>();
      var projectAssemblyName = AnyRoot.Root.Any.String();
      var project = new NamespaceBasedRuleTarget(
        projectAssemblyName,
        AnyRoot.Root.Any.ReadOnlyList<ISourceCodeFile>(),
        namespacesCache);

      //WHEN
      project.Evaluate(rule, report);

      //THEN
      rule.Received(1).Evaluate(projectAssemblyName, namespacesCache, report);
    }

    [Fact]
    public void ShouldAddAllFilesInfoToNamespacesCacheWhenAskedToRefreshIt()
    {
      //GIVEN
      var file1 = Substitute.For<ISourceCodeFile>();
      var file2 = Substitute.For<ISourceCodeFile>();
      var file3 = Substitute.For<ISourceCodeFile>();
      var files = new List<ISourceCodeFile>()
      {
        file1, file2, file3
      };
      var namespacesCache = AnyRoot.Root.Any.Instance<INamespacesDependenciesCache>();
      var project = new NamespaceBasedRuleTarget(
        AnyRoot.Root.Any.String(),
        files,
        namespacesCache);

      //WHEN
      project.RefreshNamespacesCache();

      //THEN
      Received.InOrder(() =>
      {
        file1.AddNamespaceMappingTo(namespacesCache);
        file2.AddNamespaceMappingTo(namespacesCache);
        file3.AddNamespaceMappingTo(namespacesCache);
      });
    }
  }
}