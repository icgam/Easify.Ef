using Easify.Ef.ComponentModel;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Easify.Ef.UnitTests.Models
{
    public sealed class SampleEntity : Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}