namespace NScanSpecification.Lib.AutomationLayer;

public abstract class ReportedMessage(string text)
{
  public override string ToString()
  {
    return text;
  }
}
