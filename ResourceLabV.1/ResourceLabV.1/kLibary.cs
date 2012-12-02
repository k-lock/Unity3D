using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace klock
{
    public class kLibary
    {
        /** Loads a binary data resources from a dll and coverted in a bitmap .
        *  @param string resourceName - The name of the data file in the dll.
        *  @param int width  - The width for the returning texture2D.
        *  @param int height - The height for the returning texture2D.
        *  @returns Texture2D - The resource texture.*/
        public static Texture2D LoadData( string resourceName, int width, int height )
        {
            // Check if a local resource is accessible.
            Texture2D texture = (Texture2D)Resources.Load(resourceName);
            if (texture != null)
            {
               // if a local resource exists return it.
                return texture;
            }

            // Load data resource from the dll.
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream   stream   = assembly.GetManifestResourceStream("klock." + resourceName);
           
            // Convert binary data to Texture2D.
            byte[] objectToSerialize;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                objectToSerialize = (byte[])bf.Deserialize(stream);
            }
            finally
            {
                if (stream != null) stream.Close();
            }

            // Create new texture. And load the resource data in it.
            texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.LoadImage(objectToSerialize);

            // If texture has no data. Anything goes wrong.
            if (texture == null)
            {
                Debug.LogError("Missing Dll resource: " + resourceName);
            }

            return texture;
        }

        /** Loads a bitmap resources from a dll.
         *  @param string resourceName - The name of the bitmap file in the dll.
         *  @param int width  - The width for the returning texture2D.
         *  @param int height - The height for the returning texture2D.
         *  @returns Texture2D - The resource texture.*/
        public static Texture2D LoadBitmap( string resourceName, int width, int height)
        {
            // Check if a local resource is accessible.
            Texture2D texture = (Texture2D)Resources.Load(resourceName);
            if (texture != null)
            {
                // if a local resource exists return it.
                return texture;
            }

            // Load data resource from the dll.
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("klock." + resourceName);
           
           /* string[] mrn = assembly.GetManifestResourceNames();
            foreach (string s in mrn)
            {
                Debug.Log(s);
            }*/
            
            // Create new texture. And load the resource data in it.
            texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.LoadImage( ReadToEnd(stream) );

            // If texture has no data. Anything goes wrong.
            if (texture == null)
            {
                Debug.LogError("Missing Dll resource: " + resourceName);
            }
            if (stream != null) stream.Close();

            return texture;
        }

        /** Read out a stream and returns a byte array.
         *  @param Stream stream - The stream to read.
         *  @returns byte[] - The readed stream data.*/
        private static byte[] ReadToEnd( Stream stream )
        {
            long originalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;

                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }

            finally
            {
                stream.Position = originalPosition;

            }
        }
    }
}