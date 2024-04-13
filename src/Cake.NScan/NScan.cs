using System;
using AtmaFileSystem;
using Cake.Core;
using Cake.Core.Annotations;
using TddXt.NScan;

namespace Cake.NScan;

[CakeAliasCategory("NScan")]
public static class NScan
{
  [CakeMethodAlias]
  public static void NScanAnalyze(this ICakeContext context, string solutionPath, string rulesFilePath)
  {
    NScanAnalyze(context, solutionPath, rulesFilePath, new NScanSettings());
  }


  [CakeMethodAlias]
  // ReSharper disable once MemberCanBePrivate.Global
  public static void NScanAnalyze(
    this ICakeContext context,
    string solutionPath,
    string rulesFilePath,
    NScanSettings settings)
  {
    ArgumentNullException.ThrowIfNull(context);
    ArgumentNullException.ThrowIfNull(solutionPath);
    ArgumentNullException.ThrowIfNull(rulesFilePath);
    ArgumentNullException.ThrowIfNull(settings);

    var result = NScanMain.Run(
      new InputArgumentsDto
      {
        RulesFilePath = AnyFilePath.Value(rulesFilePath),
        SolutionPath = AnyFilePath.Value(solutionPath)
      },
      new CakeContextOutput(context.Log),
      new CakeContextSupport(context.Log));

    if (result != 0)
    {
      throw new NScanFailedException(result);
    }

  }

}

public class NScanFailedException(int result) : Exception($"NScan failed with result: {result}");

public class NScanSettings;
