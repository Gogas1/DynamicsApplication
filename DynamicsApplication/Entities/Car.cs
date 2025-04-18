using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Entities
{
    internal class Car
    {
        public Guid Id { get; set; }
        public string VinNumber { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public ManufacturerType Manufacturer { get; set; }
        public DateTime ProductionDate { get; set; } = DateTime.MinValue;
        public DateTime PurchaseDate { get; set; } = DateTime.MinValue;

        public Guid CarClassId { get; set; }
        public CarClass CarClass { get; set; }
    }

    internal enum ManufacturerType
    {
        NotSpecified,
        BMW = 478010000,
        Volkswagen,
        Audi,
        MercedesBenz,
    }
}
