using FluentAssertions;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;
using MorleyDev.Reactive.Monad.Extra;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class EitherIOTests
	{
		[Fact]
		public async Task BasicTest()
		{
			IO<Either<int, double>> left = MonadicAsync.Run(() => (Either<int, double>)Either.Left(10));
			(await left.Lhs().ToList()).Should().BeEquivalentTo(new[] { 10 });
			(await left.Rhs().ToArray()).Should().BeEmpty();

			IO<Either<int, double>> right = MonadicAsync.Run(() => (Either<int, double>)Either.Right(25.0));
			(await right.Rhs().ToList()).Should().BeEquivalentTo(new[] { 25.0 });
			(await right.Lhs().ToList()).Should().BeEmpty();

		}

		[Fact]
		public async Task MatchTest()
		{
			IO<Either<int, double>> left = MonadicAsync.Run(() => (Either<int, double>)Either.Left(10));
			(await left.Match(x => x * 2.5, y => y * 2)).Should().Be(25.0);

			IO<Either<int, double>> right = MonadicAsync.Run(() => (Either<int, double>)Either.Right(25.0));
			(await right.Match(x => x * 2.5, y => y * 2)).Should().Be(50.0);
		}

		[Fact]
		public async Task MatchMapTest()
		{
			IO<Either<int, double>> left = MonadicAsync.Run(() => (Either<int, double>)Either.Left(10));
			(await left.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Lhs()).Should().Be("10");
			(await left.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Rhs().IsEmpty()).Should().Be(true);

			IO<Either<int, double>> right = MonadicAsync.Run(() => (Either<int, double>)Either.Right(25.0));
			(await right.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Rhs()).Should().Be(10);
			(await right.MatchMap(l => l.ToString(), r => (int)(r / 2.5)).Lhs().IsEmpty()).Should().Be(true);


			(await left.MatchMap(e => Either.Left(e.ToString()), e => Either.Right(e * 0.5)).Lhs()).Should().Be("10");
			(await left.MatchMap(e => Either.Left(e.ToString()), e => Either.Right(e * 0.5)).Rhs().IsEmpty()).Should().Be(true);
			(await left.MatchMap(e => Either.Right(e.ToString()), e => Either.Left(e * 0.5)).Rhs()).Should().Be("10");
			(await left.MatchMap(e => Either.Right(e.ToString()), e => Either.Left(e * 0.5)).Lhs().IsEmpty()).Should().Be(true);

			(await right.MatchMap(e => Either.Left(e.ToString()), e => Either.Right((int)(e * 0.5))).Rhs()).Should().Be(12);
			(await right.MatchMap(e => Either.Left(e.ToString()), e => Either.Right((int)(e * 0.5))).Lhs().IsEmpty()).Should().Be(true);
			(await right.MatchMap(e => Either.Right(e.ToString()), e => Either.Left((int)(e * 0.5))).Lhs()).Should().Be(12);
			(await right.MatchMap(e => Either.Right(e.ToString()), e => Either.Left((int)(e * 0.5))).Rhs().IsEmpty()).Should().Be(true);
		}

		[Fact]
		public async Task MatchManyTest()
		{
			IO<Either<int, double>> left = MonadicAsync.Run(() => (Either<int, double>)Either.Left(10));
			(await left.MatchMany(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(25.0);
			(await left.MatchMany(x => MonadicAsync.Run(() => x * 2.5), y => IO.Run(() => y * 2))).Should().Be(25.0);
			(await left.MatchMany(x => MonadicAsync.Run(() => Maybe.Just(x * 2.5)), y => MonadicAsync.Run(() => Maybe.Just(y * 2)))).Should().Be(25.0);
			(await left.MatchMany(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(25.0);

			IO<Either<int, double>> right = MonadicAsync.Run(() => (Either<int, double>)Either.Right(25.0));
			(await right.MatchMany(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(50.0);
			(await right.MatchMany(x => MonadicAsync.Run(() => x * 2.5), y => MonadicAsync.Run(() => y * 2))).Should().Be(50.0);
			(await right.MatchMany(x => MonadicAsync.Run(() => Maybe.Just(x * 2.5)), y => MonadicAsync.Run(() => Maybe.Just(y * 2)))).Should().Be(50.0);
			(await right.MatchMany(x => Observable.Return(x * 2.5), y => Observable.Return(y * 2))).Should().Be(50.0);
		}

		[Fact]
		public async Task MatchManyMapTest()
		{
			IO<Either<int, double>> left = MonadicAsync.Run(() => (Either<int, double>)Either.Left(10));
			(await left.MatchManyMap(l => MonadicAsync.Run(() => l.ToString()), r => MonadicAsync.Run(() => (int)(r / 2.5))).Lhs()).Should().Be("10");
			(await left.MatchManyMap(l => MonadicAsync.Run(() => l.ToString()), r => MonadicAsync.Run(() => (int)(r / 2.5))).Rhs().IsEmpty()).Should().Be(true);

			IO<Either<int, double>> right = MonadicAsync.Run(() => (Either<int, double>)Either.Right(25.0));
			(await right.MatchManyMap(l => MonadicAsync.Run(() => l.ToString()), r => IO.Run(() => (int)(r / 2.5))).Rhs()).Should().Be(10);
			(await right.MatchManyMap(l => MonadicAsync.Run(() => l.ToString()), r => MonadicAsync.Run(() => (int)(r / 2.5))).Lhs().IsEmpty()).Should().Be(true);
		}
	}
}
