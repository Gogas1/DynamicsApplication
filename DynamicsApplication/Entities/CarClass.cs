using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Entities
{
    internal class CarClass
    {
        public Guid Id { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public string ClassDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
