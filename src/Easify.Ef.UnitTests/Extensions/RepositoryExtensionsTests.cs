using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Easify.Ef.UnitTests.Models;
using Easify.Extensions.Specifications;
using EfCore.UnitOfWork;
using NSubstitute;
using Xunit;

namespace Easify.Ef.UnitTests.Extensions
{
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
            Easify.Ef.Extensions.RepositoryExtensions.GetList(repository, spec, q => q);

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
            await Easify.Ef.Extensions.RepositoryExtensions.GetListAsync(repository, spec, q => q);

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
            Easify.Ef.Extensions.RepositoryExtensions.GetFirstOrDefault(repository, spec, q => q);

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
            await Easify.Ef.Extensions.RepositoryExtensions.GetFirstOrDefaultAsync(repository, spec, q => q);

            // Assert
            await repository.Received(1).GetFirstOrDefaultAsync(Arg.Is((Expression<Func<SampleEntity, bool>> m)  => m.ToString() == expression.ToString()),
                Arg.Any<Func<IQueryable<SampleEntity>, IQueryable<SampleEntity>>>());
        }
    }
}