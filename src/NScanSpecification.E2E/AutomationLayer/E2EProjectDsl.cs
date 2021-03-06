﻿using NScanSpecification.Lib.AutomationLayer;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class E2EProjectDsl
  {
    private readonly string _projectName;
    private readonly ProjectFiles _projectFiles;
    private string _rootNamespace = "WhateverNamespace";
    private readonly AssemblyReferences _assemblyReferences;
    private readonly ProjectDefinition _projectDefinition;

    public E2EProjectDsl(string projectName,
      ProjectFiles projectFiles,
      AssemblyReferences assemblyReferences, 
      ProjectDefinition projectDefinition)
    {
      _projectName = projectName;
      _projectFiles = projectFiles;
      _assemblyReferences = assemblyReferences;
      _projectDefinition = projectDefinition;
    }

    public E2EProjectDsl WithAssemblyReferences(params string[] assemblyNames)
    {
      _assemblyReferences.Add(_projectName, assemblyNames);

      return this;
    }

    public E2EProjectDsl WithRootNamespace(string @namespace)
    {
      _rootNamespace = @namespace;
      return this;
    }

    public E2EProjectDsl With(SourceCodeFileDtoBuilder sourceCodeFileDtoBuilder)
    {
      _projectFiles.InitializeForProject(_projectName);
      _projectFiles.Add(_projectName, sourceCodeFileDtoBuilder.BuildWith(_projectName, _rootNamespace));
      return this;
    }

    public E2EProjectDsl WithTargetFramework(string targetFramework)
    {
      _projectDefinition.TargetFramework = targetFramework;
      return this;
    }
  }
}