using AtmaFileSystem;
using AwesomeAssertions;
using NScan.Adapters.Secondary.NotifyingSupport;
using NScan.Adapters.Secondary.ReportingOfResults;
using TddXt.NScan;

namespace NScanSpecification.E2E;

public class SanityTests
{
  [Fact]
  //Any is a very specific solution with multi-targeting and directory.props
  public void ShouldAnalyzeAnyWithoutExceptions()
  {
    new Action(() =>
    {
      NScanMain.Run(
        new InputArgumentsDto
        {
          RulesFilePath =
            AnyFilePath.Value(
              "C:\\Users\\HYPERBOOK\\Documents\\GitHub\\any\\src\\netstandard2.0\\BuildScript\\rules.txt"),
          SolutionPath =
            AnyFilePath.Value("C:\\Users\\HYPERBOOK\\Documents\\GitHub\\any\\src\\netstandard2.0\\Any.sln"),
        },
        new ConsoleOutput(Console.WriteLine),
        new ConsoleSupport(Console.WriteLine)
      );
    }).Should().NotThrow();
  }
}
