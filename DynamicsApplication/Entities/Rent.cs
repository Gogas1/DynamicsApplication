using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Entities
{
    internal class Rent
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public DateTime ReservedPickup { get; set; } = DateTime.MinValue;
        public DateTime ReservedReturn { get; set; } = DateTime.MinValue;
        public DateTime ActualPickup { get; set; } = DateTime.MinValue;
        public DateTime ActualReturn { get; set; } = DateTime.MinValue;

        public RentLocationType PickupLocation { get; set; }
        public RentLocationType ReturnLocation { get; set; }
        public RentStatusType Status { get; set; }

        public decimal Price { get; set; }

        public bool IsPaid { get; set; }

        public Car Car { get; set; }
        public Customer Customer { get; set; }
        public CarTransferReport PickupReport { get; set; }
        public CarTransferReport ReturnReport { get; set; }
    }

    internal enum RentStatusType
    {
        NotSpecified,
        Created = 1,
        Confirmed = 478010001,
        Renting,
        Returned = 2,
        Canceled = 478010003,
    }

    internal enum RentLocationType
    {
        NotSpecified,
        Airport = 478010000,
        CityCenter,
        Office,
    }
}
