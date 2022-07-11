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
        public virtual ICollection<ExampleDataChild> Children { get; set; } = new List<ExampleDataChild>();
        public virtual ExampleDataSingle? Single { get; set; }
    }

    public class ExampleDataChild
    {
        [Key]
        public int Id { get; set; }
        public string Group { get; set; }
        public int ExampleDataId { get; set; }
        public ExampleData ExampleData { get; set; }
    }
    public class ExampleDataSingle
    {
        [Key]
        public int Id { get; set; }
        public string Standard { get; set; }
        public int ExampleDataId { get; set; }
        public ExampleData ExampleData { get; set; }
    }
}
