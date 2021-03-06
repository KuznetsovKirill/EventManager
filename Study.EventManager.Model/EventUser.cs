using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.EventManager.Model
{
    public class EventUser
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
