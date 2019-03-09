using AtmaFileSystem;

namespace TddXt.NScan.Domain.SharedKernel
{
  public class InputArgumentsDto
  {
    public AnyFilePath SolutionPath { get; set; }
    public AnyFilePath RulesFilePath { get; set; }
  }
}