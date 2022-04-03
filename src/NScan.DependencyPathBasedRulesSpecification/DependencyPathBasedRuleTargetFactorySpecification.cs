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
    IReadOnlyList<CsharpProjectDto> xmlProjects = new List<CsharpProjectDto>();
    var projectDictionary = csharpWorkspaceModel.CreateDependencyPathRuleTargetsByIds(xmlProjects);

    //THEN
    projectDictionary.Should().BeEmpty();
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
    IEnumerable<CsharpProjectDto> xmlProjects = new List<CsharpProjectDto>
    {
      xmlProject1,
      xmlProject2,
      xmlProject3
    };
    var projectDictionary = csharpWorkspaceModel.CreateDependencyPathRuleTargetsByIds(
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
