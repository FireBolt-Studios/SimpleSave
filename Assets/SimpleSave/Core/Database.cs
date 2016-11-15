using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleSave.Core {
	public class Database : MonoBehaviour {

		// List to hold data
		public List<Data> DataList = new List<Data>();
		// Path, filename and file type settings
		public FileSettings FileSettings;
		// Load on run and save on quit settings
		public RuntimeOptions RuntimeOptions;

		void Awake ()
		{
			// Dont destroy on this gameobject on scene change
			DontDestroyOnLoad(this);
			// Check if load on run is true
			if (RuntimeOptions.LoadOnRun)
			{
				// Check if file type is binary
				if (FileSettings.Type == "BINARY")
				{
					// Load binary file from path
					DataList = Serialize.Load<List<Data>>(FileSettings.Root);
				}
				// Check if file type is xml
				if (FileSettings.Type == "XML")
				{
					// Load xml file from path
					DataList = Serialize.LoadXML<List<Data>>(FileSettings.Root);
				}
			}
		}

		// Run of application quit
		void OnApplicationQuit ()
		{
			// Check if save on quite is true
			if (RuntimeOptions.SaveOnQuit)
			{
				// Check if file type is binary
				if (FileSettings.Type == "BINARY")
				{
					// Define path variable from filename and path settings
					string path = FileSettings.Root+"/"+FileSettings.FileName+".bin";
					// Save datalist to path as binary
					Serialize.Save(path,DataList);
				}
				// Check if file type is xml
				if (FileSettings.Type == "XML")
				{
					// Define path variable from filename and path settings
					string path = FileSettings.Root+"/"+FileSettings.FileName+".xml";
					// Save datalist to path as xml
					Serialize.SaveXML(path,DataList);
				}
			}
		}

	}

	#region CUSTOM CLASSES
	[System.Serializable]
	public class RuntimeOptions {

		public bool LoadOnRun; 
		public bool SaveOnQuit;

	}

	[System.Serializable]
	public class FileSettings {

		public string FileName;
		public string Root;
		public string Type;

	}

	[System.Serializable]
	public class Data {

		public string Name;
		public List<Property> Properties = new List<Property>();

	}

	[System.Serializable]
	public class Property {

		public string Name;
		public string Value;

	}
	#endregion
}
