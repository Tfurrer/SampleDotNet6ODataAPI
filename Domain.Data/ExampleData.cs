using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data
{
    public class ExampleData
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
