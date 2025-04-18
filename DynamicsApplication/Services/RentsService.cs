using DynamicsApplication.Entities;
using DynamicsApplication.Entities.Builders;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Services
{
    internal class RentsService
    {
        private IOrganizationService _organizationService;

        public RentsService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public Guid CreateRent(Rent rent)
        {
            bool requireTwoStep = false;

            if (rent.PickupReport != null && rent.PickupReport.Id == Guid.Empty)
            {
                CreateTransferReport(rent.PickupReport);
            }

            if (rent.ReturnReport != null && rent.ReturnReport.Id == Guid.Empty)
            {
                CreateTransferReport(rent.ReturnReport);
            }

            var builder = new RentEntityBuilder();
            builder.Title = rent.Title;
            builder.ReservedPickup = rent.ReservedPickup;
            builder.ReservedReturn = rent.ReservedReturn;
            builder.ActualPickup = rent.ActualPickup;
            builder.ActualReturn = rent.ActualReturn;
            builder.PickupLocation = rent.PickupLocation;
            builder.ReturnLocation = rent.ReturnLocation;
            builder.Price = rent.Price;
            builder.IsPaid = rent.IsPaid;
            builder.Car = rent.Car;
            builder.Customer = rent.Customer;
            builder.PickupReport = rent.PickupReport;
            builder.ReturnReport = rent.ReturnReport;

            switch (rent.Status)
            {
                case RentStatusType.Returned:
                case RentStatusType.Canceled:
                    requireTwoStep = true;
                    break;
                default:
                    break;
            }

            if(!requireTwoStep)
            {
                builder.Status = rent.Status;
            }

            var id = _organizationService.Create(builder.Entity);
            rent.Id = id;

            if(requireTwoStep)
            {
                var twoStepBuilder = new RentEntityBuilder(id);
                twoStepBuilder.Status = rent.Status;

                _organizationService.Update(twoStepBuilder.Entity);
            }

            return id;
        }

        public Guid CreateTransferReport(CarTransferReport report)
        {
            var builder = new CarTransferReportEntityBuilder();
            builder.Date = report.Date;
            builder.IsDamaged = report.IsDamaged;
            builder.DamageDescription = report.DamageDescription;
            builder.Type = report.Type;
            builder.Car = report.Car;
            builder.Title = report.Title;

            var id = _organizationService.Create(builder.Entity);
            report.Id = id;

            return id;
        }
    }
}
