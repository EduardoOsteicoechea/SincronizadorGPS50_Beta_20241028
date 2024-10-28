using SincronizadorGPS50.GestprojectDataManager;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using sage.ew.db;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Sage50Connection
{
   internal class SelectCompanyGroupPanel : ISage50ConnectionUIStateTracker, ISage50ConnectionUIComponent
   {
      public bool IsConnected { get; set; } = false;
      public UltraPanel Panel { get; set; } = null;
      public TableLayoutPanel PanelTableLayoutPanel { get; set; } = null;
      public UltraLabel SelectEnterpryseGroupLabel { get; set; } = null;
      public UltraComboEditor SelectEnterpryseGroupMenu { get; set; } = null;
      public UltraButton ConnectButton { get; set; } = null;
      public System.Windows.Forms.ImageList ImageList { get; set; } = new ImageList();
      public Sage50ConnectionUIManager Sage50ConnectionUIManager { get; set; } = null;

      public bool IsDataCleared => throw new NotImplementedException();

      public event EventHandler ConnectionStateChanged;
      public event EventHandler DataCleared;

      public SelectCompanyGroupPanel
      (
          Sage50ConnectionUIManager sage50ConnectionUIManager,
          System.Windows.Forms.TableLayoutControlCollection parentControl,
          int parentControlColumn,
          int parentControlRow
      )
      {
         Sage50ConnectionUIManager = sage50ConnectionUIManager;

         Panel = new UltraPanel();
         Panel.Dock = DockStyle.Fill;

         PanelTableLayoutPanel = new TableLayoutPanel();
         PanelTableLayoutPanel.ColumnCount = 1;
         PanelTableLayoutPanel.RowCount = 2;
         PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
         PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
         PanelTableLayoutPanel.Dock = DockStyle.Fill;

         ImageList.Images.Add(Resources.SemaforoRojo);
         ImageList.Images.Add(Resources.Semaforo_verde);

         // EnterpryseGroup
         // EnterpryseGroup
         // EnterpryseGroup
         // EnterpryseGroup
         // EnterpryseGroup

         SelectEnterpryseGroupLabel = new UltraLabel();
         SelectEnterpryseGroupLabel.Dock = DockStyle.Fill;
         SelectEnterpryseGroupLabel.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
         SelectEnterpryseGroupLabel.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;
         SelectEnterpryseGroupLabel.Text = "Grupo de empresa";


         SelectEnterpryseGroupMenu = new UltraComboEditor();
         SelectEnterpryseGroupMenu.Dock = DockStyle.Fill;

         PanelTableLayoutPanel.Controls.Add(SelectEnterpryseGroupLabel, 0, 0);
         PanelTableLayoutPanel.Controls.Add(SelectEnterpryseGroupMenu, 0, 1);

         Panel.ClientArea.Controls.Add(PanelTableLayoutPanel);

         parentControl.Add(Panel, parentControlColumn, parentControlRow);
      }

      public void EnableControls() { SelectEnterpryseGroupMenu.Enabled = true; }
      public void DisableControls() { SelectEnterpryseGroupMenu.Enabled = false; }
      public void SetUIToConnected() { DisableControls(); }
      public void SetUIToDisconnected() { EnableControls(); }
      public void GetCompanyGroupsFromTerminal()
      {
         List<SincronizadorGPS50.Sage50Connector.CompanyGroup> sage50CompanyGroupsList = SincronizadorGPS50.Sage50Connector.Sage50CompanyGroupActions.GetCompanyGroups();

         //MessageBox.Show(DB.SQLDatabase("COMUNES"));

         for(global::System.Int32 i = 0; i < sage50CompanyGroupsList.Count; i++)
         {
            SelectEnterpryseGroupMenu.Items.Add(sage50CompanyGroupsList[i].CompanyName);
         };

         SelectEnterpryseGroupMenu.SelectedIndex = 0;
      }
      public void Forget() => throw new NotImplementedException();
      public void Remember()
      {
         SynchronizerUserRememberableDataModel userRememberableData = GestprojectDataManager.ManageRememberableUserData.GetSynchronizerUserRememberableDataForConnection(GestprojectDataHolder.GestprojectDatabaseConnection);

         //MessageBox.Show(userRememberableData.SAGE_50_LOCAL_TERMINAL_PATH);

         SincronizadorGPS50.Sage50Connector.ConnectionActions.Connect(
            userRememberableData.SAGE_50_LOCAL_TERMINAL_PATH,
            userRememberableData.SAGE_50_USER_NAME,
            userRememberableData.SAGE_50_PASSWORD
         );

         List<SincronizadorGPS50.Sage50Connector.CompanyGroup> sageCompanyGroups = Sage50CompanyGroupActions.GetCompanyGroups();

         int selectEnterpryseGroupindex = 0;

         for(global::System.Int32 i = 0; i < sageCompanyGroups.Count; i++)
         {
            SelectEnterpryseGroupMenu.Items.Add(sageCompanyGroups[i].CompanyName);
            if(userRememberableData.SAGE_50_COMPANY_GROUP_NAME.Trim() == sageCompanyGroups[i].CompanyName.Trim())
            {
               selectEnterpryseGroupindex = i;
            };
         };
         SelectEnterpryseGroupMenu.SelectedIndex = selectEnterpryseGroupindex;
      }
      public void Dispose()
      {
         Panel.Dispose();
         GC.SuppressFinalize(Panel);
      }
   }
}