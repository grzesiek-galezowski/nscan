using System.Linq;
using AtmaFileSystem;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace NScan.Adapters.SecondarySpecification.ReadingCSharpSolution;

public class MsBuildPlayground
{
  public MsBuildPlayground(ITestOutputHelper output)
  {
    this.Output = output;
  }

  [Fact]
  public void Lol()
  {
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

  public ITestOutputHelper Output { get; init; }
}
