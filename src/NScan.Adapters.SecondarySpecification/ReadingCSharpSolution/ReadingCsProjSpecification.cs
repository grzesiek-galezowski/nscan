using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using AtmaFileSystem;
using LanguageExt;
using Microsoft.Build.Construction;
using Microsoft.Build.Utilities.ProjectCreation;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution;

public class ReadingCsProjSpecification : INScanSupport
{

  [Fact]
  public void ShouldBeAbleToCreateDtoFromSdkCsprojDefaultTemplate()
  {
    //GIVEN
    var csprojName = Any.String();
    var csprojPath = CsProjPathTo(csprojName);
    var project = ProjectCreator.Templates.SdkCsproj(
      csprojPath.ToString());

    using (new FileScope(project))
    {
      //WHEN
      var csharpProjectDto = ReadCSharpProjectFrom(csprojPath);

      //THEN
      csharpProjectDto.Should().BeEquivalentTo(
        new CsharpProjectDto(new ProjectId(csprojPath.ToString()),
          csprojName,
          Arr<SourceCodeFileDto>.Empty,
          HashMap<string, string>.Empty,
          Arr<PackageReference>.Empty, 
          Arr<AssemblyReference>.Empty,
          Arr<ProjectId>.Empty,
          Arr.create("netstandard2.0")),
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
      csharpProjectDto.TargetFrameworks.Single().Should().Be(targetFramework);
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
    var outputPath = Any.AlphaString() + "\\";
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

    var globalProperties = new Dictionary<string, string>
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
      csharpProjectDto.Properties.ToDictionary().Should().Contain(globalProperties
        .Concat(new Dictionary<string, string>
        {
          ["OutputType"] = outputType, 
          ["TargetFramework"] = targetFramework,
        }));
    }
  }
  
  [Fact]
  public void ShouldSupportMultipleTargetFrameworks()
  {
    //GIVEN
    var csprojName = Any.String();
    var csprojPath = CsProjPathTo(csprojName);
    var targetFramework1 = Any.String("TargetFramework");
    var targetFramework2 = Any.String("TargetFramework");
    var targetFrameworksString = $"{targetFramework1};{targetFramework2}";

    var globalProperties = new Dictionary<string, string>
    {
      ["TargetFrameworks"] = targetFrameworksString
    };
    var project = ProjectCreator.Templates.SdkCsproj(csprojPath.ToString());
    foreach (var (key, value) in globalProperties)
    {
      project.Property(key, value);
    }

    using (new FileScope(project))
    {
      //WHEN
      var csharpProjectDto = ReadCSharpProjectFrom(csprojPath);

      //THEN
      csharpProjectDto.Properties.ToDictionary().Should().Contain(globalProperties
        .Concat(new Dictionary<string, string>
        {
          ["TargetFrameworks"] = targetFrameworksString,
        }));
      csharpProjectDto.TargetFrameworks.ToList().Should().Equal(targetFramework1, targetFramework2);

    }
  }

  [Fact]
  public void ShouldReadReferences()
  {
    //GIVEN
    var csprojName = Any.String();
    var csprojPath = CsProjPathTo(csprojName);
    var targetFramework = Any.String("TargetFramework");
    var outputType = Any.String();
    var packageName = Any.AlphaString();
    var packageVersion = Any.String();
    var assemblyHintPath = Any.AlphaString();
    var projectDependencyName = Any.String();

    var project = ProjectCreator.Templates.SdkCsproj(
      csprojPath.ToString(),
      targetFramework: targetFramework,
      outputType: outputType);
    project.ItemInclude("AssemblyReference", "MyAssembly",
      metadata: new Dictionary<string, string?> { ["HintPath"] = assemblyHintPath });
    project.ItemInclude("PackageReference", packageName,
      metadata: new Dictionary<string, string?> { ["Version"] = packageVersion });
    project.ItemInclude("ProjectReference", projectDependencyName);

    using (new FileScope(project))
    {
      //WHEN
      var csharpProjectDto = ReadCSharpProjectFrom(csprojPath);

      //THEN
      csharpProjectDto.AssemblyReferences.ToList().Should().Equal(
        new AssemblyReference("MyAssembly", assemblyHintPath));
      csharpProjectDto.PackageReferences.ToList().Should().Equal(
        new PackageReference(packageName, packageVersion));
      csharpProjectDto.ReferencedProjectIds.ToList().Should().Equal(
        new ProjectId(csprojPath.ParentDirectory().AddFileName(projectDependencyName).ToString()));
    }
  }
  
  [Fact]
  public void ShouldReadOneSpecificFileThatWasCausingIssuesInE2eTests()
  {
    //GIVEN
    var csprojName = Any.String();
    var csprojPath = CsProjPathTo(csprojName);

    using var xmlTextReader = new XmlTextReader(new StringReader(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
      new XElement("ItemGroup",
        new XElement("ProjectReference",
          new XAttribute("Include",
            "..\\0acc9ee5-560e-45a8-baee-0e5d5d42b2ca\\0acc9ee5-560e-45a8-baee-0e5d5d42b2ca.csproj"))),
      new XElement("PropertyGroup", new XElement("TargetFramework", "netstandard2.1"))).ToString()));

    var projectRootElement = ProjectRootElement.Create(xmlTextReader);
    projectRootElement.FullPath = csprojPath.ToString();
    using (new FileScope(projectRootElement))
    {
      //WHEN
      var csharpProjectDto = ReadCSharpProjectFrom(csprojPath);

      //THEN
      csharpProjectDto.ReferencedProjectIds.ToList().Should().Equal(
        new ProjectId(
          csprojPath
            .ParentDirectory()
            .ParentDirectory().Value()
            .AddDirectoryName("0acc9ee5-560e-45a8-baee-0e5d5d42b2ca")
            .AddFileName("0acc9ee5-560e-45a8-baee-0e5d5d42b2ca.csproj").ToString()));
    }
  }

  private static AbsoluteFilePath CsProjPathTo(string csprojName)
  {
    return AbsoluteDirectoryPath.Value(Path.GetTempPath()).AddDirectoryName(Guid.NewGuid().ToString("N"))
      .AddFileName(csprojName + ".csproj");
  }

  private CsharpProjectDto ReadCSharpProjectFrom(AbsoluteFilePath absoluteFilePath)
  {
    return new MsBuildSolution(new[] { absoluteFilePath, }, this).LoadCsharpProjects().Single();
  }

#pragma warning disable xUnit1013
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
#pragma warning restore xUnit1013
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
