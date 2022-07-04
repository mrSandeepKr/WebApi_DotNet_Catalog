using System;

namespace Catalog.Dtos
{
    // NOTES
    
    // Record Types
    /// Immutable objects , with expression support, value types -> so equating is simpler

    // init 
    /// Can only be set during initiation but we cannot assign this values

    public record ItemDto 
    {
        public Guid Id {get; init;}
        public string Name {get; init;}
        public decimal Price {get; init;}  
        public DateTimeOffset CreatedDate {get; init;}
    }
}