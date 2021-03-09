using FirstDataBank.DrugServer.API;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DrugServerConsole
{
    internal class ProductUtility : IProductUtility
    {
        private readonly IDrugSystemFactory _drugSystemFactory;
        private IDrugSystem _drugSystem;

        public ProductUtility()
        {
            _drugSystemFactory = new DrugSystemFactory()
        }

        public void ListProductsByName(string searchTerm)
        {
            _drugSystem = _drugSystemFactory.CreateSystem();
            _drugSystem.Environment.DrugTerminology = DrugTerminology.MDDF;
            _drugSystem.Environment.AttributePopulation.All();

            var filter = new Filter
            {
                DrugClass =
                {
                    Brand = true,
                    BrandedGeneric = true
                }
            };
            var products = _drugSystem.Navigation.GetProductsByName(searchTerm, filter);

            foreach (var product in products)
            {
                Console.WriteLine($"Product Name: {product.PrimaryPreferredName}");
            }
        }
    }

}
