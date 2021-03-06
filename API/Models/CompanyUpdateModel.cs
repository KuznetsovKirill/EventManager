using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CompanyUpdateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Type { get; set; }

        public string Description { get; set; }
    }
}
