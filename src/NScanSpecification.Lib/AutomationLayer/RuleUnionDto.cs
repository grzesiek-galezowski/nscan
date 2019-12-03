using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class RuleUnionDto : Union<
    IndependentRuleComplementDto, 
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    NoUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto>
  {
    public static RuleUnionDto With(CorrectNamespacesRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public static RuleUnionDto With(NoCircularUsingsRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }
    
    public static RuleUnionDto With(NoUsingsRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public static RuleUnionDto With(IndependentRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public static RuleUnionDto With(HasAttributesOnRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public static RuleUnionDto With(HasTargetFrameworkRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    private RuleUnionDto(IndependentRuleComplementDto o) : base(o)
    {
    }

    private RuleUnionDto(CorrectNamespacesRuleComplementDto o) : base(o)
    {
    }

    private RuleUnionDto(NoCircularUsingsRuleComplementDto o) : base(o)
    {
    }
    
    private RuleUnionDto(NoUsingsRuleComplementDto o) : base(o)
    {
    }

    private RuleUnionDto(HasAttributesOnRuleComplementDto dto) : base(dto)
    {
    }

    private RuleUnionDto(HasTargetFrameworkRuleComplementDto dto) : base(dto)
    {
    }
  }


}