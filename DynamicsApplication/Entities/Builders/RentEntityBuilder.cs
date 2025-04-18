using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Entities.Builders
{
    internal class RentEntityBuilder
    {
        private Entity entity = new Entity("svnt_rent");

        private Guid id;
        private string title = string.Empty;
        private DateTime reservedPickup = DateTime.MinValue;
        private DateTime reservedReturn = DateTime.MinValue;
        private DateTime actualPickup = DateTime.MinValue;
        private DateTime actualReturn = DateTime.MinValue;
        private RentLocationType pickupLocation;
        private RentLocationType returnLocation;
        private RentStatusType status;
        private decimal price;
        private bool isPaid;
        private Car car;
        private Customer customer;
        private CarTransferReport pickupReport;
        private CarTransferReport returnReport;

        public Guid Id 
        { 
            get => id;
            set
            {
                id = value;
                entity.Id = value;
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

        public DateTime ReservedPickup 
        { 
            get => reservedPickup;
            set
            {
                if (value == DateTime.MinValue) return;
                reservedPickup = value;
                entity.Attributes.Add("svnt_reservedpickup", reservedPickup);
            }
        }

        public DateTime ReservedReturn 
        { 
            get => reservedReturn;
            set
            {
                if (value == DateTime.MinValue) return;
                reservedReturn = value;
                entity.Attributes.Add("svnt_reservedhandover", reservedReturn);
            }
        }

        public DateTime ActualPickup
        {
            get => actualPickup;
            set
            {
                if (value == DateTime.MinValue) return;
                actualPickup = value;
                entity.Attributes.Add("svnt_actualpickup", actualPickup);
            }
        }

        public DateTime ActualReturn 
        { 
            get => actualReturn;
            set
            {
                if (value == DateTime.MinValue) return;
                actualReturn = value;
                entity.Attributes.Add("svnt_actualreturn", actualReturn);
            }
        }

        public RentLocationType PickupLocation 
        { 
            get => pickupLocation;
            set
            {
                pickupLocation = value;
                var selectOption = new OptionSetValue((int)pickupLocation);
                entity.Attributes.Add("svnt_pickuplocation", selectOption);
            }
        }
        public RentLocationType ReturnLocation 
        { 
            get => returnLocation;
            set
            {
                returnLocation = value;
                var selectOption = new OptionSetValue((int)returnLocation);
                entity.Attributes.Add("svnt_returnlocation", selectOption);
            }
        }

        public RentStatusType Status 
        { 
            get => status;
            set
            {
                status = value;

                int stateType = 0;
                switch(status)
                {
                    case RentStatusType.Created:
                    case RentStatusType.Confirmed:
                    case RentStatusType.Renting:
                        stateType = 0;
                        break;
                    default:
                        stateType = 1;
                        break;
                }

                var stateCodeOption = new OptionSetValue(stateType);
                var statusCodeOption = new OptionSetValue((int)status);

                entity.Attributes.Add("statecode", stateCodeOption);
                entity.Attributes.Add("statuscode", statusCodeOption);
            }
        }

        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                var money = new Money(price);
                entity.Attributes.Add("svnt_price", money);
            }
        }

        public bool IsPaid
        {
            get => isPaid;
            set
            {
                isPaid = value;
                entity.Attributes.Add("svnt_paid", isPaid);
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

        public Customer Customer
        {
            get => customer;
            set 
            {
                if (value != null)
                {
                    customer = value;
                    var reference = new EntityReference("contact", customer.Id);
                    entity.Attributes.Add("svnt_customer", reference);
                }
            }
        }

        public CarTransferReport PickupReport
        {
            get => pickupReport;
            set
            {
                if (value != null)
                {
                    pickupReport = value;
                    var reference = new EntityReference("svnt_cartransferreport", pickupReport.Id);
                    entity.Attributes.Add("svnt_pickupreport", reference);
                }
            }
        }
        public CarTransferReport ReturnReport
        {
            get => returnReport;
            set
            {
                if (value != null)
                {
                    returnReport = value;
                    var reference = new EntityReference("svnt_cartransferreport", returnReport.Id);
                    entity.Attributes.Add("svnt_returnreport", reference);
                }
            }
        }

        public Entity Entity { get => entity; }

        public RentEntityBuilder()
        {
            
        }

        public RentEntityBuilder(Guid id)
        {
            Id = id;
        }
    }
}
