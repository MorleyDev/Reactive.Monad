using FluentAssertions;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class EitherIOTests
	{
		[Fact]
		public async Task BasicTest()
		{
			EitherIO<int, double> left = IO.From(() => Either.Left(10));
			(await left.Lhs().ToList()).Should().BeEquivalentTo(new[] { 10 });
			(await left.Rhs().ToArray()).Should().BeEmpty();

			EitherIO<int, double> right = IO.From(() => Either.Right(25.0));
			(await right.Rhs().ToList()).Should().BeEquivalentTo(new[] { 25.0 });
			(await right.Lhs().ToList()).Should().BeEmpty();

		}

		[Fact]
		public async Task MatchTest()
		{
			EitherIO<int, double> left = IO.From(() => Either.Left(10));
			(await left.Match(x => x * 2.5, y => y * 2)).Should().Be(25.0);
			(await left.Match(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(25.0);
			(await left.Match(x => IO.From(() => x * 2.5), y => IO.From(() => y * 2))).Should().Be(25.0);
			(await left.Match(x => MaybeIO.From(() => Maybe.Just(x * 2.5)), y => MaybeIO.From(() => Maybe.Just(y * 2)))).Should().Be(25.0);
			(await left.Match(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(25.0);

			EitherIO<int, double> right = IO.From(() => Either.Right(25.0));
			(await right.Match(x => x * 2.5, y => y * 2)).Should().Be(50.0);
			(await right.Match(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(50.0);
			(await right.Match(x => IO.From(() => x * 2.5), y => IO.From(() => y * 2))).Should().Be(50.0);
			(await right.Match(x => MaybeIO.From(() => Maybe.Just(x * 2.5)), y => MaybeIO.From(() => Maybe.Just(y * 2)))).Should().Be(50.0);
			(await right.Match(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(50.0);
		}

		[Fact]
		public async Task MatchMapTest()
		{
			EitherIO<int, double> left = IO.From(() => Either.Left(10));
			(await left.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Lhs()).Should().Be("10");
			(await left.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Rhs().IsEmpty()).Should().Be(true);

			EitherIO<int, double> right = IO.From(() => Either.Right(25.0));
			(await right.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Rhs()).Should().Be(10);
			(await right.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Lhs().IsEmpty()).Should().Be(true);

			(await left.MatchMap(l => IO.Run(() => l.ToString()), r => IO.Run(() => (int)(r / 2.5))).Lhs()).Should().Be("10");
			(await left.MatchMap(l => IO.Run(() => l.ToString()), r => IO.Run(() => (int)(r / 2.5))).Rhs().IsEmpty()).Should().Be(true);

			(await right.MatchMap(l => IO.Run(() => l.ToString()), r => IO.Run(() => (int)(r / 2.5))).Rhs()).Should().Be(10);
			(await right.MatchMap(l => IO.Run(() => l.ToString()), r => IO.Run(() => (int)(r / 2.5))).Lhs().IsEmpty()).Should().Be(true);
		}

		[Fact]
		public async Task MatchManyTest()
		{
			EitherIO<int, double> left = IO.From(() => Either.Left(10));
			(await left.MatchMany<string, double>(e => Either.Left(e.ToString()), e => Either.Right(e * 0.5)).Lhs()).Should().Be("10");
			(await left.MatchMany<string, double>(e => Either.Left(e.ToString()), e => Either.Right(e * 0.5)).Rhs().IsEmpty()).Should().Be(true);
			(await left.MatchMany<double, string>(e => Either.Right(e.ToString()), e => Either.Left(e * 0.5)).Rhs()).Should().Be("10");
			(await left.MatchMany<double, string>(e => Either.Right(e.ToString()), e => Either.Left(e * 0.5)).Lhs().IsEmpty()).Should().Be(true);

			EitherIO<double, int> right = IO.From(() => Either.Right(10));
			(await right.MatchMany<string, int>(e => Either.Left(e.ToString()), e => Either.Right((int)(e * 0.5))).Rhs()).Should().Be(5);
			(await right.MatchMany<string, int>(e => Either.Left(e.ToString()), e => Either.Right((int)(e * 0.5))).Lhs().IsEmpty()).Should().Be(true);
			(await right.MatchMany<int, string>(e => Either.Right(e.ToString()), e => Either.Left((int)(e * 0.5))).Lhs()).Should().Be(5);
			(await right.MatchMany<int, string>(e => Either.Right(e.ToString()), e => Either.Left((int)(e * 0.5))).Rhs().IsEmpty()).Should().Be(true);
		}
	}
}
