using Infragistics.Win.Misc;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Sage50Connection {
   internal class ManageConnectionPanel : ISage50ConnectionUIStateTracker, ISage50ConnectionUIComponent {
      public bool IsConnected { get; set; } = false;
      public UltraPanel Panel { get; set; } = null;
      public TableLayoutPanel PanelTableLayoutPanel { get; set; } = null;
      public UltraButton ConnectButton1 { get; set; } = null;
      public System.Windows.Forms.ImageList ImageList { get; set; } = new ImageList();
      public Sage50ConnectionUIManager Sage50ConnectionUIManager { get; set; } = null;

      public bool IsDataCleared => throw new NotImplementedException();

      public event EventHandler ConnectionStateChanged;
      public event EventHandler DataCleared;

      public ManageConnectionPanel
      (
          Sage50ConnectionUIManager sage50ConnectionUIManager,
          System.Windows.Forms.TableLayoutControlCollection parentControl,
          int parentControlColumn,
          int parentControlRow
      ) {
         Sage50ConnectionUIManager = sage50ConnectionUIManager;

         Panel = new UltraPanel();
         Panel.Dock = DockStyle.Fill;

         PanelTableLayoutPanel = new TableLayoutPanel();
         PanelTableLayoutPanel.ColumnCount = 1;
         PanelTableLayoutPanel.RowCount = 2;
         PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
         PanelTableLayoutPanel.Dock = DockStyle.Fill;

         ImageList.Images.Add(Resources.Deshacer);
         ImageList.Images.Add(Resources.delete);

         ConnectButton1 = new UltraButton();
         ConnectButton1.Dock = DockStyle.Fill;
         ConnectButton1.AutoSize = true;
         ConnectButton1.Text = "Desconectar";

         PanelTableLayoutPanel.Controls.Add(ConnectButton1, 0, 0);

         Panel.ClientArea.Controls.Add(PanelTableLayoutPanel);

         parentControl.Add(Panel, parentControlColumn, parentControlRow);

         ////////////////////////////////
         // Manage Events
         ////////////////////////////////

         ConnectButton1.Click += ConnectButton1_Click;
      }

      private void ConnectButton1_Click(object sender, EventArgs e) {
         GestprojectDataManager.ManageRememberableUserData.ChangeRememberUserDataFeature(
            GestprojectDataHolder.GestprojectDatabaseConnection,
            GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
            GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
            GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID, 
            0
         );
         GestprojectDataManager.ManageRememberableUserData.ForgetUserRememberableData(
            GestprojectDataHolder.GestprojectDatabaseConnection,
            GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
            GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
            GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID
         );
         Sage50ConnectionUIManager.SetStatelessStartUI();
         Application.Exit();
         Process.Start(Application.ExecutablePath);
      }

      public void EnableControls() {
         ConnectButton1.Enabled = true;
      }
      public void DisableControls() {
         ConnectButton1.Enabled = false;
      }
      public void SetUIToConnected() {
         EnableControls();
      }
      public void SetUIToDisconnected() {
         DisableControls();
      }
      public void Forget() => throw new NotImplementedException();
      public void Remember() => throw new NotImplementedException();
      public void Dispose() {
         Panel.Dispose();
         GC.SuppressFinalize(Panel);
      }
   }
}