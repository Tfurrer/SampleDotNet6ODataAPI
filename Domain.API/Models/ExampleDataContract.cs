using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.API.Models
{
    public class ExampleDataContractNew
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ExampleDataContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ExampleDataChildContract> ExampleDataChild { get; set; } = new List<ExampleDataChildContract>();
        public ExampleDataSingleContract Single { get; set; } = new ExampleDataSingleContract();
    }
    public class ExampleDataChildContractNew
    {
        public int Id { get; set; }
        public string Group { get; set; }

    }
    public class ExampleDataChildContract
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public int ExampleDataId { get; set; }
    }

    public class ExampleDataSingleContract
    {
        public int Id { get; set; }
        public string Standard { get; set; }
        public int ExampleDataId { get; set; }
    }

}
