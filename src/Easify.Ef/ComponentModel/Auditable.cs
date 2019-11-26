using System;

namespace Easify.Ef.ComponentModel
{
    public abstract class Auditable : IAuditable
    {
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}