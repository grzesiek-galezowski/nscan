namespace NScan.SharedKernel.ReadingSolution.Ports;

public record AssemblyReference(string Name, string HintPath)
{
  private string HintPath { get; init; } = HintPath;
}
