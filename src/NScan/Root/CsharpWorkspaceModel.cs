using System;
using System.Collections.Generic;
using System.Linq;
using NScan.Domain.NamespaceBasedRules;
using NScan.Domain.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Lib;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Domain.Root
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
      IEnumerable<IXmlProjectDataAccess> xmlProjectDataAccesses)
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();
      foreach (var dataAccess in xmlProjectDataAccesses)
      {
        var (id, project) = CreateProject(dataAccess);
        projects.Add(id, project);
      }

      return projects;
    }

    private (ProjectId, DotNetStandardProject) CreateProject(IXmlProjectDataAccess projectDataAccess)
    {
      var assemblyName = projectDataAccess.DetermineAssemblyName();
      var dotNetStandardProject = new DotNetStandardProject(
        assemblyName,
        projectDataAccess.Id(), projectDataAccess.TargetFramework(), projectDataAccess.XmlPackageReferences().Select(ToPackageReference).ToList(),
        projectDataAccess.XmlAssemblyReferences().Select(ToAssemblyReference).ToList(), 
        projectDataAccess.SourceCodeFiles().Select(ToSourceCodeFile).ToList(), 
        new NamespacesDependenciesCache(), 
        new ReferencedProjects(
          projectDataAccess.ProjectReferences().Select(ToProjectId).ToArray(), 
          _support), 
        new ReferencingProjects());
      return (projectDataAccess.Id(), dotNetStandardProject);
    }

    private SourceCodeFile ToSourceCodeFile(XmlSourceCodeFile scf)
    {
      return new SourceCodeFile(
        _ruleViolationFactory, 
        scf.DeclaredNamespaces, 
        scf.ParentProjectAssemblyName, 
        scf.ParentProjectRootNamespace, 
        scf.PathRelativeToProjectRoot, 
        scf.Usings,
        ToClasses(scf.Classes, methodDeclarationInfos => ToMethods(methodDeclarationInfos, _ruleViolationFactory)));
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

    private static ProjectId ToProjectId(XmlProjectReference dto)
    {
      return new ProjectId(dto.FullIncludePath.ToString());
    }

    private static AssemblyReference ToAssemblyReference(XmlAssemblyReference r)
    {
      return new AssemblyReference(r.Include, r.HintPath);
    }

    private static PackageReference ToPackageReference(XmlPackageReference r)
    {
      return new PackageReference(r.Include, r.Version);
    }
  }
}