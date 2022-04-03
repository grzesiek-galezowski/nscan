using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AtmaFileSystem;
using Core.NullableReferenceTypesExtensions;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution;

public record MsBuildPlayground(ITestOutputHelper Output)
{
  [Fact]
  public void Lol()
  {
    SetMsBuildExePath();
    var projectRootElement =
      ProjectRootElement.Open(
        AbsoluteFilePath.OfThisFile()
          .ParentDirectory()
          .ParentDirectory().Value()
          .AddFileName("NScan.Adapters.SecondarySpecification.csproj")
          .ToString());

    var project = new Project(projectRootElement);
    Output.WriteLine("============ All properties ==========");
    foreach (var projectAllEvaluatedProperty in project.AllEvaluatedProperties)
    {
      if (projectAllEvaluatedProperty is { } p)
      {
        Output.WriteLine(p.Name + " " + p.EvaluatedValue);
      }
    }
    Output.WriteLine("============ FILES ==========");
    foreach (var projectItem in project.Items.Where(item => item.ItemType == "Compile"))
    {
      Output.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType + " " + projectItem.EvaluatedInclude +
                       " " + MetadataString(projectItem));
    }

    Output.WriteLine("============ PACKAGE REFERENCES ==========");
    foreach (var projectItem in project.Items.Where(item => item.ItemType == "PackageReference"))
    {
      Output.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType + " " + projectItem.EvaluatedInclude +
                       " " + MetadataString(projectItem));
    }

    Output.WriteLine("============ PROJECT REFERENCES ==========");
    foreach (var projectItem in project.Items.Where(item => item.ItemType == "ProjectReference"))
    {
      Output.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType + " " + projectItem.EvaluatedInclude +
                       " " + MetadataString(projectItem));
    }

    Output.WriteLine("============ ASSEMBLY REFERENCES ==========");
    foreach (var projectItem in project.Items.Where(item => item.ItemType == "AssemblyReference"))
    {
      Output.WriteLine(projectItem.GetType() + " " + projectItem.Xml.ItemType + " " + projectItem.EvaluatedInclude +
                       " " + MetadataString(projectItem));
    }

    Output.WriteLine("============ PROJECT PROPERTIES ==========");
    foreach (var projectItem in project.Properties)
    {
      Output.WriteLine(projectItem.GetType() + " " + projectItem.Name + " " + projectItem.EvaluatedValue);
    }
  }

  private string MetadataString(ProjectItem projectItem)
  {
    return string.Join('|', projectItem.DirectMetadata.Select(md => md.Name + ":" + md.EvaluatedValue));
  }

  private static void SetMsBuildExePath()
  {
    var startInfo = new ProcessStartInfo("dotnet", "--list-sdks") { RedirectStandardOutput = true };

    var process = Process.Start(startInfo).OrThrow();
    process.WaitForExit(1000);

    var output = process.StandardOutput.ReadToEnd();
    var sdkPaths = Regex.Matches(output, "([0-9]+.[0-9]+.[0-9]+) \\[(.*)\\]")
      .OfType<Match>()
      .Select(m => Path.Combine(m.Groups[2].Value, m.Groups[1].Value, "MSBuild.dll"));

    var sdkPath = sdkPaths.Last();
    Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", sdkPath, EnvironmentVariableTarget.Process);
  }
}
