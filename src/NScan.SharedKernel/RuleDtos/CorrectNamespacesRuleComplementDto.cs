﻿using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos
{
  public class CorrectNamespacesRuleComplementDto
  {
    public CorrectNamespacesRuleComplementDto(Pattern projectAssemblyNamePattern)
    {
      ProjectAssemblyNamePattern = projectAssemblyNamePattern;
    }

    public string RuleName { get; } = HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces;
    public Pattern ProjectAssemblyNamePattern { get; }
  }
}