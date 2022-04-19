// This software is part of the Easify.Ef Library
// Copyright (C) 2018 Intermediate Capital Group
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

namespace Easify.Ef.UnitTests.Extensions;

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

    [Fact]
    public void Should_GetUserName_ReturnTheCorrectUserFromClaimsPrincipal()
    {
        //Arrange
        var identity = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, "n/a")}, "Federated", ClaimTypes.NameIdentifier, ClaimTypes.Role);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        //Act
        var actual = claimsPrincipal.GetUserName();

        //Assert
        actual.Should().Be("n/a");
    }
}
