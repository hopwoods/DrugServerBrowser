using FirstDataBank.DrugServer.API;
using FirstDataBank.DrugServer.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using DrugServer_Browser.DrugSystem;

namespace DrugServer_Browser.Pages.Products
{
    public class GetProductsModel : PageModel
    {
        [BindProperty]
        public string SearchTerm { get; set; }

        [BindProperty]
        public List<Product> ProductsList { get; set; }

        [BindProperty]
        public Region Region { get; set; }

        private readonly IBrowserDrugSystem _system;

        public GetProductsModel(IBrowserDrugSystem system)
        {
            _system = system;
        }


        public void OnGet()
        {

        }

        public void OnPost()
        {
            try
            {
                _system.DrugSystem.Environment.Language = Language.English;
                _system.DrugSystem.Environment.DrugTerminology = DrugTerminology.MDDF;
                _system.DrugSystem.Environment.AttributePopulation.All();

                var filter = new Filter
                {
                    DrugClass =
                    {
                        Brand = true,
                        BrandedGeneric = true
                    }
                };

                var products = _system.DrugSystem.Navigation.GetProductsByName(SearchTerm, filter);

                ProductsList.AddRange(products);
            }
            catch (FDBApplicationException fdbAppException)
            {
                Console.WriteLine("An FDB Application Exception was thrown");
                Console.WriteLine(fdbAppException.Message);
                if (fdbAppException.InnerException != null)
                    Console.WriteLine(fdbAppException.InnerException.Message);
                Console.WriteLine(fdbAppException.StackTrace);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }
    }
}