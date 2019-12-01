using NScan.Lib;
using NScan.Lib.Union1;
using NScan.Lib.Union3;
using NScan.Lib.Union5;

namespace NScan.SharedKernel.RuleDtos
{
  public class RuleNameExtractionVisitor : 
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

  public class RuleUnionDto : Union<
    IndependentRuleComplementDto, 
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto>
  {
    private readonly RuleNameExtractionVisitor _ruleNameExtractionVisitor = new RuleNameExtractionVisitor();

    public static RuleUnionDto With(CorrectNamespacesRuleComplementDto dto)
    {
      return new RuleUnionDto(dto);
    }

    public static RuleUnionDto With(NoCircularUsingsRuleComplementDto dto)
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

    public string RuleName => Accept(_ruleNameExtractionVisitor);

    private RuleUnionDto(IndependentRuleComplementDto o) : base(o)
    {
    }

    private RuleUnionDto(CorrectNamespacesRuleComplementDto o) : base(o)
    {
    }

    private RuleUnionDto(NoCircularUsingsRuleComplementDto o) : base(o)
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