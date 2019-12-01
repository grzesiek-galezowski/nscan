using NScan.Lib.Union1;
using NScan.Lib.Union3;
using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class RuleToStringVisitor : 
    IUnionTransformingVisitor<
      IndependentRuleComplementDto, 
      CorrectNamespacesRuleComplementDto, 
      NoCircularUsingsRuleComplementDto, 
      HasAttributesOnRuleComplementDto, 
      HasTargetFrameworkRuleComplementDto, 
      string>,
    IUnionTransformingVisitor<
      IndependentRuleComplementDto, 
      string>,
    IUnionTransformingVisitor<
      CorrectNamespacesRuleComplementDto, 
      HasAttributesOnRuleComplementDto, 
      HasTargetFrameworkRuleComplementDto, 
      string>,
    IUnionTransformingVisitor<
      NoCircularUsingsRuleComplementDto, 
      string>
  {
    public string Visit(NoCircularUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    public string Visit(CorrectNamespacesRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    public string Visit(IndependentRuleComplementDto dto)
    {
      return $"{dto.DependingPattern.Description()} {dto.RuleName} {dto.DependencyType}:{dto.DependencyPattern.Pattern}";
    }

    public string Visit(HasAttributesOnRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName} {dto.ClassNameInclusionPattern.Description()}:{dto.MethodNameInclusionPattern.Description()}";
    }

    public string Visit(HasTargetFrameworkRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName} {dto.TargetFramework}";
    }
  }
}