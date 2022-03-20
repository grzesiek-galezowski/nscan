using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AtmaFileSystem;
using Core.NullableReferenceTypesExtensions;
using FluentAssertions;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Utilities.ProjectCreation;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;
using Project = Microsoft.Build.Evaluation.Project;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution
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
            ImmutableDictionary<string, string>.Empty, 
            ImmutableList<PackageReference>.Empty, 
            ImmutableList<AssemblyReference>.Empty, 
            ImmutableList<ProjectId>.Empty), 
          options => options
            .WithTracing()
            .ComparingByMembers<CsharpProjectDto>()
            .Excluding(dto => dto.Properties));
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

    [Fact]
    public void ShouldExposePropertiesAsDictionary()
    {
      //GIVEN
      var csprojName = Any.String();
      var csprojPath = CsProjPathTo(csprojName);
      var targetFramework = Any.String("TargetFramework");
      var outputType = Any.String();

      var outputPath = Any.String();
      var assemblyName = Any.String();
      var rootNamespace = Any.String();
      var packAsTool = Any.String();
      var toolCommandName = Any.String();
      var langVersion = Any.String();
      var nullable = Any.String();
      var warningsAsErrors = Any.String();
      var packageRequiresLicenseAcceptance = Any.String();
      var packageId = Any.String();
      var authors = Any.String();
      var product = Any.String();
      var description = Any.String();
      var packageLicenseFile = Any.String();
      var packageProjectUrl = Any.String();
      var packageIconUrl = Any.String();
      var repositoryUrl = Any.String();
      var repositoryType = Any.String();
      var packageTags = Any.String();
      var packageReleaseNotes = Any.String();
      var generatePackageOnBuild = Any.String();
      var assemblyOriginatorKeyFile = Any.String();

      var globalProperties = new Dictionary<string, string>()
      {
        ["AssemblyName"] = assemblyName,
        ["RootNamespace"] = rootNamespace,
        ["PackAsTool"] = packAsTool,
        ["ToolCommandName"] = toolCommandName,
        ["LangVersion"] = langVersion,
        ["Nullable"] = nullable,
        ["WarningsAsErrors"] = warningsAsErrors,
        ["PackageRequireLicenseAcceptance"] = packageRequiresLicenseAcceptance,
        ["PackageId"] = packageId,
        ["Authors"] = authors,
        ["Product"] = product,
        ["Description"] = description,
        ["OutputPath"] = outputPath,
        ["PackageLicenseFile"] = packageLicenseFile,
        ["PackageProjectUrl"] = packageProjectUrl,
        ["PackageIconUrl"] = packageIconUrl,
        ["RepositoryUrl"] = repositoryUrl,
        ["RepositoryType"] = repositoryType,
        ["PackageTags"] = packageTags,
        ["PackageReleaseNotes"] = packageReleaseNotes,
        ["GeneratePackageOnBuild"] = generatePackageOnBuild,
        ["AssemblyOriginatorKeyFile"] = assemblyOriginatorKeyFile,
      };
      var project = ProjectCreator.Templates.SdkCsproj(
        csprojPath.ToString(),
        targetFramework: targetFramework,
        outputType: outputType);
      
      foreach (var (key, value) in globalProperties)
      {
        project.Property(key, value);
      }

      using (new FileScope(project))
      {
        //WHEN
        var csharpProjectDto = ReadCSharpProjectFrom(csprojPath);

        //THEN
        csharpProjectDto.Properties.Should().BeEquivalentTo(globalProperties
          .Concat(new Dictionary<string, string>
          {
            ["OutputType"] = outputType,
            ["TargetFramework"] = targetFramework,
          }));
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

    public void Log(HasPropertyRuleComplementDto dto)
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

  public class MsBuildPlayground
  {
    [Fact]
    public void Lol()
    {
      SetMsBuildExePath();
      var projectRootElement = ProjectRootElement.Open("C:\\Users\\HYPERBOOK\\Documents\\GitHub\\cabs-refactored-csharp\\src\\CabsTests\\CabsTests.csproj");
      
      var project = new Project(projectRootElement);
      //bug foreach (var projectAllEvaluatedProperty in project.AllEvaluatedProperties)
      //bug {
      //bug   if (projectAllEvaluatedProperty is { } p)
      //bug   {
      //bug     Console.WriteLine(p.Name + " " + p.EvaluatedValue);
      //bug   }
      //bug }
      Console.WriteLine("============ FILES ==========");
      foreach (var projectItem in project.Items.Where(item => item.ItemType == "Compile"))
      {
        Console.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType +" " + projectItem.EvaluatedInclude + " " + MetadataString(projectItem));
      }

      Console.WriteLine("============ PACKAGE REFERENCES ==========");
      foreach (var projectItem in project.Items.Where(item => item.ItemType == "PackageReference"))
      {
        Console.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType +" " + projectItem.EvaluatedInclude + " " + MetadataString(projectItem));
      }

      Console.WriteLine("============ PROJECT REFERENCES ==========");
      foreach (var projectItem in project.Items.Where(item => item.ItemType == "ProjectReference"))
      {
        Console.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType +" " + projectItem.EvaluatedInclude + " " + MetadataString(projectItem));
      }

      Console.WriteLine("============ PROJECT REFERENCES ==========");
      foreach (var projectItem in project.Items.Where(item => item.ItemType == "AssemblyReference"))
      {
        Console.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType +" " + projectItem.EvaluatedInclude + " " + MetadataString(projectItem));
      }

      Console.WriteLine("============ PROJECT PROPERTIES ==========");
      foreach (var projectItem in project.Properties)
      {
        Console.WriteLine(projectItem.GetType() + " " + projectItem.Name +" " + projectItem.EvaluatedValue);
      }
    }

    private string MetadataString(ProjectItem projectItem)
    {
      return string.Join('|', projectItem.DirectMetadata.Select(md => md.Name + ":" + md.EvaluatedValue));
    }

    private static void SetMsBuildExePath()
    {
      try
      {
        var startInfo = new ProcessStartInfo("dotnet", "--list-sdks") { RedirectStandardOutput = true };
      

      var process = Process.Start(startInfo).OrThrow();
      process.WaitForExit(1000);

      var output = process.StandardOutput.ReadToEnd();
      var sdkPaths = Regex.Matches(output, "([0-9]+.[0-9]+.[0-9]+) \\[(.*)\\]")
        .OfType<Match>()
        .Select(m => System.IO.Path.Combine(m.Groups[2].Value, m.Groups[1].Value, "MSBuild.dll"));

      var sdkPath = sdkPaths.Last();
      Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", sdkPath, EnvironmentVariableTarget.Process);
    }
    catch (Exception exception)
    {
      Console.WriteLine("Could not set MSBUILD_EXE_PATH: " + exception);
    }
  }
  }
}
