namespace SFA.Apprenticeships.Domain.Entities
{
    using System;

    /// <summary>
    /// Base type for persistent domain entities
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}
