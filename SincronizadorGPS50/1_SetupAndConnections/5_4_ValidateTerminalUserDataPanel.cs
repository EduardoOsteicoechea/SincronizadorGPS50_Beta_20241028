using Infragistics.Win.Misc;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Sage50Connection {
   internal class ValidateTerminalUserDataPanel : ISage50ConnectionUIStateTracker, ISage50ConnectionUIModifiableControls {
      public bool IsConnected { get; set; } = false;
      public bool AreControlsEnabled { get; set; } = false;
      public System.Windows.Forms.ImageList ImageList { get; set; } = new ImageList();
      public UltraButton ConnectButton { get; set; } = null;
      public UltraLabel ConectingMessageLabel { get; set; } = null;
      public UltraPanel Panel { get; set; } = null;
      public TableLayoutPanel PanelTableLayoutPanel { get; set; } = null;
      public Sage50ConnectionUIManager Sage50ConnectionUIManager { get; set; } = null;

      public ValidateTerminalUserDataPanel
      (
          Sage50ConnectionUIManager sage50ConnectionUIManager,
          System.Windows.Forms.TableLayoutControlCollection parentControl,
          int parentControlColumn,
          int parentControlRow
      ){
         Sage50ConnectionUIManager = sage50ConnectionUIManager;

         Panel = new UltraPanel();
         Panel.Dock = DockStyle.Fill;

         PanelTableLayoutPanel = new TableLayoutPanel();
         PanelTableLayoutPanel.ColumnCount = 1;
         PanelTableLayoutPanel.RowCount = 1;
         PanelTableLayoutPanel.Dock = DockStyle.Fill;

         ConnectButton = new UltraButton();
         ConnectButton.Dock = DockStyle.Fill;
         ConnectButton.AutoSize = true;
         ConnectButton.ShowOutline = false;
         ConnectButton.Text = "Validar Terminal";

         ConectingMessageLabel = new UltraLabel();
         ConectingMessageLabel.Dock = DockStyle.Fill;
         ConectingMessageLabel.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
         ConectingMessageLabel.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;
         ConectingMessageLabel.Text = "Validando terminal. Espere un momento";

         PanelTableLayoutPanel.Controls.Add(ConnectButton, 0, 0);

         Panel.ClientArea.Controls.Add(PanelTableLayoutPanel);

         parentControl.Add(Panel, parentControlColumn, parentControlRow);

         ////////////////////////////////
         // Handle Events
         ////////////////////////////////

         ConnectButton.Click += ConnectButton_Click;
      }

      private async void ConnectButton_Click(object sender, EventArgs e) {
         Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.SetUIToConnected();
         PanelTableLayoutPanel.Controls.Remove(ConnectButton);
         PanelTableLayoutPanel.Controls.Add(ConectingMessageLabel, 0, 0);
         await Task.Delay(500);

         if(SincronizadorGPS50.Sage50Connector.ConnectionActions.Connect(
               Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.LocalInstanceTextBox.Text,
               Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.UsernameTextBox.Text,
               Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.PasswordTextBox.Text
             )
         ){
            ConectingMessageLabel.Text = "Terminal validado";
            Sage50ConnectionUIManager.SetValidatedTerminalSelectCompanyGroupUI();
         }
         else {
            PanelTableLayoutPanel.Controls.Remove(ConectingMessageLabel);
            PanelTableLayoutPanel.Controls.Add(ConnectButton, 0, 0);
            Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.SetUIToDisconnected();
         }
      }

      public void SetUIToAwaitingForData() {
         DisableControls();
      }
      public void SetUIToReadyForValidation() {
         EnableControls();
      }
      public void SetUIToValidationExecuted() { 
         DisableControls();
      }
      public void SetUIToConnected() {
         IsConnected = true;
         DisableControls();
      }
      public void SetUIToDisconnected() {
         IsConnected = false;
         EnableControls();
      }
      public void EnableControls() {
         ConnectButton.Enabled = true;
      }
      public void DisableControls() {
         ConnectButton.Enabled = false;
      }
      public void Dispose() {
         Panel.Dispose();
         GC.SuppressFinalize(Panel);
      }
   }
}