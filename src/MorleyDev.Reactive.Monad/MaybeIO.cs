using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive;

namespace MorleyDev.Reactive.Monad
{
	public static class MaybeIO
	{
		public static MaybeIO<T> Defer<T>(Func<IObservable<T>> self) => IO.Defer(() => self().Select(Maybe.Just).DefaultIfEmpty(Maybe.None));

		public static MaybeIO<T> Defer<T>(Func<Task<Maybe<T>>> self) => IO.Defer(self);

		public static MaybeIO<T> From<T>(IObservable<T> self) => IO.From(self.Select(Maybe.Just).DefaultIfEmpty(Maybe.None));

		public static MaybeIO<T> Run<T>(Func<Maybe<T>> self) => IO.Run(self);

		public static MaybeIO<Unit> Run<T>(Func<MaybeNone> self) => Run(() => (Maybe<Unit>)self());
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

		public static MaybeIO<T> None => new MaybeIO<T>(IO.From(Observable.Return(Maybe<T>.None)));

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

		public IObservable<T> AsObservable() => this;

		public IDisposable Subscribe(IObserver<T> observer)
		{
			return _unsafeIO.AsObservable().SelectMany(maybe => maybe).Subscribe(observer);
		}

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(MaybeIO<T> lhs, MaybeIO<T> rhs) => IO.From(lhs.Concat(rhs).Take(1).Select(Maybe.Just).DefaultIfEmpty(Maybe.None));

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(MaybeIO<T> lhs, Maybe<T> rhs) => IO.From(lhs.Concat(rhs.ToObservable()).Take(1).Select(Maybe.Just).DefaultIfEmpty(Maybe.None));

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(Maybe<T> lhs, MaybeIO<T> rhs) => IO.From(lhs.ToObservable().Concat(rhs).Take(1).Select(Maybe.Just).DefaultIfEmpty(Maybe.None));

		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(Maybe<T> lhs, IO<T> rhs) => IO.From(lhs.ToObservable().Concat(rhs).Take(1).Select(Maybe.Just).DefaultIfEmpty(Maybe.None));


		/// <summary>Retrieve the option on the left if it has a value, the otherwise the option on the right (None if both are empty)</summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static MaybeIO<T> Or(MaybeIO<T> lhs, IO<T> rhs) => IO.From(lhs.Concat(rhs).Take(1).Select(Maybe.Just).DefaultIfEmpty(Maybe.None));

		public IO<U> Match<U>(Func<T, U> some, Func<U> none)
			=> IO.From(
				_unsafeIO.AsObservable()
					.SelectMany(maybe => maybe)
					.Select(some)
					.Concat(IO.Run(none))
					.Take(1)
			);
	}
}
