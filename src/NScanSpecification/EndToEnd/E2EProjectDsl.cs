using System.Collections.Generic;
using TddXt.NScan.Specification.AutomationLayer;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class E2EProjectDsl
  {
    private readonly string _projectName;
    private readonly List<(string, string)> _assemblyReferences;
    private readonly ProjectFiles _projectFiles;
    private string _rootNamespace;


    public E2EProjectDsl(string projectName, List<(string, string)> assemblyReferences, ProjectFiles projectFiles)
    {
      _projectName = projectName;
      _assemblyReferences = assemblyReferences;
      _projectFiles = projectFiles;
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
      _projectFiles.InitializeForProject(_projectName);
      _projectFiles.Add(_projectName, sourceCodeFileBuilder.BuildWith(_projectName, _rootNamespace));
      return this;
    }
  }
}