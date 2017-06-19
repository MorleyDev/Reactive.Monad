using System.Threading.Tasks;
using Xunit;
using System.Reactive.Linq;
using FluentAssertions;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class LinqExtensionsTests
	{
		[Fact]
		public async Task IOTests()
		{
			var io = IO.From(() => 10)
				.Select(v => v * 10.0)
				.SelectMany(v => IO.From(() => v * 10));

			(await io.IsEmpty()).Should().Be(false);
			(await io).Should().Be(1000.0);
			(await io.Where(v => v == 1000.0)).Should().Be(1000.0);
			(await io.Where(v => v != 1000.0).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => Maybe.Just(v * 10))).Should().Be(10000);
			(await io.SelectMany(v => MaybeIO.From(() => Maybe.Just((int)(v * 10))))).Should().Be(10000);
			(await io.SelectMany(v => IO.From(() => Maybe.Just((int)(v * 10))))).Should().Be(10000);
			(await io.SelectMany(v => (Maybe<int>)Maybe.None).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => IO.From(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => MaybeIO.From(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => ManyIO.From(new[] { (int)v * 1, (int)v * 2, (int)v * 3 }.ToObservable())).ToList()).Should().BeEquivalentTo(new[] { 1000, 2000, 3000 });
		}

		[Fact]
		public async Task MaybeIOTests()
		{
			var some = MaybeIO.From(() => Maybe.Just(10.0))
				.Select(v => (int)v * 10)
				.SelectMany(v => IO.From(() => v * 10.0));
			var none = MaybeIO.From(() => (Maybe<int>)Maybe.None)
				.Select(v => (decimal)v * 10)
				.SelectMany(v => IO.From(() => (double)v * 10));

			(await some).Should().Be(1000.0);
			(await none.IsEmpty()).Should().Be(true);
			(await some.Where(v => v == 1000.0)).Should().Be(1000.0);
			(await some.Where(v => v != 1000.0).IsEmpty()).Should().Be(true);
			(await none.Where(v => true).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => Maybe.Just((int)(v * 2.5)))).Should().Be(2500);
			(await some.SelectMany(v => (Maybe<int>)Maybe.None).IsEmpty()).Should().Be(true);
			(await some.SelectMany(v => LazyValue.From(v * 2))).Should().Be(2000);
			(await none.SelectMany(v => LazyValue.From(v * 2)).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => IO.From(() => Maybe.Just((int)(v * 2.5))))).Should().Be(2500);
			(await some.SelectMany(v => IO.From(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => MaybeIO.From(() => Maybe.Just((int)(v * 2.5))))).Should().Be(2500);
			(await some.SelectMany(v => MaybeIO.From(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => ManyIO.From(new[] { (int)v * 1, (int)v * 2, (int)v * 3 }.ToObservable())).ToList()).Should().BeEquivalentTo(new[] { 1000, 2000, 3000 });
			(await none.SelectMany(v => ManyIO.From(new[] { (int)v * 1, (int)v * 2, (int)v * 3 }.ToObservable())).IsEmpty()).Should().Be(true);

			(await some.DefaultIfEmpty(25.5)).Should().Be(1000.0);
			(await none.DefaultIfEmpty(25.5)).Should().Be(25.5);
		}
	}
}
