using FluentAssertions;
using MorleyDev.Reactive.Monad.Extensions;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class MaybeTests
	{
		[Fact]
		public async Task Basic()
		{
			Maybe<int> maybe = 10;
			maybe.ToList().ShouldBeEquivalentTo(new[] { 10 });

			Maybe<int> v = Maybe.Just(20);
			v.Select(t => t * 20).ToList().ShouldBeEquivalentTo(new[] { 400 });

			Maybe<int> none = Maybe.None;
			none.ToList().Should().BeEmpty();

			Maybe<int>.Just(10).Or(Maybe.Just(20)).Single().Should().Be(10);
			Maybe<int>.None.Or(Maybe.Just(20)).Single().Should().Be(20);
			Maybe<int>.Just(10).Or(20).Single().Should().Be(10);

			Maybe<int>.Just(10).Or(LazyValue<int>.From(() => 20)).Single().Should().Be(10);
			Maybe<int>.None.Or(LazyValue<int>.From(() => 20)).Single().Should().Be(20);
			Maybe<int>.Just(10).Or(LazyValue<int>.From(() => throw new Exception())).Single().Should().Be(10);

			(await Maybe<int>.Just(10).Or(MaybeIO.From(() => Observable.Return(20)))).Should().Be(10);
			(await Maybe<int>.Just(10).Or(MaybeIO.From(() => Observable.Throw<int>(new Exception())))).Should().Be(10);
			(await Maybe<int>.None.Or(MaybeIO.From(() => Observable.Return(20)))).Should().Be(20);

			(await Maybe<int>.None.Or(IO.Run(() => 10))).Should().Be(10);
			(await Maybe<int>.Just(10).Or(IO.Run(() => 20))).Should().Be(10);

			(await Maybe<int>.Just(10).Or(IO.Run<int>(() => throw new Exception()))).Should().Be(10);
			(await Maybe<int>.None.Or(IO.Run(() => 10))).Should().Be(10);
			(await Maybe<int>.Just(10).Or(IO.Run(() => 20))).Should().Be(10);
		}
	}
}
