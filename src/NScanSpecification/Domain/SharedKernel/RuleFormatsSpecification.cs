using FluentAssertions;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.SharedKernel
{
  public class RuleFormatsSpecification
  {
    [Fact]
    public void ShouldProvideFormattedDescriptionOfHasTargetFrameworkRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<HasTargetFrameworkRuleComplementDto>();
      //WHEN
      var text = RuleFormats.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {RuleNames.HasTargetFramework} {dto.TargetFramework}");
    }
  }
}
