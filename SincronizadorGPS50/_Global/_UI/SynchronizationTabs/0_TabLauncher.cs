using System.Reflection;

namespace SincronizadorGPS50
{
	public static class TabLauncher
	{
		public static void LaunchTabs(string selectedCompanyGroupName) 
		{
         try
         {
			   IGestprojectConnectionManager gestprojectConnectionManager = new GestprojectConnectionManager();
			   ISage50ConnectionManager sage50ConnectionManager = new Sage50ConnectionManager(selectedCompanyGroupName);

			   new CompaniesSynchronizationManager()
            .Launch( gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.CompaniesTab );

			   //new ClientSynchronizationManager()
      //      .Launch( GestprojectDataHolder.GestprojectDatabaseConnection,sage50ConnectionManager.CompanyGroupData,MainWindowUIHolder.CustomersTab );

            new CustomersSynchronizationManager().Launch(gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.CustomersTab);

			   new ProviderSynchronizationManager()
            .Launch( gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.ProvidersTab );

            new ProjectsSynchronizationManager()
            .Launch( gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.ProjectsTab );

            new TaxesSynchronizationManager()
            .Launch(gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.TaxesTab);

            new SubaccountableAccountsSynchronizationManager()
            .Launch(gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.SubaccountableAccountsTab);

            new IssuedInvoicesSynchronizationManager()
            .Launch(gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.IssuedBillsTab);

            new ReceivedInvoicesSynchronizationManager()
            .Launch(gestprojectConnectionManager, sage50ConnectionManager, MainWindowUIHolder.ReceivedBillsTab);
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}
