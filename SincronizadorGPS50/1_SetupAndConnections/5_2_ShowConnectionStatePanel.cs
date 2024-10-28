using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using sage.ew.objetos;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Sage50Connection {
   internal class ShowConnectionStatePanel : ISage50ConnectionUIStateTracker {
      public bool IsConnected { get; set; } = false;
      public UltraPanel Panel { get; set; } = null;
      public TableLayoutPanel PanelTableLayoutPanel { get; set; } = null;
      public UltraLabel GestprojectUserInformationLabel { get; set; } = null;
      public UltraLabel StateLabel { get; set; } = null;
      public UltraPictureBox StateImage1 { get; set; } = null;
      public UltraPictureBox StateImage2 { get; set; } = null;
      public UltraPictureBox StateImage3 { get; set; } = null;
      public System.Windows.Forms.ImageList ImageList { get; set; } = new ImageList();
      public Sage50ConnectionUIManager Sage50ConnectionUIManager { get; set; } = null;

      public ShowConnectionStatePanel
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
         PanelTableLayoutPanel.RowCount = 3;
         PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
         PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60f));
         //PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
         //PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
         //PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 34f));
         PanelTableLayoutPanel.Dock = DockStyle.Fill;

         ImageList.Images.Add(Resources.SemaforoRojo);
         ImageList.Images.Add(Resources.Semaforo_verde);

         GestprojectUserInformationLabel = new UltraLabel();
         GestprojectUserInformationLabel.Dock = DockStyle.Fill;
         GestprojectUserInformationLabel.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
         GestprojectUserInformationLabel.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;
         GestprojectUserInformationLabel.Text = "UsuarioGP: " + GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO ?? "";
         GestprojectUserInformationLabel.Text += "; EquipoLocal: " + GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO ?? "";
         GestprojectUserInformationLabel.Text += "; NombrePersonal: " + GestprojectDataHolder.LocalDeviceUserSessionData.CNX_PERSONAL.ToString() ?? "";
         GestprojectUserInformationLabel.Text += "; IdUsuarioGP: " + GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID.ToString() ?? "";
         GestprojectUserInformationLabel.WrapText = true;

         StateLabel = new UltraLabel();
         StateLabel.Dock = DockStyle.Fill;
         StateLabel.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
         StateLabel.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;

         StateImage1 = new UltraPictureBox();
         StateImage1.Height = 20;
         StateImage1.Width = 20;
         StateImage1.Dock = DockStyle.Fill;
         StateImage1.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
         StateImage1.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;

         UltraPanel StateIconsPanel = new UltraPanel();
         StateIconsPanel.Dock = DockStyle.Fill;

         TableLayoutPanel StateIconsPanelTableLayoutPanel = new TableLayoutPanel();
         StateIconsPanelTableLayoutPanel.RowCount = 1;
         StateIconsPanelTableLayoutPanel.ColumnCount = 3;
         StateIconsPanelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f));
         StateIconsPanelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
         StateIconsPanelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f));
         StateIconsPanelTableLayoutPanel.Dock = DockStyle.Fill;

         StateIconsPanelTableLayoutPanel.Controls.Add(StateImage1, 1, 0);

         StateIconsPanel.ClientArea.Controls.Add(StateIconsPanelTableLayoutPanel);

         SetUIToDisconnected();

         //PanelTableLayoutPanel.Controls.Add(GestprojectUserInformationLabel, 0, 0);
         //PanelTableLayoutPanel.Controls.Add(StateLabel, 0, 1);
         //PanelTableLayoutPanel.Controls.Add(StateIconsPanel, 0, 2);
         PanelTableLayoutPanel.Controls.Add(StateLabel, 0, 0);
         PanelTableLayoutPanel.Controls.Add(StateIconsPanel, 0, 1);

         Panel.ClientArea.Controls.Add(PanelTableLayoutPanel);

         parentControl.Add(Panel, parentControlColumn, parentControlRow);
      }

      public void SetUIToConnected() {
         IsConnected = true;
         StateLabel.Text = "Conectado";
         StateImage1.Image = ImageList.Images[1];
      }
      public void SetUIToDisconnected() {
         IsConnected = false;
         StateLabel.Text = "Desconectado";
         StateImage1.Image = ImageList.Images[0];
      }

      public void Dispose() {
         foreach(Control control in PanelTableLayoutPanel.Controls) {
            if(control is IDisposable disposableControl) {
               disposableControl.Dispose();
               GC.SuppressFinalize(control);
            }
         };

         PanelTableLayoutPanel.Dispose();
         GC.SuppressFinalize(PanelTableLayoutPanel);

         Panel.Dispose();
         GC.SuppressFinalize(Panel);
      }
   }
}