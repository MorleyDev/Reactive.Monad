using FluentAssertions;
using System.Linq;
using System.Reactive.Linq;
using Xunit;

namespace MorleyDev.Reactive.Monad.SampleTests
{
	public class MaybeTests
	{
		[Fact]
		public void Basic()
		{
			Maybe<int> maybe = 10;
			maybe.ToList().ShouldBeEquivalentTo(new[] { 10 });

			Maybe<int> v = Maybe.Just(20);
			v.Select(t => t * 20).ToList().ShouldBeEquivalentTo(new[] { 400 });

			Maybe<int> none = Maybe.None;
			none.ToList().Should().BeEmpty();
		}
	}
}
