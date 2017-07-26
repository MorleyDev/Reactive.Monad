using FluentAssertions;
using MorleyDev.Reactive.Monad.Extensions;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class MaybeIOTests
	{
		[Fact]
		public async Task Basic()
		{
			(await MaybeIO.Defer(() => Maybe.Just(10)).ToMaybeIO()).Should().Be(10);
			(await IO.From(() => Maybe.Just(10)).ToMaybeIO()).Should().Be(10);
			(await IO.From(() => 10).ToMaybeIO()).Should().Be(10);
			(await Observable.Return(10).ToMaybeIO()).Should().Be(10);
			(await MaybeIO.From(Observable.Return(10))).Should().Be(10);

			(await Observable.Empty<int>().ToMaybeIO().IsEmpty()).Should().Be(true);
			(await MaybeIO.From<int>(() => Maybe.None).IsEmpty()).Should().Be(true);
			(await IO.From(() => (Maybe<int>)Maybe.None).ToMaybeIO().IsEmpty()).Should().Be(true);
			(await MaybeIO.From(Observable.Empty<int>()).IsEmpty()).Should().Be(true);


			(await MaybeIO.From(Observable.Return(10)).Or(Maybe.Just(20)).SingleAsync()).Should().Be(10);
			(await MaybeIO.From(Observable.Return(10)).Or(Maybe.Just(20)).SingleAsync()).Should().Be(10);
			(await MaybeIO.From(Observable.Return(10)).Or(20).SingleAsync()).Should().Be(10);
			(await MaybeIO.From(Observable.Empty<int>()).Or(Maybe.Just(20)).SingleAsync()).Should().Be(20);
			(await MaybeIO.From(Observable.Empty<int>()).Or(20).SingleAsync()).Should().Be(20);

			(await MaybeIO.From(Observable.Return(10)).Or(MaybeIO.From(Observable.Return(20))).SingleAsync()).Should().Be(10);
			(await MaybeIO.From(Observable.Return(10)).Or(MaybeIO.From(Observable.Throw<int>(new Exception()))).SingleAsync()).Should().Be(10);

			(await MaybeIO.From(Observable.Empty<int>()).Or(MaybeIO.From(Observable.Return(20))).SingleAsync()).Should().Be(20);
		}
	}
}
