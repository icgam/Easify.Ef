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

using LittleBlocks.Ef.UnitOfWork;

namespace LittleBlocks.Ef.Testing.UnitTests;

public class DbContextExtensionsTests
{
    [Fact]
    public void Should_AddInMemoryUnitOfWork_RegisterValidUnitOfWorkWitDependencyChainInContainer()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddInMemoryUnitOfWork<SampleDbContext>();
        var sp = services.BuildServiceProvider();

        // Act
        var actual = sp.GetRequiredService<IUnitOfWork<SampleDbContext>>();

        // Assert
        actual.Should().NotBeNull();
    }

    [Fact]
    public void Should_AddInMemoryUnitOfWorkAndResolutionFromContainer_GiveValidInstanceOfUnitOfWork()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddInMemoryUnitOfWork<SampleDbContext>();
        var sp = services.BuildServiceProvider();

        // Act
        var uow = sp.GetRequiredService<IUnitOfWork<SampleDbContext>>();
        uow.GetRepository<SampleEntity>()
            .Insert(new SampleEntity { Id = 1, Name = "1" }, new SampleEntity { Id = 2, Name = "2" });
        uow.SaveChanges();

        var actual = uow.GetRepository<SampleEntity>().GetList();

        // Assert
        actual.Should().HaveCount(2)
            .And.Contain(m => m.Id == 1 && m.Name == "1")
            .And.Contain(m => m.Id == 2 && m.Name == "2");
    }
}
