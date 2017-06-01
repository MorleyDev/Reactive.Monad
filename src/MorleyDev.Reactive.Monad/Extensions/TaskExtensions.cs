using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace MorleyDev.Reactive.Monad.Extensions
{
	public static class TaskExtensions
	{
		public static IO<T> ToIO<T>(this Task<T> self) => IO<T>.From(self.ToObservable());
	}
}