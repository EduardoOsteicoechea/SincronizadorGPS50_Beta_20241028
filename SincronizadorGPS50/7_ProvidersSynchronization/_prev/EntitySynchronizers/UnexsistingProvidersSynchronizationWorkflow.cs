using Infragistics.Designers.SqlEditor;
using sage.ew.db;
using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
	public class UnexsistingProvidersSynchronizationWorkflow : IUnexsistingEntitySynchronizationWorkflow<GestprojectProviderModel>
	{
		public List<GestprojectProviderModel> ExistingEntities { get; set; } = new List<GestprojectProviderModel>();
		public List<GestprojectProviderModel> UnexistingEntities { get; set; } = new List<GestprojectProviderModel>();
		public DialogResult SynchronizationDialogResult { get; set; } = DialogResult.Cancel;      
      private string _EntityTypeNameRoot {get;set;} = "";
      private string _EntityTypeNameGender {get;set;} = "";
      private string _EntityTypeNamePlural {get;set;} = "";
      private string _EntityTypeVerbsPlural {get;set;} = "";
      private string _GestprojectDatabaseEntityTableName {get;set;} = "";
      private IGestprojectConnectionManager _GestprojectConnectionManager {get;set;} = null;
      private CompanyGroup _Sage50CompanyGroupData {get;set;} = null;
      private ISynchronizationTableSchemaProvider _TableSchema {get;set;} = null;
      private List<GestprojectProviderModel> _SynchronizationTableEntities {get;set;} = null;
      private List<GestprojectProviderModel> _DesynchronizedEntities {get;set;} = null;
      private List<Sage50ProviderModel> _Sage50Entities {get;set;} = null;
      private DialogResult _DialogResult {get;set;} = DialogResult.Cancel;
      private List<GestprojectProviderModel> _EntitiesToBeCreated {get;set;} = null;

      private List<GestprojectProviderModel> _GestprojectEntityTableEntities = null;
      private List<int?>   _GestprojectEntityTableIds = null;
      private List<string> _GestprojectEntityTableNames = null;
      private List<string> _GestprojectEntityTableCifs = null;
      private List<string> _GestprojectEntityTableAccountableAccounts = null;


		public void Execute
		(
		   IGestprojectConnectionManager gestprojectConnectionManager,
		   ISage50ConnectionManager sage50ConnectionManager,
		   List<GestprojectProviderModel> entityList,
		   ISynchronizationTableSchemaProvider tableSchema
		)
		{
			try
			{
            this._GestprojectConnectionManager = gestprojectConnectionManager;
            this._Sage50CompanyGroupData = sage50ConnectionManager.CompanyGroupData;
            this._TableSchema = tableSchema;

            this._SynchronizationTableEntities = entityList;

            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
            //   "this._SynchronizationTableEntities",
            //   this._SynchronizationTableEntities
            //);

            this._EntitiesToBeCreated = new List<GestprojectProviderModel>();
            this._GestprojectEntityTableEntities = new List<GestprojectProviderModel>();
            this._GestprojectDatabaseEntityTableName = "PARTICIPANTE";

            this._DetermineEntityGenderAndPluralFormat();
            this._DisplaySynchronizationAutorizationDialog();
            if(this._DialogResult == DialogResult.OK)
            {
               this._GetGestprojectEntityTableEntityData();
               this._PopulateEntitiesToBeCreatedAndUpdatedLists();
               this._CreateEntities();
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

      public void _DetermineEntityGenderAndPluralFormat()
      {
         try
         {
            this._EntityTypeNameRoot = "proveed";
            this._EntityTypeNameGender = "or";
            this._EntityTypeNamePlural = "(es)";
            this._EntityTypeVerbsPlural = "os";
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

      public void _DisplaySynchronizationAutorizationDialog()
      {
         try
         {
            if(this._SynchronizationTableEntities.Count > 0)
            {
               this._DialogResult = MessageBox.Show(
               $"Partiendo de la selección encontramos {this._SynchronizationTableEntities.Count} {this._EntityTypeNameRoot + this._EntityTypeNameGender + this. _EntityTypeNamePlural} desincronizad{this._EntityTypeVerbsPlural}.\n\n¿Desea sincronizarl{this._EntityTypeVerbsPlural}?", "Confirmación de actualización", MessageBoxButtons.OKCancel
               );
            }
            else
            {
               MessageBox.Show($"Los {this._SynchronizationTableEntities.Count - (this._SynchronizationTableEntities.Count - _Sage50Entities.Count)} {this._EntityTypeNameRoot + this._EntityTypeNameGender + this. _EntityTypeNamePlural} están sincronizad{this._EntityTypeVerbsPlural}.");
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

      public void _GetGestprojectEntityTableEntityData()
      {
         SqlConnection connection = this._GestprojectConnectionManager.GestprojectSqlConnection;
         try
         {                     
            this._GestprojectEntityTableEntities = new GestprojectProvidersManager().GetAll(this._GestprojectConnectionManager.GestprojectSqlConnection);
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
            connection.Close();
         };      
      }

      public void _PopulateEntitiesToBeCreatedAndUpdatedLists()
      { 
         try
         {
            foreach (GestprojectProviderModel synchronizationTableGestprojectEntity in this._SynchronizationTableEntities)
            {
               bool syncronizationTableEntityExistInGestproject = this._GestprojectEntityTableEntities.FirstOrDefault(
                  gestprojectEntityTableEntity 
                     =>
                  gestprojectEntityTableEntity.PAR_ID == synchronizationTableGestprojectEntity.PAR_ID
                     &&
                  gestprojectEntityTableEntity.PAR_NOMBRE == synchronizationTableGestprojectEntity.PAR_NOMBRE
                     &&
                  gestprojectEntityTableEntity.PAR_CIF_NIF == synchronizationTableGestprojectEntity.PAR_CIF_NIF
                     &&
                  gestprojectEntityTableEntity.PAR_SUBCTA_CONTABLE_2 == synchronizationTableGestprojectEntity.PAR_SUBCTA_CONTABLE_2
               ) != null;

               if(syncronizationTableEntityExistInGestproject == false)
                  this._EntitiesToBeCreated.Add( synchronizationTableGestprojectEntity );
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

      public void _CreateEntities()
      {
         try
         {
            if(this._EntitiesToBeCreated.Count > 0)
            {
               foreach (GestprojectProviderModel entityToBeCreated in this._EntitiesToBeCreated)
               {
                  GestprojectProviderModel entity = entityToBeCreated;
                  this._InsertSynchronizationTableEntityIntoGestprojectEntityTable(ref entity);
                  this._AppendGestprojectTaxTableIdToSynchronizationTable(ref entity);
                  this._AppendSynchronizationDataToEntityRegistry(ref entity);
               };
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

      private void _InsertSynchronizationTableEntityIntoGestprojectEntityTable(ref GestprojectProviderModel entity)
      {
         SqlConnection connection = this._GestprojectConnectionManager.GestprojectSqlConnection;
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               MAX(PAR_ID)
            FROM
               {this._GestprojectDatabaseEntityTableName}
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     int maxIdValue = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? 0 : reader.GetValue(0));
                     entity.PAR_ID = ++maxIdValue;
                  };
               };
            };

            string sqlString2 = $@"
            INSERT INTO 
               {this._GestprojectDatabaseEntityTableName} 
               (
                  PAR_ID
                  ,PAR_SUBCTA_CONTABLE_2
                  ,PAR_NOMBRE
                  ,PAR_APELLIDO_1
                  ,PAR_APELLIDO_2
                  ,PAR_NOMBRE_COMERCIAL
                  ,PAR_CIF_NIF
                  ,PAR_DIRECCION_1
                  ,PAR_CP_1
                  ,PAR_LOCALIDAD_1
                  ,PAR_PROVINCIA_1
                  ,PAR_PAIS_1
               )
            VALUES
               (
                  @PAR_ID
                  ,@PAR_SUBCTA_CONTABLE_2
                  ,@PAR_NOMBRE
                  ,@PAR_APELLIDO_1
                  ,@PAR_APELLIDO_2 
                  ,@PAR_NOMBRE_COMERCIAL
                  ,@PAR_CIF_NIF
                  ,@PAR_DIRECCION_1
                  ,@PAR_CP_1
                  ,@PAR_LOCALIDAD_1
                  ,@PAR_PROVINCIA_1
                  ,@PAR_PAIS_1
               )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString2, connection))
            {
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE_2", entity.PAR_SUBCTA_CONTABLE_2);
               command.Parameters.AddWithValue("@PAR_NOMBRE", entity.PAR_NOMBRE);
               command.Parameters.AddWithValue("@PAR_APELLIDO_1", entity.PAR_APELLIDO_1);
               command.Parameters.AddWithValue("@PAR_APELLIDO_2", entity.PAR_APELLIDO_2);
               command.Parameters.AddWithValue("@PAR_NOMBRE_COMERCIAL", entity.PAR_NOMBRE_COMERCIAL);
               command.Parameters.AddWithValue("@PAR_CIF_NIF", entity.PAR_CIF_NIF);
               command.Parameters.AddWithValue("@PAR_DIRECCION_1", entity.PAR_DIRECCION_1);
               command.Parameters.AddWithValue("@PAR_CP_1", entity.PAR_CP_1);
               command.Parameters.AddWithValue("@PAR_LOCALIDAD_1", entity.PAR_LOCALIDAD_1);
               command.Parameters.AddWithValue("@PAR_PROVINCIA_1", entity.PAR_PROVINCIA_1);
               command.Parameters.AddWithValue("@PAR_PAIS_1", entity.PAR_PAIS_1);

               command.ExecuteNonQuery();
            };

            // Indicate to Gestproject that this entity added as a participant is a supplier

            string sqlString3 = $@"
            INSERT INTO 
               PAR_TPA 
            (
               TPA_ID
               ,PAR_ID
            )
            VALUES
            (
               @TPA_ID
               ,@PAR_ID
            )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString3, connection))
            {
               command.Parameters.AddWithValue("@TPA_ID", "12");
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);

               command.ExecuteNonQuery();
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
         }
         finally
         {
            connection.Close();
         };
      
      }

      private void _AppendGestprojectTaxTableIdToSynchronizationTable(ref GestprojectProviderModel entity)
      {      
         SqlConnection connection = this._GestprojectConnectionManager.GestprojectSqlConnection;
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               PAR_ID
            FROM
               {this._GestprojectDatabaseEntityTableName}
            WHERE
               PAR_SUBCTA_CONTABLE_2=@PAR_SUBCTA_CONTABLE_2
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE_2",entity.PAR_SUBCTA_CONTABLE_2);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.PAR_ID = Convert.ToInt32(reader.GetValue(0));
                     break;
                  };
               };
            };

            string sqlString2 = $@"
            UPDATE 
               {this._TableSchema.TableName} 
            SET
               PAR_ID=@PAR_ID
            WHERE
               S50_GUID_ID=@S50_GUID_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString2, connection))
            {
               command.Parameters.AddWithValue("@PAR_ID",entity.PAR_ID);
               command.Parameters.AddWithValue("@S50_GUID_ID",entity.S50_GUID_ID);

               command.ExecuteNonQuery();
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
         }
         finally
         {
            connection.Close();
         };
      }
      private void _AppendSynchronizationDataToEntityRegistry(ref GestprojectProviderModel entity)
      {
         SqlConnection connection = this._GestprojectConnectionManager.GestprojectSqlConnection;
         try
         {
            connection.Open();

            string sqlString = $@"
            UPDATE 
               {this._TableSchema.TableName} 
            SET
               S50_CODE=@S50_CODE
               ,S50_COMPANY_GROUP_NAME=@S50_COMPANY_GROUP_NAME
               ,S50_COMPANY_GROUP_CODE=@S50_COMPANY_GROUP_CODE
               ,S50_COMPANY_GROUP_MAIN_CODE=@S50_COMPANY_GROUP_MAIN_CODE
               ,S50_COMPANY_GROUP_GUID_ID=@S50_COMPANY_GROUP_GUID_ID
               ,GP_USU_ID=@GP_USU_ID
            WHERE
               S50_GUID_ID=@S50_GUID_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID",entity.S50_GUID_ID);
               command.Parameters.AddWithValue("@S50_CODE",entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME",this._Sage50CompanyGroupData.CompanyName);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE",this._Sage50CompanyGroupData.CompanyCode);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE",this._Sage50CompanyGroupData.CompanyMainCode);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID",this._Sage50CompanyGroupData.CompanyGuidId);
               command.Parameters.AddWithValue("@GP_USU_ID",this._GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID);

               command.ExecuteNonQuery();
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
         }
         finally
         {
            connection.Close();
         };
      }
	}
}
