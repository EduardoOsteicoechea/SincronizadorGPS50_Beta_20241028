

using Infragistics.Documents.Excel;
using System.Linq;

namespace SincronizadorGPS50.Workflows.Sage50Connection
{
	internal class Sage50ConnectionUIManager
	{
		internal System.Windows.Forms.TableLayoutControlCollection ParentControl { get; set; } = null;
		internal ShowConnectionStatePanel ShowConnectionStateUI { get; set; } = null;
		internal GetLocalTerminalUserDataPanel GetLocalTerminalUserDataUI { get; set; } = null;
		internal ValidateTerminalUserDataPanel ValidateTerminalUserDataUI { get; set; } = null;
		internal SelectCompanyGroupPanel SelectCompanyGroupUI { get; set; } = null;
		internal ConnectPanel ConnectUI { get; set; } = null;
		internal RememberAllDataPanel RememberAllDataUI { get; set; } = null;
		internal ManageConnectionPanel ManageConnectionUI { get; set; } = null;
		internal Sage50ConnectionUIManager(System.Windows.Forms.TableLayoutControlCollection parentControl, string uiModel)
		{
			try
			{
				ParentControl = parentControl;
				if(uiModel == "stateless")
				{
					CreateStatelessUI();
				}
				else
				{
					CreateStatefulUI();
				};
			}
			catch(System.Exception exception)
			{
				throw exception;
			};
		}
		internal void CreateStatelessUI()
		{
			RemoveAllUIElements();

			ShowConnectionStateUI = new ShowConnectionStatePanel(this, ParentControl, 0, 1);
			GetLocalTerminalUserDataUI = new GetLocalTerminalUserDataPanel(this, ParentControl, 0, 2);
			ValidateTerminalUserDataUI = new ValidateTerminalUserDataPanel(this, ParentControl, 0, 3);
			ValidateTerminalUserDataUI.SetUIToAwaitingForData();
		}
		internal void CreateStatefulUI()
		{
			RemoveAllUIElements();

			ShowConnectionStateUI = new ShowConnectionStatePanel(this, ParentControl, 0, 1);

			GetLocalTerminalUserDataUI = new GetLocalTerminalUserDataPanel(this, ParentControl, 0, 2);
			GetLocalTerminalUserDataUI.Remember();

			SelectCompanyGroupUI = new SelectCompanyGroupPanel(this, ParentControl, 0, 4);
			SelectCompanyGroupUI.Remember();
			SelectCompanyGroupUI.DisableControls();

			ManageConnectionUI = new ManageConnectionPanel(this, ParentControl, 0, 6);
			ManageConnectionUI.SetUIToConnected();

			SetConnetedUI();

			/////////////////////////////////////
			/// Launch Tab Pages genration
			/////////////////////////////////////

			TabLauncher.LaunchTabs(SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Text);
			;
		}
		internal void SetEditingTerminalDataUI()
		{
			if(SelectCompanyGroupUI != null)
			{
				SelectCompanyGroupUI.Dispose();
				SelectCompanyGroupUI = null;
			};
			if(RememberAllDataUI != null)
			{
				RememberAllDataUI.Dispose();
				RememberAllDataUI = null;
			};
			if(ConnectUI != null)
			{
				ConnectUI.Dispose();
				ConnectUI = null;
			};
			if(ManageConnectionUI != null)
			{
				ManageConnectionUI.Dispose();
				ManageConnectionUI = null;
			};
		}
		internal void SetValidatedTerminalSelectCompanyGroupUI()
		{
			if(SelectCompanyGroupUI != null)
			{
				SelectCompanyGroupUI.Dispose();
				SelectCompanyGroupUI = null;
			};
			if(ConnectUI != null)
			{
				ConnectUI.Dispose();
				ConnectUI = null;
			};
			if(RememberAllDataUI != null)
			{
				RememberAllDataUI.Dispose();
				RememberAllDataUI = null;
			};

			ValidateTerminalUserDataUI.DisableControls();

			SelectCompanyGroupUI = new SelectCompanyGroupPanel(this, ParentControl, 0, 4);
			SelectCompanyGroupUI.GetCompanyGroupsFromTerminal();

			RememberAllDataUI = new RememberAllDataPanel(this, ParentControl, 0, 5);

			ConnectUI = new ConnectPanel(this, ParentControl, 0, 6);

			if(ManageConnectionUI != null)
			{
				ManageConnectionUI.Dispose();
				ManageConnectionUI = null;
			};
		}
		internal void SetDataAcceptedAndConnetedUI()
		{
			if(ConnectUI != null)
			{
				ConnectUI.Dispose();
				ConnectUI = null;
			};
			ManageConnectionUI = new ManageConnectionPanel(this, ParentControl, 0, 6);
			SetConnetedUI();
		}
		internal void SetConnetedUI()
		{
			StateManager.State = UIStates.Connected;
			ShowConnectionStateUI.SetUIToConnected();

			GetLocalTerminalUserDataUI.SetUIToConnected();

			SelectCompanyGroupUI.SetUIToConnected();

			ManageConnectionUI.SetUIToConnected();
		}
		internal void SetStatelessStartUI()
		{
			SincronizadorGPS50.Sage50Connector.ConnectionActions.Disconnect();
			RemoveAllUIElements();
		}
		internal void RemoveAllUIElements()
		{

			if(ShowConnectionStateUI != null)
			{
				ShowConnectionStateUI.Dispose();
				ShowConnectionStateUI = null;
			};
			if(GetLocalTerminalUserDataUI != null)
			{
				GetLocalTerminalUserDataUI.Dispose();
				GetLocalTerminalUserDataUI = null;
			};
			if(ValidateTerminalUserDataUI != null)
			{
				ValidateTerminalUserDataUI.Dispose();
				ValidateTerminalUserDataUI = null;
			};
			if(SelectCompanyGroupUI != null)
			{
				SelectCompanyGroupUI.Dispose();
				SelectCompanyGroupUI = null;
			};
			if(RememberAllDataUI != null)
			{
				RememberAllDataUI.Dispose();
				RememberAllDataUI = null;
			};
			if(ConnectUI != null)
			{
				ConnectUI.Dispose();
				ConnectUI = null;
			};
			if(ManageConnectionUI != null)
			{
				ManageConnectionUI.Dispose();
				ManageConnectionUI = null;
			};
		}
	}
}
