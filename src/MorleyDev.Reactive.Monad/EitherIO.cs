using MorleyDev.Reactive.Monad.Extensions;
using System;
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

		public static implicit operator EitherIO<L, R>(IO<Either<L, R>> either) => new EitherIO<L, R>(either);

		public static implicit operator EitherIO<L, R>(IO<Either.EitherLeft<L>> either) => new EitherIO<L, R>(either.Select(v => (Either<L, R>)v).ToIO());

		public static implicit operator EitherIO<L, R>(IO<Either.EitherRight<R>> either) => new EitherIO<L, R>(either.Select(v => (Either<L, R>)v).ToIO());

		public static implicit operator IO<Either<L, R>>(EitherIO<L, R> either) => either.ToIO();

		public MaybeIO<L> Lhs() => _either.SelectMany(e => e.Lhs()).ToMaybeIO();

		public MaybeIO<R> Rhs() => _either.SelectMany(e => e.Rhs()).ToMaybeIO();

		public IDisposable Subscribe(IObserver<Either<L, R>> observer) => _either.Subscribe(observer);
	}
}