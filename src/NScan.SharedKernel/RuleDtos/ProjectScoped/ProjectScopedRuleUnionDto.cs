using NScan.Lib.Union3;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public class ProjectScopedRuleUnionDto : Union<
    CorrectNamespacesRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto>
  {
    private readonly IUnionTransformingVisitor<
      CorrectNamespacesRuleComplementDto, 
      HasAttributesOnRuleComplementDto, 
      HasTargetFrameworkRuleComplementDto, 
      string> _ruleNameExtractionVisitor = new RuleNameExtractionVisitor();

    public static ProjectScopedRuleUnionDto With(CorrectNamespacesRuleComplementDto dto)
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

    private ProjectScopedRuleUnionDto(CorrectNamespacesRuleComplementDto o) : base(o)
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