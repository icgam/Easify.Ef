using Easify.Ef.Testing.Extensions;
using Easify.Ef.Testing.UnitTests.Models;
using FluentAssertions;
using Xunit;

namespace Easify.Ef.Testing.UnitTests.Extensions
{
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
}