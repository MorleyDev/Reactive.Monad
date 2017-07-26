using System;
using System.Linq;

namespace MorleyDev.Reactive.Monad.Extra
{
	public class Either<L, R>
	{
		private readonly Maybe<L> _left;
		private readonly Maybe<R> _right;

		private Either(Maybe<Either<L, R>> left, Maybe<Either<L, R>> right)
		{
			_left = Maybe.Defer(left.AsEnumerable().SelectMany(d => d._left).Concat(right.AsEnumerable().SelectMany(d => d._left)).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single);
			_right = Maybe.Defer(left.AsEnumerable().SelectMany(d => d._right).Concat(right.AsEnumerable().SelectMany(d => d._right)).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single);
		}

		private Either(Maybe<L> left, Maybe<R> right)
		{
			_left = left;
			_right = right;
		}

		public static implicit operator Either<L, R>(R value) => Right(value);
		public static implicit operator Either<L, R>(Func<R> value) => LazyValue<R>.From(value);
		public static implicit operator Either<L, R>(LazyValue<R> value) => Right(value);
		public static implicit operator Either<L, R>(Either.EitherRight<R> value) => Right(value.Value);

		public static implicit operator Either<L, R>(Func<L> value) => LazyValue<L>.From(value);
		public static implicit operator Either<L, R>(LazyValue<L> value) => Left(value);
		public static implicit operator Either<L, R>(L value) => Left(value);
		public static implicit operator Either<L, R>(Either.EitherLeft<L> value) => Left(value.Value);
		
		public static Either<L, R> Left(Maybe<L> value) => new Either<L, R>(value, Maybe.None);
		public static Either<L, R> Right(Maybe<R> value) => new Either<L, R>(Maybe.None, value);

		public Maybe<L> Lhs() => _left;
		public Maybe<R> Rhs() => _right;

		public LazyValue<V> Match<V>(Func<L, V> lhs, Func<R, V> rhs)
			=> LazyValue.Defer(_left.AsEnumerable().Select(lhs).Concat(_right.AsEnumerable().Select(rhs)).Single);

		public Maybe<V> Match<V>(Func<L, Maybe<V>> lhs, Func<R, Maybe<V>> rhs)
			=> Maybe.Defer(_left.AsEnumerable().SelectMany(lhs).Concat(_right.AsEnumerable().SelectMany(rhs)).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single);

		public Either<VL, VR> MatchMany<VL, VR>(Func<L, Either<VL, VR>> lhs, Func<R, Either<VL, VR>> rhs)
			=> new Either<VL, VR>(
				Maybe.Defer(_left.AsEnumerable().Select(lhs).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single),
				Maybe.Defer(_right.AsEnumerable().Select(rhs).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single)
			);

		public Either<VL, VR> MatchMap<VL, VR>(Func<L, VL> lhs, Func<R, VR> rhs)
			=> new Either<VL, VR>(
				Maybe.Defer(_left.AsEnumerable().Select(lhs).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single),
				Maybe.Defer(_right.AsEnumerable().Select(rhs).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).Single)
			);
	}

	public static class Either
	{
		public class EitherLeft<T> { public LazyValue<T> Value { get; set; } }

		public class EitherRight<T> { public LazyValue<T> Value { get; set; } }

		public static EitherLeft<T> Left<T>(LazyValue<T> lhs) => new EitherLeft<T> { Value = lhs };

		public static EitherRight<T> Right<T>(LazyValue<T> rhs) => new EitherRight<T> { Value = rhs };

		public static EitherLeft<T> Left<T>(Func<T> lhs) => new EitherLeft<T> { Value = LazyValue.Defer(lhs) };

		public static EitherRight<T> Right<T>(Func<T> rhs) => new EitherRight<T> { Value = LazyValue.Defer(rhs) };

		public static EitherLeft<T> Left<T>(T lhs) => new EitherLeft<T> { Value = LazyValue.Return(lhs) };

		public static EitherRight<T> Right<T>(T rhs) => new EitherRight<T> { Value = LazyValue.Return(rhs) };
	}
}