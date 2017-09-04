using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace MorleyDev.Reactive.Monad
{
	public static class IO
	{
		/// <summary>
		/// Wraps an Asynchronous Action into an asynchronous IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> Defer<T>(Func<Task<T>> unsafeIO) => IO<T>.From(Observable.Defer(() => unsafeIO().ToObservable()));

		/// <summary>
		/// Wraps an Asynchronous observable into an asynchronous IO (will error if unsafeIO returns more or less than 1 result)
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> Defer<T>(Func<IObservable<T>> unsafeIO) => IO<T>.From(Observable.Defer(unsafeIO));

		/// <summary>
		/// Runs a synchronous IO async via Task.Run and wraps the result in an IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> Run<T>(Func<T> unsafeIO) => IO<T>.From(Observable.Defer(() => Task.Run(unsafeIO).ToObservable()));

		/// <summary>
		/// Runs a synchronous action via Task.Run that returns a void
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public static IO<Unit> Run(Action action) => IO<Unit>.From(Observable.Defer(() => Task.Run(action).ToObservable()));

		/// <summary>
		/// Wraps an Asynchronous observable into an asynchronous IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> From<T>(IObservable<T> unsafeIO) => IO<T>.From(unsafeIO);

		/// <summary>
		/// Wraps an Asynchronous observable into an asynchronous IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> From<T>(IEnumerable<T> unsafeIO) => IO<T>.From(unsafeIO.ToObservable());

		/// <summary>
		/// Returns an IO that contains the specified value
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static IO<T> Return<T>(T value) => IO<T>.From(Observable.Return(value));
	}

	/// <summary>
	/// IO encapsulates a deferred IO operation
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class IO<T> : IObservable<T>
	{
		private readonly IObservable<T> _unsafeIO;

		private IO(IObservable<T> unsafeIO)
		{
			_unsafeIO = unsafeIO;
		}

		public static IO<T> From(IObservable<T> lazy) => new IO<T>(lazy);

		public Task<T> RunUnsafeIO() => _unsafeIO.ToTask();

		public IObservable<T> AsObservable() => _unsafeIO;

		public IDisposable Subscribe(IObserver<T> observer)
		{
			return _unsafeIO.SingleAsync().Subscribe(observer);
		}
	}
}
