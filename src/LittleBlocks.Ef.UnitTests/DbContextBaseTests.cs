// This software is part of the LittleBlocks.Ef Library
// Copyright (C) 2022 Little Blocks
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

namespace LittleBlocks.Ef.UnitTests;

public class DbContextBaseTests
{
    [Fact]
    public void Should_SaveChanges_GivenAuditableEntity_SetAuditableFieldsCorrectly()
    {
        // Arrange
        var entity = new SampleEntity {Id = 1, Name = "1"};
        var options = new DbContextOptionsBuilder<SampleDbContext>()
            .UseInMemoryDatabase($"SampleDb{Guid.NewGuid()}")
            .Options;

        var dbContext = new SampleDbContext(options, new FakeOperationContext());

        // Act
        dbContext.Add(entity);
        dbContext.SaveChanges();

        var actual = dbContext.Set<SampleEntity>().FirstOrDefault();

        // Assert
        actual.Should().NotBeNull().And.Match<SampleEntity>(m =>
            m.Id == 1 && m.LastModifiedDate.Date == DateTime.Today &&
            m.LastModifiedBy == PrincipalExtensions.AnonymousUser);
    }
}
