using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

namespace SimpleSave.Editor {
	public class SimpleSaveHelp : EditorWindow {

		public GUISkin eSkin;

		public HelpFile Overview;
		public HelpFile Basic;
		public HelpFile Advanced;
		public HelpFile Reference;

		public HelpFile CurrentHelpFile;

		public Vector2 InfoScrollPosition = Vector2.zero;

		// Add new option to unity menu
		[MenuItem ("Simple Save/Help")]
		static void Init () 
		{
			// Create new or open exisiting window
			SimpleSaveHelp window = (SimpleSaveHelp)EditorWindow.GetWindow (typeof (SimpleSaveHelp),false,"Simple Save Help");
			window.Show();
		}

		void GetHelpFiles ()
		{
			string[] overviewFile = Directory.GetFiles(Application.dataPath, "*" + "Overview" + "*", SearchOption.AllDirectories);
			HelpFile newOverview = new HelpFile();
			newOverview.Contents = File.ReadAllText(overviewFile[0]);
			Overview = newOverview;

			string[] basicFile = Directory.GetFiles(Application.dataPath, "*" + "Basic" + "*", SearchOption.AllDirectories);
			HelpFile newBasic = new HelpFile();
			newBasic.Contents = File.ReadAllText(basicFile[0]);
			Basic = newBasic;

			string[] advancedFile = Directory.GetFiles(Application.dataPath, "*" + "Advanced" + "*", SearchOption.AllDirectories);
			HelpFile newAdvanced = new HelpFile();
			newAdvanced.Contents = File.ReadAllText(advancedFile[0]);
			Advanced = newAdvanced;

			string[] referenceFile = Directory.GetFiles(Application.dataPath, "*" + "Reference" + "*", SearchOption.AllDirectories);
			HelpFile newReference = new HelpFile();
			newReference.Contents = File.ReadAllText(referenceFile[0]);
			Reference = newReference;

		}

		void OnGUI ()
		{
			if (Overview == null)
			{
				GetHelpFiles();
			}

			minSize = new Vector2(700,300);

			// Set up GUI skin to be used
			eSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
			GUI.skin = eSkin;

			// Create menu title with skin changes (Bold)
			eSkin.box.fontStyle = FontStyle.Bold;
			GUI.Box(new Rect(5,5,120,20), "MENU");
			eSkin.box.fontStyle = FontStyle.Normal;

			GUI.Box(new Rect(5,25,120,position.height-30), "");

			if (GUI.Button(new Rect(5,25,120,20), "Overview"))
			{
				CurrentHelpFile = Overview;
			}

			if (GUI.Button(new Rect(5,45,120,20), "Basic (GUI)"))
			{
				CurrentHelpFile = Basic;
			}

			if (GUI.Button(new Rect(5,65,120,20), "Advanced (Script)"))
			{
				CurrentHelpFile = Advanced;
			}

			if (GUI.Button(new Rect(5,85,120,20), "Reference"))
			{
				CurrentHelpFile = Reference;
			}

			GUI.Box(new Rect(125,5,position.width-130,20), "INFORMATION");

			GUI.Box(new Rect(125,25,position.width-130,position.height-30), "");

			if (CurrentHelpFile != null)
			{
				InfoScrollPosition = GUI.BeginScrollView(new Rect(125,25,position.width-130,position.height-30),InfoScrollPosition,new Rect(125,25,position.width-145,1500));

				GUI.TextArea(new Rect(125,25,position.width-130,1500),CurrentHelpFile.Contents);
			
				GUI.EndScrollView();
			}
		}
	}


	[System.Serializable]
	public class HelpFile {

		public string[] Lines;
		public string Contents;

	}

}