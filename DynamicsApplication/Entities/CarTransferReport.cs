using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Entities
{
    internal class CarTransferReport
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DamageDescription { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public bool IsDamaged { get; set; }
        public CarTransferReportType Type { get; set; }
        public Car Car { get; set; }
    }

    internal enum CarTransferReportType
    {
        Pickup = 478010000,
        Return,
    }
}
