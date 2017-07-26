using System;
using System.Linq;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad.Extra
{
	public static class EitherIO
	{
		public static MaybeIO<L> Lhs<L, R>(this IO<Either<L, R>> self) => self.SelectMany(s => s.Lhs());

		public static MaybeIO<R> Rhs<L, R>(this IO<Either<L, R>> self) => self.SelectMany(s => s.Rhs());

		public static IO<U> Match<U, L, R>(this IO<Either<L,R>> self, Func<L, U> lhs, Func<R, U> rhs)
		{
			return IO.From(self.AsObservable().SelectMany(either => either.Match(lhs, rhs)));
		}

		public static IO<U> MatchMany<U, L, R>(this IO<Either<L, R>> self, Func<L, IO<U>> lhs, Func<R, IO<U>> rhs)
		{
			return IO.From(self.AsObservable().SelectMany(either => either.Match(lhs, rhs).Merge()));
		}

		public static MaybeIO<U> MatchMany<U, L, R>(this IO<Either<L, R>> self, Func<L, MaybeIO<U>> lhs, Func<R, MaybeIO<U>> rhs)
		{
			return MaybeIO.From(self.AsObservable().SelectMany(either => either.Match(lhs, rhs).Merge()));
		}

		public static IObservable<U> MatchMany<U, L, R>(this IO<Either<L, R>> self, Func<L, IObservable<U>> lhs, Func<R, IObservable<U>> rhs)
		{
			return self.AsObservable().SelectMany(either => either.Match(lhs, rhs).Merge());
		}

		public static IO<Either<UL, UR>> MatchMap<UL, UR, L, R>(this IO<Either<L, R>> self, Func<L, UL> lhs, Func<R, UR> rhs)
		{
			return IO.From(self.AsObservable().Select(either => either.MatchMap(lhs, rhs)));
		}

		public static IO<Either<UL, UR>> MatchMap<UL, UR, L, R>(this IO<Either<L, R>> self, Func<L, Either<UL, UR>> lhs, Func<R, Either<UL, UR>> rhs)
		{
			return IO.From(self.AsObservable().Select(either => either.MatchMany(lhs, rhs)));
		}

		public static IO<Either<UL, UR>> MatchMap<UL, UR, L, R>(this IO<Either<L, R>> self, Func<L, Either.EitherLeft<UL>> lhs, Func<R, Either.EitherRight<UR>> rhs)
		{
			return IO.From(self.AsObservable().Select(either => either.MatchMany(v => (Either<UL, UR>)lhs(v), v => (Either<UL, UR>)rhs(v))));
		}

		public static IO<Either<UL, UR>> MatchMap<UL, UR, L, R>(this IO<Either<L, R>> self, Func<L, Either.EitherRight<UR>> lhs, Func<R, Either.EitherLeft<UL>> rhs)
		{
			return IO.From(self.AsObservable().Select(either => either.MatchMany(v => (Either<UL, UR>)lhs(v), v => (Either<UL, UR>)rhs(v))));
		}

		public static IO<Either<UL, UR>> MatchManyMap<UL, UR, L, R>(this IO<Either<L, R>> self, Func<L, IO<UL>> lhs, Func<R, IO<UR>> rhs)
		{
			return IO.From(
				self.AsObservable()
					.SelectMany(either => IO<Either<UL, UR>>.From(either
						.MatchMap(lhs, rhs)
						.Match(
							lhs2 => lhs2.Select(v => Either<UL, UR>.Left(Maybe.Just(v))), 
							rhs2 => rhs2.Select(v => Either<UL,UR>.Right(Maybe.Just(v)))
						)
						.ToObservable()
						.SelectMany(v => v)
					)
				)
			);
		}

		public static IO<Either<UL, UR>> MatchManyMap<UL, UR, L, R>(this IO<Either<L, R>> self, Func<L, IO<Either<UL, UR>>> lhs, Func<R, IO<Either<UL, UR>>> rhs)
		{
			return IO.From(self.AsObservable().SelectMany(either => either.Match(lhs, rhs)).Merge());
		}
	}
}