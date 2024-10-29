using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SincronizadorGPS50
{
   internal class CreateCenterRowUI
   {
      internal List<UltraGridRow> UltraGridRowList { get; set; } = new List<UltraGridRow>();
      internal List<int> GestprojectClientIdList { get; set; } = new List<int>();
      internal CreateCenterRowUI
      (
         System.Data.SqlClient.SqlConnection connection,
         CompanyGroup sage50CompanyGroupData
      )
      {
         try
         {
            ClientsUIHolder.ClientDataTable = new UltraGrid();

            ClientsUIHolder.ClientDataTable.DisplayLayout.Override.FilterUIProvider = new ColumnsFilter();
            ClientsUIHolder.ClientDataTable.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            ClientsUIHolder.ClientDataTable.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
            ClientsUIHolder.ClientDataTable.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;

            ClientsUIHolder.ClientDataTable.DisplayLayout.Override.AllowColSizing = AllowColSizing.Free;
            ClientsUIHolder.ClientDataTable.DisplayLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
            ClientsUIHolder.ClientDataTable.DisplayLayout.Override.DefaultColWidth = 80;

            ClientsUIHolder.ClientDataTable.Dock = System.Windows.Forms.DockStyle.Fill;

            DataTable synchronizationTable =  CustomerSynchronizationDataTable.Create(
               connection, 
               sage50CompanyGroupData, 
               new GestprojectDataManager.CustomerSyncronizationTableSchema()
            );

            ClientsUIHolder.ClientDataTable.DataSource = synchronizationTable;

            ClientsUIHolder.CenterRow.ClientArea.Controls.Add(ClientsUIHolder.ClientDataTable);

            ///////////////////////////////////
            // Handle events
            ///////////////////////////////////

            ClientsUIHolder.ClientDataTable.AfterRowFilterChanged += ClientDataTable_AfterRowFilterChanged;
            ClientsUIHolder.ClientDataTable.ClickCell += ManageUserInteractionWithUI.ConfigureTable;
         }
         catch(System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }

      private void ClientDataTable_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
      {
         ManageUserInteractionWithUI.DeselectRows(ClientsUIHolder.ClientDataTable);
      }
   }
}