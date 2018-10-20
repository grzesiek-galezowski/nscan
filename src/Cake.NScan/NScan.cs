using Cake.Core;
using Cake.Core.Annotations;
using TddXt.NScan;
using TddXt.NScan.CompositionRoot;

namespace Cake.TddXt.NScan
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
