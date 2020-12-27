using System;
using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.ProjectScopedRules
{
  public class ProjectScopedRuleTargetFactory
  {
    private readonly IProjectScopedRuleViolationFactory _ruleViolationFactory;

    public ProjectScopedRuleTargetFactory(IProjectScopedRuleViolationFactory ruleViolationFactory)
    {
      _ruleViolationFactory = ruleViolationFactory;
    }

    public IReadOnlyList<IProjectScopedRuleTarget> ProjectScopedRuleTargets(IEnumerable<CsharpProjectDto> csharpProjectDtos)
    {
      return csharpProjectDtos
        .Select(dataAccess => 
          new ProjectScopedRuleTarget(
            dataAccess.AssemblyName, 
            SourceCodeFiles(dataAccess), 
            dataAccess.TargetFramework))
        .Cast<IProjectScopedRuleTarget>()
        .ToList();
    }

    private List<SourceCodeFile> SourceCodeFiles(CsharpProjectDto projectDataAccess)
    {
      return projectDataAccess.SourceCodeFiles.Select(ToSourceCodeFile).ToList();
    }

    private SourceCodeFile ToSourceCodeFile(SourceCodeFileDto scf)
    {
      return new(
        _ruleViolationFactory, 
        scf.DeclaredNamespaces, 
        scf.ParentProjectAssemblyName, 
        scf.ParentProjectRootNamespace, 
        scf.PathRelativeToProjectRoot, 
        ToClasses(scf.Classes, 
          methodDeclarationInfos => 
            ToMethods(methodDeclarationInfos, _ruleViolationFactory)));
    }

    private static ICSharpClass[] ToClasses(
      IEnumerable<ClassDeclarationInfo> classDeclarationInfos, 
      Func<List<MethodDeclarationInfo>, ICSharpMethod[]> methodFactory)
    { 
      return classDeclarationInfos.Select(c => new CSharpClass(c, methodFactory(c.Methods))).ToArray<ICSharpClass>();
    }

    private static ICSharpMethod[] ToMethods(List<MethodDeclarationInfo> methodDeclarationInfos,
      IProjectScopedRuleViolationFactory violationFactory)
    {
      return methodDeclarationInfos.Select(m => new CSharpMethod(m, violationFactory)).ToArray<ICSharpMethod>();
    }

  }
}
