using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NScan.Domain.ProjectScopedRules;
using NScan.Domain.Root;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Lib;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScanSpecification.Lib;
using NSubstitute;
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
      var csharpWorkspaceModel = new CsharpWorkspaceModel(Any.Instance<INScanSupport>(), Any.Instance<IProjectScopedRuleViolationFactory>());

      //WHEN
      IReadOnlyList<XmlProject> xmlProjects = new List<XmlProject>();
      var projectDictionary = csharpWorkspaceModel.CreateProjectsDictionaryFrom(xmlProjects.Select(p => new XmlProjectDataAccess(p)));

      //THEN
      projectDictionary.Should().BeEmpty();
    }

    [Fact]
    public void ShouldCreateDotNetStandardProjectsFromInput()
    {
      //GIVEN
      var csharpWorkspaceModel = new CsharpWorkspaceModel(
        Any.Instance<INScanSupport>(), Any.Instance<IProjectScopedRuleViolationFactory>());
      var xmlProject1 = Substitute.For<IXmlProjectDataAccess>();
      var xmlProject2 = Substitute.For<IXmlProjectDataAccess>();
      var xmlProject3 = Substitute.For<IXmlProjectDataAccess>();
      var expectedProjectId1 = Any.ProjectId();
      var expectedProjectId2 = Any.ProjectId();
      var expectedProjectId3 = Any.ProjectId();

      xmlProject1.Id().Returns(expectedProjectId1);
      xmlProject2.Id().Returns(expectedProjectId2);
      xmlProject3.Id().Returns(expectedProjectId3);

      //WHEN
      IEnumerable<IXmlProjectDataAccess> xmlProjects = new List<IXmlProjectDataAccess>()
      {
        xmlProject1,
        xmlProject2,
        xmlProject3
      };
      var projectDictionary = csharpWorkspaceModel.CreateProjectsDictionaryFrom(
        xmlProjects);

      //THEN
      projectDictionary.Should().ContainKey(expectedProjectId1);
      projectDictionary.Should().ContainKey(expectedProjectId2);
      projectDictionary.Should().ContainKey(expectedProjectId3);
      projectDictionary[expectedProjectId1].Should().BeOfType<DotNetStandardProject>();
      projectDictionary[expectedProjectId2].Should().BeOfType<DotNetStandardProject>();
      projectDictionary[expectedProjectId3].Should().BeOfType<DotNetStandardProject>();
    }
       
  }
}