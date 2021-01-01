using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.Adapter.ReadingCSharpSolutionSpecification
{
  public class ReadingCsProjSpecification : INScanSupport
  {
    [Fact]
    public void ShouldBeAbleToCreateDtoFromSdkCsprojDefaultTemplate()
    {
      //GIVEN
      var csprojName = Any.String();
      var csprojPath = CsProjPathTo(csprojName);
      var project = ProjectCreator.Templates.SdkCsproj(csprojPath.ToString());

      using (new FileScope(project))
      {
        //WHEN
        var csharpProjectDto = ReadCSharpProjectFrom(csprojPath);

        //THEN
        csharpProjectDto.Should().BeEquivalentTo(
          new CsharpProjectDto(new ProjectId(csprojPath.ToString()),
            csprojName,
            "netstandard2.0",
            ImmutableList<SourceCodeFileDto>.Empty,
            ImmutableList<PackageReference>.Empty, 
            ImmutableList<AssemblyReference>.Empty, 
            ImmutableList<ProjectId>.Empty));
      }
    }

    [Fact]
    public void ShouldBeAbleToReadTargetFrameworkWhateverItIs()
    {
      //GIVEN
      var csprojName = Any.String();
      var csprojPath = CsProjPathTo(csprojName);
      var targetFramework = Any.String("TargetFramework");
      var project = ProjectCreator.Templates.SdkCsproj(
        csprojPath.ToString(),
        targetFramework: targetFramework);

      using (new FileScope(project))
      {
        //WHEN
        var csharpProjectDto = ReadCSharpProjectFrom(csprojPath);

        //THEN
        csharpProjectDto.TargetFramework.Should().Be(targetFramework);
      }
    }

    //bug tests for missing fields

    private static AbsoluteFilePath CsProjPathTo(string csprojName)
    {
      return AbsoluteFilePath.Value(Path.GetFullPath(csprojName+".csproj"));
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
