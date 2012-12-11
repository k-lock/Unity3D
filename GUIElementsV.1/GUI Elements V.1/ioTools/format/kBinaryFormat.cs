/* kBinaryFormat V.1.1 - 06|2012 - Paul Knab */
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

namespace klock.io
{
    /** Class to read, write and convert binary byte arrays. */
    public class kBinaryFormat
    {
       /** Converts an object in a binary byte array, and saves the result in a file.
         * @param string filename - The location to save.
         * @parma object objectToSerialize - The object to save.*/
        public static void Serialize( string filename, object objectToSerialize )
        {
            Stream stream = null;

            try
            {
                stream = File.Open( filename, FileMode.Create );
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, objectToSerialize);
            }

            finally
            {
                if ( stream != null ) stream.Close();
            }
        }

        /** Open a binary byte array file. Converted to an object and return the result.
         * @param string filename - The file to load.
         * @returns object objectToSerialize - The converted file.*/
        public static T Deserialize<T>( string filename )
        {
            T objectToSerialize;
            Stream stream = null;

            try
            {
                stream = File.Open( filename, FileMode.Open );
                BinaryFormatter bf = new BinaryFormatter();
                objectToSerialize = (T)bf.Deserialize( stream );
            }

            finally
            {
                if ( stream != null ) stream.Close();
            }

            return objectToSerialize;
        }
    }
}
