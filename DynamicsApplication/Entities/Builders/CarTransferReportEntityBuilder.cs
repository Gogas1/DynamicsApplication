using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Entities.Builders
{
    internal class CarTransferReportEntityBuilder
    {
        private Entity entity = new Entity("svnt_cartransferreport");

        private Guid id;
        private DateTime date = DateTime.MinValue;
        private bool isDamaged;
        private string damageDescription = string.Empty;
        private CarTransferReportType type;
        private Car car;
        private string title;

        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                entity.Id = value;
            }
        }

        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                entity.Attributes.Add("svnt_date", date);
            }
        }

        public bool IsDamaged 
        { 
            get => isDamaged;
            set
            {
                isDamaged = value;
                entity.Attributes.Add("svnt_damages", isDamaged);
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                entity.Attributes.Add("svnt_title", title);
            }
        }

        public string DamageDescription
        {
            get => damageDescription;
            set
            {
                damageDescription = value;
                entity.Attributes.Add("svnt_damagedescription", damageDescription);
            }
        }

        public CarTransferReportType Type
        {
            get => type;
            set
            {
                type = value;
                var optionSet = new OptionSetValue((int)type);
                entity.Attributes.Add("svnt_type", optionSet);
            }
        }

        public Car Car
        {
            get => car;
            set
            {
                if (value != null)
                {
                    car = value;
                    var reference = new EntityReference("svnt_car", car.Id);
                    entity.Attributes.Add("svnt_car", reference);
                }
            }
        }

        public Entity Entity { get => entity; }
    }
}
