using System;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class EitherIOExtensions
	{
		public static IO<U> Match<L, R, U>(this EitherIO<L,R> self, Func<L, U> lhs, Func<R, U> rhs)
		{
			return self.SelectMany(either => either.Match(lhs, rhs)).ToIO();
		}

		public static IO<U> Match<L, R, U>(this EitherIO<L, R> self, Func<L, IO<U>> lhs, Func<R, IO<U>> rhs)
		{
			return self.SelectMany(either => either.Match(lhs, rhs).Merge()).ToIO();
		}

		public static ManyIO<U> Match<L, R, U>(this EitherIO<L, R> self, Func<L, ManyIO<U>> lhs, Func<R, ManyIO<U>> rhs)
		{
			return self.SelectMany(either => either.Match(lhs, rhs).Merge()).ToManyIO();
		}

		public static MaybeIO<U> Match<L, R, U>(this EitherIO<L, R> self, Func<L, MaybeIO<U>> lhs, Func<R, MaybeIO<U>> rhs)
		{
			return self.SelectMany(either => either.Match(lhs, rhs).Merge()).ToMaybeIO();
		}

		public static IObservable<U> Match<L, R, U>(this EitherIO<L, R> self, Func<L, IObservable<U>> lhs, Func<R, IObservable<U>> rhs)
		{
			return self.SelectMany(either => either.Match(lhs, rhs).Merge());
		}
	}
}
