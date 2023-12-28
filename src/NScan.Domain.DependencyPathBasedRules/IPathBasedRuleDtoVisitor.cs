using NScan.Lib.Union1;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules;

public interface IPathBasedRuleDtoVisitor : IUnionVisitor<IndependentRuleComplementDto>;