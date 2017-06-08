using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace MorleyDev.Reactive.Monad
{
	public static class Maybe
	{
		public static Maybe<T> Just<T>(T value) => Maybe<T>.Just(value);

		public static Maybe<Unit> Just() => Maybe<Unit>.Just(Unit.Default);
		
		public static MaybeNone None { get; } = MaybeNone.Default;
	}

	public class MaybeNone
	{
		public static readonly MaybeNone Default = new MaybeNone();
	}

	public class Maybe<T> : IEnumerable<T>
	{
		private readonly IEnumerable<T> _value;

		public static Maybe<T> Just(T value) => new Maybe<T>(new[] { value });

		public static Maybe<T> None { get; } = new Maybe<T>(new T[0]);

		/// <summary>
		/// Useful for internal conversions
		/// </summary>
		/// <param name="value"></param>
		public static implicit operator Maybe<T>(LazyValue<T> value) => new Maybe<T>(value);

		/// <summary>
		/// Allows return to be used without needing Maybe.Just
		/// </summary>
		/// <param name="value"></param>
		public static implicit operator Maybe<T>(T value) => Just(value);

		/// <summary>
		/// Allows none to be used without needing to declare the type
		/// </summary>
		/// <param name="none"></param>
		public static implicit operator Maybe<T>(MaybeNone none) => None;

		/// <summary>
		/// Maybe is Lazy so this allows for a lazy maybe
		/// </summary>
		/// <param name="value"></param>
		public static implicit operator Maybe<T>(LazyValue<Maybe<T>> value) => new Maybe<T>(value.SelectMany(maybe => maybe));

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Maybe<T> Or(Maybe<T> lhs, Maybe<T> rhs) => new Maybe<T>(lhs.Concat(rhs).Take(1));

		private Maybe(IEnumerable<T> value)
		{
			_value = value;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _value.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _value.GetEnumerator();
		}
	}
}