using SincronizadorGPS50.GestprojectDataManager;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinTabControl;
using SincronizadorGPS50.Workflows.Clients;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Sage50Connection
{
	internal class ConnectPanel : ISage50ConnectionUIStateTracker, ISage50ConnectionUIComponent
	{
		public bool IsConnected { get; set; } = false;
		public UltraPanel Panel { get; set; } = null;
		public TableLayoutPanel PanelTableLayoutPanel { get; set; } = null;
		public UltraButton ConnectButton { get; set; } = null;
		public System.Windows.Forms.ImageList ImageList { get; set; } = new ImageList();
		public Sage50ConnectionUIManager Sage50ConnectionUIManager { get; set; } = null;

		public bool IsDataCleared => throw new NotImplementedException();

		public event EventHandler ConnectionStateChanged;
		public event EventHandler DataCleared;

		public ConnectPanel(
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
			PanelTableLayoutPanel.RowCount = 1;
			PanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
			PanelTableLayoutPanel.Dock = DockStyle.Fill;

			ImageList.Images.Add(Resources.SemaforoRojo);
			ImageList.Images.Add(Resources.Semaforo_verde);

			ConnectButton = new UltraButton();
			ConnectButton.Dock = DockStyle.Fill;
			ConnectButton.AutoSize = true;
			ConnectButton.Text = "Conectar";

			PanelTableLayoutPanel.Controls.Add(ConnectButton, 0, 0);

			Panel.ClientArea.Controls.Add(PanelTableLayoutPanel);

			parentControl.Add(Panel, parentControlColumn, parentControlRow);

			/////////////////////////////////
			// Handle Events
			/////////////////////////////////

			ConnectButton.Click += ConnectButton_Click;
		}

		private void ConnectButton_Click(object sender, EventArgs e)
		{
			if(
				SincronizadorGPS50.Sage50Connector
				.Sage50CompanyGroupActions
				.ChangeCompanyGroup(
					Sage50ConnectionUIManager.SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Text
				)
			)
			{
				Sage50ConnectionUIManager.ShowConnectionStateUI.StateImage1.Image = Sage50ConnectionUIManager.ShowConnectionStateUI.ImageList.Images[1];
				Sage50ConnectionUIManager.SetDataAcceptedAndConnetedUI();

				if(Sage50ConnectionUIManager.RememberAllDataUI.CheckBox.Checked)
				{

					var sage50CompanyGroup = SincronizadorGPS50.Sage50Connector
				   .Sage50CompanyGroupActions
				   .GetCompanyGroups()
				   .FirstOrDefault(companyGroup => companyGroup.CompanyName == Sage50ConnectionUIManager.SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Text);

					GestprojectDataManager.ManageRememberableUserData.Save(
					   GestprojectDataHolder.GestprojectDatabaseConnection,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_ID,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_PERSONAL,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_PERFIL,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
					   GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID,
					   Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.LocalInstanceTextBox.Text,
					   Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.UsernameTextBox.Text,
					   Sage50ConnectionUIManager.GetLocalTerminalUserDataUI.PasswordTextBox.Text,
					   Sage50ConnectionUIManager.SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Text,
					   sage50CompanyGroup.CompanyMainCode,
					   sage50CompanyGroup.CompanyCode,
					   sage50CompanyGroup.CompanyGuidId
					);

					Sage50ConnectionUIManager.SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Appearance.BackColor = StyleHolder.c_gray_200;
					Sage50ConnectionUIManager.SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Appearance.ForeColor = StyleHolder.c_gray_100;

					/////////////////////////////////////
					/// Launch Tab Pages genration
					/////////////////////////////////////

					TabLauncher.LaunchTabs(Sage50ConnectionUIManager.SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Text);
				}
				else
				{
					ManageRememberableUserData.ForgetUserRememberableData(
					   GestprojectDataHolder.GestprojectDatabaseConnection,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
					   GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID
					);
					ManageRememberableUserData.ChangeRememberUserDataFeature(
					   GestprojectDataHolder.GestprojectDatabaseConnection,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
					   GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
					   GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID,
					   0
					);
				};
			}
			else
			{
				MessageBox.Show("Encontramos un error con el grupo de empresas");
				SincronizadorGPS50.Sage50Connector.ConnectionActions.Disconnect();
				Sage50ConnectionUIManager.SetStatelessStartUI();
			};
		}

		public void EnableControls() { ConnectButton.Enabled = true; }
		public void DisableControls()
		{
			ConnectButton.Enabled = false;
		}
		public void SetUIToConnected() { DisableControls(); }
		public void SetUIToDisconnected() { EnableControls(); }

		public void Forget() { }
		public void Remember() { }
		public void Dispose()
		{
			Panel.Dispose();
			GC.SuppressFinalize(Panel);
		}
	}
}