using System;
using System.Collections;
using System.Collections.Generic;

namespace MorleyDev.Reactive.Monad
{
	/// <summary>
	/// Lazy-evaluated. Does not cache so will constantly re-evaluate, mostly for internal use.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class LazyValue<T> : IEnumerable<T>
	{
		private readonly Func<T> _value;

		public static implicit operator LazyValue<T>(T value) => new LazyValue<T>(() => value);

		public static implicit operator LazyValue<T>(Func<T> value) => new LazyValue<T>(value);

		public static LazyValue<T> From(Func<T> value) => new LazyValue<T>(value);

		private LazyValue(Func<T> value)
		{
			_value = value;
		}

		public T Extract() => _value();

		public IEnumerator<T> GetEnumerator()
		{
			yield return _value();
		}

		public Lazy<T> ToLazy() => new Lazy<T>(_value);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}