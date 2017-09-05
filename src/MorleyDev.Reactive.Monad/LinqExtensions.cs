using MorleyDev.Reactive.Monad;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;

namespace System.Linq
{
	public static class Monadic
	{
        public static LazyValue<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, T> functor)
            where TDisposable : IDisposable => Defer(() =>
            {
                using (var disposable = factory())
                    return functor(disposable);
            });

        public static Maybe<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, Maybe<T>> functor)
            where TDisposable : IDisposable => Defer(() =>
            {
                using (var disposable = factory())
                    return functor(disposable);
            });

        public static LazyValue<T> Return<T>(T value) => LazyValue.Return(value);
		public static LazyValue<T> Defer<T>(Func<T> value) => LazyValue.Defer(value);
		public static LazyValue<System.Reactive.Unit> Defer<T>(Action value) => LazyValue.Defer(() => { value(); return System.Reactive.Unit.Default; });
		public static Maybe<T> Defer<T>(Func<Maybe<T>> value) => Maybe.Defer(value);

		public static Maybe<U> Select<U, T>(this Maybe<T> self, Func<T, U> mapper) => Maybe<U>.From(self.AsEnumerable().Select(mapper));
		public static Maybe<T> Where<T>(this Maybe<T> self, Func<T, bool> predicate) => Maybe<T>.From(self.AsEnumerable().Where(predicate));
		public static Maybe<U> SelectMany<U, T>(this Maybe<T> self, Func<T, LazyValue<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static Maybe<U> SelectMany<U, T>(this Maybe<T> self, Func<T, Maybe<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static LazyValue<T> DefaultIfEmpty<T>(this Maybe<T> self, T defaultValue = default(T)) => LazyValue<T>.From(self.AsEnumerable().DefaultIfEmpty(defaultValue).Single);

		public static LazyValue<U> Select<U, T>(this LazyValue<T> self, Func<T, U> mapper) => LazyValue<U>.From(self.AsEnumerable().Select(mapper).Single);
		public static Maybe<T> Where<T>(this LazyValue<T> self, Func<T, bool> predicate) => Maybe<T>.From(self.AsEnumerable().Where(predicate));
		public static LazyValue<U> SelectMany<U, T>(this LazyValue<T> self, Func<T, LazyValue<U>> mapper) => LazyValue<U>.From(self.AsEnumerable().SelectMany(mapper).Single);
		public static Maybe<U> SelectMany<U, T>(this LazyValue<T> self, Func<T, Maybe<U>> mapper) => Maybe<U>.From(self.AsEnumerable().SelectMany(mapper));
		public static LazyValue<U> DefaultIfEmpty<U, T>(this LazyValue<T> self, Func<T, Maybe<U>> mapper) => LazyValue<U>.From(self.AsEnumerable().SelectMany(mapper).Single);

		public static LazyValue<T> SingleLazy<T>(this IEnumerable<T> self) => LazyValue<T>.From(self.Single);
		public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> self) => Maybe<T>.From(self.Take(1));

		public static LazyValue<TSource> AggregateLazy<TSource>(this IEnumerable<TSource> self, Func<TSource, TSource, TSource> reducer)
			=> Defer(() => self.Aggregate(reducer));
		public static LazyValue<TAccumulate> AggregateLazy<TSource, TAccumulate>(this IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> reducer)
			=> Defer(() => self.Aggregate(seed, reducer));
	}
}

namespace System.Reactive.Linq
{
	public static class MonadicAsync
	{
		public static IO<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, T> method)
			where TDisposable : IDisposable
			=> MorleyDev.Reactive.Monad.IO.From(Observable.Using(factory, disposable => Observable.Return(method(disposable))));
			
		public static IO<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, IO<T>> method)
			where TDisposable : IDisposable
			=> MorleyDev.Reactive.Monad.IO.From(Observable.Using(factory, disposable => method(disposable)));

		public static IO<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, Task<T>> method)
			where TDisposable : IDisposable
			=> MorleyDev.Reactive.Monad.IO.From(Observable.Using(factory, disposable => method(disposable).ToObservable()));

		public static MaybeIO<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, Maybe<T>> method)
			where TDisposable : IDisposable
			=> MorleyDev.Reactive.Monad.IO.From(Observable.Using(factory, disposable => Observable.Return(method(disposable))));
			
		public static MaybeIO<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, MaybeIO<T>> method)
			where TDisposable : IDisposable
			=> MorleyDev.Reactive.Monad.MaybeIO.From(Observable.Using(factory, disposable => method(disposable)));

		public static MaybeIO<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, Task<Maybe<T>>> method)
			where TDisposable : IDisposable
			=> MorleyDev.Reactive.Monad.IO.From(Observable.Using(factory, disposable => method(disposable).ToObservable()));

		public static IObservable<T> Using<T, TDisposable>(Func<TDisposable> factory, Func<TDisposable, IObservable<T>> method)
			where TDisposable : IDisposable
			=> (Observable.Using(factory, disposable => method(disposable)));

		public static IO<T> Throw<T>(Exception value)
			=> MorleyDev.Reactive.Monad.IO.From(Observable.Throw<T>(value));

		public static IO<T> Return<T>(T value) => MorleyDev.Reactive.Monad.IO.Return(value);
		public static MaybeIO<T> Return<T>(Maybe<T> value) => MorleyDev.Reactive.Monad.IO.Return(value);

		public static IO<T> Run<T>(Func<T> value) => MorleyDev.Reactive.Monad.IO.Run(value);
		public static IO<Unit> Run(Action value) => MorleyDev.Reactive.Monad.IO.Run(value);
		public static MaybeIO<T> Run<T>(Func<Maybe<T>> value) => MorleyDev.Reactive.Monad.IO.Run(value);

		public static IO<T> Defer<T>(Func<Task<T>> value) => MorleyDev.Reactive.Monad.IO.Defer(value);
		public static IO<Unit> Defer(Func<Task> value) => MorleyDev.Reactive.Monad.IO.Defer(async () => { await value(); return Unit.Default; });
		public static MaybeIO<T> Defer<T>(Func<Task<Maybe<T>>> value) => MorleyDev.Reactive.Monad.IO.Defer(value);
		public static MaybeIO<T> Defer<T>(Func<IO<Maybe<T>>> value) => MorleyDev.Reactive.Monad.IO.Defer(value);
		public static IO<T> Defer<T>(Func<IO<T>> value) => MorleyDev.Reactive.Monad.IO.Defer(value);
		public static MaybeIO<T> Defer<T>(Func<MaybeIO<T>> value) => MorleyDev.Reactive.Monad.MaybeIO.Defer(value);
		public static IObservable<T> Defer<T>(Func<IObservable<T>> value) => Observable.Defer(value);

		public static IO<U> Select<U, T>(this IO<T> self, Func<T, U> mapper) => IO<U>.From(self.AsObservable().Select(mapper));
		public static MaybeIO<T> Where<T>(this IO<T> self, Func<T, bool> predicate) => MaybeIO.From(self.AsObservable().Where(predicate));
		public static IO<U> SelectMany<U, T>(this IO<T> self, Func<T, Task<U>> mapper) => IO<U>.From(self.AsObservable().SelectMany(mapper));
		public static IO<U> SelectMany<U, T>(this IO<T> self, Func<T, IO<U>> mapper) => IO<U>.From(self.AsObservable().SelectMany(mapper));
		public static IO<U> SelectMany<U, T>(this IO<T> self, Func<T, LazyValue<U>> mapper) => IO<U>.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, Maybe<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(f => mapper(f)));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, MaybeIO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this IO<T> self, Func<T, IO<Maybe<U>>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(value => mapper(value).AsObservable().SelectMany(maybe => maybe)));
		public static IO<bool> IsEmpty<T>(this IO<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.Return(false));

		public static MaybeIO<U> Select<U, T>(this MaybeIO<T> self, Func<T, U> mapper) => MaybeIO.From(self.AsObservable().Select(mapper));
		public static MaybeIO<T> Where<T>(this MaybeIO<T> self, Func<T, bool> predicate) => MaybeIO.From(self.AsObservable().Where(predicate));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, Task<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(value => mapper(value)));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, IO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, Maybe<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, LazyValue<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, MaybeIO<U>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(mapper));
		public static MaybeIO<U> SelectMany<U, T>(this MaybeIO<T> self, Func<T, IO<Maybe<U>>> mapper) => MaybeIO.From(self.AsObservable().SelectMany(value => mapper(value).AsObservable().SelectMany(maybe => maybe)));
		public static IO<bool> IsEmpty<T>(this MaybeIO<T> self) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().IsEmpty());
		public static IO<T> DefaultIfEmpty<T>(this MaybeIO<T> self, T defaultValue = default(T)) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().DefaultIfEmpty(defaultValue));

		public static IObservable<U> SelectMany<U, T>(this IObservable<T> self, Func<T, IObservable<Maybe<U>>> mapper) => Observable.AsObservable(self).SelectMany(value => mapper(value).SelectMany(maybe => maybe));
		public static MaybeIO<T> FirstOrNone<T>(this IObservable<T> self) => MaybeIO.From(Observable.AsObservable(self).Take(1));
		public static IO<T> SingleIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).SingleAsync());
		public static IO<bool> AnyIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).Any());
		public static IO<bool> AnyIO<T>(this IObservable<T> self, Func<T, bool> predicate) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).Any(predicate));
		public static IO<bool> AllIO<T>(this IObservable<T> self, Func<T, bool> predicate) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).All(predicate));
		public static IO<bool> ContainsIO<T>(this IObservable<T> self, T value) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).Contains(value));
		public static IO<bool> IsEmptyIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).IsEmpty());
		public static IO<T[]> ToArrayIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).ToArray());
		public static IO<IList<T>> ToListIO<T>(this IObservable<T> self) => MorleyDev.Reactive.Monad.IO.From(Observable.AsObservable(self).ToList());

		public static IO<T> Do<T>(this IO<T> self, Action<T> actor) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().Do(actor));
		public static MaybeIO<T> Do<T>(this MaybeIO<T> self, Action<T> actor) => MaybeIO.From(self.AsObservable().Do(actor));

		public static IO<T> Delay<T>(this IO<T> self, TimeSpan offset) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().Delay(offset));
		public static MaybeIO<T> Delay<T>(this MaybeIO<T> self, TimeSpan offset) => MaybeIO.From(self.AsObservable().Delay(offset));
		public static IO<T> DelaySubscription<T>(this IO<T> self, TimeSpan offset) => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().DelaySubscription(offset));
		public static MaybeIO<T> DelaySubscription<T>(this MaybeIO<T> self, TimeSpan offset) => MaybeIO.From(self.AsObservable().DelaySubscription(offset));

		public static MaybeIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, Task<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex).ToObservable()));
		public static MaybeIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, IO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static MaybeIO<T> Catch<T, TException>(this MaybeIO<T> self, Func<TException, MaybeIO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static IO<T> Catch<T, TException>(this IO<T> self, Func<TException, IO<T>> catcher) where TException : Exception => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static MaybeIO<T> Catch<T, TException>(this IO<T> self, Func<TException, MaybeIO<T>> catcher) where TException : Exception => MaybeIO.From(self.AsObservable().Catch((TException ex) => catcher(ex)));
		public static IO<T> Catch<T, TException>(this IO<T> self, Func<TException, Task<T>> catcher) where TException : Exception => MorleyDev.Reactive.Monad.IO.From(self.AsObservable().Catch((TException ex) => catcher(ex).ToObservable()));

		public static IO<U> Zip<U, T1, T2>(this IO<T1> lhs, IO<T2> rhs, Func<T1, T2, U> mapper) => MorleyDev.Reactive.Monad.IO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));
		public static MaybeIO<U> Zip<U, T1, T2>(this IO<T1> lhs, MaybeIO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));
		public static MaybeIO<U> Zip<U, T1, T2>(this IO<T1> lhs, IObservable<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));
		public static MaybeIO<U> Zip<U, T1, T2>(this MaybeIO<T1> lhs, IO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));
		public static MaybeIO<U> Zip<U, T1, T2>(this MaybeIO<T1> lhs, MaybeIO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));
		public static MaybeIO<U> Zip<U, T1, T2>(this MaybeIO<T1> lhs, IObservable<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));
		public static MaybeIO<U> Zip<U, T1, T2>(this IObservable<T1> lhs, IO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));
		public static MaybeIO<U> Zip<U, T1, T2>(this IObservable<T1> lhs, MaybeIO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().Zip(rhs.AsObservable(), mapper));

		public static IO<(T1,T2)> Zip<T1, T2>(this IO<T1> lhs, IO<T2> rhs) => Zip(lhs, rhs, (l,r) => (l,r));
		public static MaybeIO<(T1,T2)> Zip<T1, T2>(this IO<T1> lhs, MaybeIO<T2> rhs) => Zip(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> Zip<T1, T2>(this IO<T1> lhs, IObservable<T2> rhs) => Zip(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> Zip<T1, T2>(this MaybeIO<T1> lhs, IO<T2> rhs) => Zip(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> Zip<T1, T2>(this MaybeIO<T1> lhs, MaybeIO<T2> rhs) => Zip(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> Zip<T1, T2>(this MaybeIO<T1> lhs, IObservable<T2> rhs) => Zip(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> Zip<T1, T2>(this IObservable<T1> lhs, IO<T2> rhs) => Zip(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> Zip<T1, T2>(this IObservable<T1> lhs, MaybeIO<T2> rhs) => Zip(lhs, rhs, (l, r) => (l, r));

		public static IO<U> CombineLatest<U, T1, T2>(this IO<T1> lhs, IO<T2> rhs, Func<T1, T2, U> mapper) => MorleyDev.Reactive.Monad.IO.From(lhs.AsObservable().CombineLatest(rhs.AsObservable(), mapper));
		public static MaybeIO<U> CombineLatest<U, T1, T2>(this IO<T1> lhs, MaybeIO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().CombineLatest(rhs.AsObservable(), mapper));
		public static MaybeIO<U> CombineLatest<U, T1, T2>(this MaybeIO<T1> lhs, IO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().CombineLatest(rhs.AsObservable(), mapper));
		public static MaybeIO<U> CombineLatest<U, T1, T2>(this MaybeIO<T1> lhs, MaybeIO<T2> rhs, Func<T1, T2, U> mapper) => MaybeIO.From(lhs.AsObservable().CombineLatest(rhs.AsObservable(), mapper));

		public static IO<(T1, T2)> CombineLatest<T1, T2>(this IO<T1> lhs, IO<T2> rhs) => CombineLatest(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> CombineLatest<T1, T2>(this IO<T1> lhs, MaybeIO<T2> rhs) => CombineLatest(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> CombineLatest<T1, T2>(this MaybeIO<T1> lhs, IO<T2> rhs) => CombineLatest(lhs, rhs, (l, r) => (l, r));
		public static MaybeIO<(T1,T2)> CombineLatest<T1, T2>(this MaybeIO<T1> lhs, MaybeIO<T2> rhs) => CombineLatest(lhs, rhs, (l, r) => (l, r));
		
		public static IO<TSource> AggregateIO<TSource>(this IObservable<TSource> self, Func<TSource, TSource, TSource> reducer)
			=> MorleyDev.Reactive.Monad.IO.From(self.Aggregate(reducer));

		public static IO<TAccumulate> AggregateIO<TSource, TAccumulate>(this IObservable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> reducer)
			=> MorleyDev.Reactive.Monad.IO.From(self.Aggregate(seed, reducer));

		public static IO<double> AverageIO(this IObservable<int> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<double> AverageIO(this IObservable<long> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<double> AverageIO(this IObservable<double> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<float> AverageIO(this IObservable<float> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<decimal> AverageIO(this IObservable<decimal> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());

		public static IO<double?> AverageIO(this IObservable<int?> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<double?> AverageIO(this IObservable<long?> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<double?> AverageIO(this IObservable<double?> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<float?> AverageIO(this IObservable<float?> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());
		public static IO<decimal?> AverageIO(this IObservable<decimal?> self) => MorleyDev.Reactive.Monad.IO.From(self.Average());

		public static IO<double> AverageIO<T>(this IObservable<T> self, Func<T, int> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<double> AverageIO<T>(this IObservable<T> self, Func<T, long> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<double> AverageIO<T>(this IObservable<T> self, Func<T, double> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<float> AverageIO<T>(this IObservable<T> self, Func<T, float> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<decimal> AverageIO<T>(this IObservable<T> self, Func<T, decimal> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));

		public static IO<double?> AverageIO<T>(this IObservable<T> self, Func<T, int?> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<double?> AverageIO<T>(this IObservable<T> self, Func<T, long?> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<double?> AverageIO<T>(this IObservable<T> self, Func<T, double?> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<float?> AverageIO<T>(this IObservable<T> self, Func<T, float?> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));
		public static IO<decimal?> AverageIO<T>(this IObservable<T> self, Func<T, decimal?> mapper) => MorleyDev.Reactive.Monad.IO.From(self.Average(mapper));

		public static IO<T> ElementAtIO<T>(this IObservable<T> self, int index) => MorleyDev.Reactive.Monad.IO.From(self.ElementAt(index));
		public static IO<T> ElementAtOrDefaultIO<T>(this IObservable<T> self, int index) => MorleyDev.Reactive.Monad.IO.From(self.ElementAtOrDefault(index));
		public static MaybeIO<T> ElementAtOrNone<T>(this IObservable<T> self, int index) => MorleyDev.Reactive.Monad.MaybeIO.From(self.Skip(index).Take(1));
	}

	public static class Linq2Reactive
	{
		public static IObservable<U> SelectMany<T, U>(this IEnumerable<T> self, Func<T, IObservable<U>> mapper) => self.ToObservable().SelectMany(mapper);
		public static IO<U> SelectMany<T, U>(this LazyValue<T> self, Func<T, IO<U>> mapper) => MorleyDev.Reactive.Monad.IO.From(self).SelectMany(mapper);
		public static MaybeIO<U> SelectMany<T, U>(this LazyValue<T> self, Func<T, MaybeIO<U>> mapper) => MorleyDev.Reactive.Monad.IO.From(self).SelectMany(mapper);

		public static MaybeIO<U> SelectMany<T, U>(this Maybe<T> self, Func<T, IO<U>> mapper) => MorleyDev.Reactive.Monad.MaybeIO.From(self).SelectMany(mapper);
		public static MaybeIO<U> SelectMany<T, U>(this Maybe<T> self, Func<T, MaybeIO<U>> mapper) => MorleyDev.Reactive.Monad.IO.From(self).SelectMany(mapper);
	}
}
