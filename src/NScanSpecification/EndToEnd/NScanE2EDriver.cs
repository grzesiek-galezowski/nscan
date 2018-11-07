using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using GlobExpressions;
using RunProcessAsTask;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.CompositionRoot;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class NScanE2EDriver : IDisposable
  {
    private readonly List<string> _projects = new List<string>();
    private readonly DirectoryInfo _solutionDir = GetTemporaryDirectory();
    private readonly string _solutionName = Root.Any.AlphaString();
    private readonly List<RuleDto> _rules = new List<RuleDto>();

    private ProcessResults _analysisResult;
    private readonly string _fullSolutionPath;
    private string _fullRulesPath;

    public NScanE2EDriver()
    {
      _fullSolutionPath = Path.Combine(_solutionDir.FullName, _solutionName + ".sln");
      _fullRulesPath = Path.Combine(_solutionDir.FullName, RulesFileName);
    }

    private const string RulesFileName = "rules.config";

    public static DirectoryInfo GetTemporaryDirectory()
    {
      var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(tempDirectory);
      return new DirectoryInfo(tempDirectory);
    }

    public void HasProject(string projectName)
    {
      _projects.Add(projectName);
    }

    public void AddIndependentOfAssemblyRule(string projectName, string assemblyName)
    {
      _rules.Add(new RuleDto()
      {
        DependingPattern = new Glob(projectName),
        DependencyPattern = new Glob(assemblyName),
        DependencyType = "assembly",
        RuleName = "independentOf"
      });

    }

    public void PerformAnalysis()
    {
      CreateSolutionAsync().Wait();
      CreateAllProjects();
      AddAllProjectsToSolution();
      CreateRulesFile();
      RunAnalysis();
    }

    private void RunAnalysis()
    {
      string nscanConsoleProjectPath = "C:\\Users\\grzes\\Documents\\GitHub\\nscan\\src\\NScan.Console\\NScan.Console.csproj";
      _analysisResult = RunDotNetExe($"run --project {nscanConsoleProjectPath} -- -p {_fullSolutionPath} -r {_fullRulesPath}").Result;
    }

    private void CreateRulesFile()
    {
      var lines = _rules.Select(ToRuleString)
        .ToList();
      File.WriteAllLines(_fullRulesPath, lines);
    }

    private static string ToRuleString(RuleDto r)
    {
      return $"{r.DependingPattern.Pattern} {r.RuleName} {r.DependencyType}:{r.DependencyPattern.Pattern}";
    }

    private void AddAllProjectsToSolution()
    {
      RunDotNetExe($"sln {_solutionName}.sln add {string.Join(" ", _projects.Select(s => s + ".csproj"))}")
        .Wait();
    }

    private void CreateAllProjects()
    {
      _projects.AsParallel().ForAll(CreateProjectAsync);
    }

    private void CreateProjectAsync(string projectName)
    {
      RunDotNetExe($"new classlib --name {Path.Combine(_solutionDir.FullName, projectName)}").Wait();
    }

    private Task CreateSolutionAsync()
    {
      return RunDotNetExe($"new sln --name {_solutionName}");
    }

    private async Task<ProcessResults> RunDotNetExe(string arguments)
    {
      var processInfo = await ProcessEx.RunAsync(
        new ProcessStartInfo("dotnet.exe", arguments)
        {
          WorkingDirectory = _solutionDir.FullName,
        }).ConfigureAwait(false);
      processInfo.ExitCode.Should().Be(0, string.Join(Environment.NewLine, processInfo.StandardError));
      return processInfo;
    }

    public void ReportShouldContainText(string ruleText)
    {
      _analysisResult.StandardOutput.Should().Contain(ruleText);
    }

    public void ShouldIndicateSuccess()
    {
      _analysisResult.ExitCode.Should().Be(0);
    }

    public void Dispose()
    {
      _solutionDir.Delete(true);
    }
  }
}