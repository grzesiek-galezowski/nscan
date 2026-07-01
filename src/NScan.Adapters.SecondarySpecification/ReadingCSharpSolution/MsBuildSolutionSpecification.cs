using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using Microsoft.Build.Utilities.ProjectCreation;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Serializer;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;
using NScan.SharedKernel.NotifyingSupport.Ports;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution;

public class MsBuildSolutionSpecification : INScanSupport
{
  [Fact]
  public async Task ShouldLoadProjectsFromSlnxFile()
  {
    //GIVEN
    var projectName = Any.AlphaString();
    var tempDirScope = new TempSubdirectoryScope();
    var tempDir = tempDirScope.TempDir;
    var slnxPath = tempDir.AddFileName("Test.slnx");
    var projectDir = tempDir.AddDirectoryName(projectName);
    ProjectCreator.Templates.SdkCsproj(
      projectDir.AddFileName(projectName + ".csproj").ToString()).Save();

    var slnModel = new SolutionModel();
    slnModel.AddProject($"{projectName}/{projectName}.csproj");
    await SolutionSerializers.SlnXml.SaveAsync(slnxPath.ToString(), slnModel, CancellationToken.None);

    //WHEN
    var solution = await MsBuildSolution.From(slnxPath.AsAnyFilePath(), this, CancellationToken.None);
    var projects = solution.LoadCsharpProjects();

    //THEN
    projects.Single().AssemblyName.Should().Be(projectName);
  }

  [Fact]
  public async Task ShouldLoadMultipleProjectsFromSlnxFile()
  {
    //GIVEN
    using var tempDirScope = new TempSubdirectoryScope();
    var tempDir = tempDirScope.TempDir;
    var projectNameA = Any.AlphaString();
    var projectNameB = Any.AlphaString();
    var slnxPath = tempDir.AddFileName("Test.slnx");
    var projectADir = tempDir.AddDirectoryName(projectNameA);
    var projectBDir = tempDir.AddDirectoryName(projectNameB);
    ProjectCreator.Templates.SdkCsproj(projectADir.AddFileName(projectNameA + ".csproj").ToString()).Save();
    ProjectCreator.Templates.SdkCsproj(projectBDir.AddFileName(projectNameB + ".csproj").ToString()).Save();

    var slnModel = new SolutionModel();
    slnModel.AddProject($"{projectNameA}/{projectNameA}.csproj");
    slnModel.AddProject($"{projectNameB}/{projectNameB}.csproj");
    await SolutionSerializers.SlnXml.SaveAsync(slnxPath.ToString(), slnModel, CancellationToken.None);

    //WHEN
    var solution = await MsBuildSolution.From(slnxPath.AsAnyFilePath(), this, CancellationToken.None);
    var projects = solution.LoadCsharpProjects();

    //THEN
    projects.Select(p => p.AssemblyName).Should().BeEquivalentTo(projectNameA, projectNameB);
  }

  [Fact]
  public async Task ShouldThrowForUnsupportedFileExtension()
  {
    //GIVEN
    var badPath = Path.Combine(Path.GetTempPath(), "solution.xyz");

    //WHEN
    var act = () => MsBuildSolution.From(AnyFilePath.Value(badPath), this, CancellationToken.None);

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

public class TempSubdirectoryScope : IDisposable
{
  private readonly AbsoluteDirectoryPath _tempDir;
  public TempSubdirectoryScope()
  {
    _tempDir = TempDirectory.CreateTempSubdirectory();
  }
  public AbsoluteDirectoryPath TempDir => _tempDir;
  public void Dispose()
  {
    _tempDir.Delete(true);
  }
}
