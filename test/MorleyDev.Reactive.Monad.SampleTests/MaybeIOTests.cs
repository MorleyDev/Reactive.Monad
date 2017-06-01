using FluentAssertions;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MorleyDev.Reactive.Monad.SampleTests
{
	public class MaybeIOTests
	{
		[Fact]
		public async Task Basic()
		{
			(await MaybeIO.From(() => Maybe.Just(10)).ToMaybeIO()).Should().Be(10);
			(await IO.From(() => Maybe.Just(10)).ToMaybeIO()).Should().Be(10);
			(await IO.From(() => 10).ToMaybeIO()).Should().Be(10);
			(await Observable.Return(10).ToMaybeIO()).Should().Be(10);
			(await MaybeIO.From(Observable.Return(10))).Should().Be(10);

			(await Observable.Empty<int>().ToMaybeIO().IsEmpty()).Should().Be(true);
			(await MaybeIO.From<int>(() => Maybe.None).IsEmpty()).Should().Be(true);
			(await IO.From(() => (Maybe<int>)Maybe.None).ToMaybeIO().IsEmpty()).Should().Be(true);
			(await MaybeIO.From(Observable.Empty<int>()).IsEmpty()).Should().Be(true);
		}
	}
}
