using NScan.DependencyPathBasedRules;
using TddXt.XFluentAssert.Api;

namespace NScan.DependencyPathBasedRulesSpecification;

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