using System.Security.Principal;
using Easify.Ef.Extensions;
using FluentAssertions;
using Xunit;

namespace Easify.Ef.UnitTests.Extensions
{
    public class PrincipalExtensionsTests
    {
        [Theory]
        [InlineData(true, "", PrincipalExtensions.AnonymousUser)]
        [InlineData(true, "username", "username")]
        [InlineData(false, "", null)]
        public void Should_GetUserName_ReturnRightUserFromPrincipal_WhenThereIsOneOtherwiseAnonymous(bool validIdentity, string username, string expected)
        {
            // Arrange
            var principal = validIdentity ? new GenericPrincipal(new GenericIdentity(username), new string[] {}) : null;

            // Act
            var actual = principal?.GetUserName();

            // Assert
            actual.Should().Be(expected);
        }
    }
}