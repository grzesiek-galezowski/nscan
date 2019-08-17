using AtmaFileSystem;

namespace TddXt.NScan
{
  #nullable disable
  public class InputArgumentsDto
  {
    public AnyFilePath SolutionPath { get; set; }
    public AnyFilePath RulesFilePath { get; set; }
  }
}