using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using RunProcessAsTask;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class NScanE2EDriver : IDisposable
  {
    private readonly DirectoryInfo _solutionDir = TemporaryDirectory.CreateNew();
    private readonly string _solutionName = Any.AlphaString();

    private ProcessResults _analysisResult;
    private readonly string _fullSolutionPath;
    private readonly string _fullRulesPath;
    private const string RulesFileName = "rules.config";
    private readonly ProjectFiles _projectFiles;
    private readonly AssemblyReferences _references;
    private readonly DotNetExe _dotNetExe;
    private readonly Rules _rules;
    private readonly ProjectsCollection _projectsCollection;

    public NScanE2EDriver()
    {
      _fullSolutionPath = Path.Combine(_solutionDir.FullName, _solutionName + ".sln");
      _fullRulesPath = Path.Combine(_solutionDir.FullName, RulesFileName);
      _projectFiles = new ProjectFiles(_solutionDir);
      _dotNetExe = new DotNetExe(_solutionDir);
      _references = new AssemblyReferences(_dotNetExe);
      _rules = new Rules();
      _projectsCollection = new ProjectsCollection(_dotNetExe);
    }

    public E2EProjectDsl HasProject(string projectName)
    {
      _projectsCollection.Add(projectName);
      return new E2EProjectDsl(projectName, _projectFiles, _references);
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

    private void CreateSolution()
    {
      ProcessAssertions.AssertSuccess(_dotNetExe.RunWith($"new sln --name {_solutionName}").Result);
    }

    public void ReportShouldContain(ReportedMessage reportedMessage)
    {
      ReportShouldContainText(reportedMessage.ToString());
    }
  }
}