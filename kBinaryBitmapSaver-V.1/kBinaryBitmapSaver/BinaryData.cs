/* BinaryData V.0.1 - 2010 - Paul Knab */

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace klock
{
    public static class BinaryData
    {
    #region SAVE

		public static void Save( Texture2D picture, string filename )
        {

            byte[] bytes = picture.EncodeToPNG();
            Serialize(filename, bytes);

        }
        private static void Serialize(string filename, object objectToSerialize)
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
		
		public static Texture2D Loader( string filename )
        {
            byte[] bytes = Deserialize<byte[]>(filename);
            Texture2D tex = new Texture2D(23, 23);
            tex.LoadImage(bytes);
            return tex;
        }
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

        public static string CutFile(string file)
        {
            string s;

            if ( file.Contains(Application.dataPath) )
            {
                string appPath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
                s = file.Substring(appPath.Length, file.Length - appPath.Length);
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
