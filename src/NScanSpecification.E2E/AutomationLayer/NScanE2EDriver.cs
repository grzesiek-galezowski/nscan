using System.Text;
using System.Threading;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using AwesomeAssertions;
using TddXt.NScan.Console;
using static AtmaFileSystem.AtmaFileSystemPaths;
using AbsoluteFilePath = AtmaFileSystem.AbsoluteFilePath;

namespace NScanSpecification.E2E.AutomationLayer;

public sealed class NScanE2EDriver : IDisposable
{
  private readonly string _solutionName = Any.AlphaString();

  private readonly AbsoluteFilePath _fullFixtureRulesPath;
  private static readonly FileName RulesFileName = FileName("rules.config");
  private readonly ProjectFiles _projectFiles;
  private readonly ProjectReferences _references;
  private readonly DotNetExe _dotNetExe;
  private readonly Rules _rules;
  private readonly ProjectsCollection _projectsCollection;
  private readonly AnalysisResult _analysisResult;
  private readonly SolutionDir _fixtureSolutionDir;
  private readonly CancellationTokenSource _cts = new();

  public NScanE2EDriver(ITestOutputHelper output)
  {
    ITestSupport testSupport = new ConsoleXUnitTestSupport(output);
    _fixtureSolutionDir = RelevantPaths.CreateHomeForFixtureSolution(_solutionName);
    _fullFixtureRulesPath = _fixtureSolutionDir.PathToFile(RulesFileName);
    _projectFiles = new ProjectFiles(_fixtureSolutionDir);
    _dotNetExe = new DotNetExe(_fixtureSolutionDir, testSupport);
    _references = new ProjectReferences(_dotNetExe);
    _rules = new Rules();
    _projectsCollection = new ProjectsCollection(_dotNetExe, testSupport);
    _analysisResult = new AnalysisResult();
  }

  public E2EProjectDsl HasProject(string projectName)
  {
    var projectDefinition = new ProjectDefinition(projectName);
    _projectsCollection.Add(projectDefinition);
    return new E2EProjectDsl(projectName, _projectFiles, _references, projectDefinition);
  }

  public void Add(IFullDependencyPathRuleConstructed ruleDefinitionBuilder)
  {
    _rules.Add(ruleDefinitionBuilder.Build());
  }
    
  public void Add(IFullNamespaceBasedRuleConstructed ruleDefinitionBuilder)
  {
    _rules.Add(ruleDefinitionBuilder.Build());
  }

  public void Add(IFullProjectScopedRuleConstructed ruleDefinitionBuilder)
  {
    _rules.Add(ruleDefinitionBuilder.Build());
  }


  public async Task PerformAnalysis()
  {
    await SaveNewSolutionOnDisk();
    // After creating the solution, detect which format was used (.sln or .slnx)
    var fullFixtureSolutionPath = _fixtureSolutionDir.SolutionFilePath();
    _references.AddTo(_projectsCollection);
    await _projectsCollection.SaveIn(_fixtureSolutionDir, _cts.Token);
    await _projectsCollection.AddToSolution(fullFixtureSolutionPath, _cts.Token);
    await _projectFiles.AddFilesToProjects(_cts.Token);
    await _rules.SaveIn(_fullFixtureRulesPath);
    RunAnalysis(fullFixtureSolutionPath);
  }

  public void ReportShouldNotContainText(string text)
  {
    _analysisResult.ReportShouldNotContainText(text);
  }

  public void ShouldIndicateSuccess()
  {
    _analysisResult.ShouldIndicateSuccess();
  }

  public void ShouldIndicateFailure()
  {
    _analysisResult.ShouldIndicateFailure();
  }

  public void Dispose()
  {
    _fixtureSolutionDir.DeleteWithContent();
    _cts.Cancel();
  }

  private void RunAnalysis(AbsoluteFilePath fullFixtureSolutionPath)
  {
    AssertFileExists(fullFixtureSolutionPath);
    AssertFileExists(_fullFixtureRulesPath);
    var output = new StringBuilder();
    var resultCode = new Program
    {
      WriteLine = o => output.AppendLine(o.ToString())
    }.ExecuteWith(
      [
        "-p", $"\"{fullFixtureSolutionPath}\"",
        "-r", $"\"{_fullFixtureRulesPath}\""
      ]
    );

    _analysisResult.Assign(resultCode, output.ToString());
  }

  private static void AssertFileExists(AbsoluteFilePath filePath)
  {
    filePath.Exists().Should().BeTrue(filePath + " should exist");
  }

  private async Task SaveNewSolutionOnDisk()
  {
    await _dotNetExe.RunWith($"new sln --name {_solutionName}", _cts.Token);
  }

  public void ReportShouldContain(ReportedMessage reportedMessage)
  {
    _analysisResult.ReportShouldContainText(reportedMessage.ToString());
  }
}
