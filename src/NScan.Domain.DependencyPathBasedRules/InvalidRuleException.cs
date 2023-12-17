using System;

namespace NScan.DependencyPathBasedRules;

public class InvalidRuleException(string ruleType) : Exception("Invalid rule: " + ruleType);
