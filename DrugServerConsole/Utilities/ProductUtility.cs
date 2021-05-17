using System;
using FirstDataBank.DrugServer.API;

namespace DrugServerConsole.Utilities
{
    internal class ProductUtility : IProductUtility
    {
        private readonly IDrugSystem _drugSystem;

        public ProductUtility(IDrugSystem drugSystem)
        {
            _drugSystem = drugSystem;
            _drugSystem.Environment.Language = Language.English;
            _drugSystem.Environment.DrugTerminology = DrugTerminology.MDDF;
            _drugSystem.Environment.AttributePopulation.All();
        }

        public void ListProductsByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) 
                return;

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
