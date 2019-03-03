using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingSolution.Lib;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Domain.Root
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
        projectDataAccess.Id(),
        projectDataAccess.XmlPackageReferences().Select(ToPackageReference).ToList(),
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
      return new SourceCodeFile(scf, _ruleViolationFactory);
    }

    private static ProjectId ToProjectId(XmlProjectReference dto)
    {
      return new ProjectId(dto.Include);
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