using FluentAssertions;
using MorleyDev.Reactive.Monad.Extensions;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class EitherIOTests
	{
		[Fact]
		public async Task BasicMatchTest()
		{
			EitherIO<int, double> left = IO.From(() => Either.Left(10));
			(await left.Lhs().ToList()).Should().BeEquivalentTo(new[] { 10 });
			(await left.Rhs().ToArray()).Should().BeEmpty();

			(await left.Match(x => x * 2.5, y => y * 2)).Should().Be(25.0);
			(await left.Match(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(25.0);
			(await left.Match(x => IO.From(() => x * 2.5), y => IO.From(() => y * 2))).Should().Be(25.0);
			(await left.Match(x => MaybeIO.From(() => Maybe.Just(x * 2.5)), y => MaybeIO.From(() => Maybe.Just(y * 2)))).Should().Be(25.0);
			(await left.Match(x => ManyIO.From(() => Observable.Return(x * 2.5)), y => ManyIO.From(() => Observable.Return(y * 2)))).Should().Be(25.0);

			EitherIO<int, double> right = IO.From(() => Either.Right(25.0));
			(await right.Rhs().ToList()).Should().BeEquivalentTo(new[] { 25.0 });
			(await right.Lhs().ToList()).Should().BeEmpty();

			(await right.Match(x => x * 2.5, y => y * 2)).Should().Be(50.0);
			(await right.Match(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(50.0);
			(await right.Match(x => IO.From(() => x * 2.5), y => IO.From(() => y * 2))).Should().Be(50.0);
			(await right.Match(x => MaybeIO.From(() => Maybe.Just(x * 2.5)), y => MaybeIO.From(() => Maybe.Just(y * 2)))).Should().Be(50.0);
			(await right.Match(x => ManyIO.From(() => Observable.Return(x * 2.5)), y => ManyIO.From(() => Observable.Return(y * 2)))).Should().Be(50.0);
		}
	}
}
