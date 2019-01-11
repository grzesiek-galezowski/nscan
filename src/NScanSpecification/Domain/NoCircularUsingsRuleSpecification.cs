using FluentAssertions;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class NoCircularUsingsRuleSpecification
  {
    [Fact]
    public void ShouldReturnDescriptionBasedOnDto()
    {
      //GIVEN
      var dto = Any.Instance<NoCircularUsingsRuleComplementDto>();
      var rule = new NoCircularUsingsRule(dto);

      //WHEN
      var description = rule.Description();

      //THEN
      description.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

  }
}