using NScan.Lib.Union1;
using NScan.Lib.Union3;
using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.SharedKernel.RuleDtos
{
  public class RuleNameExtractionVisitor : 
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
    public string Visit(HasTargetFrameworkRuleComplementDto dto)
    {
      return dto.RuleName;
    }

    public string Visit(HasAttributesOnRuleComplementDto dto)
    {
      return dto.RuleName;
    }

    public string Visit(NoCircularUsingsRuleComplementDto dto)
    {
      return dto.RuleName;
    }

    public string Visit(CorrectNamespacesRuleComplementDto dto)
    {
      return dto.RuleName;
    }

    public string Visit(IndependentRuleComplementDto dto)
    {
      return dto.RuleName;
    }
  }
}