namespace NScanSpecification.E2E.AutomationLayer;

public class E2EProjectDsl(
  string projectName,
  ProjectFiles projectFiles,
  ProjectReferences projectReferences,
  ProjectDefinition projectDefinition)
{
  private string _rootNamespace = "WhateverNamespace";

  public E2EProjectDsl WithReferences(params string[] assemblyNames)
  {
    projectReferences.Add(projectName, assemblyNames);

    return this;
  }

  public E2EProjectDsl WithRootNamespace(string @namespace)
  {
    _rootNamespace = @namespace;
    return this;
  }

  public E2EProjectDsl With(SourceCodeFileDtoBuilder sourceCodeFileDtoBuilder)
  {
    projectFiles.InitializeForProject(projectName);
    projectFiles.Add(projectName, sourceCodeFileDtoBuilder.BuildWith(projectName, _rootNamespace));
    return this;
  }

  public E2EProjectDsl WithTargetFramework(string targetFramework)
  {
    projectDefinition.TargetFramework = targetFramework;
    return this;
  }
}
