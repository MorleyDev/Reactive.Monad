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
			(await MonadicAsync.Run(() => 10).RunUnsafeIO()).Should().Be(10);
			(await MonadicAsync.Defer(() => Task.FromResult(10)).RunUnsafeIO()).Should().Be(10);
			(await MonadicAsync.Run(() => 10).RunUnsafeIO()).Should().Be(10);
			(await MonadicAsync.Run(() => 10).Select(m => m * 10)).Should().Be(100);
			(await MonadicAsync.Return(10)).Should().Be(10);
		}
	}
}
