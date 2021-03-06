using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetChangeHistoryForIssue
        {
            [Fact]
            public async Task Valid_Connection_Returns_Changeset_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();
                
                // Act
                var result = await service.GetChangeHistoryForIssue("DP1-1");
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.Any());

                // Get an item and check for two common properties (updaterName & updated)
                var firstChange = result.First();

                Assert.True(firstChange.Fields.Count > 0);
                Assert.True(firstChange.ForField("updaterName") != null);
                Assert.True(firstChange.ForField("updated") != null && firstChange.ForField("updated").To.AsDateTime() < DateTime.UtcNow);
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssuesService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetChangeHistoryForIssue("NOT-EXIST"));
            }
        }
    }
}