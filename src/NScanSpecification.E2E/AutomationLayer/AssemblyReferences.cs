using System.Collections.Generic;
using System.Threading.Tasks;

namespace NScanSpecification.E2E.AutomationLayer;

public class AssemblyReferences
{
  private readonly DotNetExe _dotNetExe;

  public AssemblyReferences(DotNetExe dotNetExe)
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

  private async Task AddReferenceAsync((string dependent, string dependency) obj)
  {
    await _dotNetExe.RunWith("add " +
                             $"{obj.dependent} " +
                             "reference " +
                             $"{obj.dependency}");
  }

  public async Task AddToProjectsAsync() //bug add cancellationToken and remove Async suffix
  {
    foreach (var valueTuple in ReferencesByProjectName)
    {
      await AddReferenceAsync(valueTuple);
    }
  }
}