using LanguageExt;
using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace TddXt.NScan.Domain;

public class Analysis(
  IAnalysisReportInProgress analysisReportInProgress,
  IDependencyAnalysis dependencyAnalysis,
  IProjectAnalysis projectAnalysis,
  IProjectNamespacesAnalysis projectNamespacesAnalysis,
  IResultBuilderFactory resultBuilderFactory)
{
  public const int ReturnCodeOk = 0;
  public const int ReturnCodeAnalysisFailed = -1;

  public string Report
  {
    get
    {
      var resultBuilder = resultBuilderFactory.NewResultBuilder();
      analysisReportInProgress.PutContentInto(resultBuilder);
      return resultBuilder.Text();
    }
  }

  public int ReturnCode => analysisReportInProgress.IsFailure() ? -1 : 0; //bug UI implementation leak

  public static Analysis PrepareFor(Seq<CsharpProjectDto> csharpProjectDtos, INScanSupport support)
  {
    return new Analysis(new AnalysisReportInProgress(new RuleReportFactory()), 
      DependencyAnalysis.PrepareFor(csharpProjectDtos, support), 
      ProjectAnalysis.PrepareFor(csharpProjectDtos), 
      ProjectNamespacesAnalysis.PrepareFor(csharpProjectDtos), new ResultBuilderFactory());
  }

  public void Run()
  {
    //_solution.PrintDebugInfo();
    dependencyAnalysis.Perform(analysisReportInProgress);
    projectAnalysis.Perform(analysisReportInProgress);
    projectNamespacesAnalysis.PerformOn(analysisReportInProgress);
  }

  public void AddDependencyPathRules(Seq<DependencyPathBasedRuleUnionDto> rules)
  {
    dependencyAnalysis.Add(rules);
  }

  public void AddProjectScopedRules(Seq<ProjectScopedRuleUnionDto> rules)
  {
    projectAnalysis.Add(rules);
  }

  public void AddNamespaceBasedRules(Seq<NamespaceBasedRuleUnionDto> rules)
  {
    projectNamespacesAnalysis.Add(rules);
  }
}
