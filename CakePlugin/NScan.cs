using System;
using Cake.Core;
using Cake.Core.Annotations;
using NScanRoot;
using NScanRoot.CompositionRoot;

namespace CakePlugin
{
  public static class NScan
  {
    [CakeMethodAlias]
    public static void Analyze(this ICakeContext context, string solutionPath, string rulesFilePath)
    {
      ProgramRoot.RunProgramInConsole(new InputArgumentsDto()
      {
        RulesFilePath = rulesFilePath,
        SolutionPath = solutionPath
      });
    }

    [CakeMethodAlias]
    public static void Analyze(
      this ICakeContext context, 
      string solutionPath, 
      string rulesFilePath,
      NScanSettings settings)
    {
      ProgramRoot.RunProgramInConsole(new InputArgumentsDto()
      {
        RulesFilePath = rulesFilePath,
        SolutionPath = solutionPath
      });
    }

  }

  public class NScanSettings
  {
  }
}
