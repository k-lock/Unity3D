/* klock.kio.kIO V.1.1 - 06|2012 - Paul Knab */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace klock.io
{
    /** Class to handle with System.IO methods.*/
    public class kIO
    {
        /** Return the filename without extensions
         * @param string file - The path to the file. */
        public static string FileName( string file )
        {
            return Path.GetFileNameWithoutExtension( file );
        }

        /**
         * Create a Directory on given location.
         * @param string path - The location for creation. */
        public static void CreateDirectory( string path )
        {
            if (!Directory.Exists( path ))
            {
                Directory.CreateDirectory( path );
                AssetDatabase.Refresh();
            }
        }

        public static bool FileExists( string file )
        {
            return File.Exists( file );
        }
    }
}
