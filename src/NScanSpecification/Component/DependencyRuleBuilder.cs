using System.Threading.Tasks;
using GlobExpressions;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.Domain;
using TddXt.NScan.Lib;
using TddXt.NScan.RuleInputData;
using static TddXt.NScan.Lib.Maybe;

namespace TddXt.NScan.Specification.Component
{
  public interface IFullDependingPartStated
  {
    IFullRuleConstructed IndependentOfProject(string dependentAssemblyName);
    IFullRuleConstructed IndependentOfPackage(string packageName);
    IFullRuleConstructed IndependentOfAssembly(string assemblyName);
  }

  public interface IProjectNameStated : IFullDependingPartStated
  {
    IFullDependingPartStated Except(string exclusionPattern);
    IFullRuleConstructed HasCorrectAssemblyNames();
  }

  public interface IRuleDefinitionStart
  {
    IProjectNameStated Project(string dependingAssemblyName);
  }

  public interface IFullRuleConstructed
  {
    RuleDto Build();
  }

  public class DependencyRuleBuilder : IRuleDefinitionStart, IFullRuleConstructed, IProjectNameStated
  {
    private string _dependingPattern;
    private string _ruleName;
    private Maybe<string> _exclusionPattern = Nothing<string>();
    private Maybe<IndependentRuleComplementDto> _independentRuleComplementDto = Nothing<IndependentRuleComplementDto>();

    public IProjectNameStated Project(string dependingAssemblyName)
    {

      _dependingPattern = dependingAssemblyName;
      return this;
    }

    public IFullRuleConstructed IndependentOfProject(string dependentAssemblyName)
    {
      _independentRuleComplementDto = Just(new IndependentRuleComplementDto()
      {
        DependencyType = "project",
        DependencyPattern = new Glob(dependentAssemblyName)
      });
      
      _ruleName = RuleNames.IndependentOf;
      return this;
    }

    public IFullRuleConstructed IndependentOfPackage(string packageName)
    {
      _independentRuleComplementDto = Maybe.Just(new IndependentRuleComplementDto()
      {
        DependencyType = "package",
        DependencyPattern = new Glob(packageName)
      });
      _ruleName = RuleNames.IndependentOf;
      return this;

    }

    public IFullRuleConstructed IndependentOfAssembly(string assemblyName)
    {
      _independentRuleComplementDto = Just(new IndependentRuleComplementDto
      {
        DependencyType = "assembly",
        DependencyPattern = new Glob(assemblyName)
      });
      _ruleName = RuleNames.IndependentOf;
      return this;
    }

    public IFullDependingPartStated Except(string exclusionPattern)
    {
      _exclusionPattern = Just(exclusionPattern);
      return this;
    }

    public IFullRuleConstructed HasCorrectAssemblyNames()
    {
      _ruleName = RuleNames.HasCorrectNamespaces;
      return this;
    }

    public RuleDto Build()
    {
      var dependingPattern = _exclusionPattern
        .Select(p => Pattern.WithExclusion(_dependingPattern, p))
        .Otherwise(() => Pattern.WithoutExclusion(_dependingPattern));
      return new RuleDto
      {
        DependingPattern = dependingPattern,
        IndependentRuleComplement = _independentRuleComplementDto.Value(),
        RuleName = _ruleName,
      };
    }

    public static IRuleDefinitionStart Rule()
    {
      return new DependencyRuleBuilder();
    }
  }
}