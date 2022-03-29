using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

  private async Task AddReference((string dependent, string dependency) obj, CancellationToken cancellationToken)
  {
    await _dotNetExe.RunWith("add " +
                             $"{obj.dependent} " +
                             "reference " +
                             $"{obj.dependency}", cancellationToken);
  }

  public async Task AddToProjects(CancellationToken cancellationToken)
  {
    foreach (var valueTuple in ReferencesByProjectName)
    {
      await AddReference(valueTuple, cancellationToken);
    }
  }
}
