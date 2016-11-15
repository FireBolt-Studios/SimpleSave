using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleSave.Core {
	public class Serialize : MonoBehaviour {

		// Load binary file at path filename and return data
		public static T Load<T>(string filename) where T : class
		{
			// Check if file exists
			if (File.Exists(filename))
			{
				// Open file
				using (Stream stream = File.OpenRead(filename))
				{
					// Define new binary formatter
					BinaryFormatter formatter = new BinaryFormatter();
					// Deserialize the file and return data
					return formatter.Deserialize(stream) as T;
				}
			}
			return null;
		}

		// Save binary file to path
		public static void Save<T>(string filename, T data) where T : class
		{
			// Open or create file
			using (Stream stream = File.Open(filename, FileMode.OpenOrCreate))
			{
				// Define new binary formatter
				BinaryFormatter formatter = new BinaryFormatter();
				// Serialize the data to the file
				formatter.Serialize(stream, data);
			}
		}

		// Load xml file at path filename and return data
		public static T LoadXML<T>(string filename) where T : class
		{
			// Check if file exists
			if (File.Exists(filename))
			{
				// Open file
				using (Stream stream = File.OpenRead(filename))
				{
					// Define new xml serializer
					XmlSerializer serial = new XmlSerializer(typeof(T));
					// Deserialize the file and return data
					return serial.Deserialize(stream) as T;
				}
			}

			return null;
		}

		// Save xml file to path
		public static void SaveXML<T>(string filename, T data) where T : class
		{
			// Open or create file
			using (Stream stream = File.Open(filename, FileMode.OpenOrCreate))
			{
				// Define new xml serializer
				XmlSerializer serial = new XmlSerializer(typeof (T));
				// Serialize the data to the file
				serial.Serialize(stream, data);
			}
		}

	}
}
