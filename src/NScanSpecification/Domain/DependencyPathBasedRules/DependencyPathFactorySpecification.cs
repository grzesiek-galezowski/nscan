using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NSubstitute;
using TddXt.XFluentAssertRoot;
using Xunit;

namespace NScanSpecification.Domain.DependencyPathBasedRules
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
