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
using TddXt.NScan.ReadingRules;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.NScan.ReadingSolution;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.NScan.Specification.AutomationLayer;
using TddXt.NScan.Specification.Component.AutomationLayer;
using static System.Environment;

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

          File.WriteAllText(fullFilePath, StringBodyOf(sourceCodeFile));
        }

      }
    }

    private static string StringBodyOf(XmlSourceCodeFile sourceCodeFile)
    {
      return string.Join(NewLine, sourceCodeFile.Usings.Select(n => $"using {n};")) + $"namespace {sourceCodeFile.DeclaredNamespaces.Single()}" + " {}";
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
      return string.Join(NewLine, _analysisResult.StandardOutput);
    }

    private string ConsoleStandardOutputAndErrorString()
    {
      return string.Join(NewLine, _analysisResult.StandardOutput.Concat(_analysisResult.StandardError));
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

      _analysisResult = RunDotNetExe($"run --project {nscanConsoleProjectPath} -- -p \"{_fullSolutionPath}\" -r \"{_fullRulesPath}\"").Result;
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

    private void CreateRulesFile()
    {
      var lines = _rules.Select(dto => dto.Switch(
          independent => ToRuleString(dto.IndependentRule),
          correctNamespaces => ToRuleString(dto.CorrectNamespacesRule), 
          noCircularUsings => ToRuleString(dto.NoCircularUsingsRule))
        ).ToList();
      File.WriteAllLines(_fullRulesPath, lines);
    }

    private string ToRuleString(NoCircularUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
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
      processInfo.ExitCode.Should().Be(0, string.Join(NewLine, processInfo.StandardError.Concat(processInfo.StandardOutput)));
    }

    private static DirectoryInfo GetTemporaryDirectory()
    {
      var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(tempDirectory);
      return new DirectoryInfo(tempDirectory);
    }

    public void ReportShouldContain(ReportedMessage reportedMessage)
    {
      ReportShouldContainText(reportedMessage.ToString());
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

    public E2EProjectDsl With(XmlSourceCodeFileBuilder sourceCodeFileBuilder)
    {
      if (!_filesByProject.ContainsKey(_projectName))
      {
        _filesByProject[_projectName] = new List<XmlSourceCodeFile>();
      }
      _filesByProject[_projectName].Add(
        sourceCodeFileBuilder.BuildWith(_projectName, _rootNamespace));
      
      return this;
    }
  }


}