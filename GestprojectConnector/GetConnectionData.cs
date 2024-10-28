using System.Xml;

namespace SincronizadorGPS50.GestprojectConnector
{
    internal class GetConnectionData
    {
        internal bool IsSuccessfull { get; set; } = false;
        public GetConnectionData()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(ConnectionDataHolder.GestprojectXMLConfigurationFilePath);

                string xmlContent = xmlDocument.OuterXml;

                ConnectionDataHolder.Server = xmlDocument.SelectSingleNode("/configuration/conexion/Servidor").InnerText;
                ConnectionDataHolder.DatabaseInstance = xmlDocument.SelectSingleNode("/configuration/conexion/Instancia").InnerText;
                ConnectionDataHolder.DatabaseName = xmlDocument.SelectSingleNode("/configuration/conexion/NombreBD").InnerText;
                ConnectionDataHolder.DatabaseUser = xmlDocument.SelectSingleNode("/configuration/conexion/Usuario").InnerText;
                ConnectionDataHolder.RecordPasswordFromXML(xmlDocument.SelectSingleNode("/configuration/conexion/Password").InnerText);
                ConnectionDataHolder.AskForServer = xmlDocument.SelectSingleNode("/configuration/conexion/AskServerAtStartup").InnerText;
                ConnectionDataHolder.LastServer = xmlDocument.SelectSingleNode("/configuration/conexion/LastServer").InnerText;

                IsSuccessfull = true;
            }
            catch(System.Exception exception)
            {
                throw new System.Exception($"Error: \n\n{exception.ToString()}. \n\nProcederemos a detener la aplicación. Contacte a nuestro servicio de atención al cliente para reportar el error y recibir servicio técnico al respecto.");
            };
        }
    }
}

