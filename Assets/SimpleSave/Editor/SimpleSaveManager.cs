using SimpleSave.Core;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace SimpleSave.Editor {
	public class SimpleSaveManager: EditorWindow { 

		// Where data is stored
		public Database database;

		// Current Index of data and property
		public int DataIndex;
		public int PropertyIndex;

		// Current position on scroll windows
		public Vector2 DataScroll = Vector2.zero;
		public Vector2 PropertyScroll = Vector2.zero;

		// Edit data and property swtiches
		public bool DataEdit;
		public bool PropertyEdit;

		// Popup lists (Dropdowns)
		public string[] typeList;
		public int typeIndex;
		public string[] loadOnRun;
		public int loadOnRunIndex;
		public string[] saveOnQuit;
		public int saveOnQuitIndex;

		// Add new option to unity menu
		[MenuItem ("Simple Save/Manager")]
		static void Init () 
		{
			// Create new or open exisiting window
			SimpleSaveManager window = (SimpleSaveManager)EditorWindow.GetWindow (typeof (SimpleSaveManager),false,"Simple Save");
			window.Show();
		}

		// Redraws the window when inspector is updated
		void OnInspectorUpdate() 
		{
			Repaint();
		}

		// Display window contents
		void OnGUI ()
		{
			#region INIT
			if (database == null)
			{
				// Checks if the database exists in the scene
				if (!GameObject.FindObjectOfType<Database>())
				{
					// If database doesn't exist create a new databse gameobject
					new GameObject("Database",typeof(Database));
				}

				// Assign database to a variable and setup switch lists
				database = GameObject.FindObjectOfType<Database>();
				Debug.Log("Simple Save: Found database");
				typeList = new string[2] {"BINARY","XML"};
				loadOnRun = new string[2] {"YES","NO"};
				saveOnQuit = new string[2] {"YES","NO"};

				// Set indexes to 0
				DataIndex = 0;
				PropertyIndex = 0;
			}

			// Checks if there is data contained within the list on the database
			if (database.DataList == null)
			{
				// Set indexes to 0
				DataIndex = 0;
				PropertyIndex = 0;
			}

			// Set up GUI skin to be used
			GUISkin eSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
			GUI.skin = eSkin;

			eSkin.label.alignment = TextAnchor.MiddleLeft;
			eSkin.box.alignment = TextAnchor.MiddleLeft;
			eSkin.box.clipping = TextClipping.Overflow;
			eSkin.box.wordWrap = false;

			// Set window minimum size
			minSize = new Vector2(515,160);
			#endregion

			#region DATA
			// Data list title with skin changes (Bold)
			eSkin.box.fontStyle = FontStyle.Bold;
			GUI.Box(new Rect(5,5,150,20), "DATA");
			eSkin.box.fontStyle = FontStyle.Normal;

			// Remove data button
			if (GUI.Button(new Rect(95,5,20,20),"-"))
			{
				// Check to see if data list contains data
				if (database.DataList.Count > 0)
				{
					// Defines undo method (sets database to dirty)
					Undo.RecordObject(database,"Simple Save: Remove data.");
					// Remove selected data remove data list
					database.DataList.RemoveAt(DataIndex);
					// Checks if data index is greater than 0 
					if (DataIndex > 0)
					{
						// Decrease the data index and set property index
						DataIndex -= 1;
						PropertyIndex = 0;
					}
				}
			}

			// Add data button
			if (GUI.Button(new Rect(115,5,20,20),"+"))
			{
				// Defines undo method (sets database to dirty)
				Undo.RecordObject(database,"Simple Save: Add data.");
				// Create new data, add to the data list and set the data index
				Data newData = DataUtility.CreateData("New Data");
				database.DataList.Add(newData);
				DataIndex = database.DataList.IndexOf(newData);
				DataEdit = true;
			}

			// Edit data button
			if (GUI.Button(new Rect(135,5,20,20),""))
			{
				// Enable data edit switch
				DataEdit = true;
			}
			// Display edit unicode icon with skin changes (Anchor,Font Size)
			eSkin.label.alignment = TextAnchor.MiddleCenter;
			eSkin.label.fontSize = 16;
			GUI.Label(new Rect(135,5,20,20),"✏");
			eSkin.label.alignment = TextAnchor.MiddleLeft;
			eSkin.label.fontSize = 0;

			// Data list display border
			GUI.Box(new Rect(5,25,150,position.height - 30), "");

			// Begins the data list scroll box
			DataScroll = GUI.BeginScrollView(new Rect(6,26,148,position.height - 32),DataScroll,new Rect(6,26,123,1000));

			// Define offset variable for button display
			float yOffest = 0;

			// Check if data list is not empty and loop through data in data list
			if (database.DataList != null)
			{
				foreach (Data data in database.DataList)
				{
					// Check if the data index is same as the index of the current element
					if (DataIndex == database.DataList.IndexOf(data))
					{
						// Check if data edit switch is true
						if (DataEdit)
						{
							// Create button to confirm changes with skin changes (Anchor,Font Size)
							if (GUI.Button(new Rect(120,25+yOffest,20,15),""))
							{
								DataEdit = false;
							}

							// Create text field to set data name
							data.Name = EditorGUI.TextField(new Rect(5,25+yOffest,115,15), data.Name);

							eSkin.label.alignment = TextAnchor.MiddleCenter;
							eSkin.label.fontSize = 16;
							GUI.Label(new Rect(117.5f,25+yOffest,25,15),"✓");
							eSkin.label.alignment = TextAnchor.MiddleLeft;
							eSkin.label.fontSize = 0;
						}
						else
						{
							// Create button for data element
							if (GUI.Button(new Rect(5,25+yOffest,135,15),""))
							{
								// Set data index to index of current element
								DataIndex = database.DataList.IndexOf(data);
							}
							// Set the text to show that this button is selected with skin changes (Anchor)
							GUI.Label(new Rect(5,25+yOffest,135,15),">"+data.Name);
							eSkin.label.alignment = TextAnchor.MiddleRight;
							GUI.Label(new Rect(5,25+yOffest,135,15),data.Properties.Count.ToString());
							eSkin.label.alignment = TextAnchor.MiddleLeft;
						}
					}
					else
					{
						// Create button for data element
						if (GUI.Button(new Rect(5,25+yOffest,135,15),""))
						{
							// Set data index to index of current element and set property index
							DataIndex = database.DataList.IndexOf(data);
							PropertyIndex = 0;
						}
						// Set the text to show that this button is not selected with skin changes (Anchor)
						GUI.Label(new Rect(5,25+yOffest,135,15),data.Name);
						eSkin.label.alignment = TextAnchor.MiddleRight;
						GUI.Label(new Rect(5,25+yOffest,135,15),data.Properties.Count.ToString());
						eSkin.label.alignment = TextAnchor.MiddleLeft;
					}

					// Increase the button offset 
					yOffest += 15;
				}
			}

			GUI.EndScrollView();
			#endregion

			#region PROPERTIES
			// Properties list relative to data at data index

			// Property list title with skin changes (Bold)
			eSkin.box.fontStyle = FontStyle.Bold;
			GUI.Box(new Rect(155,5,150,20), "PROPERTIES");
			eSkin.box.fontStyle = FontStyle.Normal;

			// Remove property at property index
			if (GUI.Button(new Rect(245,5,20,20),"-"))
			{
				// Check data list is not empty
				if (database.DataList.Count > 0)
				{
					// Check property list is not empty
					if (database.DataList[DataIndex].Properties.Count > 0)
					{
						// Defines undo method (sets database to dirty)
						Undo.RecordObject(database,"Simple Save: Remove property.");
						// Remove property at property index
						database.DataList[DataIndex].Properties.RemoveAt(PropertyIndex);
						// Check property index is greater than 0
						if (PropertyIndex > 0)
						{
							// Descrease property index
							PropertyIndex -= 1;
						}
					}
				}
			}

			// Add property to data's properties
			if (GUI.Button(new Rect(265,5,20,20),"+"))
			{
				// Check if data list is not empty
				if (database.DataList.Count > 0)
				{
					// Defines undo method (sets database to dirty)
					Undo.RecordObject(database,"Simple Save: Add property.");
					// Create new property, add to properties list and set property index
					Property newProperty = DataUtility.CreateProperty("New Property");
					database.DataList[DataIndex].Properties.Add(newProperty);
					PropertyIndex = database.DataList[DataIndex].Properties.IndexOf(newProperty);
					PropertyEdit = true;
				}
			}

			// Display edit unicode icon with skin changes (Anchor,Font Size)
			if (GUI.Button(new Rect(285,5,20,20),""))
			{
				PropertyEdit = true;
			}
			eSkin.label.alignment = TextAnchor.MiddleCenter;
			eSkin.label.fontSize = 16;
			GUI.Label(new Rect(285,5,20,20),"✏");
			eSkin.label.alignment = TextAnchor.MiddleLeft;
			eSkin.label.fontSize = 0;

			// Property list border
			GUI.Box(new Rect(155,25,150,position.height - 30), "");

			// Begin the property scroll box
			PropertyScroll = GUI.BeginScrollView(new Rect(156,26,148,position.height - 32),PropertyScroll,new Rect(161,26,123,1000));

			// Check if the data list is not empty
			if (database)
			{
				// Check if data list is not empty
				if (database.DataList.Count > 0)
				{
					// Define property offset variable
					float ypOffset = 0;

					// Loop through properties in property list
					foreach (Property property in database.DataList[DataIndex].Properties)
					{
						// Check if the property index is the same as index of element
						if (PropertyIndex == database.DataList[DataIndex].Properties.IndexOf(property))
						{
							// Check if property edit switch is true
							if (PropertyEdit)
							{
								// Create button to confirm changes with skin changes (Anchor,Font Size)
								if (GUI.Button(new Rect(275,25+ypOffset,20,15),""))
								{
									PropertyEdit = false;
								}

								// Create text fields to set propery name and value
								property.Name = EditorGUI.TextField(new Rect(160,25+ypOffset,75,15),property.Name);
								property.Value = EditorGUI.TextField(new Rect(235,25+ypOffset,40,15),property.Value);

								eSkin.label.alignment = TextAnchor.MiddleCenter;
								eSkin.label.fontSize = 16;
								GUI.Label(new Rect(275,25+ypOffset,20,15),"✓");
								eSkin.label.alignment = TextAnchor.MiddleLeft;
								eSkin.label.fontSize = 0;
							}
							else
							{
								// Create button for property element
								if (GUI.Button(new Rect(160,25+ypOffset,135,15),""))
								{
									PropertyIndex = database.DataList[DataIndex].Properties.IndexOf(property);
								}
								// Set the text to show that this property button is selected
								GUI.Label(new Rect(160,25+ypOffset,135,15),">"+property.Name + " : " + property.Value);
							}
						}
						else
						{
							// Create button for property element
							if (GUI.Button(new Rect(160,25+ypOffset,135,15),""))
							{
								PropertyIndex = database.DataList[DataIndex].Properties.IndexOf(property);
							}
							// Set the text to show that this property button is not selected
							GUI.Label(new Rect(160,25+ypOffset,135,15),property.Name + " : " + property.Value);
						}

						// Increase the property button offset
						ypOffset += 15;
					}
				}
			}

			GUI.EndScrollView();
			#endregion

			#region FILE SETTINGS
			// Create file settings title with skin changes (Bold)
			eSkin.box.fontStyle = FontStyle.Bold;
			GUI.Box(new Rect(305,5,position.width-310,20),"FILE SETTINGS");
			eSkin.box.fontStyle = FontStyle.Normal;

			// Create filename title and text field for filename input
			//GUI.Box(new Rect(305,25,40,15),"FILE");
			//database.FileSettings.FileName = EditorGUI.TextField(new Rect(345,25,position.width-350,15),database.FileSettings.FileName);

			// Create button to open folder panel
			if (GUI.Button(new Rect(position.width-30,25,25,15),"..."))
			{
				// Defines undo method (sets database to dirty)
				Undo.RecordObject(database,"Simple Save: Set root.");
				// Create a path variable and open folder select panel
				string path = EditorUtility.OpenFolderPanel("Select Folder",Application.persistentDataPath,"");
				// Assign path to the path variable in database file settings
				database.FileSettings.Root = path;
			}

			// Create path title and text field for path input
			GUI.Box(new Rect(305,25,40,15),"ROOT");
			database.FileSettings.Root = EditorGUI.TextField(new Rect(345,25,position.width-375,15),database.FileSettings.Root);

			// Create file type title and popup using the type list
			GUI.Box(new Rect(305,40,40,15),"TYPE");
			typeIndex = EditorGUI.Popup(new Rect(345,40,position.width-350,15),typeIndex,typeList);
			// Detects a change to popup
			if (GUI.changed)
			{
				// Sets the file type in the file settings to the current selection
				database.FileSettings.Type = typeList[typeIndex];
			}

			// Create save/load title with skin changes (Bold)
			eSkin.box.fontStyle = FontStyle.Bold;
			GUI.Box(new Rect(305,55,position.width-310,20),"SAVE/LOAD");
			eSkin.box.fontStyle = FontStyle.Normal;

			// Creates button to save all data
			if (GUI.Button(new Rect(305,75,position.width-310,15),"SAVE DATA"))
			{
				// Check if data list is not empty
				if (database.DataList.Count > 0)
				{
					// Checks to see if file type is binary
					if (database.FileSettings.Type == "BINARY")
					{
						string path = EditorUtility.SaveFilePanel("Save binary",database.FileSettings.Root,database.DataList[DataIndex].Name,"bin");
						Serialize.Save(path,database.DataList[DataIndex]);
					}
					// Checks to see if file type is xml
					if (database.FileSettings.Type == "XML")
					{
						string path = EditorUtility.SaveFilePanel("Save xml",database.FileSettings.Root,database.DataList[DataIndex].Name,"xml");
						Serialize.SaveXML(path,database.DataList[DataIndex]);
					}
				}
			}
			// Creates button to load all data
			if (GUI.Button(new Rect(305,90,position.width-310,15),"LOAD DATA"))
			{
				// Checks to see if file type is binary
				if (database.FileSettings.Type == "BINARY")
				{
					// Opens file panel
					string path = EditorUtility.OpenFilePanel("Load binary",database.FileSettings.Root,"bin");
					// Check if path is not empty
					if (path != "")
					{
						// Check if data list is not empty
						if (database.DataList.Count > 0)
						{
							// Loads file into data at data index
							database.DataList[DataIndex] = Serialize.Load<Data>(path);
						}
						else
						{
							// Create new data from loaded data
							Data loadedData = Serialize.Load<Data>(path);
							// Add the loaded data to data list
							database.DataList.Add(loadedData);
						}
					}
				}
				// Checks to see if file type is xml
				if (database.FileSettings.Type == "XML")
				{
					// Opens file panel
					string path = EditorUtility.OpenFilePanel("Load xml",database.FileSettings.Root,"xml");
					if (path != "")
					{
						// Check if data list is not empty
						if (database.DataList.Count > 0)
						{
							// Loads file into data at data index
							database.DataList[DataIndex] = Serialize.LoadXML<Data>(path);
						}
						else
						{
							// Create new data from loaded data
							Data loadedData = Serialize.LoadXML<Data>(path);
							// Add the loaded data to data list
							database.DataList.Add(loadedData);
						}
					}
				}
			}
			#endregion

			#region RUNTIME OPTIONS
			// Create runtime options tile with skin changes (Bold)
			eSkin.box.fontStyle = FontStyle.Bold;
			GUI.Box(new Rect(305,105,position.width-310,20), "RUNTIME OPTIONS");
			eSkin.box.fontStyle = FontStyle.Normal;

			// Create load on run title and popup using load on run list
			GUI.Box(new Rect(305,125,100,15), "LOAD ON RUN");
			loadOnRunIndex = EditorGUI.Popup(new Rect(405,125,position.width-410,15),loadOnRunIndex,loadOnRun);
			// Checks for 
			if (GUI.changed)
			{
				if (loadOnRun[loadOnRunIndex] == "YES")
				{
					database.RuntimeOptions.LoadOnRun = true;
				}
				else
				{
					database.RuntimeOptions.LoadOnRun = false;
				}
			}
			GUI.Box(new Rect(305,140,100,15), "SAVE ON QUIT");
			saveOnQuitIndex = EditorGUI.Popup(new Rect(405,140,position.width-410,15),saveOnQuitIndex,saveOnQuit);
			if (GUI.changed)
			{
				if (saveOnQuit[saveOnQuitIndex] == "YES")
				{
					database.RuntimeOptions.SaveOnQuit = true;
				}
				else
				{
					database.RuntimeOptions.SaveOnQuit = false;
				}
			}
			#endregion
		}
	}
}
