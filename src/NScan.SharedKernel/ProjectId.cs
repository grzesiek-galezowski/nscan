namespace NScan.SharedKernel
{
  public struct ProjectId
  {
    // ReSharper disable once NotAccessedField.Local
    private readonly string _absolutePath;

    public ProjectId(string absolutePath)
    {
      _absolutePath = absolutePath;
    }

    public override string ToString()
    {
      return _absolutePath;
    }
  }
}