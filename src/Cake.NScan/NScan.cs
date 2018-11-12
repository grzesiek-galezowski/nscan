using System;
using Cake.Core;
using Cake.Core.Annotations;
using TddXt.NScan;
using TddXt.NScan.CompositionRoot;

namespace Cake.NScan
{
  public static class NScan
  {
    [CakeMethodAlias]
    public static void NScanAnalyze(this ICakeContext context, string solutionPath, string rulesFilePath)
    {
      NScanAnalyze(context, solutionPath, rulesFilePath, new NScanSettings());
    }


    [CakeMethodAlias]
    public static void NScanAnalyze(
      this ICakeContext context, 
      string solutionPath, 
      string rulesFilePath,
      NScanSettings settings)
    {
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }
      if (solutionPath == null)
      {
        throw new ArgumentNullException(nameof(solutionPath));
      }
      if (rulesFilePath == null)
      {
        throw new ArgumentNullException(nameof(rulesFilePath));
      }
      if (settings == null)
      {
        throw new ArgumentNullException(nameof(settings));
      }
      var result = NScanMain.Run(
        new InputArgumentsDto()
        {
          RulesFilePath = rulesFilePath,
          SolutionPath = solutionPath
        },
        new CakeContextOutput(context.Log),
        new CakeContextSupport(context.Log));

      if (result != 0)
      {
        throw new NScanFailedException(result);
      }

    }

  }

  public class NScanFailedException : Exception
  {
    public NScanFailedException(int result) : base($"NScan failed with result: {result}")
    {
      
    }
  }

  public class NScanSettings
  {
  }
}
