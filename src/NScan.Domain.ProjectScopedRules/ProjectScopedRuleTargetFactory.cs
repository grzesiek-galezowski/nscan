using System;
using System.Collections.Generic;
using System.Linq;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleTargetFactory(IProjectScopedRuleViolationFactory ruleViolationFactory)
{
  public IReadOnlyList<IProjectScopedRuleTarget> ProjectScopedRuleTargets(IEnumerable<CsharpProjectDto> csharpProjectDtos)
  {
    return csharpProjectDtos
      .Select(dataAccess => 
        new ProjectScopedRuleTarget(
          new AssemblyName(dataAccess.AssemblyName), 
          SourceCodeFiles(dataAccess), 
          dataAccess.Properties))
      .Cast<IProjectScopedRuleTarget>()
      .ToList();
  }

  private IReadOnlyList<SourceCodeFile> SourceCodeFiles(CsharpProjectDto projectDataAccess)
  {
    return projectDataAccess.SourceCodeFiles
      .OrderBy(dto => dto.PathRelativeToProjectRoot)
      .Select(ToSourceCodeFile).ToList();
  }

  private SourceCodeFile ToSourceCodeFile(SourceCodeFileDto scf)
  {
    return new SourceCodeFile(
      ruleViolationFactory, 
      scf.DeclaredNamespaces, 
      scf.ParentProjectAssemblyName, 
      scf.ParentProjectRootNamespace, 
      scf.PathRelativeToProjectRoot, 
      ToClasses(scf.Classes, 
        methodDeclarationInfos => 
          ToMethods(methodDeclarationInfos, ruleViolationFactory)));
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
