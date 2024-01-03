using LanguageExt;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public interface IProjectFilesetScopedRule
{
  void Check(Seq<ISourceCodeFileInNamespace> sourceCodeFiles, IAnalysisReportInProgress report);
}

public interface IProjectScopedRule
{
  void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report);

  public RuleDescription Description();
}
