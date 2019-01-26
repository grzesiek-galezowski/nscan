using System.Collections.Generic;
using System.IO;
using System.Linq;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.NScan.Specification.Component.AutomationLayer;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class Rules
  {
    private readonly List<RuleUnionDto> _rules = new List<RuleUnionDto>();

    public void SaveIn(string fullRulesPath)
    {
      var lines = _rules.Select(dto => dto.Switch(
        independent => Rules.ToRuleString((IndependentRuleComplementDto) dto.IndependentRule),
        correctNamespaces => Rules.ToRuleString((CorrectNamespacesRuleComplementDto) dto.CorrectNamespacesRule), 
        noCircularUsings => Rules.ToRuleString((NoCircularUsingsRuleComplementDto) dto.NoCircularUsingsRule))
      ).ToList();
      File.WriteAllLines(fullRulesPath, lines);
    }

    public void Add(IFullRuleConstructed ruleDefinition)
    {
      _rules.Add(ruleDefinition.Build());
    }

    private static string ToRuleString(NoCircularUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    private static string ToRuleString(CorrectNamespacesRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    private static string ToRuleString(IndependentRuleComplementDto dto)
    {
      return $"{dto.DependingPattern.Description()} {dto.RuleName} {dto.DependencyType}:{dto.DependencyPattern.Pattern}";
    }
  }
}