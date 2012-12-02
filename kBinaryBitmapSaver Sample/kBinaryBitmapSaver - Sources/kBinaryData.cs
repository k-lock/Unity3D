/* kBinaryData V.0.1 - 2012 - Paul Knab */
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace klock
{
    public static class kBinaryData
    {
        /** 
         * Class to deserialize a byte array, from a file, to Texture2D or 
         * serialize a Texture2D to byte array and save it in a file. 
         */
     #region SAVE

        /* 
         * Save a Unity Texture2D to a given filename and path
         * @param picture is Textur2D
         * @param filename is String
         * @see Serialize( filename, objectToSerialize )
         */
        public static void Save( Texture2D picture, string filename )
        {

            byte[] bytes = picture.EncodeToPNG();
            Serialize(filename, bytes);
			

        }
         /* 
         * Serialize a Unity Texture2D to a byte array and write it into a file
         * @param filename is String
         * @param objectToSerialize is object
         */
        private static void Serialize( string filename, object objectToSerialize )
        {
            Stream stream = null;

            try
            {
                stream = File.Open(filename, FileMode.Create, FileAccess.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, objectToSerialize);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

    #endregion
    #region LOAD 
        /* 
         * Load a data file and deserialized it to a Unity Textur2D
         * @param filename is String
         * @param objectToSerialize is object
         * @see Deserialize( filename )
         * @return The converted Bitmap Texture.
         */
        public static Texture2D Loader( string filename )
        {
            byte[] bytes = Deserialize<byte[]>(filename);
            Texture2D tex = new Texture2D(23, 23);
            tex.LoadImage(bytes);

			
            return tex;
        }
        /* 
        * Open a byte array data file and deserialized it to a Unity Textur2D
        * @param filename is String
        * @return The converted Bitmap Texture.
        */
        private static T Deserialize<T>(string filename)
        {
            T objectToSerialize;
            Stream stream = null;
			try
            {	
                stream = File.Open(filename, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                objectToSerialize = (T)bf.Deserialize(stream);
		
            }
            finally
            {
                if(stream != null) stream.Close();
            }
            return objectToSerialize;
        }

    #endregion
    #region HELPERZ

        /* 
        * Cuts a complete environment path string conform to the Application.dataPath
        * C:/Space/UnityProjects/DemoProject/Assets/myFolder -> ../Assets/myFolder
        * @param filename is String
        * @return The path in your unity project ../Assets/myFolder .
        */
        public static string CutFile(string file)
        {
            string s;

            if ( file.Contains(Application.dataPath) )
            {
                string appPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
                s = "/" + file.Substring(appPath.Length, file.Length - appPath.Length);
            }
            else
            {
                s = file;
            }
            return s;
       
        }

        #endregion
    }
}