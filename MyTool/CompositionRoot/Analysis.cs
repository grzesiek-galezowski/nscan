using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  internal class Analysis
  {
    private readonly ISolution _dotNetStandardSolution;

    public Analysis(ISolution dotNetStandardSolution)
    {
      _dotNetStandardSolution = dotNetStandardSolution;
    }

    public void Run()
    {
      var solution = _dotNetStandardSolution;
      solution.ResolveAllProjectsReferences();
      solution.Print();
    }

    public static Analysis Of(Dictionary<ProjectId, IDotNetProject> projects)
    {
      return new Analysis(new DotNetStandardSolution(projects));
    }
  }
}