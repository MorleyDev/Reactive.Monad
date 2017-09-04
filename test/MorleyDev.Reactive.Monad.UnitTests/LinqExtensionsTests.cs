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
			var io = MonadicAsync.Run(() => 10)
				.Select(v => v * 10.0)
				.SelectMany(v => MonadicAsync.Run(() => v * 10));

			(await io.IsEmpty()).Should().Be(false);
			(await io).Should().Be(1000.0);
			(await io.Where(v => v == 1000.0)).Should().Be(1000.0);
			(await io.Where(v => v != 1000.0).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => Maybe.Just(v * 10))).Should().Be(10000);
			(await io.SelectMany(v => MonadicAsync.Run(() => Maybe.Just((int)(v * 10))))).Should().Be(10000);
			(await io.SelectMany(v => MonadicAsync.Run(() => Maybe.Just((int)(v * 10))))).Should().Be(10000);
			(await io.SelectMany(v => (Maybe<int>)Maybe.None).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => MonadicAsync.Run(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => MonadicAsync.Run(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);
			(await io.SelectMany(v => new[] { (int)v * 1, (int)v * 2, (int)v * 3 }.ToObservable()).ToListIO()).Should().BeEquivalentTo(new[] { 1000, 2000, 3000 });
		}

		[Fact]
		public async Task MaybeIOTests()
		{
			var some = MonadicAsync.Run(() => Maybe.Just(10.0))
				.Select(v => (int)v * 10)
				.SelectMany(v => IO.Run(() => v * 10.0));
			var none = MonadicAsync.Run(() => (Maybe<int>)Maybe.None)
				.Select(v => (decimal)v * 10)
				.SelectMany(v => IO.Run(() => (double)v * 10));

			(await some).Should().Be(1000.0);
			(await none.IsEmpty()).Should().Be(true);
			(await some.Where(v => v == 1000.0)).Should().Be(1000.0);
			(await some.Where(v => v != 1000.0).IsEmpty()).Should().Be(true);
			(await none.Where(v => true).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => Maybe.Just((int)(v * 2.5)))).Should().Be(2500);
			(await some.SelectMany(v => (Maybe<int>)Maybe.None).IsEmpty()).Should().Be(true);
			(await some.SelectMany(v => LazyValue.Return(v * 2))).Should().Be(2000);
			(await none.SelectMany(v => LazyValue.Return(v * 2)).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => MonadicAsync.Run(() => Maybe.Just((int)(v * 2.5))))).Should().Be(2500);
			(await some.SelectMany(v => MonadicAsync.Run(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => MonadicAsync.Run(() => Maybe.Just((int)(v * 2.5))))).Should().Be(2500);
			(await some.SelectMany(v => MonadicAsync.Run(() => (Maybe<int>)Maybe.None)).IsEmpty()).Should().Be(true);

			(await some.SelectMany(v => (new[] { (int)v * 1, (int)v * 2, (int)v * 3 }.ToObservable())).ToListIO()).Should().BeEquivalentTo(new[] { 1000, 2000, 3000 });
			(await none.SelectMany(v => (new[] { (int)v * 1, (int)v * 2, (int)v * 3 }.ToObservable())).IsEmptyIO()).Should().Be(true);

			(await some.DefaultIfEmpty(25.5)).Should().Be(1000.0);
			(await none.DefaultIfEmpty(25.5)).Should().Be(25.5);
		}
	}
}
