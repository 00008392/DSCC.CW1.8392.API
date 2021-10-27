using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSCC._8392.Domain.Entities
{
    //base entity in order to work with generic repository
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}
