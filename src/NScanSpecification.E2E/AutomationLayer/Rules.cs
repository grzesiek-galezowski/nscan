﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;

namespace NScanSpecification.E2E.AutomationLayer;

public class Rules
{
  private readonly List<ITestedRuleDefinition> _rules = [];

  public Task SaveIn(AbsoluteFilePath fullRulesPath)
  {
    var lines = _rules.Select(r => r.Name()).ToList();
    return File.WriteAllLinesAsync(fullRulesPath.ToString(), lines);
  }

  public void Add(ITestedRuleDefinition definition)
  {
    _rules.Add(definition);
  }
}