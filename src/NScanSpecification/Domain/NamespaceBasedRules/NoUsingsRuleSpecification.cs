using FluentAssertions;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.NamespaceBasedRules
{
  public class NoUsingsRuleSpecification
  {
    [Fact]
    public void ShouldReturnDescriptionBasedOnDto()
    {
      //GIVEN
      var dto = Any.Instance<NoUsingsRuleComplementDto>();
      var rule = new NoUsingsRule(dto);

      //WHEN
      var description = rule.Description();

      //THEN
      //bug
      description.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName} from {dto.FromPattern.Description()} to {dto.ToPattern.Description()}");
    }
  }
}