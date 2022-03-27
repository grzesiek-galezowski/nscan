using GlobExpressions;
using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.DependencyPathBased;

public sealed record IndependentRuleComplementDto(
  string DependencyType, 
  Pattern DependingPattern, 
  Glob DependencyPattern)
{
  public string RuleName => IndependentRuleMetadata.IndependentOf;
}