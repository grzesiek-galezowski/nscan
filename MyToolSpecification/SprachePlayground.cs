using System;
using System.Linq;
using FluentAssertions;
using Sprache;
using Xunit;

namespace MyTool
{
  public class SpracheSpecification
  {

    [Fact]
    public void ShouldDOWHAT()
    {
      //GIVEN
      Parser<string> rule = from depending in Parse.AnyChar.Until(Parse.Char(' '))
        from firstSpace in Parse.Letter.Once()
        from ruleName in Parse.AnyChar.Until(Parse.Char(' '))
        from secondSpace in Parse.Letter.Once()
        from dependency in Parse.AnyChar.Until(Parse.LineEnd)
                            select new string(depending.Concat(ruleName).Concat(dependency).ToArray());

      var s = rule.Parse("ABCder345 independentOf asldkljasdkajsd\n");
      s.Should().Be("abc123");

      //WHEN

      //THEN
      
    }
  }
}