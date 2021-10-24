using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSCC._8392.Domain.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int IssueYear { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
    }
}
