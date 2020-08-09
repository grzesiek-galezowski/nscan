using System.Collections.Generic;
using System.Linq;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public class SolutionForDependencyPathRules : ISolutionForDependencyPathBasedRules //bug move
  {
    private readonly IPathCache _pathCache;
    private readonly IReadOnlyDictionary<ProjectId, IDotNetProject> _projectsById;

    public SolutionForDependencyPathRules(
      IPathCache pathCache,
      IReadOnlyDictionary<ProjectId, IDotNetProject> projectsById) //bug not DotNetProjects!
    {
      _pathCache = pathCache;
      _projectsById = projectsById;
    }

    public void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_pathCache, analysisReportInProgress); //bug UT
    }

    public void BuildDependencyPathCache()
    {
      _pathCache.BuildStartingFrom(RootProjects()); //bug pass as read-only collection
    }

    private IDotNetProject[] RootProjects()
    {
      return Projects().Where(project => project.IsRoot()).ToArray();
    }

    private IReadOnlyList<IDotNetProject> Projects()
    {
      return _projectsById.Values.ToList();
    }

  }
}