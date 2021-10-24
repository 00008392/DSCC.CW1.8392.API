using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DSCC._8392.Domain.Entities
{
    public class Genre : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [JsonIgnore]
        public ICollection<Book> Books { get; set; }
    }
}
