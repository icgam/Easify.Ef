using System.ComponentModel.DataAnnotations;

namespace LittleBlocks.Ef.UnitOfWork.UnitTests.Entities
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<City> Cities { get; set; }
    }
}
