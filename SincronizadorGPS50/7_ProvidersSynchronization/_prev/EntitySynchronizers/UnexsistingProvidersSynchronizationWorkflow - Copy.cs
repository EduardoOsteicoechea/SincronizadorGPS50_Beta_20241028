//using Infragistics.Designers.SqlEditor;
//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Reflection;
//using System.Security.Cryptography.X509Certificates;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//	public class UnexsistingProvidersSynchronizationWorkflow : IUnexsistingEntitySynchronizationWorkflow<GestprojectProviderModel>
//	{
//		public List<GestprojectProviderModel> ExistingEntities { get; set; } = new List<GestprojectProviderModel>();
//		public List<GestprojectProviderModel> UnexistingEntities { get; set; } = new List<GestprojectProviderModel>();
//		public DialogResult SynchronizationDialogResult { get; set; }
//		public void Execute
//		(
//		   IGestprojectConnectionManager GestprojectConnectionManager,
//		   ISage50ConnectionManager Sage50ConnectionManager,
//		   List<GestprojectProviderModel> entityList,
//		   ISynchronizationTableSchemaProvider tableSchemaProvider
//		)
//		{
//			try
//			{
//				ValidateIfEntityExistsInSage50(entityList);
//				GenerateSynchronizationWorkflowDialog();
//				RequestExistingEntitiesLinking(
//					GestprojectConnectionManager,
//					Sage50ConnectionManager,
//					tableSchemaProvider,
//					SynchronizationDialogResult,
//					ExistingEntities
//				);
//				RequestUnexistingEntitiesCreation(
//					GestprojectConnectionManager,
//					Sage50ConnectionManager,
//					tableSchemaProvider,
//					SynchronizationDialogResult,
//					UnexistingEntities
//				);
//			}
//			catch(System.Exception exception)
//			{
//				throw ApplicationLogger.ReportError(
//				   MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//				   MethodBase.GetCurrentMethod().DeclaringType.Name,
//				   MethodBase.GetCurrentMethod().Name,
//				   exception
//				);
//			};
//		}

//		void ValidateIfEntityExistsInSage50
//		(
//		   List<GestprojectProviderModel> entities
//		)
//		{
//			for(global::System.Int32 i = 0; i < entities.Count; i++)
//			{
//				var entity = entities[i];

//				var entityComparer = new ProviderComparer(
//				  entity.NOMBRE_COMPLETO,
//				  entity.PAR_CIF_NIF
//			   );

//				if(entityComparer.Exists)
//				{
//					entity.S50_CODE = entityComparer.Sage50Code;
//					entity.PAR_SUBCTA_CONTABLE_2 = entityComparer.Sage50Code;
//					entity.S50_GUID_ID = entityComparer.Sage50Guid;
//					ExistingEntities.Add(entity);
//				}
//				else
//				{
//					UnexistingEntities.Add(entity);
//				};
//			};
//		}

//		void GenerateSynchronizationWorkflowDialog()
//		{
//			string dialogMessage = "";
//			string entityName = "proveedor(es)";
//			if(ExistingEntities.Count > 0 && UnexistingEntities.Count > 0)
//			{
//				dialogMessage = $"Partiendo de la selección encontramos {UnexistingEntities.Count} {entityName} desactualizados y {UnexistingEntities.Count} inexistentes en Sage50.\n\n¿Desea vincular los clientes existentes y crear los faltantes en Sage50?";
//			}
//			else if(ExistingEntities.Count > 0 && UnexistingEntities.Count == 0)
//			{
//				dialogMessage = $"Partiendo de la selección encontramos {UnexistingEntities.Count} {entityName} que ya existen en Sage50.\n\n¿Desea vincularlo(s)?";
//			}
//			else if(ExistingEntities.Count == 0 && UnexistingEntities.Count > 0)
//			{
//				dialogMessage = $"Partiendo de la selección encontramos {UnexistingEntities.Count} {entityName} inexistentes en Sage50.\n\n¿Desea crearlos y sincronizar sus datos?";
//			};

//			SynchronizationDialogResult = MessageBox.Show(dialogMessage, "Confirmación de actualización y creación", MessageBoxButtons.OKCancel);
//		}

//		void RequestExistingEntitiesLinking
//		(
//		   IGestprojectConnectionManager GestprojectConnectionManager,
//		   ISage50ConnectionManager Sage50ConnectionManager,
//		   ISynchronizationTableSchemaProvider tableSchemaProvider,
//		   DialogResult synchronizationDialogResult,
//		   List<GestprojectProviderModel> existingEntities
//		)
//		{
//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "existingEntities",
//         //   existingEntities
//         //);

//         if(synchronizationDialogResult == DialogResult.OK)
//			{
//				for(global::System.Int32 i = 0; i < existingEntities.Count; i++)
//				{
//					LinkExistingEntity(GestprojectConnectionManager, Sage50ConnectionManager, existingEntities[i], tableSchemaProvider);
//				};
//			};
//		}

//		void RequestUnexistingEntitiesCreation
//		(
//		   IGestprojectConnectionManager GestprojectConnectionManager,
//		   ISage50ConnectionManager Sage50ConnectionManager,
//		   ISynchronizationTableSchemaProvider tableSchemaProvider,
//		   DialogResult synchronizationDialogResult,
//		   List<GestprojectProviderModel> unexistingEntities
//		)
//		{
//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "unexistingEntities",
//         //   unexistingEntities
//         //);

//			//MessageBox.Show("unexistingEntities: " + unexistingEntities.Count);

//			if(synchronizationDialogResult == DialogResult.OK)
//			{
//				for(global::System.Int32 i = 0; i < unexistingEntities.Count; i++)
//				{
//					CreateUnexistingEntity(GestprojectConnectionManager, Sage50ConnectionManager, unexistingEntities[i], tableSchemaProvider);
//				};
//			};
//		}

//		void LinkExistingEntity
//		(
//		   IGestprojectConnectionManager GestprojectConnectionManager,
//		   ISage50ConnectionManager Sage50ConnectionManager,
//		   GestprojectProviderModel entity,
//		   ISynchronizationTableSchemaProvider tableSchemaProvider
//		)
//		{
//			new LinkProviderWorkflow().Execute(GestprojectConnectionManager, Sage50ConnectionManager, entity, tableSchemaProvider);
//		}

//		void CreateUnexistingEntity
//		(
//		   IGestprojectConnectionManager GestprojectConnectionManager,
//		   ISage50ConnectionManager Sage50ConnectionManager,
//		   GestprojectProviderModel entity,
//		   ISynchronizationTableSchemaProvider tableSchemaProvider
//		)
//		{
//			new CreateProviderWorkflow().Execute(GestprojectConnectionManager, Sage50ConnectionManager, entity, tableSchemaProvider);
//		}
//	}
//}
