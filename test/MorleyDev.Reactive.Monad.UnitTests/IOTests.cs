using FluentAssertions;
using MorleyDev.Reactive.Monad.Extensions;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MorleyDev.Reactive.Monad.UnitTests
{
	public class IOTests
	{
		[Fact]
		public async Task Basic()
		{
			(await IO.From(() => 10).RunUnsafeIO()).Should().Be(10);
			(await IO.From(() => Task.FromResult(10)).RunUnsafeIO()).Should().Be(10);
			(await IO.Run(() => 10).RunUnsafeIO()).Should().Be(10);
			(await IO.From(() => 10).Select(m => m * 10)).Should().Be(100);
			(await Observable.Return(10).ToIO()).Should().Be(10);
		}
	}
}
