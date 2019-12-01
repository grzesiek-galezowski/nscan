using NScan.Lib;
using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class RuleToStringVisitor : IUnionTransformingVisitor<
    IndependentRuleComplementDto,
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto,
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