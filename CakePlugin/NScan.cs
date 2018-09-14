using System;
using Cake.Core;
using Cake.Core.Annotations;
using Root;

namespace CakePlugin
{
  public static class NScan
  {
    [CakeMethodAlias]
    public static void Analyze(this ICakeContext context, string solutionPath, string rulesFilePath)
    {
      ProgramRoot.RunProgram(new InputArgumentsDto()
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
      ProgramRoot.RunProgram(new InputArgumentsDto()
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
