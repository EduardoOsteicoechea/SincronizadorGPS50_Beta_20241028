using Infragistics.Win.Misc;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Clients
{
   internal class CreateCustomersTabPageUI
   {
      internal CreateCustomersTabPageUI
      (
         System.Data.SqlClient.SqlConnection connection,
         CompanyGroup sage50CompanyGroupData
      )
      {
         try
         {
            ////////////////////////////////////////
            // ClientsTab Rows Container
            ////////////////////////////////////////

            ClientsUIHolder.MainPanel = new UltraPanel();
            ClientsUIHolder.MainPanel.Dock = DockStyle.Fill;

            ClientsUIHolder.TableLayoutPanel = new TableLayoutPanel();
            ClientsUIHolder.TableLayoutPanel.ColumnCount = 1;
            ClientsUIHolder.TableLayoutPanel.RowCount = 3;
            ClientsUIHolder.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            ClientsUIHolder.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 87.50f));
            ClientsUIHolder.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            ClientsUIHolder.TableLayoutPanel.Dock = DockStyle.Fill;

            ClientsUIHolder.MainPanel.ClientArea.Controls.Add(ClientsUIHolder.TableLayoutPanel);

            MainWindowUIHolder.CustomersTab.TabPage.Controls.Add(ClientsUIHolder.MainPanel);

            ////////////////////////////////////////
            // Clients TopRow
            ////////////////////////////////////////

            ClientsUIHolder.TopRow = new UltraPanel();
            ClientsUIHolder.TopRow.Dock = System.Windows.Forms.DockStyle.Fill;
            ClientsUIHolder.TopRow.Appearance.BackColor = StyleHolder.c_transparent;

            ClientsUIHolder.TableLayoutPanel.Controls.Add(ClientsUIHolder.TopRow, 0, 0);

            new CreateTopRowUI(connection, sage50CompanyGroupData);

            ////////////////////////////////////////
            // Clients CenterRow
            ////////////////////////////////////////

            ClientsUIHolder.CenterRow = new UltraPanel();
            ClientsUIHolder.CenterRow.Height = StyleHolder.CenterRowHeight;
            ClientsUIHolder.CenterRow.Dock = System.Windows.Forms.DockStyle.Fill;
            ClientsUIHolder.CenterRow.Appearance.BackColor = StyleHolder.c_white;

            ClientsUIHolder.TableLayoutPanel.Controls.Add(ClientsUIHolder.CenterRow, 0, 1);

            new CreateCenterRowUI(connection, sage50CompanyGroupData);

            ////////////////////////////////////////
            // Clients BottomRow
            ////////////////////////////////////////

            ClientsUIHolder.BottomRow = new UltraPanel();
            ClientsUIHolder.BottomRow.Dock = System.Windows.Forms.DockStyle.Fill;
            ClientsUIHolder.BottomRow.Appearance.BackColor = StyleHolder.c_transparent;

            ClientsUIHolder.TableLayoutPanel.Controls.Add(ClientsUIHolder.BottomRow, 0, 2);

            new CreateBottomRowUI();
         }
         catch(Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
