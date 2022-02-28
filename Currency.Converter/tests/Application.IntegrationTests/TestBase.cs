using NUnit.Framework;
using static Testing;

namespace Currency.Converter.Application.IntegrationTests
{
    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}