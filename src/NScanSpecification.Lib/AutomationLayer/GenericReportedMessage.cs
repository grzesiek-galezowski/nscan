namespace NScanSpecification.Lib.AutomationLayer;

public abstract class GenericReportedMessage<T> : ReportedMessage
{
  protected GenericReportedMessage(string text) : base(text)
  {
  }

  public T Error()
  {
    return NewInstance(ToString() + ": [ERROR]");
  }

  public T Ok()
  {
    return NewInstance(ToString() + ": [OK]");
  }

  protected abstract T NewInstance(string str);
}