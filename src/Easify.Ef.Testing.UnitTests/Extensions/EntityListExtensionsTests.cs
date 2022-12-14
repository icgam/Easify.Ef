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

namespace LittleBlocks.Ef.Testing.UnitTests.Extensions;

public class EntityListExtensionsTests : IClassFixture<EntityListExtensionsFixture>
{
    private readonly EntityListExtensionsFixture _fixture;

    public EntityListExtensionsTests(EntityListExtensionsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Should_CallingToDbContext_ReturnValidContext()
    {
        // Arrange
        var uow = _fixture.Entities.ToTypedUnitOfWork<SampleEntity, SampleDbContext>();

        // Act
        var actual = uow.GetRepository<SampleEntity>().GetList();

        // Assert
        actual.Should().HaveCount(2)
            .And.Contain(m => m.Id == 1 && m.Name == "Sample #1")
            .And.Contain(m => m.Id == 2 && m.Name == "Sample #2");
    }

    [Fact]
    public void Should_CallingToDbContextTwice_ReturnTwoIsolatedContext()
    {
        // Arrange
        var uow1 = _fixture.Entities.ToTypedUnitOfWork<SampleEntity, SampleDbContext>();
        var uow2 = _fixture.Entities.ToTypedUnitOfWork<SampleEntity, SampleDbContext>();

        // Act
        uow1.GetRepository<SampleEntity>().Insert(new SampleEntity { Id = 3, Name = "Sample #3"});
        uow1.SaveChanges();

        var actual = uow2.GetRepository<SampleEntity>().GetList();

        // Assert
        actual.Should().HaveCount(2)
            .And.Contain(m => m.Id == 1 && m.Name == "Sample #1")
            .And.Contain(m => m.Id == 2 && m.Name == "Sample #2");
    }
}
