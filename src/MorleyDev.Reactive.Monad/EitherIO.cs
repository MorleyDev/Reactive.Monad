using MorleyDev.Reactive.Monad.Extensions;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad
{
	public class EitherIO<L, R> : IObservable<Either<L, R>>
	{
		private readonly IO<Either<L, R>> _either;

		private EitherIO(IO<Either<L, R>> either)
		{
			_either = either;
		}

		private EitherIO(MaybeIO<L> lhs, MaybeIO<R> rhs)
		{
			_either = lhs.Select(l => Either<L, R>.MakeLeft(l)).Concat(rhs.Select(r => Either<L, R>.MakeRight(r))).ToIO();
		}

		public static implicit operator EitherIO<L, R>(IO<Either<L, R>> either) => new EitherIO<L, R>(either);
		public static implicit operator IO<Either<L, R>>(EitherIO<L, R> either) => either._either;

		public static EitherIO<L, R> From(Either<IO<L>, IO<R>> source) => new EitherIO<L, R>(source.Lhs().ToMaybeIO(), source.Rhs().ToMaybeIO());

		public static implicit operator EitherIO<L, R>(IO<Either.EitherLeft<L>> either) => new EitherIO<L, R>(either.Select(v => (Either<L, R>)v).ToIO());
		public static implicit operator EitherIO<L, R>(IO<Either.EitherRight<R>> either) => new EitherIO<L, R>(either.Select(v => (Either<L, R>)v).ToIO());

		public MaybeIO<L> Lhs() => _either.SelectMany(e => e.Lhs()).ToMaybeIO();

		public MaybeIO<R> Rhs() => _either.SelectMany(e => e.Rhs()).ToMaybeIO();

		public IDisposable Subscribe(IObserver<Either<L, R>> observer) => _either.Subscribe(observer);


		public IO<U> Match<U>(Func<L, U> lhs, Func<R, U> rhs)
		{
			return this.SelectMany(either => either.Match(lhs, rhs)).ToIO();
		}

		public IO<U> Match<U>(Func<L, IO<U>> lhs, Func<R, IO<U>> rhs)
		{
			return this.SelectMany(either => either.Match(lhs, rhs).Merge()).ToIO();
		}

		public ManyIO<U> Match<U>(Func<L, ManyIO<U>> lhs, Func<R, ManyIO<U>> rhs)
		{
			return this.SelectMany(either => either.Match(lhs, rhs).Merge()).ToManyIO();
		}

		public MaybeIO<U> Match<U>(Func<L, MaybeIO<U>> lhs, Func<R, MaybeIO<U>> rhs)
		{
			return this.SelectMany(either => either.Match(lhs, rhs).Merge()).ToMaybeIO();
		}

		public IObservable<U> Match<U>(Func<L, IObservable<U>> lhs, Func<R, IObservable<U>> rhs)
		{
			return this.SelectMany(either => either.Match(lhs, rhs).Merge());
		}

		public EitherIO<UL, UR> MatchMap<UL, UR>(Func<L, UL> lhs, Func<R, UR> rhs)
		{
			return this.Select(either => either.MatchMap(lhs, rhs)).ToIO();
		}

		public EitherIO<UL, UR> MatchMap<UL, UR>(Func<L, IO<UL>> lhs, Func<R, IO<UR>> rhs)
		{
			return this.SelectMany(either => EitherIO<UL, UR>.From(either.MatchMap(lhs, rhs))).ToIO();
		}

		public EitherIO<UL, UR> MatchMany<UL, UR>(Func<L, Either<UL, UR>> lhs, Func<R, Either<UL, UR>> rhs)
		{
			return this.Select(either => either.MatchMany(lhs, rhs)).ToIO();
		}

		public EitherIO<UL, UR> MatchMany<UL, UR>(Func<L, EitherIO<UL, UR>> lhs, Func<R, EitherIO<UL, UR>> rhs)
		{
			return this.SelectMany(either => either.Match(lhs, rhs)).Merge().ToIO();
		}
	}
}