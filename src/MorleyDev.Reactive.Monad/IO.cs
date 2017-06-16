using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace MorleyDev.Reactive.Monad
{
	public static class IO
	{
		/// <summary>
		/// Wraps an IO Action into an synchronous IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> From<T>(Func<T> unsafeIO) => From(Observable.Defer(() => Observable.Return(unsafeIO())));

		/// <summary>
		/// Wraps an Asynchronous Action into an asynchronous IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> From<T>(Func<Task<T>> unsafeIO) => From(Observable.Defer(() => unsafeIO().ToObservable()));

		/// <summary>
		/// Wraps an Asynchronous observable into an asynchronous IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> From<T>(Func<IObservable<T>> unsafeIO) => From(Observable.Defer(unsafeIO));

		/// <summary>
		/// Wraps an Asynchronous observable into an asynchronous IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> From<T>(IObservable<T> unsafeIO) => IO<T>.From(unsafeIO);

		/// <summary>
		/// Runs a synchronous IO async via Task.Run and wraps the result in an IO
		/// </summary>
		/// <param name="unsafeIO"></param>
		/// <returns></returns>
		public static IO<T> Run<T>(Func<T> unsafeIO) => From(() => Task.Run(unsafeIO));
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
