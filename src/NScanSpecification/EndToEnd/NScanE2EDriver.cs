using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RunProcessAsTask;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.RuleInputData;
using TddXt.NScan.Specification.Component;
using TddXt.NScan.Specification.Component.AutomationLayer;
using TddXt.NScan.Xml;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class NScanE2EDriver : IDisposable
  {
    private readonly List<string> _projects = new List<string>();
    private readonly Dictionary<string, List<XmlSourceCodeFile>> _filesByProject = new Dictionary<string, List<XmlSourceCodeFile>>();
    private readonly DirectoryInfo _solutionDir = GetTemporaryDirectory();
    private readonly string _solutionName = Root.Any.AlphaString();
    private readonly List<RuleUnionDto> _rules = new List<RuleUnionDto>();

    private ProcessResults _analysisResult;
    private readonly string _fullSolutionPath;
    private readonly string _fullRulesPath;
    private const string RulesFileName = "rules.config";
    private readonly List<(string, string)> _projectReferences 
        = new List<(string, string)>();

    public NScanE2EDriver()
    {
      _fullSolutionPath = Path.Combine(_solutionDir.FullName, _solutionName + ".sln");
      _fullRulesPath = Path.Combine(_solutionDir.FullName, RulesFileName);
    }

    public E2EProjectDsl HasProject(string projectName)
    {
      _projects.Add(projectName);
      return new E2EProjectDsl(projectName, _projectReferences, _filesByProject);
    }

    public void Add(IFullRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition.Build());
    }


    public void PerformAnalysis()
    {
      CreateSolution();
      CreateAllProjects();
      AddProjectsReferences();
      AddAllProjectsToSolution();
      AddFilesToProjects();

      CreateRulesFile();
      RunAnalysis();
    }

    private void AddFilesToProjects()
    {
      foreach (var projectName in _filesByProject.Keys)
      {
        foreach (var sourceCodeFile in _filesByProject[projectName])
        {
          var fullFilePath = Path.Combine(Path.Combine(_solutionDir.FullName, projectName), sourceCodeFile.Name);
          var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(fullFilePath));
          if (!directoryInfo.Exists)
          {
            directoryInfo.Create();
          }

          File.WriteAllText(fullFilePath, $"namespace {sourceCodeFile.DeclaredNamespace}" + " {}");
          
        }

      }
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
      var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      var nscanConsoleProjectPath = $"{homeFolder}\\Documents\\GitHub\\nscan\\src\\NScan.Console\\NScan.Console.csproj";
      _analysisResult = RunDotNetExe($"run --project {nscanConsoleProjectPath} -- -p {_fullSolutionPath} -r {_fullRulesPath}").Result;
    }

    private void CreateRulesFile()
    {
      var lines = _rules.Select(dto => dto.Switch(
          independent => ToRuleString(dto.Left),
          correctNamespaces => ToRuleString(dto.Right))
        ).ToList();
      File.WriteAllLines(_fullRulesPath, lines);
    }

    private string ToRuleString(CorrectNamespacesRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    private static string ToRuleString(IndependentRuleComplementDto dto)
    {
      return $"{dto.DependingPattern.Description()} {dto.RuleName} {dto.DependencyType}:{dto.DependencyPattern.Pattern}";
    }

    private void AddAllProjectsToSolution()
    {
      AssertSuccess(
        RunDotNetExe($"sln {_solutionName}.sln add {string.Join(" ", _projects)}")
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
                                 $"{obj.dependent} " +
                                 $"reference " +
                                 $"{obj.dependency}").Result);
    }

    private void CreateProjectAsync(string projectName)
    {
      var projectDirPath = Path.Combine(_solutionDir.FullName, projectName);
      AssertSuccess(
        RunDotNetExe($"new classlib --name {projectName}")
          .Result);
      RemoveDefaultFileCreatedbyTemplate(projectDirPath);
    }

    private static void RemoveDefaultFileCreatedbyTemplate(string projectDirPath)
    {
      File.Delete(Path.Combine(projectDirPath, "Class1.cs"));
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
      processInfo.ExitCode.Should().Be(0, string.Join(Environment.NewLine, processInfo.StandardError.Concat(processInfo.StandardOutput)));
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
    private readonly Dictionary<string, List<XmlSourceCodeFile>> _filesByProject;
    private string _rootNamespace;


    public E2EProjectDsl(string projectName, List<(string, string)> assemblyReferences,
      Dictionary<string, List<XmlSourceCodeFile>> filesByProject)
    {
      _projectName = projectName;
      _assemblyReferences = assemblyReferences;
      _filesByProject = filesByProject;
    }

    public E2EProjectDsl WithAssemblyReferences(params string[] assemblyNames)
    {
      foreach (var assemblyName in assemblyNames)
      {
        _assemblyReferences.Add((_projectName, assemblyName));
      }

      return this;
    }

    public E2EProjectDsl WithRootNamespace(string @namespace)
    {
      _rootNamespace = @namespace;
      return this;
    }

    public E2EProjectDsl WithFile(string fileName, string fileNamespace)
    {
      if (!_filesByProject.ContainsKey(_projectName))
      {
        _filesByProject[_projectName] = new List<XmlSourceCodeFile>();
      }
      _filesByProject[_projectName].Add(new XmlSourceCodeFile(fileName, fileNamespace, _rootNamespace, _projectName));
      
      return this;
    }
  }
}