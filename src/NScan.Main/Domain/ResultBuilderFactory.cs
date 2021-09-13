using NScan.SharedKernel;

namespace TddXt.NScan.Domain
{
  public interface IResultBuilderFactory
  {
    IResultBuilder NewResultBuilder();
  }

  public class ResultBuilderFactory : IResultBuilderFactory
  {
    public IResultBuilder NewResultBuilder()
    {
      return new PlainTextResultBuilder();
    }
  }
}
