using System;
using System.IO;
using AtmaFileSystem;
using FluentAssertions;
using NScanSpecification.Lib.AutomationLayer;
using TddXt.AnyRoot.Strings;
using static AtmaFileSystem.AtmaFileSystemPaths;
using static TddXt.AnyRoot.Root;
using AbsoluteFilePath = AtmaFileSystem.AbsoluteFilePath;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class NScanE2EDriver : IDisposable
  {
    private readonly string _solutionName = Any.AlphaString();

    private readonly AbsoluteFilePath _fullSolutionPath;
    private readonly AbsoluteFilePath _fullRulesPath;
    private static readonly FileName RulesFileName = FileName("rules.config");
    private readonly ProjectFiles _projectFiles;
    private readonly AssemblyReferences _references;
    private readonly DotNetExe _dotNetExe;
    private readonly Rules _rules;
    private readonly ProjectsCollection _projectsCollection;
    private readonly AnalysisResult _analysisResult;
    private readonly SolutionDir _solutionDir;

    public NScanE2EDriver()
    {
      _solutionDir = new SolutionDir(RelevantPaths.CreateRandomDirectory(), _solutionName);
      _fullSolutionPath = _solutionDir.SolutionFilePath();
      _fullRulesPath = _solutionDir.PathToFile(RulesFileName);
      _projectFiles = new ProjectFiles(_solutionDir);
      _dotNetExe = new DotNetExe(_solutionDir);
      _references = new AssemblyReferences(_dotNetExe);
      _rules = new Rules();
      _projectsCollection = new ProjectsCollection(_dotNetExe);
      _analysisResult = new AnalysisResult();
    }

    public E2EProjectDsl HasProject(string projectName)
    {
      var projectDefinition = new ProjectDefinition(projectName);
      _projectsCollection.Add(projectDefinition);
      return new E2EProjectDsl(projectName, _projectFiles, _references, projectDefinition);
    }

    public void Add(IFullRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition);
    }


    public void PerformAnalysis()
    {
      CreateSolution();
      _projectsCollection.CreateOnDisk(_solutionDir, _dotNetExe);
      _references.AddToProjects();
      _projectsCollection.AddToSolution(_solutionName);
      _projectFiles.AddFilesToProjects();
      _rules.SaveIn(_fullRulesPath);
      RunAnalysis();
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
      _solutionDir.DeleteWithContent();
    }

    private void RunAnalysis()
    {

      var repositoryPath = RelevantPaths.RepositoryPath();
      AssertDirectoryExists(repositoryPath);

      var nscanConsoleProjectPath = RelevantPaths.NscanConsoleProjectPath(repositoryPath);
      
      AssertFileExists(nscanConsoleProjectPath);

      //RunForDebug();
      var analysisResultAnalysisResult = _dotNetExe.RunWith($"run --project {nscanConsoleProjectPath} -- -p \"{_fullSolutionPath}\" -r \"{_fullRulesPath}\"").Result;
      _analysisResult.Assign(analysisResultAnalysisResult);
    }

    private void RunForDebug() //todo expand on this ability. This may be interesting if there's a good way to capture console output or when I add logging to a file
    {
      var repositoryPath2 = RelevantPaths.RepositoryPath();
      var nscanConsoleDllPath = 
        repositoryPath2 
        + DirectoryName("src") 
        + DirectoryName("NScan.Console") 
        + DirectoryName("bin") 
        + DirectoryName("Debug")
        + DirectoryName("netcoreapp2.1") 
        + FileName("NScan.Console.dll");

      AssertFileExists(nscanConsoleDllPath);
      AppDomain.CurrentDomain.ExecuteAssembly(nscanConsoleDllPath.ToString(),
        new[] {"-p", _fullSolutionPath.ToString(), "-r", _fullRulesPath.ToString()});
    }

    private void AssertFileExists(AbsoluteFilePath filePath)
    {
      File.Exists(filePath.ToString()).Should().BeTrue(filePath + " should exist");
    }

    private static void AssertDirectoryExists(AbsoluteDirectoryPath directoryPath)
    {
      Directory.Exists(directoryPath.ToString()).Should().BeTrue(directoryPath + " should exist");
    }

    private void CreateSolution()
    {
      ProcessAssertions.AssertSuccess(_dotNetExe.RunWith($"new sln --name {_solutionName}").Result);
    }

    public void ReportShouldContain(ReportedMessage reportedMessage)
    {
      _analysisResult.ReportShouldContainText(reportedMessage.ToString());
    }
  }
}