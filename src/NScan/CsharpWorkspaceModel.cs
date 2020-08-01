using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Domain
{
  public class CsharpWorkspaceModel
  {
    private readonly IProjectScopedRuleViolationFactory _ruleViolationFactory;
    private readonly INScanSupport _support;

    public CsharpWorkspaceModel(
      INScanSupport support,
      IProjectScopedRuleViolationFactory ruleViolationFactory)
    {
      _support = support;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public Dictionary<ProjectId, IDotNetProject> CreateProjectsDictionaryFrom(
      IEnumerable<CsharpProjectDto> xmlProjectDataAccesses)
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();
      foreach (var dataAccess in xmlProjectDataAccesses)
      {
        var (id, project) = CreateProject(dataAccess);
        projects.Add(id, project);
      }

      return projects;
    }

    public (ProjectId, DotNetStandardProject) CreateProject(CsharpProjectDto projectDataAccess)
    {
      var assemblyName = projectDataAccess.AssemblyName;
      var dotNetStandardProject = new DotNetStandardProject(
        assemblyName,
        projectDataAccess.Id, 
        projectDataAccess.PackageReferences,
        projectDataAccess.AssemblyReferences, 
        new ReferencedProjects(
          projectDataAccess.ReferencedProjectIds, 
          _support), 
        new ReferencingProjects());
      return (projectDataAccess.Id, dotNetStandardProject);
    }

    private List<SourceCodeFile> SourceCodeFiles(CsharpProjectDto projectDataAccess)
    {
      return projectDataAccess.SourceCodeFiles.Select(ToSourceCodeFile).ToList();
    }

    private SourceCodeFile ToSourceCodeFile(SourceCodeFileDto scf)
    {
      return new SourceCodeFile(
        _ruleViolationFactory, 
        scf.DeclaredNamespaces, 
        scf.ParentProjectAssemblyName, 
        scf.ParentProjectRootNamespace, 
        scf.PathRelativeToProjectRoot, 
        scf.Usings, ToClasses(scf.Classes, methodDeclarationInfos => ToMethods(methodDeclarationInfos, _ruleViolationFactory)));
    }

    private static ICSharpClass[] ToClasses(
      IEnumerable<ClassDeclarationInfo> classDeclarationInfos, 
      Func<List<MethodDeclarationInfo>, ICSharpMethod[]> methodFactory)
    { 
      return classDeclarationInfos.Select(c => new CSharpClass(c, methodFactory(c.Methods))).ToArray<ICSharpClass>();
    }

    private static ICSharpMethod[] ToMethods(List<MethodDeclarationInfo> methodDeclarationInfos,
      IProjectScopedRuleViolationFactory violationFactory)
    {
      return methodDeclarationInfos.Select(m => new CSharpMethod(m, violationFactory)).ToArray<ICSharpMethod>();
    }

    public List<NamespaceBasedRuleTarget> NamespaceBasedRuleTargets(IEnumerable<CsharpProjectDto> csharpProjectDtos)
    {
      return csharpProjectDtos.Select(dataAccess =>
          new NamespaceBasedRuleTarget(
            dataAccess.AssemblyName,
            SourceCodeFiles(dataAccess),
            new NamespacesDependenciesCache()))
        .ToList();
    }

    public IReadOnlyList<IProjectScopedRuleTarget> ProjectScopedRuleTargets(IEnumerable<CsharpProjectDto> csharpProjectDtos)
    {
      return csharpProjectDtos
        .Select(dataAccess => 
          new ProjectScopedRuleTarget(
            dataAccess.AssemblyName, 
            SourceCodeFiles(dataAccess), 
            dataAccess.TargetFramework))
        .Cast<IProjectScopedRuleTarget>()
        .ToList();
    }
  }
}