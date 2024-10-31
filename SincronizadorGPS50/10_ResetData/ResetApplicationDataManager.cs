using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinTabControl;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class ResetApplicationDataManager
   {
      public string ReceivedInvoicesDetailsSynchronizationTableName { get; set; } = "INT_SAGE_SINC_FACTURA_RECIBIDA";
      public string ReceivedInvoicesSynchronizationTableName { get; set; } = "INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES";
      public string IssuedInvoicesSynchronizationTableName { get; set; } = "INT_SAGE_SINC_FACTURA_EMITIDA_DETALLES";
      public string IssuedInvoicesDetailsSynchronizationTableName { get; set; } = "INT_SAGE_SINC_FACTURA_EMITIDA";
      public string SuppliersSynchronizationTableName { get; set; } = "INT_SAGE_SINC_PROVEEDORES";
      public string TaxesSynchronizationTableName { get; set; } = "INT_SAGE_SINC_IMPUESTOS";
      public string AccountsSynchronizationTableName { get; set; } = "INT_SAGE_SINC_CUENTAS_CONTABLES";
      public string ProjectsSynchronizationTableName { get; set; } = "INT_SAGE_SINC_PROYECTOS";
      public string CustomersSynchronizationTableName { get; set; } = "INT_SAGE_SINC_CLIENTES";
      public string CompaniesSynchronizationTableName { get; set; } = "INT_SAGE_SINC_EMPRESAS";
      public string[] SynchronizationTablesNames { get; set;} = {
         "INT_SAGE_SINC_FACTURA_RECIBIDA",
         "INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES",
         "INT_SAGE_SINC_FACTURA_EMITIDA_DETALLES",
         "INT_SAGE_SINC_FACTURA_EMITIDA",
         "INT_SAGE_SINC_PROVEEDORES",
         "INT_SAGE_SINC_IMPUESTOS",
         "INT_SAGE_SINC_CUENTAS_CONTABLES",
         "INT_SAGE_SINC_PROYECTOS",
         "INT_SAGE_SINC_CLIENTES",
         "INT_SAGE_SINC_EMPRESAS"
      };
      public SqlConnection Connection { get; set; }
      public UltraTab Tab { get; set; }
      public ResetApplicationDataManager
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         UltraTab hostTab
      )
      {
         try
         {
            Connection = gestprojectConnectionManager.GestprojectSqlConnection;
            Tab = hostTab;
            Tab.Enabled = true;
            GenerateUI();
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }

      public void DeleteDatabases()
      {
         try
         {
            Connection.Open();

            foreach(string tableName in SynchronizationTablesNames)
            {
               string sqlString = $@"
                  drop table {tableName};
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  command.ExecuteNonQuery();
               }
            }
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         }
      }

      public void GenerateUI()
      {
         try
         {
            UltraPanel panel = new UltraPanel();
            panel.Dock = DockStyle.Fill;

            UltraButton ExitButton = new UltraButton();
            ExitButton.Text = "Reestablecer Datos";
            ExitButton.Width = 175;
            ExitButton.Height = 45;
            
            ExitButton.Click += ResetApplicationButton_Click;
            ExitButton.Dock = DockStyle.None;
            ExitButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

            panel.ClientArea.Controls.Add(ExitButton);

            Tab.TabPage.Controls.Add(panel);
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }

      public void ResetApplicationButton_Click(object sender, EventArgs e)
      {
         try
         {
            DialogResult dialogResult = MessageBox.Show("Esta acción eliminará y recreará las bases de datos del sincronizador leyendo los datos actualizados de ambos programas\n\n¿Desea reestrablecer los datos de la aplicación?", "Confirmación", MessageBoxButtons.OKCancel);

            if ( dialogResult == DialogResult.OK ) 
            {
               DeleteDatabases();
               MessageBox.Show($"Eliminamos exitosamente las siguientes bases de datos:\n\n{string.Join("\n", SynchronizationTablesNames)}");
               Application.Exit();
               //Process.Start(Application.ExecutablePath);
            }
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }
   }
}
