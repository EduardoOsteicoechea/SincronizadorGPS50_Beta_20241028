using SincronizadorGPS50.GestprojectDataManager;
using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Sage50Connection
{
    internal class GetLocalTerminalUserDataPanel : ISage50ConnectionUIStateTracker, ISage50ConnectionUIComponent
    {

        public bool IsDataCleared => throw new NotImplementedException();

        public event EventHandler ConnectionStateChanged;
        public event EventHandler DataCleared;
        public bool IsConnected { get; set; } = false;
        public UltraPanel Panel { get; set; } = null;
        public TableLayoutPanel PanelTableLayoutPanel { get; set; } = null;
        public UltraLabel LocalInstanceLabel { get; set; } = null;
        public UltraTextEditor LocalInstanceTextBox { get; set; } = null;
        public UltraLabel UsernameLabel { get; set; } = null;
        public UltraTextEditor UsernameTextBox { get; set; } = null;
        public UltraLabel PasswordLabel { get; set; } = null;
        public UltraTextEditor PasswordTextBox { get; set; } = null;
        public UltraButton ValidateUserDataButton { get; set; } = null;
        public UltraLabel SesionDataValidationLabel { get; set; } = null;
        public Sage50ConnectionUIManager Sage50ConnectionUIManager { get; set; } = null;


        public GetLocalTerminalUserDataPanel
        (
            Sage50ConnectionUIManager sage50ConnectionUIManager,
            System.Windows.Forms.TableLayoutControlCollection parentControl,
            int parentControlColumn,
            int parentControlRow
        )
        {
            Sage50ConnectionUIManager = sage50ConnectionUIManager;

            // Generate Controls
            // Generate Controls
            // Generate Controls
            // Generate Controls
            // Generate Controls

            Panel = new UltraPanel();
            Panel.Dock = DockStyle.Fill;

            PanelTableLayoutPanel = new TableLayoutPanel();
            PanelTableLayoutPanel.ColumnCount = 1;
            PanelTableLayoutPanel.RowCount = 4;
            PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            PanelTableLayoutPanel.Dock = DockStyle.Fill;

            // LocalInstance
            // LocalInstance
            // LocalInstance
            // LocalInstance
            // LocalInstance

            LocalInstanceLabel = new UltraLabel();
            LocalInstanceLabel.Dock = DockStyle.Fill;
            LocalInstanceLabel.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            LocalInstanceLabel.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            LocalInstanceLabel.Text = "Ruta del Terminal de Sage50";

            LocalInstanceTextBox = new UltraTextEditor();
            LocalInstanceTextBox.Dock = DockStyle.Fill;
            LocalInstanceTextBox.Text = "C:\\Sage50\\Sage50Term";

            LocalInstanceLabel.Height = 20;
            LocalInstanceTextBox.Height = 20;

            // Username
            // Username
            // Username
            // Username
            // Username

            UsernameLabel = new UltraLabel();
            UsernameLabel.Dock = DockStyle.Fill;
            UsernameLabel.Text = "Nombre de Usuario";
            UsernameLabel.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            UsernameLabel.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;

            UsernameTextBox = new UltraTextEditor();
            UsernameTextBox.Dock = DockStyle.Fill;

            UsernameLabel.Height = 20;
            UsernameTextBox.Height = 20;

            // Password
            // Password
            // Password
            // Password
            // Password

            PasswordLabel = new UltraLabel();
            PasswordLabel.Dock = DockStyle.Fill;
            PasswordLabel.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            PasswordLabel.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            PasswordLabel.Text = "Contraseña";

            PasswordTextBox = new UltraTextEditor();
            PasswordTextBox.Dock = DockStyle.Fill;
            PasswordTextBox.PasswordChar = '*';

            PasswordLabel.Height = 20;
            PasswordTextBox.Height = 20;

            // Organize username label and password label in panel
            // Organize username label and password label in panel
            // Organize username label and password label in panel
            // Organize username label and password label in panel

            UltraPanel labelsPanel = new UltraPanel();
            labelsPanel.Dock = DockStyle.Fill;

            TableLayoutPanel labelsPanelTableLayoutPanel = new TableLayoutPanel();
            labelsPanelTableLayoutPanel.RowCount = 1;
            labelsPanelTableLayoutPanel.ColumnCount = 2;
            labelsPanelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            labelsPanelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            labelsPanelTableLayoutPanel.Dock = DockStyle.Fill;

            labelsPanelTableLayoutPanel.Controls.Add(UsernameLabel, 0, 0);
            labelsPanelTableLayoutPanel.Controls.Add(PasswordLabel, 1, 0);

            labelsPanel.ClientArea.Controls.Add(labelsPanelTableLayoutPanel);

            // Organize username textBox and password textBox in panel
            // Organize username textBox and password textBox in panel
            // Organize username textBox and password textBox in panel
            // Organize username textBox and password textBox in panel
            // Organize username textBox and password textBox in panel

            UltraPanel textBoxesPanel = new UltraPanel();
            textBoxesPanel.Dock = DockStyle.Fill;

            TableLayoutPanel textBoxesPanelTableLayoutPanel = new TableLayoutPanel();
            textBoxesPanelTableLayoutPanel.RowCount = 1;
            textBoxesPanelTableLayoutPanel.ColumnCount = 2;
            textBoxesPanelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            textBoxesPanelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            textBoxesPanelTableLayoutPanel.Dock = DockStyle.Fill;

            textBoxesPanelTableLayoutPanel.Controls.Add(UsernameTextBox, 0, 0);
            textBoxesPanelTableLayoutPanel.Controls.Add(PasswordTextBox, 1, 0);

            textBoxesPanel.ClientArea.Controls.Add(textBoxesPanelTableLayoutPanel);

            // AddToPanel
            // AddToPanel
            // AddToPanel
            // AddToPanel
            // AddToPanel

            PanelTableLayoutPanel.Controls.Add(LocalInstanceLabel, 0, 0);
            PanelTableLayoutPanel.Controls.Add(LocalInstanceTextBox, 0, 1);
            PanelTableLayoutPanel.Controls.Add(labelsPanel, 0, 2);
            PanelTableLayoutPanel.Controls.Add(textBoxesPanel, 0, 3);

            Panel.ClientArea.Controls.Add(PanelTableLayoutPanel);

            parentControl.Add(Panel, parentControlColumn, parentControlRow);

            // Manage Events
            // Manage Events
            // Manage Events
            // Manage Events
            // Manage Events

            LocalInstanceTextBox.TextChanged += TextBox_TextChanged;
            UsernameTextBox.TextChanged += TextBox_TextChanged;
            PasswordTextBox.TextChanged += TextBox_TextChanged;

        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if
            (
                MainWindowUIHolder.MainWindow.IsHandleCreated
                &&
                LocalInstanceTextBox.Text != ""
                &&
                LocalInstanceTextBox != null
                &&
                UsernameTextBox.Text != ""
                &&
                UsernameTextBox != null
                &&
                PasswordTextBox.Text != ""
                &&
                PasswordTextBox != null
            )
            {
                if(Sage50ConnectionUIManager.ValidateTerminalUserDataUI != null)
                {
                    Sage50ConnectionUIManager.SetEditingTerminalDataUI();
                    Sage50ConnectionUIManager.ValidateTerminalUserDataUI.EnableControls();
                };
            };

            if(StateManager.State == "AwaitingLocalTerminalUserData")
            {
                Sage50ConnectionUIManager.SetEditingTerminalDataUI();
            };
        }

        public void EnableControls()
        {
            LocalInstanceTextBox.Enabled = true;
            LocalInstanceTextBox.Appearance.BackColor = StyleHolder.c_white;
            LocalInstanceTextBox.Appearance.ForeColor = System.Drawing.Color.Black;

            UsernameTextBox.Enabled = true;
            UsernameTextBox.Appearance.BackColor = StyleHolder.c_white;
            UsernameTextBox.Appearance.ForeColor = System.Drawing.Color.Black;

            PasswordTextBox.Enabled = true;
            PasswordTextBox.Appearance.BackColor = StyleHolder.c_white;
            PasswordTextBox.Appearance.ForeColor = System.Drawing.Color.Black;
        }
        public void DisableControls()
        {
            LocalInstanceTextBox.Enabled = false;
            LocalInstanceTextBox.Appearance.BackColor = StyleHolder.c_gray_200;
            LocalInstanceTextBox.Appearance.ForeColor = StyleHolder.c_gray_100;

            UsernameTextBox.Enabled = false;
            UsernameTextBox.Appearance.BackColor = StyleHolder.c_gray_200;
            UsernameTextBox.Appearance.ForeColor = StyleHolder.c_gray_100;

            PasswordTextBox.Enabled = false;
            PasswordTextBox.Appearance.BackColor = StyleHolder.c_gray_200;
            PasswordTextBox.Appearance.ForeColor = StyleHolder.c_gray_100;
        }
        public void Forget() => throw new NotImplementedException();
        public void Remember() 
        {
            SynchronizerUserRememberableDataModel userRememberableData = 
            GestprojectDataManager
            .ManageRememberableUserData
            .GetSynchronizerUserRememberableDataForConnection(
                GestprojectDataHolder.GestprojectDatabaseConnection
            );

            LocalInstanceTextBox.Text = userRememberableData.SAGE_50_LOCAL_TERMINAL_PATH;
            UsernameTextBox.Text = userRememberableData.SAGE_50_USER_NAME;
            PasswordTextBox.Text = userRememberableData.SAGE_50_PASSWORD;

        }
        public void SetUIToConnected()
        {
            IsConnected = true;
            DisableControls();
        }
        public void SetUIToDisconnected()
        {
            IsConnected = false;
            EnableControls();
        }
        public void Dispose()
        {
            Panel.Dispose();
            GC.SuppressFinalize(Panel);
        }
    }
}