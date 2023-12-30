using LanguageExt;

namespace NScan.DependencyPathBasedRules;

public delegate IProjectDependencyPath ProjectDependencyPathFactory(Seq<IDependencyPathBasedRuleTarget> projects);
