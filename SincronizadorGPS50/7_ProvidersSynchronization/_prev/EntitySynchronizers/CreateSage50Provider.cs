using sage.ew.db;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
	public class CreateSage50Provider
	{
		public string EntityCode { get; set; } = "";
		public string GUID_ID { get; set; } = "";
		public CreateSage50Provider
		(
		   string country,
		   string name,
		   string cif,
		   string postalCode,
		   string address,
		   string province,
		   string ivaType = "03"
		)
		{
			try
			{
				int nextCodeAvailable = new SincronizadorGPS50.Sage50Connector.GetSage50Providers().NextProviderCodeAvailable;

				ProviderBussinessClass entity = new ProviderBussinessClass();
				clsEntityProvider entityInstance = new clsEntityProvider();

				if(nextCodeAvailable < 10000)
				{
               if(nextCodeAvailable < 10)
               {
                  entityInstance.codigo = "4000000" + nextCodeAvailable;
                  EntityCode = entityInstance.codigo;
               }
               else if(nextCodeAvailable < 100)
               {
                  entityInstance.codigo = "400000" + nextCodeAvailable;
                  EntityCode = entityInstance.codigo;
               }
               else if(nextCodeAvailable < 1000)
               {
                  entityInstance.codigo = "40000" + nextCodeAvailable;
                  EntityCode = entityInstance.codigo;
               }
               else
               {
                  entityInstance.codigo = "4000" + nextCodeAvailable;
                  EntityCode = entityInstance.codigo;
               };

               entityInstance.pais = country.Trim();
               entityInstance.nombre = name.Trim();
               entityInstance.codpos = postalCode.Trim();
               entityInstance.cif = cif.Trim();
               entityInstance.direccion = address.Trim();
               entityInstance.provincia = province.Trim();
               entityInstance.tipo_iva = ivaType.Trim();

               if(entity._Create(entityInstance))
					{
						string getSage50EntitySQLQuery = $@"
						SELECT 
							guid_id 
						FROM 
							{DB.SQLDatabase("gestion","proveed")}
						WHERE 
							codigo='{EntityCode}'
						;";

						DataTable sage50EntityDataTable = new DataTable();

						DB.SQLExec(getSage50EntitySQLQuery, ref sage50EntityDataTable);

						GUID_ID = sage50EntityDataTable.Rows[0].ItemArray[0].ToString().Trim();
					}
					else
					{
						MessageBox.Show(
							"Error en la creación del cliente empleando estos datos: " + "\n\n" +
							"EntityCode: " + entityInstance.codigo + "\n" +
							"country: " + entityInstance.pais + "\n" +
							"name: " + entityInstance.nombre + "\n" +
							"postalCode: " + entityInstance.codpos + "\n" +
							"cif: " + entityInstance.cif + "\n" +
							"address: " + entityInstance.direccion + "\n" +
							"province: " + entityInstance.provincia + "\n" +
							"ivaType: " + entityInstance.tipo_iva + "\n"
						);
					};
				}
				else
				{
					MessageBox.Show("Sage50 admite un máximo de 9999 proveedores por grupo de empresas y su base de proveedores de Gestproject supera éste límite.");
				};
			}
			catch(System.Exception exception)
			{
				throw ApplicationLogger.ReportError(
				   MethodBase.GetCurrentMethod().DeclaringType.Namespace,
				   MethodBase.GetCurrentMethod().DeclaringType.Name,
				   MethodBase.GetCurrentMethod().Name,
				   exception
				);
			};
		}
	}
}