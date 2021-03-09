using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstDataBank.DrugServer.API;

namespace ExampleWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDrugSystemFactory _factory;
        
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IDrugSystemFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        public void OnGet()
        {
            var drugSystem = _factory.CreateSystem();
            var filter = new Filter
            {
                DrugClass =
                {
                    Brand = true,
                    BrandedGeneric = true
                }
            };
            var products = drugSystem.Navigation.GetProductsByName("para*", filter);

        }
    }
}
