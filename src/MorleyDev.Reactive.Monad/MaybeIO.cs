using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MorleyDev.Reactive.Monad.Extensions;

namespace MorleyDev.Reactive.Monad
{
	public static class MaybeIO
	{
		public static MaybeIO<T> From<T>(Func<IObservable<T>> self) => Observable.Defer(self).Select(Maybe.Just).DefaultIfEmpty(Maybe.None).ToIO();

		public static MaybeIO<T> From<T>(IObservable<T> self) => self.Select(Maybe.Just).DefaultIfEmpty(Maybe.None).ToIO();

		public static MaybeIO<T> From<T>(Func<Maybe<T>> self) => IO.From(self);

		public static MaybeIO<T> From<T>(Func<Task<Maybe<T>>> self) => IO.From(self);

		public static MaybeIO<T> ToMaybeIO<T>(this IObservable<T> self) => self.Select(Maybe.Just).DefaultIfEmpty(Maybe.None).ToIO();

		public static MaybeIO<T> ToMaybeIO<T>(this IO<Maybe<T>> self) => self;
	}

	/// <summary>
	/// MaybeIO encapsulates an IO operation that can return 0-1 results
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MaybeIO<T> : IObservable<T>
	{
		private readonly IO<Maybe<T>> _unsafeIO;

		private MaybeIO(IO<Maybe<T>> unsafeIO)
		{
			_unsafeIO = unsafeIO;
		}

		/// <summary>
		/// MaybeIO is equivalent to IO<Maybe>.Merge() and Vice-Versa
		/// </summary>
		/// <param name="self"></param>
		public static implicit operator IO<Maybe<T>>(MaybeIO<T> self) => self._unsafeIO;

		/// <summary>
		/// MaybeIO is equivalent to IO<Maybe>.Merge() and Vice-Versa
		/// </summary>
		/// <param name="self"></param>
		public static implicit operator MaybeIO<T>(IO<Maybe<T>> self) => new MaybeIO<T>(self);

		public static MaybeIO<T> From(IO<Maybe<T>> unsafeIO) => new MaybeIO<T>(unsafeIO);

		public IDisposable Subscribe(IObserver<T> observer)
		{
			return _unsafeIO.SelectMany(maybe => maybe).Subscribe(observer);
		}

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(MaybeIO<T> lhs, MaybeIO<T> rhs) => lhs.Concat(rhs).Take(1).ToMaybeIO();

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(MaybeIO<T> lhs, Maybe<T> rhs) => lhs.Concat(rhs.ToObservable()).Take(1).ToMaybeIO();

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(Maybe<T> lhs, MaybeIO<T> rhs) => lhs.ToObservable().Concat(rhs).Take(1).ToMaybeIO();

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(Maybe<T> lhs, IO<T> rhs) => lhs.ToObservable().Concat(rhs).Take(1).ToMaybeIO();


		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(MaybeIO<T> lhs, IO<T> rhs) => lhs.Concat(rhs).Take(1).ToMaybeIO();
	}
}
