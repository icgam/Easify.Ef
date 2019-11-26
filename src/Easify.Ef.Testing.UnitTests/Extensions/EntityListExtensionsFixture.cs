using System.Collections.Generic;
using Easify.Ef.Testing.UnitTests.Models;

namespace Easify.Ef.Testing.UnitTests.Extensions
{
    public class EntityListExtensionsFixture
    {
        public EntityListExtensionsFixture()
        {
            Entities = new List<SampleEntity>
            {
                new SampleEntity() {Id = 1, Name = "Sample #1"},
                new SampleEntity() {Id = 2, Name = "Sample #2"}
            };
        }

        public IEnumerable<SampleEntity> Entities { get; }
    }
}