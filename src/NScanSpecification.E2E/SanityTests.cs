using System.Text;
using System.Threading;
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
  public async Task ShouldAnalyzeAnyWithoutExceptions()
  {
    await new Func<Task>(async () =>
    {
      var output = new StringBuilder();
      var result = await NScanMain.Run(
        new InputArgumentsDto
        {
          RulesFilePath =
            AnyFilePath.Value(
              @"C:\Users\HYPERBOOK\Documents\GitHub\any\src\BuildScript\rules.txt"),
          SolutionPath =
            AnyFilePath.Value(@"C:\Users\HYPERBOOK\Documents\GitHub\any\src\Any.slnx"),
        },
        new ConsoleOutput(s => output.AppendLine(s)),
        new ConsoleSupport(s => output.AppendLine(s.ToString())),
        CancellationToken.None
      );
      result.Should().Be(0, "did not expect " + output);
    }).Should().NotThrowAsync();
  }
}
