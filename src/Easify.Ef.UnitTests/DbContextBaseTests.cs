using System;
using System.Linq;
using Easify.Ef.Extensions;
using Easify.Ef.Testing.Extensions;
using Easify.Ef.UnitTests.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Easify.Ef.UnitTests
{
    public class DbContextBaseTests
    {
        [Fact]
        public void Should_SaveChanges_GivenAuditableEntity_SetAuditableFieldsCorrectly()
        {
            // Arrange
            var entity = new SampleEntity() {Id = 1, Name = "1"};
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
}