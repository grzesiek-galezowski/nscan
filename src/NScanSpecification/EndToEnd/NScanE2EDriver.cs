﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fclp.Internals.Extensions;
using FluentAssertions;
using FluentAssertions.Numeric;
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
    private readonly string _fullRulesPath;
    private const string RulesFileName = "rules.config";
    private readonly List<(string, string)> _projectReferences = new List<(string, string)>();

    public NScanE2EDriver()
    {
      _fullSolutionPath = Path.Combine(_solutionDir.FullName, _solutionName + ".sln");
      _fullRulesPath = Path.Combine(_solutionDir.FullName, RulesFileName);
    }

    public E2EProjectDsl HasProject(string projectName)
    {
      _projects.Add(projectName);
      return new E2EProjectDsl(projectName, _projectReferences);
    }

    public void AddIndependentOfProjectRule(string projectName, string dependencyProjectName)
    {
      _rules.Add(new RuleDto()
      {
        DependingPattern = Pattern.WithoutExclusion(projectName),
        DependencyPattern = new Glob(dependencyProjectName),
        DependencyType = "project",
        RuleName = "independentOf"
      });

    }

    public void PerformAnalysis()
    {
      CreateSolution();
      CreateAllProjects();
      AddProjectsReferences();
      AddAllProjectsToSolution();
      CreateRulesFile();
      RunAnalysis();
    }



    public void ReportShouldContainText(string ruleText)
    {
      String.Join(Environment.NewLine, _analysisResult.StandardOutput).Should().Contain(ruleText);
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
      return $"{r.DependingPattern.Description()} {r.RuleName} {r.DependencyType}:{r.DependencyPattern.Pattern}";
    }

    private void AddAllProjectsToSolution()
    {
      AssertSuccess(
        RunDotNetExe($"sln {_solutionName}.sln add {string.Join(" ", _projects.Select(s => s + ".csproj"))}")
          .Result);
    }

    private void CreateAllProjects()
    {
      _projects.AsParallel().ForAll(CreateProjectAsync);
    }

    private void AddProjectsReferences()
    {
      _projectReferences.AsParallel().ForAll(AddReferenceAsync);
    }

    private void AddReferenceAsync((string dependent, string dependency) obj)
    {
      AssertSuccess(RunDotNetExe($"add " +
                                 $"{obj.dependent}.csproj " +
                                 $"reference " +
                                 $"{obj.dependency}.csproj").Result);
    }

    private void CreateProjectAsync(string projectName)
    {
      AssertSuccess(
        RunDotNetExe($"new classlib --name {Path.Combine(_solutionDir.FullName, projectName)}")
          .Result);
    }

    private void CreateSolution()
    {
      AssertSuccess(RunDotNetExe($"new sln --name {_solutionName}").Result);
    }

    private async Task<ProcessResults> RunDotNetExe(string arguments)
    {
      var processInfo = await ProcessEx.RunAsync(
        new ProcessStartInfo("dotnet.exe", arguments)
        {
          WorkingDirectory = _solutionDir.FullName,
        }).ConfigureAwait(false);
      return processInfo;
    }

    private static void AssertSuccess(ProcessResults processInfo)
    {
      processInfo.ExitCode.Should().Be(0, string.Join(Environment.NewLine, processInfo.StandardError));
    }

    private static DirectoryInfo GetTemporaryDirectory()
    {
      var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(tempDirectory);
      return new DirectoryInfo(tempDirectory);
    }

  }

  public class E2EProjectDsl
  {
    private readonly string _projectName;
    private readonly List<(string, string)> _assemblyReferences;

    public E2EProjectDsl(string projectName, List<(string, string)> assemblyReferences)
    {
      _projectName = projectName;
      _assemblyReferences = assemblyReferences;
    }

    public void WithAssemblyReferences(params string[] assemblyNames)
    {
      foreach (var assemblyName in assemblyNames)
      {
        _assemblyReferences.Add((_projectName, assemblyName));
      }
    }
  }
}