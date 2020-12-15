using System;

namespace DrugServerConsole
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Startup.BuildConfiguration();
            Startup.ConfigureServices();

            IProductUtility productUtility = new ProductUtility();
            try
            {
                productUtility.ListProductsByName("para*");
            }
            finally
            {
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }
    }
}
