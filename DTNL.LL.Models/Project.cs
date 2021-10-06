using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTNL.LL.Models
{
    public class Project
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public string ProjectName { get; set; }
        
        [DataType(DataType.Text)]
        public string CustomerName { get; set; }
        
        [Display(Name = "Is Active")]
        [Range(typeof(bool), "false", "true")]
        public bool Active { get; set; } = true;

        public string GAProperty { get; set; }

        public virtual ICollection<Lamp> Lamps { get; set; } = new List<Lamp>();
    }
}
