using Easify.Ef.Testing.UnitTests.Models;
using EfCore.UnitOfWork;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Easify.Ef.Testing.UnitTests
{
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
                .Insert(new SampleEntity {Id = 1, Name = "1"}, new SampleEntity { Id = 2, Name = "2"});
            uow.SaveChanges();

            var actual = uow.GetRepository<SampleEntity>().GetList();

            // Assert
            actual.Should().HaveCount(2)
                .And.Contain(m => m.Id == 1 && m.Name == "1")
                .And.Contain(m => m.Id == 2 && m.Name == "2");
        }
    }
}