using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using AtmaFileSystem;
using FluentAssertions;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;
using NScan.SharedKernel;
using NScanSpecification.Lib;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution
{
  public class XmlProjectDataAccessSpecification
  {
    [Fact]
    public void ShouldCorrectlyResolveAbsolutePathsOfReferencedProjects()
    {
      //GIVEN
      var projectFilePath = AbsoluteFilePath.Value(
        $"{FileSystemRoot.PlatformSpecificValue()}{Path.DirectorySeparatorChar}A{Path.DirectorySeparatorChar}A.csproj");
      var referenceToProjectB = "..\\B\\B.csproj";
      var dataAccess = XmlProjectDataAccess.From(projectFilePath, 
        XmlProjectWith(projectFilePath, referenceToProjectB)
      );

      //WHEN
      var dto = dataAccess.BuildCsharpProjectDto();

      //THEN
      dto.ReferencedProjectIds.Should().HaveCount(1);
      dto.ReferencedProjectIds.Should().Equal(
        ImmutableList<ProjectId>.Empty.Add(
          new ProjectId($"{FileSystemRoot.PlatformSpecificValue()}{Path.DirectorySeparatorChar}B{Path.DirectorySeparatorChar}B.csproj")));
    }

    private static XmlProject XmlProjectWith(AbsoluteFilePath projectFilePath, string referenceToProjectB)
    {
      return new XmlProject()
      {
        ItemGroups = new List<XmlItemGroup>()
        {
          Any.Instance<XmlItemGroup>()
            with
            {
              ProjectReferences = new List<XmlProjectReference>()
              {
                new XmlProjectReference()
                {
                  Include = referenceToProjectB
                }
              }
            }
        },
        PropertyGroups = new List<XmlPropertyGroup>()
        {
          Any.Instance<XmlPropertyGroup>()
        },
        AbsolutePath = projectFilePath,
        Sdk = "net"
      };
    }
  }
}
