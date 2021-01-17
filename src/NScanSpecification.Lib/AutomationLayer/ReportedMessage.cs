namespace NScanSpecification.Lib.AutomationLayer
{
  public abstract class ReportedMessage
  {
    private readonly string _text;

    public override string ToString()
    {
      return _text;
    }

    protected ReportedMessage(string text)
    {
      _text = text;
    }
  }
}
