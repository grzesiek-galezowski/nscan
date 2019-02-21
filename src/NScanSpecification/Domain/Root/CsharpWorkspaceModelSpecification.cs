using System.Collections.Generic;
using FluentAssertions;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.NScan.Specification.Component.AutomationLayer;
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
      var projectDictionary = csharpWorkspaceModel.CreateProjectsDictionaryFrom(new List<XmlProject>());

      //THEN
      projectDictionary.Should().BeEmpty();
    }

    [Fact]
    public void ShouldCreateXXXX()
    {
      //GIVEN
      var csharpWorkspaceModel = new CsharpWorkspaceModel(Any.Instance<INScanSupport>(), Any.Instance<IProjectScopedRuleViolationFactory>());
      var xmlProject1 = XmlProjectBuilder.WithAssemblyName(Any.String()).Build();
      var expectedProjectId = new ProjectId(xmlProject1.AbsolutePath);

      //WHEN
      var projectDictionary = csharpWorkspaceModel.CreateProjectsDictionaryFrom(new List<XmlProject>()
      {
        xmlProject1
      });

      //THEN
      projectDictionary.Should().ContainKey(expectedProjectId);
      projectDictionary[expectedProjectId].Should().BeOfType<DotNetStandardProject>();
    }
       
  }
}