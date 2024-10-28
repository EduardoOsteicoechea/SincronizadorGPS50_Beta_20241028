using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class CreateGestprojectSynchronizationImageTable
    {
        internal CreateGestprojectSynchronizationImageTable() 
        {
            string sqlString = @"
            CREATE TABLE INT_SAGE_SINC_CLIENTE_IMAGEN
            (
                id INT PRIMARY KEY IDENTITY(1,1), 
                PAR_ID INT, 
                PAR_SUBCTA_CONTABLE VARCHAR(MAX), 
                PAR_NOMBRE VARCHAR(MAX), 
                PAR_NOMBRE_COMERCIAL VARCHAR(MAX), 
                PAR_CIF_NIF VARCHAR(MAX), 
                PAR_DIRECCION_1 VARCHAR(MAX), 
                PAR_CP_1 VARCHAR(MAX), 
                PAR_LOCALIDAD_1 VARCHAR(MAX), 
                PAR_PROVINCIA_1 VARCHAR(MAX), 
                PAR_PAIS_1 VARCHAR(MAX), 
                synchronization_status VARCHAR(MAX), 
                sage50_client_code VARCHAR(MAX), 
                sage50_guid_id VARCHAR(MAX), 
                sage50_instance VARCHAR(MAX),
                comments VARCHAR(MAX),
                last_record DATETIME DEFAULT GETDATE() NOT NULL
            )
            ;";

            using(SqlCommand SQLCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection))
            {
                SQLCommand.ExecuteNonQuery();
                using(
                    SqlCommand command = new SqlCommand(
                        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'INT_SAGE_SINC_CLIENTE';", GestprojectDataHolder.GestprojectDatabaseConnection
                    )
                )
                {
                    object tableRow = command.ExecuteScalar();
                    if(tableRow != null)
                    {
                        Console.WriteLine("La tabla \"INT_SAGE_SINC_CLIENTE_IMAGEN\" fue creada exitosamente");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo crear la tabla \"INT_SAGE_SINC_CLIENTE_IMAGEN\"");
                    };
                }
            };
        }
    }
}
