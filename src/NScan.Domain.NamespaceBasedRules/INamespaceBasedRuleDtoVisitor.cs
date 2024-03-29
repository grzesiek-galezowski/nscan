﻿using NScan.Lib.Union2;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public interface INamespaceBasedRuleDtoVisitor : IUnionVisitor<NoCircularUsingsRuleComplementDto, NoUsingsRuleComplementDto>;
