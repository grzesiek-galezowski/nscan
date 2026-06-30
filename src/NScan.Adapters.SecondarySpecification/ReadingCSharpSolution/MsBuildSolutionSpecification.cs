using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AtmaFileSystem;
using Microsoft.Build.Utilities.ProjectCreation;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using TddXt.AnyRoot.Strings;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution;

public class MsBuildSolutionSpecification : INScanSupport
{
  [Fact]
  public async Task ShouldLoadProjectsFromSlnxFile()
  {
    //GIVEN
    var projectName = Any.AlphaString();
    var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    Directory.CreateDirectory(tempDir);
    try
    {
      var projectDir = Path.Combine(tempDir, projectName);
      Directory.CreateDirectory(projectDir);
      ProjectCreator.Templates.SdkCsproj(
        Path.Combine(projectDir, projectName + ".csproj")).Save();

      var slnxContent = $"<Solution><Project Path=\"{projectName}/{projectName}.csproj\" /></Solution>";
      var slnxPath = Path.Combine(tempDir, "Test.slnx");
      await File.WriteAllTextAsync(slnxPath, slnxContent);

      //WHEN
      var solution = await MsBuildSolution.FromAsync(AnyFilePath.Value(slnxPath), this, CancellationToken.None);
      var projects = solution.LoadCsharpProjects();

      //THEN
      projects.Single().AssemblyName.Should().Be(projectName);
    }
    finally
    {
      Directory.Delete(tempDir, true);
    }
  }

  [Fact]
  public async Task ShouldLoadMultipleProjectsFromSlnxFile()
  {
    //GIVEN
    var projectNameA = Any.AlphaString();
    var projectNameB = Any.AlphaString();
    var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    Directory.CreateDirectory(tempDir);
    try
    {
      foreach (var name in new[] { projectNameA, projectNameB })
      {
        var dir = Path.Combine(tempDir, name);
        Directory.CreateDirectory(dir);
        ProjectCreator.Templates.SdkCsproj(Path.Combine(dir, name + ".csproj")).Save();
      }

      var slnxContent = $"""
        <Solution>
          <Project Path="{projectNameA}/{projectNameA}.csproj" />
          <Project Path="{projectNameB}/{projectNameB}.csproj" />
        </Solution>
        """;
      var slnxPath = Path.Combine(tempDir, "Test.slnx");
      await File.WriteAllTextAsync(slnxPath, slnxContent);

      //WHEN
      var solution = await MsBuildSolution.FromAsync(AnyFilePath.Value(slnxPath), this, CancellationToken.None);
      var projects = solution.LoadCsharpProjects();

      //THEN
      projects.Select(p => p.AssemblyName).Should().BeEquivalentTo(projectNameA, projectNameB);
    }
    finally
    {
      Directory.Delete(tempDir, true);
    }
  }

  [Fact]
  public async Task ShouldThrowForUnsupportedFileExtension()
  {
    //GIVEN
    var badPath = Path.Combine(Path.GetTempPath(), "solution.xyz");

    //WHEN
    var act = () => MsBuildSolution.FromAsync(AnyFilePath.Value(badPath), this, CancellationToken.None);

    //THEN
    await act.Should().ThrowAsync<ArgumentException>();
  }

#pragma warning disable xUnit1013
  public void Report(Exception exceptionFromResolution) { }
  public void SkippingProjectBecauseOfError(InvalidOperationException e, AbsoluteFilePath path) { }
  public void Log(IndependentRuleComplementDto dto) { }
  public void Log(CorrectNamespacesRuleComplementDto dto) { }
  public void Log(NoCircularUsingsRuleComplementDto dto) { }
  public void Log(HasAttributesOnRuleComplementDto dto) { }
  public void Log(HasTargetFrameworkRuleComplementDto dto) { }
  public void Log(NoUsingsRuleComplementDto dto) { }
  public void Log(HasPropertyRuleComplementDto dto) { }
#pragma warning restore xUnit1013
}
