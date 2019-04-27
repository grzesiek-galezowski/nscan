using AtmaFileSystem;

namespace TddXt.NScan.Domain.SharedKernel
{
  #nullable disable
  public class InputArgumentsDto
  {
    public AnyFilePath SolutionPath { get; set; }
    public AnyFilePath RulesFilePath { get; set; }
  }
}