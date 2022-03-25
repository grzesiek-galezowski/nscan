using System;
using System.IO;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using NScan.SharedKernel.WritingProgramOutput.Ports;
using static Bullseye.Targets;
using static SimpleExec.Command;

const string solutionName = "NScan.sln";
const string configuration = "Release";
const string version = "0.90.1";
var root = AbsoluteFilePath.OfThisFile().ParentDirectory(2).Value();
var buildDir = root.AddDirectoryName("build").AddDirectoryName(configuration);
var srcDir = root.AddDirectoryName("src");
var slnNetStandard = srcDir.AddFileName(solutionName);
var nugetPath = root.AddDirectoryName("nuget");

if (!buildDir.Exists())
{
  buildDir.Create();
}

//////////////////////////////////////////////////////////////////////
// HELPER FUNCTIONS
//////////////////////////////////////////////////////////////////////
void Build(DirectoryName workingDirectoryLastSegment)
{
  Run($"dotnet",
    "build " +
    $"-c {configuration} " +
    $"-p:VersionPrefix={version}",
    workingDirectory: (srcDir + workingDirectoryLastSegment).ToString());
}

void Test(AbsoluteDirectoryPath workingDirectory)
{
  Run($"dotnet",
    "test" +
    $" -c {configuration}" +
    $" -p:VersionPrefix={version}",
    workingDirectory: workingDirectory.ToString());
}

void Pack(AbsoluteDirectoryPath outputPath, AbsoluteDirectoryPath rootSourceDir, string projectName)
{
  Run("dotnet",
    $"pack" +
    $" -c {configuration}" +
    $" --include-symbols" +
    $" --no-build" +
    $" -p:SymbolPackageFormat=snupkg" +
    $" -p:VersionPrefix={version}" +
    $" -o {outputPath}",
    workingDirectory: rootSourceDir.AddDirectoryName(projectName).ToString());
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Target("Clean", () =>
{
  if(buildDir.Exists()) buildDir.Delete(true);
  if(nugetPath.Exists()) nugetPath.Delete(true);
  Run($"dotnet",
    "clean " +
    $"-c {configuration} ",
    workingDirectory: srcDir.ToString());
});

Target("RunPreviousNScan", () =>
{
  //bug NScanMain.Run(
  //bug   new InputArgumentsDto
  //bug   {
  //bug     RulesFilePath = AbsoluteDirectoryPath.OfThisFile().AddFileName("nscan.config").AsAnyFilePath(),
  //bug     SolutionPath = slnNetStandard.AsAnyFilePath()
  //bug   },
  //bug   new ConsoleOutput(Console.WriteLine),
  //bug   new ConsoleSupport(Console.WriteLine)
  //bug ).Should().Be(0);
});

Target("BuildNScan", DependsOn("RunPreviousNScan"), () =>
{
  Build(DirectoryName.Value("NScan.Main"));
});

Target("BuildNScanConsole", () =>
{
  Build(DirectoryName.Value("NScan.Console"));
});

Target("BuildCakeNScan", () =>
{
  Build(DirectoryName.Value("Cake.NScan"));
});

Target("RunNScanUnitTests", DependsOn("BuildNScan"), () => //todo this runs integration tests as well
{
  var projectFiles = srcDir.GetFiles("*Specification.csproj", SearchOption.AllDirectories);
  foreach (var file in projectFiles)
  {
    Test(file.ParentDirectory());
  }

  Test(srcDir.AddDirectoryName("NScanSpecification.Component"));
});

Target("RunE2ETests", DependsOn("BuildNScanConsole", "RunNScanUnitTests"), () =>
{
  Test(srcDir.AddDirectoryName("NScanSpecification.E2E"));
});

Target("PackNScanDependencies", DependsOn("BuildNScan", "RunE2ETests"), () =>
{
  Pack(nugetPath, srcDir, "NScan.Domain.DependencyPathBasedRules");
  Pack(nugetPath, srcDir, "NScan.Domain.NamespaceBasedRules");
  Pack(nugetPath, srcDir, "NScan.Domain.ProjectScopedRules");
  Pack(nugetPath, srcDir, "NScan.SharedKernel");
  Pack(nugetPath, srcDir, "NScan.Adapters.Secondary");
  Pack(nugetPath, srcDir, "NScan.Lib");
});

Target("PackNScan", DependsOn("BuildNScan", "RunE2ETests"), () =>
{
  Pack(nugetPath, srcDir, "NScan.Main");
});

Target("PackNScanConsole", DependsOn("BuildNScanConsole", "RunE2ETests"), () =>
{
  Pack(nugetPath, srcDir, "NScan.Console");
});

Target("PackCakeNScan", DependsOn("BuildCakeNScan", "RunE2ETests"), () =>
{
  Pack(nugetPath, srcDir, "Cake.NScan");
});

Target("Push", DependsOn(
  "Clean", 
  "PackNScanDependencies", 
  "PackNScan", 
  "PackNScanConsole", 
  "PackCakeNScan"), () =>
{
  foreach (var nupkgPath in nugetPath.GetFiles("*.nupkg"))
  {
    Run("dotnet", $"nuget push {nupkgPath}" +
                  $" --source https://api.nuget.org/v3/index.json");
  }
});

Target("default", DependsOn(
  "BuildNScan", 
  "BuildNScanConsole", 
  "BuildCakeNScan", 
  "RunNScanUnitTests", 
  "PackNScan", 
  "PackNScanConsole", 
  "PackCakeNScan"));

await RunTargetsAndExitAsync(args);

namespace Build
{
  public class ConsoleOutput : INScanOutput
  {
    public void WriteAnalysisReport(string analysisReport)
    {
      Console.WriteLine(analysisReport);
    }

    public void WriteVersion(string coreVersion)
    {
      Console.WriteLine(coreVersion);
    }
  }
}
