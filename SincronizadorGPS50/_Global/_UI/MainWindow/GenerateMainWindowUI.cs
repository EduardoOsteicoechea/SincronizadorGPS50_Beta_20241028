using Infragistics.Win.UltraWinTabControl;
using System.Reflection;

namespace SincronizadorGPS50
{
   internal class GenerateMainWindowUI
   {
      internal GenerateMainWindowUI()
      {
         try
         {
            // MainUltraTabControl

            MainWindowUIHolder.MainWindow.Width = StyleHolder.ScreenWorkableWidth;
            MainWindowUIHolder.MainWindow.Height = StyleHolder.ScreenWorkableHeight;

            MainWindowUIHolder.MainTabControl = new UltraTabControl();
            MainWindowUIHolder.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            MainWindowUIHolder.MainTabControl.TabStop = false;

            MainWindowUIHolder.MainWindow.Controls.Add(MainWindowUIHolder.MainTabControl);

            // MainUltraTabControlTabs

            MainWindowUIHolder.MainTabControl.SelectedTab = MainWindowUIHolder.Sage50ConnectionTab;

            MainWindowUIHolder.Sage50ConnectionTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("Sage50ConnectionTab", "Conexión con Sage50");

            MainWindowUIHolder.ResetDataTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("ResetDataTab", "Restaurar Datos");

            MainWindowUIHolder.CompaniesTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("CompaniesTab", "Empresas");

            MainWindowUIHolder.CustomersTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("CustomersTab", "Clientes");

            MainWindowUIHolder.ProjectsTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("ProjecsTab", "Proyectos");

            MainWindowUIHolder.TaxesTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("TaxesTab", "Impuestos");

            MainWindowUIHolder.SubaccountableAccountsTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("SubaccountableAccountsTab", "Cuentas Contables");

            MainWindowUIHolder.ProvidersTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("ProvidersTab", "Proveedores");

            MainWindowUIHolder.IssuedBillsTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("IssuedBillsTab", "Facturas Emitidas");

            MainWindowUIHolder.ReceivedBillsTab = 
            MainWindowUIHolder.MainTabControl.Tabs.Add("ReceivedBillsTab", "Facturas Recibidas");

            foreach(UltraTab tab in MainWindowUIHolder.MainTabControl.Tabs)
            {
               tab.Enabled = false;
            };

            MainWindowUIHolder.MainTabControl.SelectedTab.Enabled = true;

            MainWindowUIHolder.MainWindow.Controls.Add(MainWindowUIHolder.MainTabControl);
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }
   }
}
