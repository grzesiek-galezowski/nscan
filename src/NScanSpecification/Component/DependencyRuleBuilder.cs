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
    private Glob _dependencyPattern;
    private string _dependencyType;
    private string _ruleName;
    private Maybe<string> _exclusionPattern = Nothing<string>();

    public IProjectNameStated Project(string dependingAssemblyName)
    {

      _dependingPattern = dependingAssemblyName;
      return this;
    }

    public IFullRuleConstructed IndependentOfProject(string dependentAssemblyName)
    {
      _dependencyPattern = new Glob(dependentAssemblyName);
      _dependencyType = "project";
      _ruleName = RuleNames.IndependentOf;
      return this;
    }

    public IFullRuleConstructed IndependentOfPackage(string packageName)
    {
      _dependencyPattern = new Glob(packageName);
      _dependencyType = "package";
      _ruleName = RuleNames.IndependentOf;
      return this;

    }

    public IFullRuleConstructed IndependentOfAssembly(string assemblyName)
    {
      _dependencyPattern = new Glob(assemblyName);
      _dependencyType = "assembly";
      _ruleName = RuleNames.IndependentOf;
      return this;
    }

    public IFullDependingPartStated Except(string exclusionPattern)
    {
      _exclusionPattern = Just(exclusionPattern);
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
        IndependentRuleComplement = new IndependentRuleComplementDto
        {
          DependencyType = _dependencyType,
          DependencyPattern = _dependencyPattern
        },
        RuleName = _ruleName,
      };
    }

    public static IRuleDefinitionStart Rule()
    {
      return new DependencyRuleBuilder();
    }
  }
}