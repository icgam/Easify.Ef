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

namespace LittleBlocks.Ef.UnitTests.Extensions;

public class RepositoryExtensionsTests
{
    [Fact]
    public void Should_GetList_GivenSpecification_CallUnderlyingRepositoryWithTheRightPredicate()
    {
        // Arrange
        var spec = Specification<SampleEntity>.From(m => m.Id == 2);
        var uow = Substitute.For<IUnitOfWork<SampleDbContext>>();
        var repository = Substitute.For<IRepository<SampleEntity>>();
        uow.GetRepository<SampleEntity>().Returns(repository);

        var expression = spec.ToExpression();

        // Act
        repository.GetList(spec, q => q);

        // Assert
        repository.Received(1).GetList(Arg.Is((Expression<Func<SampleEntity, bool>> m)  => m.ToString() == expression.ToString()),
            Arg.Any<Func<IQueryable<SampleEntity>, IQueryable<SampleEntity>>>());
    }

    [Fact]
    public async Task Should_GetListAsync_GivenSpecification_CallUnderlyingRepositoryWithTheRightPredicate()
    {
        // Arrange
        var spec = Specification<SampleEntity>.From(m => m.Id == 2);
        var uow = Substitute.For<IUnitOfWork<SampleDbContext>>();
        var repository = Substitute.For<IRepository<SampleEntity>>();
        uow.GetRepository<SampleEntity>().Returns(repository);

        var expression = spec.ToExpression();

        // Act
        await repository.GetListAsync(spec, q => q);

        // Assert
        await repository.Received(1).GetListAsync(Arg.Is((Expression<Func<SampleEntity, bool>> m)  => m.ToString() == expression.ToString()),
            Arg.Any<Func<IQueryable<SampleEntity>, IQueryable<SampleEntity>>>());
    }

    [Fact]
    public void Should_GetFirstOrDefault_GivenSpecification_CallUnderlyingRepositoryWithTheRightPredicate()
    {
        // Arrange
        var spec = Specification<SampleEntity>.From(m => m.Id == 2);
        var uow = Substitute.For<IUnitOfWork<SampleDbContext>>();
        var repository = Substitute.For<IRepository<SampleEntity>>();
        uow.GetRepository<SampleEntity>().Returns(repository);

        var expression = spec.ToExpression();

        // Act
        repository.GetFirstOrDefault(spec, q => q);

        // Assert
        repository.Received(1).GetFirstOrDefault(Arg.Is((Expression<Func<SampleEntity, bool>> m)  => m.ToString() == expression.ToString()),
            Arg.Any<Func<IQueryable<SampleEntity>, IQueryable<SampleEntity>>>());
    }

    [Fact]
    public async Task Should_GetFirstOrDefaultAsync_GivenSpecification_CallUnderlyingRepositoryWithTheRightPredicate()
    {
        // Arrange
        var spec = Specification<SampleEntity>.From(m => m.Id == 2);
        var uow = Substitute.For<IUnitOfWork<SampleDbContext>>();
        var repository = Substitute.For<IRepository<SampleEntity>>();
        uow.GetRepository<SampleEntity>().Returns(repository);

        var expression = spec.ToExpression();

        // Act
        await repository.GetFirstOrDefaultAsync(spec, q => q);

        // Assert
        await repository.Received(1).GetFirstOrDefaultAsync(Arg.Is((Expression<Func<SampleEntity, bool>> m)  => m.ToString() == expression.ToString()),
            Arg.Any<Func<IQueryable<SampleEntity>, IQueryable<SampleEntity>>>());
    }
}
