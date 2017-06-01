using MorleyDev.Reactive.Monad.Extensions;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace MorleyDev.Reactive.Monad
{
	public class Either<L, R>
	{
		private readonly Maybe<L> _left;
		private readonly Maybe<R> _right;

		private Either(Maybe<L> left, Maybe<R> right)
		{
			_left = left;
			_right = right;
		}

		public static implicit operator Either<L, R>(L value) => MakeLeft(value);
		public static implicit operator Either<L, R>(R value) => MakeRight(value);

		public static implicit operator Either<L, R>(Func<L> value) => LazyValue<L>.From(value);
		public static implicit operator Either<L, R>(Func<R> value) => LazyValue<R>.From(value);

		public static implicit operator Either<L, R>(LazyValue<L> value) => MakeLeft(value);
		public static implicit operator Either<L, R>(LazyValue<R> value) => MakeRight(value);

		public static Either<L, R> MakeLeft(Maybe<L> value) => new Either<L, R>(value, Maybe.None);
		public static Either<L, R> MakeRight(Maybe<R> value) => new Either<L, R>(Maybe.None, value);

		public Maybe<L> Lhs() => _left;
		public Maybe<R> Rhs() => _right;

		public LazyValue<V> Match<V>(Func<L, V> lhs, Func<R, V> rhs)
			=> _left.Select(lhs).Concat(_right.Select(rhs)).ToLazyValue();

		public Maybe<V> Match<V>(Func<L, Maybe<V>> lhs, Func<R, Maybe<V>> rhs)
			=> _left.SelectMany(lhs).Concat(_right.SelectMany(rhs)).ToMaybe();
	}
}