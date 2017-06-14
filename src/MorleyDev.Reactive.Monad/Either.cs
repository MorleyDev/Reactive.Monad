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

		private Either(Maybe<Either<L, R>> left, Maybe<Either<L, R>> right)
		{
			_left = left.SelectMany(d => d._left).Concat(right.SelectMany(d => d._left)).ToMaybe();
			_right = left.SelectMany(d => d._right).Concat(right.SelectMany(d => d._right)).ToMaybe();
		}

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

		public static implicit operator Either<L, R>(Either.EitherLeft<L> value) => MakeLeft(value.Value);
		public static implicit operator Either<L, R>(Either.EitherRight<R> value) => MakeRight(value.Value);
		
		public static Either<L, R> MakeLeft(Maybe<L> value) => new Either<L, R>(value, Maybe.None);
		public static Either<L, R> MakeRight(Maybe<R> value) => new Either<L, R>(Maybe.None, value);

		public Maybe<L> Lhs() => _left;
		public Maybe<R> Rhs() => _right;

		public LazyValue<V> Match<V>(Func<L, V> lhs, Func<R, V> rhs)
			=> _left.Select(lhs).Concat(_right.Select(rhs)).ToLazyValue();

		public Maybe<V> Match<V>(Func<L, Maybe<V>> lhs, Func<R, Maybe<V>> rhs)
			=> _left.SelectMany(lhs).Concat(_right.SelectMany(rhs)).ToMaybe();

		public Either<VL, VR> MatchMany<VL, VR>(Func<L, Either<VL, VR>> lhs, Func<R, Either<VL, VR>> rhs)
			=> new Either<VL, VR>(_left.Select(lhs).ToMaybe(), _right.Select(rhs).ToMaybe());

		public Either<VL, VR> MatchMap<VL, VR>(Func<L, VL> lhs, Func<R, VR> rhs)
			=> new Either<VL, VR>(_left.Select(lhs).ToMaybe(), _right.Select(rhs).ToMaybe());
	}

	public static class Either
	{
		public class EitherLeft<T> { public LazyValue<T> Value { get; set; } }

		public class EitherRight<T> { public LazyValue<T> Value { get; set; } }

		public static EitherLeft<T> Left<T>(LazyValue<T> lhs) => new EitherLeft<T> { Value = lhs };

		public static EitherRight<T> Right<T>(LazyValue<T> rhs) => new EitherRight<T> { Value = rhs };

		public static EitherLeft<T> Left<T>(Func<T> lhs) => new EitherLeft<T> { Value = LazyValue.From(lhs) };

		public static EitherRight<T> Right<T>(Func<T> rhs) => new EitherRight<T> { Value = LazyValue.From(rhs) };

		public static EitherLeft<T> Left<T>(T lhs) => new EitherLeft<T> { Value = LazyValue.From(lhs) };

		public static EitherRight<T> Right<T>(T rhs) => new EitherRight<T> { Value = LazyValue.From(rhs) };
	}
}