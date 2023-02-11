using System.Collections.Generic;

namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectReferences
{
  private readonly DotNetExe _dotNetExe;

  public ProjectReferences(DotNetExe dotNetExe)
  {
    _dotNetExe = dotNetExe;
    ReferencesByProjectName = new List<(string, string)>();
  }

  private List<(string, string)> ReferencesByProjectName { get; } 

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
