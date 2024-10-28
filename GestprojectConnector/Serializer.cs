using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dinq.Gestproject
{
   /// <summary>
   /// Clase para la Serialización/Desserialización de objetos
   /// </summary>
   public class Serializer
   {
      /// <summary>
      /// Serializa un objeto a un archivo
      /// </summary>
      /// <param name="objectInstance">Objeto a serializar</param>
      /// <param name="filePath">Archivo destino para la serialización</param>
      /// <returns>Verdadero si la serialización se realiza correctamente, en caso contrario Falso</returns>
      public static bool SerializeObject(Object objectInstance, String filePath)
      {
         FileStream fileStream = null;
         BinaryFormatter binaryFormater;
         bool returnValue;

         try
         {
            fileStream = File.Create(filePath);
            binaryFormater = new BinaryFormatter();
            binaryFormater.Serialize(fileStream, objectInstance);
            fileStream.Close();
            returnValue = true;
         }
         catch
         {
            returnValue = false;
         }
         finally
         {
            binaryFormater = null;
         }
         return returnValue;
      }

      /// <summary>
      /// Desserializa un objeto desde un archivo
      /// </summary>
      /// <param name="filePath">Archivo origen para la desserialización</param>
      /// <returns>Objeto desserializado</returns>
      public static Object DeserializeObject(String filePath)
      {
         FileStream fileStream = null;
         BinaryFormatter binaryFormatter = null;
         Object tempObject = null;

         try
         {
            if(File.Exists(filePath))
            {
               //INCIDENCIA:110 FECHA:29/04/2008
               //DESCRIPCION: File.Open solo con permisos de lectura
               //fileStream = File.Open(filePath, FileMode.Open);
               fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
               binaryFormatter = new BinaryFormatter();
               binaryFormatter.Binder = MySerializationBinder.MyCustomBinder;
               tempObject = binaryFormatter.Deserialize(fileStream);
               fileStream.Close();
            }
         }
         catch
         {
            tempObject = null;
         }
         finally
         {
            binaryFormatter = null;
         }
         return tempObject;
      }

   }
}
