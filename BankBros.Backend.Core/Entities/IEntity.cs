using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Core.Entities
{
    public interface IEntity
    {
        EntityState EntityState { get; set; }
    }

    public enum EntityState
    {
        Added,
        Modified,
        Deleted,
        Unchanged
    }
}
