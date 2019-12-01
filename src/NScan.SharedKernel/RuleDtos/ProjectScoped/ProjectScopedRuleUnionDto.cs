using NScan.Lib.Union5;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public class ProjectScopedRuleUnionDto : Union<
    IndependentRuleComplementDto, 
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto>
  {
    private readonly RuleNameExtractionVisitor _ruleNameExtractionVisitor = new RuleNameExtractionVisitor();

    public static ProjectScopedRuleUnionDto With(CorrectNamespacesRuleComplementDto dto)
    {
      return new ProjectScopedRuleUnionDto(dto);
    }

    public static ProjectScopedRuleUnionDto With(NoCircularUsingsRuleComplementDto dto)
    {
      return new ProjectScopedRuleUnionDto(dto);
    }

    public static ProjectScopedRuleUnionDto With(IndependentRuleComplementDto dto)
    {
      return new ProjectScopedRuleUnionDto(dto);
    }

    public static ProjectScopedRuleUnionDto With(HasAttributesOnRuleComplementDto dto)
    {
      return new ProjectScopedRuleUnionDto(dto);
    }

    public static ProjectScopedRuleUnionDto With(HasTargetFrameworkRuleComplementDto dto)
    {
      return new ProjectScopedRuleUnionDto(dto);
    }

    public string RuleName => Accept(_ruleNameExtractionVisitor);

    private ProjectScopedRuleUnionDto(IndependentRuleComplementDto o) : base(o)
    {
    }

    private ProjectScopedRuleUnionDto(CorrectNamespacesRuleComplementDto o) : base(o)
    {
    }

    private ProjectScopedRuleUnionDto(NoCircularUsingsRuleComplementDto o) : base(o)
    {
    }

    private ProjectScopedRuleUnionDto(HasAttributesOnRuleComplementDto dto) : base(dto)
    {
    }

    private ProjectScopedRuleUnionDto(HasTargetFrameworkRuleComplementDto dto) : base(dto)
    {
    }
  }
}