namespace MyTool.App
{
  public struct ProjectId
  {
    // ReSharper disable once NotAccessedField.Local
    private readonly string _absolutePath;

    public ProjectId(string absolutePath)
    {
      this._absolutePath = absolutePath;
    }

    public override string ToString()
    {
      return _absolutePath;
    }
  }
}