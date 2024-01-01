using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.DependencyPathBasedRules;

public class DependencyPathBasedRuleTargetFactory(INScanSupport support)
{
  public HashMap<ProjectId, IDotNetProject> CreateDependencyPathRuleTargetsByIds(
    Seq<CsharpProjectDto> xmlProjectDataAccesses)
  {
    var projects = new Dictionary<ProjectId, IDotNetProject>();
    foreach (var dataAccess in xmlProjectDataAccesses)
    {
      var (id, project) = CreateProject(dataAccess);
      projects.Add(id, project);
    }

    return projects.ToHashMap();
  }

  private (ProjectId, DotNetStandardProject) CreateProject(CsharpProjectDto projectDataAccess)
  {
    var assemblyName = projectDataAccess.AssemblyName;
    var dotNetStandardProject = new DotNetStandardProject(
      assemblyName,
      projectDataAccess.Id, 
      projectDataAccess.PackageReferences,
      projectDataAccess.AssemblyReferences, 
      new ReferencedProjects(
        projectDataAccess.ReferencedProjectIds, 
        support), 
      new ReferencingProjects());
    return (projectDataAccess.Id, dotNetStandardProject);
  }

}
