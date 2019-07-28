namespace TddXt.NScan.Specification.AutomationLayer
{
  public abstract class ReportedMessage
  {
    private readonly string _returnValue;

    public override string ToString()
    {
      return _returnValue;
    }

    protected ReportedMessage(string returnValue)
    {
      _returnValue = returnValue;
    }
  }
}