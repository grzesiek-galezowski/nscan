using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public class NamespaceBasedRuleUnionDto : Union<
    IndependentRuleComplementDto, 
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto>
  {
    private readonly RuleNameExtractionVisitor _ruleNameExtractionVisitor = new RuleNameExtractionVisitor();

    public static NamespaceBasedRuleUnionDto With(CorrectNamespacesRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public static NamespaceBasedRuleUnionDto With(NoCircularUsingsRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public static NamespaceBasedRuleUnionDto With(IndependentRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public static NamespaceBasedRuleUnionDto With(HasAttributesOnRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public static NamespaceBasedRuleUnionDto With(HasTargetFrameworkRuleComplementDto dto)
    {
      return new NamespaceBasedRuleUnionDto(dto);
    }

    public string RuleName => Accept(_ruleNameExtractionVisitor);

    private NamespaceBasedRuleUnionDto(IndependentRuleComplementDto o) : base(o)
    {
    }

    private NamespaceBasedRuleUnionDto(CorrectNamespacesRuleComplementDto o) : base(o)
    {
    }

    private NamespaceBasedRuleUnionDto(NoCircularUsingsRuleComplementDto o) : base(o)
    {
    }

    private NamespaceBasedRuleUnionDto(HasAttributesOnRuleComplementDto dto) : base(dto)
    {
    }

    private NamespaceBasedRuleUnionDto(HasTargetFrameworkRuleComplementDto dto) : base(dto)
    {
    }
  }
}