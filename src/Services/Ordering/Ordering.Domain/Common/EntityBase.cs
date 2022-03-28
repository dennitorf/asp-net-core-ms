using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public string CreatedBy { set; get; }
        public DateTime CreatedData { get; set; }   
        public string LastModifiedBy { set; get; }
        public DateTime? LastModifiedDate { set; get; }
    }
}
