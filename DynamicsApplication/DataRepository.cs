using DynamicsApplication.Entities;
using DynamicsApplication.Mapping;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication
{
    internal class DataRepository
    {
        public Dictionary<Guid, CarClass> CarClasses = new Dictionary<Guid, CarClass>();
        public List<Car> Cars = new List<Car>();
        public List<Customer> Customers = new List<Customer>();
        public List<Rent> Rents = new List<Rent>();
        public List<CarTransferReport> TransferReports = new List<CarTransferReport>();

        private IOrganizationService _organizationService;

        public DataRepository(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public void RetreiveData()
        {
            RetreiveCarClasses();
            RetreiveCars();
            RetreiveCustomers();
        }

        private void RetreiveCarClasses()
        {
            CarClasses.Clear();

            RetreiveEntities("svnt_carclass", CarClasses, DynamicsEntityMappingExtensions.MapToCarClass, "svnt_classcode", "svnt_classdescription", "svnt_price");
        }

        private void RetreiveCars()
        {
            Cars.Clear();

            RetreiveEntities("svnt_car", Cars, DynamicsEntityMappingExtensions.MapToCar, "svnt_vinnumber", "svnt_carmodel", "svnt_carmanufacturer", "svnt_productiondate", "svnt_purchasedate", "svnt_carclass");

            foreach (var car in Cars)
            {
                if (CarClasses.TryGetValue(car.CarClassId, out CarClass carClass))
                {
                    car.CarClass = carClass;
                }
            }
        }

        private void RetreiveCustomers()
        {
            Customers.Clear();

            RetreiveEntities("contact", Customers, DynamicsEntityMappingExtensions.MapToCustomer, "firstname", "lastname", "emailaddress1", "mobilephone");
        }

        private void RetreiveEntities<T>(string entityName, ICollection<T> outCollection, Func<Entity, T> mappingFunc, params string[] fields)
        {
            var query = new QueryExpression(entityName);
            query.ColumnSet = new ColumnSet(fields);

            var result = _organizationService.RetrieveMultiple(query);

            foreach (var entity in result.Entities)
            {
                outCollection.Add(mappingFunc(entity));
            }
        }

        private void RetreiveEntities<T>(string entityName, IDictionary<Guid, T> outCollection, Func<Entity, T> mappingFunc, params string[] fields)
        {
            var query = new QueryExpression(entityName);
            query.ColumnSet = new ColumnSet(fields);

            var result = _organizationService.RetrieveMultiple(query);

            foreach (var entity in result.Entities)
            {
                outCollection.Add(entity.Id, mappingFunc(entity));
            }
        }

        private void RetreiveEntities<T>(string entityName, ICollection<T> outCollection, Func<Entity, T> mappingFunc)
        {
            var query = new QueryExpression(entityName);
            query.ColumnSet = new ColumnSet(true);

            var result = _organizationService.RetrieveMultiple(query);

            foreach (var entity in result.Entities)
            {
                outCollection.Add(mappingFunc(entity));
            }
        }
    }
}
