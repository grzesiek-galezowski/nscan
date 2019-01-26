using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using RunProcessAsTask;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class NScanE2EDriver : IDisposable
  {
    private readonly List<string> _projects = new List<string>();
    private readonly DirectoryInfo _solutionDir = TemporaryDirectory.CreateNew();
    private readonly string _solutionName = Root.Any.AlphaString();

    private ProcessResults _analysisResult;
    private readonly string _fullSolutionPath;
    private readonly string _fullRulesPath;
    private const string RulesFileName = "rules.config";
    private readonly ProjectFiles _projectFiles;
    private readonly AssemblyReferences _references;
    private readonly DotNetExe _dotNetExe;
    private readonly Rules _rules;

    public NScanE2EDriver()
    {
      _fullSolutionPath = Path.Combine(_solutionDir.FullName, _solutionName + ".sln");
      _fullRulesPath = Path.Combine(_solutionDir.FullName, RulesFileName);
      _projectFiles = new ProjectFiles(_solutionDir);
      _dotNetExe = new DotNetExe(_solutionDir);
      _references = new AssemblyReferences(_dotNetExe);
      _rules = new Rules();
    }

    public E2EProjectDsl HasProject(string projectName)
    {
      _projects.Add(projectName);
      return new E2EProjectDsl(projectName, _projectFiles, _references);
    }

    public void Add(IFullRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition);
    }


    public void PerformAnalysis()
    {
      CreateSolution();
      CreateAllProjects();
      _references.AddToProjects();
      AddAllProjectsToSolution();
      _projectFiles.AddFilesToProjects();
      _rules.SaveIn(_fullRulesPath);
      RunAnalysis();
    }


    public void ReportShouldContainText(string ruleText)
    {
      ConsoleStandardOutput().Should()
        .Contain(ruleText,
          ConsoleStandardOutputAndErrorString());
    }

    public void ReportShouldNotContainText(string text)
    {
      ConsoleStandardOutput().Should().NotContain(text,
          ConsoleStandardOutputAndErrorString());

    }

    private string ConsoleStandardOutput()
    {
      return string.Join(Environment.NewLine, _analysisResult.StandardOutput);
    }

    private string ConsoleStandardOutputAndErrorString()
    {
      return string.Join(Environment.NewLine, _analysisResult.StandardOutput.Concat(_analysisResult.StandardError));
    }



    public void ShouldIndicateSuccess()
    {
      _analysisResult.ExitCode.Should().Be(0);
    }

    public void ShouldIndicateFailure()
    {
      _analysisResult.ExitCode.Should().Be(-1);
    }


    public void Dispose()
    {
      _solutionDir.Delete(true);
    }

    private void RunAnalysis()
    {
      //RunForDebug();

      var repositoryPath = RepositoryPath();
      AssertDirectoryExists(repositoryPath);

      var nscanConsoleProjectPath = Path.Combine(
        repositoryPath, "src", "NScan.Console", "NScan.Console.csproj");
      
      AssertFileExists(nscanConsoleProjectPath);

      _analysisResult = _dotNetExe.RunWith($"run --project {nscanConsoleProjectPath} -- -p \"{_fullSolutionPath}\" -r \"{_fullRulesPath}\"").Result;
    }

    private void RunForDebug() //todo expand on this ability. This may be interesting if there's a good way to capture console output or when I add logging to a file
    {
      var repositoryPath2 = RepositoryPath();
      var nscanConsoleDllPath = Path.Combine(
        repositoryPath2, "src", "NScan.Console", "bin", "Debug", "netcoreapp2.1", "NScan.Console.dll");

      AssertFileExists(nscanConsoleDllPath);
      AppDomain.CurrentDomain.ExecuteAssembly(nscanConsoleDllPath,
        new[] {"-p", _fullSolutionPath, "-r", _fullRulesPath});
    }

    private void AssertFileExists(string filePath)
    {
      File.Exists(filePath).Should().BeTrue(filePath + " should exist");
    }

    private static void AssertDirectoryExists(string directoryPath)
    {
      Directory.Exists(directoryPath).Should().BeTrue(directoryPath + " should exist");
    }

    private static string RepositoryPath()
    {
      if (NCrunch.RunsThisTest())
      {
        var originalSolutionPath = NCrunch.OriginalSolutionPath();
        return Path.Combine(originalSolutionPath.Split("nscan").First(), "nscan");
      }
      else
      {
        var executingAssemblyPath = new FileInfo(
            Assembly.GetExecutingAssembly().EscapedCodeBase.Split("file:///").ToArray()[1]).Directory;
        while (!Directory.EnumerateDirectories(executingAssemblyPath.FullName).Any(s => s.EndsWith(".git")))
        {
          executingAssemblyPath = executingAssemblyPath.Parent;
        }

        return executingAssemblyPath.FullName;
      }

    }

    private void AddAllProjectsToSolution()
    {
      ProcessAssertions.AssertSuccess(
        _dotNetExe.RunWith($"sln {_solutionName}.sln add {string.Join(" ", _projects)}")
          .Result);
    }

    private void CreateAllProjects()
    {
      _projects.AsParallel().ForAll(CreateProjectAsync);
    }

    private void CreateProjectAsync(string projectName)
    {
      var projectDirPath = Path.Combine(_solutionDir.FullName, projectName);
      ProcessAssertions.AssertSuccess(
        _dotNetExe.RunWith($"new classlib --name {projectName}")
          .Result);
      RemoveDefaultFileCreatedbyTemplate(projectDirPath);
    }

    private static void RemoveDefaultFileCreatedbyTemplate(string projectDirPath)
    {
      File.Delete(Path.Combine(projectDirPath, "Class1.cs"));
    }

    private void CreateSolution()
    {
      ProcessAssertions.AssertSuccess(_dotNetExe.RunWith($"new sln --name {_solutionName}").Result);
    }

    public void ReportShouldContain(ReportedMessage reportedMessage)
    {
      ReportShouldContainText(reportedMessage.ToString());
    }
  }

  public class DotNetExe
  {
    private readonly DirectoryInfo _workingDirectory;

    public DotNetExe(DirectoryInfo workingDirectory)
    {
      _workingDirectory = workingDirectory;
    }

    public async Task<ProcessResults> RunWith(string arguments)
    {
      var processInfo = await ProcessEx.RunAsync(
        new ProcessStartInfo("dotnet.exe", arguments)
        {
            
          WorkingDirectory = _workingDirectory.FullName,
        }).ConfigureAwait(false);
      return processInfo;
    }
  }
}