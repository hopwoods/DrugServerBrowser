using Microsoft.Extensions.Logging;
using System;
using DrugServerConsole.Utilities;

namespace DrugServerConsole.Services
{
    public class ProductSearchService : IProductSearchService
    {
        private const string Message1 = "Please type a product name to search for and press 'Enter' or type 'exit' to quit.";
        private readonly ILogger<ProductSearchService> _logger;
        private readonly IProductUtility _productUtility;

        public ProductSearchService(ILogger<ProductSearchService> logger, IProductUtility productUtility)
        {
            _logger = logger;
            _productUtility = productUtility;
        }

        public void Execute()
        {
            _logger.LogInformation("DrugServer Search Service Started.");

            Console.WriteLine();
            Console.WriteLine(Message1);
            var searchTerm = GetAndValidateInput();

            do
            {
                _logger.LogInformation($"Searching for products with search term {searchTerm}");
                _productUtility.ListProductsByName(searchTerm);
                Console.WriteLine(Message1);
                searchTerm = GetAndValidateInput();

            } while (searchTerm != null && searchTerm.ToLower() != "exit");
        }

        private static string GetAndValidateInput()
        {
            Console.WriteLine();
            var searchTerm = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(searchTerm))
                return searchTerm;

            Console.WriteLine(Message1);
            GetAndValidateInput();

            return searchTerm;
        }

    }
}
