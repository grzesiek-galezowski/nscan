using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public class ProjectSourceCodeFilesRelatedRule : IProjectScopedRule, IProjectFilesetScopedRule
  {
    private readonly string _ruleDescription;
    private readonly ISourceCodeFileContentCheck _fileContentCheck;
    private readonly Pattern _projectAssemblyNamePattern;

    public ProjectSourceCodeFilesRelatedRule(
      Pattern projectAssemblyNamePattern, 
      string ruleDescription,
      ISourceCodeFileContentCheck fileContentCheck)
    {
      _ruleDescription = ruleDescription;
      _fileContentCheck = fileContentCheck;
      _projectAssemblyNamePattern = projectAssemblyNamePattern;
    }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      if (project.HasProjectAssemblyNameMatching(_projectAssemblyNamePattern)) //bug move this check to decorator. Some rules reuse it.
      {
        project.AnalyzeFiles(this, report);
      }
    }

    public void Check(IReadOnlyList<ISourceCodeFileInNamespace> sourceCodeFiles, IAnalysisReportInProgress report)
    {
      foreach (var sourceCodeFile in sourceCodeFiles)
      {
        _fileContentCheck.ApplyTo(sourceCodeFile, _ruleDescription, report);
      }
    }

    public override string ToString()
    {
      return _ruleDescription;
    }
  }
}