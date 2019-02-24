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

    public Dictionary<ProjectId, IDotNetProject> CreateProjectsDictionaryFrom(IEnumerable<XmlProjectDataAccess> xmlProjectDataAccesses)
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();
      foreach (var dataAccess in xmlProjectDataAccesses)
      {
        var (id, project) = CreateProject(dataAccess);
        projects.Add(id, project);
      }

      return projects;
    }

    private (ProjectId, DotNetStandardProject) CreateProject(XmlProjectDataAccess projectDataAccess)
    {
      var assemblyName = projectDataAccess.DetermineAssemblyName();
      var dotNetStandardProject = new DotNetStandardProject(
        assemblyName,
        projectDataAccess.Id(),
        PackageReferences(projectDataAccess),
        AssemblyReferences(projectDataAccess), 
        projectDataAccess.SourceCodeFiles().Select(ToSourceCodeFile).ToList(), 
        new NamespacesDependenciesCache(), 
        new ReferencedProjects(
          projectDataAccess.ProjectReferences().Select(ToProjectId).ToArray(), _support), 
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

    private static IReadOnlyList<AssemblyReference> AssemblyReferences(XmlProjectDataAccess xmlProjectDataAccess)
    {
      return xmlProjectDataAccess.XmlAssemblyReferences()
          .Select(r => new AssemblyReference(r.Include, r.HintPath)).ToList();
    }

    private IReadOnlyList<PackageReference> PackageReferences(XmlProjectDataAccess xmlProjectDataAccess)
    {
      return xmlProjectDataAccess.XmlPackageReferences()
        .Select(r => new PackageReference(r.Include, r.Version)).ToList();
    }
  }
}