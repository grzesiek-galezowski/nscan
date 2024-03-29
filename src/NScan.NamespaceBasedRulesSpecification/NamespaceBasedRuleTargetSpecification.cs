﻿using LanguageExt;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScanSpecification.Lib;

namespace NScan.NamespaceBasedRulesSpecification;

public class NamespaceBasedRuleTargetSpecification
{
  [Fact]
  public void ShouldEvaluateRuleWithItsNamespaceDependenciesMapping()
  {
    //GIVEN
    var namespacesCache = Any.Instance<INamespacesDependenciesCache>();
    var rule = Substitute.For<INamespacesBasedRule>();
    var report = Any.Instance<IAnalysisReportInProgress>();
    var projectAssemblyName = Any.Instance<AssemblyName>();
    var project = new NamespaceBasedRuleTarget(
      projectAssemblyName,
      Any.Seq<ISourceCodeFileUsingNamespaces>(),
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
    var file1 = Substitute.For<ISourceCodeFileUsingNamespaces>();
    var file2 = Substitute.For<ISourceCodeFileUsingNamespaces>();
    var file3 = Substitute.For<ISourceCodeFileUsingNamespaces>();
    var files = Seq.create(file1, file2, file3);
    var namespacesCache = Any.Instance<INamespacesDependenciesCache>();
    var project = new NamespaceBasedRuleTarget(
      Any.Instance<AssemblyName>(),
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
