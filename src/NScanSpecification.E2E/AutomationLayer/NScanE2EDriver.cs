﻿using System;
using System.Threading.Tasks;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using FluentAssertions;
using NScanSpecification.Lib.AutomationLayer;
using TddXt.AnyRoot.Strings;
using Xunit.Abstractions;
using static AtmaFileSystem.AtmaFileSystemPaths;
using static NScanSpecification.E2E.AutomationLayer.BuildEnvironment;
using static TddXt.AnyRoot.Root;
using AbsoluteFilePath = AtmaFileSystem.AbsoluteFilePath;

namespace NScanSpecification.E2E.AutomationLayer
{
  public sealed class NScanE2EDriver : IDisposable
  {
    private readonly string _solutionName = Any.AlphaString();

    private readonly AbsoluteFilePath _fullFixtureSolutionPath;
    private readonly AbsoluteFilePath _fullFixtureRulesPath;
    private static readonly FileName RulesFileName = FileName("rules.config");
    private readonly ProjectFiles _projectFiles;
    private readonly AssemblyReferences _references;
    private readonly DotNetExe _dotNetExe;
    private readonly Rules _rules;
    private readonly ProjectsCollection _projectsCollection;
    private readonly AnalysisResult _analysisResult;
    private readonly SolutionDir _fixtureSolutionDir;
    private static readonly Lazy<Task> BuildNScanConsoleOnce = new Lazy<Task>(BuildNScanConsole);

    public NScanE2EDriver(ITestOutputHelper output)
    {
      ITestSupport testSupport = new ConsoleXUnitTestSupport(output);
      _fixtureSolutionDir = RelevantPaths.CreateHomeForFixtureSolution(_solutionName);
      _fullFixtureSolutionPath = _fixtureSolutionDir.SolutionFilePath();
      _fullFixtureRulesPath = _fixtureSolutionDir.PathToFile(RulesFileName);
      _projectFiles = new ProjectFiles(_fixtureSolutionDir);
      _dotNetExe = new DotNetExe(_fixtureSolutionDir, testSupport);
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
      await CreateSolution();
      await _projectsCollection.CreateOnDisk(_fixtureSolutionDir, _dotNetExe);
      _references.AddToProjects();
      await _projectsCollection.AddToSolutionAsync(_solutionName);
      _projectFiles.AddFilesToProjects();
      await _rules.SaveIn(_fullFixtureRulesPath);
      await BuildNScanConsoleOnce.Value;
      await RunAnalysis();
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
    }

    private async Task RunAnalysis()
    {
      var nscanBinaryPath = BuildOutputDirectory().AddFileName("NScan.Console.dll");
      
      AssertFileExists(nscanBinaryPath);
      AssertFileExists(_fullFixtureSolutionPath);
      AssertFileExists(_fullFixtureRulesPath);

      var analysisResultAnalysisResult = await _dotNetExe.RunWith(
        $"{nscanBinaryPath} " +
        $"-p \"{_fullFixtureSolutionPath}\" " +
        $"-r \"{_fullFixtureRulesPath}\"");
      _analysisResult.Assign(analysisResultAnalysisResult);
    }

    private void AssertFileExists(AbsoluteFilePath filePath)
    {
      filePath.Exists().Should().BeTrue(filePath + " should exist");
    }

    private async Task CreateSolution()
    {
      ProcessAssertions.AssertSuccess(await _dotNetExe.RunWith($"new sln --name {_solutionName}"));
    }

    public void ReportShouldContain(ReportedMessage reportedMessage)
    {
      _analysisResult.ReportShouldContainText(reportedMessage.ToString());
    }
  }
}
