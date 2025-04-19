using DynamicsApplication.Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Services
{
    internal class RentGenerator
    {
        private IOrganizationService _organizationService;
        private DataRepository _dataRepository;
        private Random _random;

        private RentStatusType[] _outcomes = new RentStatusType[] { 
            RentStatusType.Created,
            RentStatusType.Confirmed,
            RentStatusType.Renting,
            RentStatusType.Returned,
            RentStatusType.Canceled,
        };
        private double[] _weights = new double[]
        {
            0.05,
            0.05,
            0.05,
            0.75,
            0.1
        };

        public RentGenerator(IOrganizationService organizationService, Random random, DataRepository dataRepository)
        {
            _organizationService = organizationService;
            _random = random;
            _dataRepository = dataRepository;
        }

        public List<Rent> GenerateRents(int number)
        {
            var rents = new List<Rent>();

            for (int i = 0; i < number; i++)
            {
                rents.Add(GenerateRent());
            }

            return rents;
        }

        private Rent GenerateRent()
        {
            var rent = new Rent();

            rent.Car = RollCar();
            rent.Customer = RollCustomer();
            rent.Title = $"{rent.Car.CarModel} - {rent.Customer.FirstName} {rent.Customer.LastName}";
            rent.Status = RollStatus();

            rent.ReservedPickup = RollDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31));
            rent.ReservedReturn = RollDateTime(rent.ReservedPickup.AddHours(1), rent.ReservedPickup.AddDays(30));
            rent.Price = rent.Car.CarClass.Price + rent.Car.CarClass.Price / 100 * Convert.ToInt32((rent.ReservedReturn - rent.ReservedPickup).TotalHours);

            rent.PickupLocation = RollLocation();
            rent.ReturnLocation = RollLocation();

            PopulateActualData(rent);
            PopulateReports(rent);
            PopulatePaidData(rent);

            return rent;
        }

        private RentStatusType RollStatus()
        {
            double statusRoll = _random.NextDouble();
            double cumulative = 0;

            for (int i = 0; i < _weights.Length; i++)
            {
                cumulative += _weights[i];
                if (statusRoll <= cumulative)
                {
                    return _outcomes[i];
                }
            }

            return default;
        }

        private Car RollCar()
        {
            var carIndex = _random.Next(_dataRepository.Cars.Count);

            if(_dataRepository.Cars.Count > carIndex)
            {
                return _dataRepository.Cars[carIndex];
            }

            return _dataRepository.Cars.First();
        }

        private Customer RollCustomer()
        {
            var customerIndex = _random.Next(_dataRepository.Customers.Count);

            if (_dataRepository.Cars.Count > customerIndex)
            {
                return _dataRepository.Customers[customerIndex];
            }

            return _dataRepository.Customers.First();
        }

        private RentLocationType RollLocation()
        {
            var availableLocations = new RentLocationType[] { RentLocationType.Airport, RentLocationType.Office, RentLocationType.CityCenter };

            return availableLocations[_random.Next(availableLocations.Length)];
        }

        private DateTime RollDateTime(DateTime from, DateTime to)
        {
            from = new DateTime(from.Year, from.Month, from.Day, from.Hour, from.Minute, 0);
            to = new DateTime(to.Year, to.Month, to.Day, to.Hour, to.Minute, 0);

            if (to < from)
            {
                var buff = from;
                from = to;
                to = buff;
            }

            var spanDiff = to - from;
            var newSpan = new TimeSpan(0, _random.Next((int)spanDiff.TotalMinutes), 0);
            var result = from.Add(newSpan);

            return result;
        }

        private void PopulateReports(Rent rent)
        {
            switch(rent.Status)
            {
                case RentStatusType.Renting:
                    rent.PickupReport = GenerateReport(rent, true);
                    break;
                case RentStatusType.Returned:
                    rent.PickupReport = GenerateReport(rent, true);
                    rent.ReturnReport = GenerateReport(rent, false);
                    break;
                default:
                    return;
            }
        }

        private CarTransferReport GenerateReport(Rent rent, bool isPickup)
        {
            var report = new CarTransferReport();

            if(isPickup)
            {
                report.Type = CarTransferReportType.Pickup;
                report.Date = rent.ActualPickup;
                report.Title = $"Pickup - {rent.Title}";
            }
            else
            {
                report.Type = CarTransferReportType.Return;
                report.Date = rent.ActualReturn;
                report.Title = $"Return - {rent.Title}";
            }

            report.Car = rent.Car;

            var roll = _random.NextDouble();
            if (roll > 0.95)
            {
                report.IsDamaged = true;
                report.DamageDescription = "damage";
            }


            return report;
        }

        private void PopulateActualData(Rent rent)
        {
            switch(rent.Status)
            {
                case RentStatusType.Renting:
                    rent.ActualPickup = RollDateTime(rent.ReservedPickup, rent.ReservedPickup.AddHours(2));
                    break;
                case RentStatusType.Returned:
                    rent.ActualPickup = RollDateTime(rent.ReservedPickup, rent.ReservedPickup.AddHours(2));

                    DateTime availableMinReturnDate = rent.ReservedReturn.AddHours(-2) < rent.ActualPickup ? rent.ActualPickup : rent.ReservedReturn.AddHours(-2);
                    rent.ActualReturn = RollDateTime(availableMinReturnDate, rent.ReservedReturn.AddHours(2));
                    break;
                default: return;
            }            
        }

        private void PopulatePaidData(Rent rent)
        {
            double weight = -1;

            switch (rent.Status)
            {
                case RentStatusType.Confirmed:
                    weight = 0.9;
                    break;
                case RentStatusType.Renting:
                    weight = 0.999;
                    break;
                case RentStatusType.Returned:
                    weight = 0.9998;
                    break;
                default: break;
            }

            var roll = _random.NextDouble();
            if (weight > roll)
            {
                rent.IsPaid = true;
            }
        }
    }
}
