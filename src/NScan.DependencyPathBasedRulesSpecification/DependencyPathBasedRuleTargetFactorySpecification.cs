﻿using LanguageExt;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.DependencyPathBasedRulesSpecification;

public class DependencyPathBasedRuleTargetFactorySpecification
{
  [Fact]
  public void ShouldCreateEmptyDictionaryWhenInputListDoesNotContainAnyProject()
  {
    //GIVEN
    var csharpWorkspaceModel = new DependencyPathBasedRuleTargetFactory(
      Any.Instance<INScanSupport>());

    //WHEN
    var projectDictionary = csharpWorkspaceModel.CreateDependencyPathRuleTargetsByIds(Seq<CsharpProjectDto>.Empty);

    //THEN
    projectDictionary.ToReadOnlyDictionary().Should().BeEmpty();
  }

  [Fact]
  public void ShouldCreateDotNetStandardProjectsFromInput()
  {
    //GIVEN
    var csharpWorkspaceModel = new DependencyPathBasedRuleTargetFactory(
      Any.Instance<INScanSupport>());
    var xmlProject1 = Any.Instance<CsharpProjectDto>();
    var xmlProject2 = Any.Instance<CsharpProjectDto>();
    var xmlProject3 = Any.Instance<CsharpProjectDto>();

    //WHEN
    var xmlProjects = Seq.create(xmlProject1, xmlProject2, xmlProject3);
    var projectDictionary = csharpWorkspaceModel.CreateDependencyPathRuleTargetsByIds(
      xmlProjects);

    //THEN
    projectDictionary.ToReadOnlyDictionary().Should().ContainKey(xmlProject1.Id);
    projectDictionary.ToReadOnlyDictionary().Should().ContainKey(xmlProject2.Id);
    projectDictionary.ToReadOnlyDictionary().Should().ContainKey(xmlProject3.Id);
    projectDictionary[xmlProject1.Id].Should().BeOfType<DotNetStandardProject>();
    projectDictionary[xmlProject2.Id].Should().BeOfType<DotNetStandardProject>();
    projectDictionary[xmlProject3.Id].Should().BeOfType<DotNetStandardProject>();
  }
       
}
