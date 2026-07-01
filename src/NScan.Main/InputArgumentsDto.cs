using AtmaFileSystem;

namespace TddXt.NScan;

public record InputArgumentsDto
{
  public AnyFilePath? SolutionPath { get; set; }
  public AnyFilePath? RulesFilePath { get; set; }
}
