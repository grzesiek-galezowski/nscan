using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
{
  public class NextAssemblyMatchesPatternConditionSpecification
  {
    [Fact]
    public void ShouldDOWHAT() //bug
    {
      //GIVEN
      var pattern = Any.String();
      var condition = new NextAssemblyMatchesPatternCondition(pattern);

      //WHEN
      //condition.Matches()

      //THEN
      //bug
    }
  }
}