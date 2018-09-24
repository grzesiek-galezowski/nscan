using FluentAssertions;
using MyTool.CompositionRoot;
using NScanRoot;
using NScanRoot.CompositionRoot;
using NSubstitute;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace MyTool
{
  public class DependencyPathFactorySpecification
  {
    [Fact]
    public void ShouldCreateDependencyPathInProgressHoldingThePassedDestination()
    {
      //GIVEN
      var factory = new DependencyPathFactory();
      var destination = Substitute.For<IFinalDependencyPathDestination>();
      
      //WHEN
      var dependencyPathInProgress = factory.NewDependencyPathFor(destination);

      //THEN
      dependencyPathInProgress.GetType().Should().Be<DependencyPathInProgress>();
      dependencyPathInProgress.Should().DependOn(destination);
    }
  }
}
