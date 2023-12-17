using System.Collections.Generic;

namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectReferences(DotNetExe dotNetExe)
{
  private readonly DotNetExe _dotNetExe = dotNetExe;

  private List<(string, string)> ReferencesByProjectName { get; } = new();

  public void Add(string projectName, string[] assemblyNames)
  {
    foreach (var assemblyName in assemblyNames)
    {
      ReferencesByProjectName.Add((projectName, assemblyName));
    }
  }

  public void AddTo(ProjectsCollection projectsCollection)
  {
    foreach (var valueTuple in ReferencesByProjectName)
    {
      projectsCollection.AddProjectReference(valueTuple.Item1, valueTuple.Item2);
    }
  }
}
