using System;
using System.Linq;
using LanguageExt;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleTargetFactory(IProjectScopedRuleViolationFactory ruleViolationFactory)
{
  public Seq<IProjectScopedRuleTarget> ProjectScopedRuleTargets(Seq<CsharpProjectDto> csharpProjectDtos)
  {
    return csharpProjectDtos
      .Select(dataAccess => 
        new ProjectScopedRuleTarget(
          new AssemblyName(dataAccess.AssemblyName), 
          SourceCodeFiles(dataAccess), 
          dataAccess.Properties))
      .Cast<IProjectScopedRuleTarget>()
      .ToSeq();
  }

  private Seq<ISourceCodeFileInNamespace> SourceCodeFiles(CsharpProjectDto projectDataAccess)
  {
    return projectDataAccess.SourceCodeFiles
      .OrderBy(dto => dto.PathRelativeToProjectRoot)
      .Select(ToSourceCodeFile).ToSeq();
  }

  private ISourceCodeFileInNamespace ToSourceCodeFile(SourceCodeFileDto scf)
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

  private static Seq<ICSharpClass> ToClasses(
    Seq<ClassDeclarationInfo> classDeclarationInfos, 
    Func<Seq<MethodDeclarationInfo>, Seq<ICSharpMethod>> methodFactory)
  { 
    return classDeclarationInfos.Select(c => new CSharpClass(c, methodFactory(c.Methods))).ToSeq<ICSharpClass>();
  }

  private static Seq<ICSharpMethod> ToMethods(Seq<MethodDeclarationInfo> methodDeclarationInfos,
    IProjectScopedRuleViolationFactory violationFactory)
  {
    return methodDeclarationInfos.Select(m => new CSharpMethod(m, violationFactory)).ToSeq<ICSharpMethod>();
  }

}
