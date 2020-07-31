using System.Collections.Generic;
using FluentAssertions;
using NScan.Domain;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class CsharpWorkspaceModelSpecification
  {
    [Fact]
    public void ShouldCreateEmptyDictionaryWhenInputListDoesNotContainAnyProject()
    {
      //GIVEN
      var csharpWorkspaceModel = new CsharpWorkspaceModel(
        Any.Instance<INScanSupport>(), 
        Any.Instance<IProjectScopedRuleViolationFactory>());

      //WHEN
      IReadOnlyList<CsharpProjectDto> xmlProjects = new List<CsharpProjectDto>();
      var projectDictionary = csharpWorkspaceModel.CreateProjectsDictionaryFrom(xmlProjects);

      //THEN
      projectDictionary.Should().BeEmpty();
    }

    [Fact]
    public void ShouldCreateDotNetStandardProjectsFromInput()
    {
      //GIVEN
      var csharpWorkspaceModel = new CsharpWorkspaceModel(
        Any.Instance<INScanSupport>(), Any.Instance<IProjectScopedRuleViolationFactory>());
      var xmlProject1 = Any.Instance<CsharpProjectDto>();
      var xmlProject2 = Any.Instance<CsharpProjectDto>();
      var xmlProject3 = Any.Instance<CsharpProjectDto>();

      //WHEN
      IEnumerable<CsharpProjectDto> xmlProjects = new List<CsharpProjectDto>()
      {
        xmlProject1,
        xmlProject2,
        xmlProject3
      };
      var projectDictionary = csharpWorkspaceModel.CreateProjectsDictionaryFrom(
        xmlProjects);

      //THEN
      projectDictionary.Should().ContainKey(xmlProject1.Id);
      projectDictionary.Should().ContainKey(xmlProject2.Id);
      projectDictionary.Should().ContainKey(xmlProject3.Id);
      projectDictionary[xmlProject1.Id].Should().BeOfType<DotNetStandardProject>();
      projectDictionary[xmlProject2.Id].Should().BeOfType<DotNetStandardProject>();
      projectDictionary[xmlProject3.Id].Should().BeOfType<DotNetStandardProject>();
    }
       
  }
}