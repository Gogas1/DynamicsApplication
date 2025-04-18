using DynamicsApplication.Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicsApplication.Mapping
{
    internal static class DynamicsEntityMappingExtensions
    {
        public static CarClass MapToCarClass(this Entity entity)
        {
            var result = new CarClass();
            result.Id = entity.Id;

            if(entity.Attributes.TryGetValue("svnt_classcode", out var classCodeObj) && classCodeObj is string classCode)
            {
                result.ClassCode = classCode;
            }

            if (entity.Attributes.TryGetValue("svnt_classdescription", out var classDescriptionObj) && classDescriptionObj is string classDescription)
            {
                result.ClassDescription = classDescription;
            }

            if (entity.Attributes.TryGetValue("svnt_price", out var priceObj) && priceObj is Money price)
            {
                result.Price = price.Value;
            }

            return result;
        }
        public static Car MapToCar(this Entity entity)
        {
            var result = new Car();
            result.Id = entity.Id;

            if (entity.Attributes.TryGetValue("svnt_vinnumber", out var vinnumObj) && vinnumObj is string vinNumber)
            {
                result.VinNumber = vinNumber;
            }

            if (entity.Attributes.TryGetValue("svnt_carmodel", out var carModelObj) && carModelObj is string carModel)
            {
                result.CarModel = carModel;
            }

            if (entity.Attributes.TryGetValue("svnt_carmanufacturer", out var manufacturerObj) && manufacturerObj is OptionSetValue manufacturer)
            {
                var value = manufacturer.Value;
                if(Enum.IsDefined(typeof(ManufacturerType), value))
                {
                    result.Manufacturer = (ManufacturerType)manufacturer.Value;
                }
                else
                {
                    throw new ArgumentException($"Car manufacturer is not defined in the {nameof(ManufacturerType)} enum. Entity Id: {entity.Id}");
                }
            }

            if(entity.Attributes.TryGetValue("svnt_productiondate", out var prodDateObj) &&  prodDateObj is DateTime prodDate)
            {
                result.ProductionDate = prodDate;
            }

            if (entity.Attributes.TryGetValue("svnt_purchasedate", out var purchDateObj) && purchDateObj is DateTime purchDate)
            {
                result.PurchaseDate = purchDate;
            }

            if(entity.Attributes.TryGetValue("svnt_carclass", out var carClassObj) &&  carClassObj is EntityReference carClass)
            {                
                result.CarClassId = carClass.Id;
            }

            return result;
        }

        public static Customer MapToCustomer(this Entity entity)
        {
            var result = new Customer();
            result.Id = entity.Id;

            if(entity.Attributes.TryGetValue("firstname", out var firstNameObj) && firstNameObj is string firstName)
            {
                result.FirstName = firstName;
            }

            if(entity.Attributes.TryGetValue("lastname", out var lastNameObj) && lastNameObj is string lastName)
            {
                result.LastName = lastName;
            }

            if(entity.Attributes.TryGetValue("emailaddress1", out var emailAdressObj) && emailAdressObj is string emailAdress)
            {
                result.Email = emailAdress;
            }

            if(entity.Attributes.TryGetValue("mobilephone", out var phoneObj) && phoneObj is string phone)
            {
                result.Phone = phone;
            }

            return result;
        }
    }
}
