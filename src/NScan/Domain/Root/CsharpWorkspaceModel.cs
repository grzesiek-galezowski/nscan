using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional.Maybe;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Domain.Root
{
  public class XmlProjectDataAccess
  {
    public XmlProject _xmlProject;

    public XmlProjectDataAccess(XmlProject xmlProject)
    {
      _xmlProject = xmlProject;
    }

    public List<XmlPackageReference> XmlPackageReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups.Where(
        ig => ig.PackageReferences != null && ig.PackageReferences.Any()).ToList();

      var references = xmlItemGroups
        .FirstMaybe().Select(pr => pr.PackageReferences.ToList()).OrElse(() => new List<XmlPackageReference>());
      return references;
    }

    public List<XmlAssemblyReference> XmlAssemblyReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups.Where(
        ig => ig.AssemblyReferences != null && ig.AssemblyReferences.Any()).ToList();


      var xmlAssemblyReferences = xmlItemGroups
        .FirstMaybe()
        .Select(ig => ig.AssemblyReferences)
        .OrElse(() => new List<XmlAssemblyReference>());
      return xmlAssemblyReferences;
    }

    public string DetermineAssemblyName()
    {
      return _xmlProject.PropertyGroups.First().AssemblyName ?? Path.GetFileNameWithoutExtension(_xmlProject.AbsolutePath);
    }

    public IEnumerable<XmlProjectReference> ProjectReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups
        .Where(ig => ig.ProjectReferences != null && ig.ProjectReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReferences;
      }

      return new List<XmlProjectReference>();
    }

    public ProjectId Id()
    {
      return new ProjectId(_xmlProject.AbsolutePath);
    }

    public List<SourceCodeFile> SourceCodeFiles(IProjectScopedRuleViolationFactory projectScopedRuleViolationFactory)
    {
      return _xmlProject.SourceCodeFiles.Select(scf => new SourceCodeFile(scf, projectScopedRuleViolationFactory)).ToList();
    }
  }

  public class CsharpWorkspaceModel
  {
    private readonly INScanSupport _support;
    private readonly IProjectScopedRuleViolationFactory _ruleViolationFactory;

    public CsharpWorkspaceModel(
      INScanSupport support,
      IProjectScopedRuleViolationFactory ruleViolationFactory)
    {
      _support = support;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public Dictionary<ProjectId, IDotNetProject> CreateProjectsDictionaryFrom(IReadOnlyList<XmlProject> xmlProjects)
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();
      foreach (var xmlProject in xmlProjects)
      {
        var (id, project) = CreateProject(xmlProject);
        projects.Add(id, project);
      }

      return projects;
    }

    private (ProjectId, DotNetStandardProject) CreateProject(XmlProject xmlProject)
    {
      var xmlProjectDataAccess = new XmlProjectDataAccess(xmlProject); //todo move elsewhere?

      var assemblyName = xmlProjectDataAccess.DetermineAssemblyName();
      var dotNetStandardProject = new DotNetStandardProject(
        assemblyName,
        xmlProjectDataAccess.Id(),
        PackageReferences(xmlProjectDataAccess),
        AssemblyReferences(xmlProjectDataAccess), 
        xmlProjectDataAccess.SourceCodeFiles(_ruleViolationFactory), 
        new NamespacesDependenciesCache(), 
        new ReferencedProjects(
          xmlProjectDataAccess.ProjectReferences().Select(MapToProjectId).ToArray(), _support), 
        new ReferencingProjects());
      return (xmlProjectDataAccess.Id(), dotNetStandardProject);
    }

    private static ProjectId MapToProjectId(XmlProjectReference dto)
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