using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using FluentAssertions.Extensions;
using NullableReferenceTypesExtensions;
using static AtmaFileSystem.AtmaFileSystemPaths;
using AbsoluteDirectoryPath = AtmaFileSystem.AbsoluteDirectoryPath;

namespace NScanSpecification.E2E.AutomationLayer
{
  internal static class BuildEnvironment
  {
    private static readonly Lazy<DirectoryInfo> _buildOutputDirectory 
      = new Lazy<DirectoryInfo>(CreateBuildDirectory);
    
    public static string CurrentConfiguration()
    {
      var assemblyConfigurationAttribute = typeof(NScanE2EDriver)
        .Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
      return assemblyConfigurationAttribute.OrThrow().Configuration;
    }

    public static async Task BuildNScanConsole()
    {
      CleanupOldBuildFolders();
      
      var processResults = await DotNetExe.RunWith(
        $"build {NScanConsoleProjectPath()} -c {CurrentConfiguration()} -o {BuildOutputDirectory()}", 
        RootPath());
      ProcessAssertions.AssertSuccess(processResults);
    }

    public static DirectoryInfo CreateBuildDirectory()
    {
      return RelevantPaths.CreateRandomDirectory(BuildDirNamePrefix);
    }

    public static void CleanupOldBuildFolders()
    {
      var tempDir = AbsoluteDirectoryPath(Path.GetTempPath());
      foreach (var expiredBuildOutput in tempDir.GetDirectories($"{BuildDirNamePrefix}*", SearchOption.TopDirectoryOnly)
        .Where(dir => dir.GetCreationTime() < 10.Minutes().Before(DateTime.Now)))
      {
        expiredBuildOutput.Delete(true);
      }
    }
    
    public static AbsoluteDirectoryPath BuildOutputDirectory()
    {
      return AbsoluteDirectoryPath(_buildOutputDirectory.Value.FullName);
    }

    public static AbsoluteFilePath NScanConsoleProjectPath()
    {
      return RootPath()
        .AddDirectoryName("src")
        .AddDirectoryName("NScan.Console")
        .AddFileName("NScan.Console.csproj");
    }

    public static AbsoluteDirectoryPath RootPath()
    {
      if (NCrunch.RunsThisTest())
      {
        var originalSolutionPath = NCrunch.OriginalSolutionPath();
        return originalSolutionPath.FragmentEndingOnLast(DirectoryName("nscan")).Value;
      }
      else
      {
        var executingAssemblyPath = new FileInfo(
          Assembly.GetExecutingAssembly().EscapedCodeBase.Split("file:///").ToArray()[1]).Directory;
        while (!Directory.EnumerateDirectories(executingAssemblyPath.FullName).Any(s => s.EndsWith(".git")))
        {
          executingAssemblyPath = executingAssemblyPath.Parent;
        }

        return AbsoluteDirectoryPath(executingAssemblyPath.FullName);
      }
    }

    public const string BuildDirNamePrefix = "NScanOutput_";
  }
}
