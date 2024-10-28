using Infragistics.Win.UltraWinTabControl;
using SincronizadorGPS50.Sage50Connector;
using SincronizadorGPS50.Workflows.Clients;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class ClientSynchronizationManager
   {
      public void Launch
      (
         System.Data.SqlClient.SqlConnection connection,
         CompanyGroup sage50CompanyGroupData,
         UltraTab hostTab
      )
      {
         try
         {
            /////////////////////////////////////////////
            /////////////////////////////////////////////
            // start point of the client synchronization workflow
            /////////////////////////////////////////////
            /////////////////////////////////////////////
            
            /////////////////////////////////////////////
            // enable ClientsTab and set it as selected
            /////////////////////////////////////////////

            MainWindowUIHolder.CustomersTab.Enabled = true;
            MainWindowUIHolder.MainTabControl.SelectedTab = MainWindowUIHolder.CustomersTab;

            /////////////////////////////////////////////
            // launch Clients Tab page generation
            /////////////////////////////////////////////

            new CreateCustomersTabPageUI(
               connection,
               sage50CompanyGroupData
            );
         }
         catch(Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}