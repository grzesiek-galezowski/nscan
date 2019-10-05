﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using NScan.SharedKernel.RuleDtos;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class Rules
  {
    private readonly List<RuleUnionDto> _rules = new List<RuleUnionDto>();

    public void SaveIn(AbsoluteFilePath fullRulesPath)
    {
      var lines = _rules.Select(Name).ToList();
      File.WriteAllLines(fullRulesPath.ToString(), lines);
    }

    public void Add(RuleUnionDto ruleDto)
    {
      _rules.Add(ruleDto);
    }

    private static string Name(RuleUnionDto dto)
    {
      return dto.Accept(new RuleToStringVisitor());
    }
  }
}