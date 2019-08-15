using AtmaFileSystem;

namespace NScan.SharedKernel.SharedKernel
{
  #nullable disable
  public class InputArgumentsDto
  {
    public AnyFilePath SolutionPath { get; set; }
    public AnyFilePath RulesFilePath { get; set; }
  }
}