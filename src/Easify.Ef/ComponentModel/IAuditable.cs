using System;

namespace Easify.Ef.ComponentModel
{
    public interface IAuditable
    {
        string LastModifiedBy { get; set; }
        DateTime LastModifiedDate { get; set; }
    }
}