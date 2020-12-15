using FirstDataBank.DrugServer.API;

namespace DrugServer_Browser.DrugSystem
{
    public class BrowserDrugSystem : IBrowserDrugSystem
    {
        public IDrugSystem DrugSystem { get; }

        public BrowserDrugSystem(IDrugSystemFactory drugSystemFactory)
        {
            DrugSystem = drugSystemFactory.CreateSystem();
        }
    }
}
