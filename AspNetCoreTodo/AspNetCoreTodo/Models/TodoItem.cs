using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models
{
    // Called Entity
    public class TodoItem
    {
        // Properties of TodoItem class
        
        // UserId
        // Update the TodoItem entity model by adding UserId
        // Create migration & update the database
        public string UserId { get; set; }
        // GUID (globally unique identifier) represent unique ID for each row 
        public Guid Id { get; set; }
        // check if a task is done
        public bool IsDone { get; set; }
        // Title of a task
        [Required]
        public string Title { get; set; }
        // Time that a task is created
        public DateTimeOffset? DueAt { get; set; }
    }
}