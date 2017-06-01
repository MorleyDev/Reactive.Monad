using FluentAssertions;
using System.Linq;
using Xunit;

namespace MorleyDev.Reactive.Monad.SampleTests
{
	public class EitherTests
	{
		[Fact]
		public void Basic()
		{
			Either<int, double> left = 10;
			left.Lhs().ToArray().Should().BeEquivalentTo(new[] { 10 });
			left.Rhs().ToArray().Should().BeEmpty();

			left.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(25.0);

			Either<int, double> right = 25.0;
			right.Rhs().ToArray().Should().BeEquivalentTo(new[] { 25.0 });
			right.Lhs().ToArray().Should().BeEmpty();
			right.Match(x => x * 2.5, y => y * 2).Extract().Should().Be(50.0);
		}
	}
}
