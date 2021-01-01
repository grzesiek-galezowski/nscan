using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AtmaFileSystem;
using FluentAssertions;
using Microsoft.Build.Construction;
using NScan.Adapter.ReadingCSharpSolution.ReadingProjects;
using Microsoft.Build.Utilities.ProjectCreation;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.Adapter.ReadingCSharpSolutionSpecification
{
  public class ReadingCsProjSpecification : INScanSupport
  {
    [Fact]
    public void Test1() //bug
    {
      //GIVEN
      var absoluteFilePath = AbsoluteFilePath.Value(Path.GetFullPath("project1.csproj"));
      ProjectRootElement project = ProjectCreator.Templates.SdkCsproj(absoluteFilePath.ToString());
      project.AddProperty("Lol", "Lol2");

      using (new FileScope(project))
      {
        //WHEN
        var csharpProjectDto = ReadCSharpProjectFrom(absoluteFilePath);

        csharpProjectDto.AssemblyName.Should().Be("project1");
        csharpProjectDto.Id.Should().Be(new ProjectId(absoluteFilePath.ToString()));
        csharpProjectDto.TargetFramework.Should().Be("netstandard2.0");
        csharpProjectDto.SourceCodeFiles.Should().BeEmpty();
        csharpProjectDto.AssemblyReferences.Should().BeEmpty();
        csharpProjectDto.PackageReferences.Should().BeEmpty();
        csharpProjectDto.ReferencedProjectIds.Should().BeEmpty();
        csharpProjectDto.Should().BeEquivalentTo(
          new CsharpProjectDto(new ProjectId(absoluteFilePath.ToString()),
            "project1",
            "netstandard2.0",
            Enumerable.Empty<SourceCodeFileDto>(), 
            Enumerable.Empty<PackageReference>().ToList(), 
            Enumerable.Empty<AssemblyReference>().ToList(), 
            Array.Empty<ProjectId>()));
      }

    }

    private CsharpProjectDto ReadCSharpProjectFrom(AbsoluteFilePath absoluteFilePath)
    {
      return new ProjectPaths(new[] {absoluteFilePath,}, this).LoadXmlProjects().Single();
    }

    public void Report(Exception exceptionFromResolution)
    {
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException,
      AbsoluteFilePath projectFilePath)
    {
    }

    public void Log(IndependentRuleComplementDto dto)
    {
    }

    public void Log(CorrectNamespacesRuleComplementDto dto)
    {
    }

    public void Log(NoCircularUsingsRuleComplementDto dto)
    {
    }

    public void Log(HasAttributesOnRuleComplementDto dto)
    {
    }

    public void Log(HasTargetFrameworkRuleComplementDto dto)
    {
    }

    public void Log(NoUsingsRuleComplementDto dto)
    {
    }
  }

  public class FileScope : IDisposable
  {
    private readonly ProjectRootElement _project;

    public FileScope(ProjectRootElement project)
    {
      _project = project;
      _project.Save();
    }

    public void Dispose()
    {
      File.Delete(_project.FullPath);
    }
  }
}
