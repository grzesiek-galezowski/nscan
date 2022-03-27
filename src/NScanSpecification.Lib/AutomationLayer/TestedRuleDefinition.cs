using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScanSpecification.Lib.AutomationLayer;

public interface ITestedRuleDefinition
{
  string Name();
}

public class TestedRuleDefinition : ITestedRuleDefinition
{
  public static TestedRuleDefinition From(IndependentRuleComplementDto dto)
  {
    return new TestedRuleDefinition($"{dto.DependingPattern.Text()} {dto.RuleName} {dto.DependencyType}:{dto.DependencyPattern.Pattern}");
  }

  private readonly string _name;

  private TestedRuleDefinition(string name)
  {
    _name = name;
  }

  public string Name()
  {
    return _name;
  }

  public static ITestedRuleDefinition From(CorrectNamespacesRuleComplementDto dto)
  {
    return new TestedRuleDefinition(
      $"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName}");
  }

  public static ITestedRuleDefinition From(NoCircularUsingsRuleComplementDto dto)
  {
    return new TestedRuleDefinition(
      $"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName}");
  }

  public static ITestedRuleDefinition From(HasAttributesOnRuleComplementDto dto)
  {
    return new TestedRuleDefinition(
      $"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName} {dto.ClassNameInclusionPattern.Text()}:{dto.MethodNameInclusionPattern.Text()}");
  }

  public static ITestedRuleDefinition From(HasTargetFrameworkRuleComplementDto dto)
  {
    return new TestedRuleDefinition(
      $"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName} {dto.TargetFramework}");
  }

  public static ITestedRuleDefinition From(NoUsingsRuleComplementDto complementDto)
  {
    return new TestedRuleDefinition(
      $"{complementDto.ProjectAssemblyNamePattern.Text()} {complementDto.RuleName} " +
      $"from {complementDto.FromPattern.Text()} to {complementDto.ToPattern.Text()}");
  }

  public static ITestedRuleDefinition From(HasPropertyRuleComplementDto dto)
  {
    return new TestedRuleDefinition(
      $"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName} {dto.PropertyName}:{dto.PropertyValue.Text()}");
  }
}