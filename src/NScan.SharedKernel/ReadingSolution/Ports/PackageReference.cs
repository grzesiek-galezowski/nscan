namespace NScan.SharedKernel.ReadingSolution.Ports;

public sealed record PackageReference(string Name, string Version)
{
  private string Version { get; init; } = Version;
  public override string ToString()
  {
    return $"{Name}, Version {Version}";
  }
}
