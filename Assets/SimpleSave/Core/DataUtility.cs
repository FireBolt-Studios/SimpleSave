using UnityEngine;
using System.Collections;

namespace SimpleSave.Core {
	public class DataUtility : MonoBehaviour {

		// Create and return new data with given name
		public static Data CreateData (string name)
		{
			Data newData = new Data();
			newData.Name = name;
			return newData;
		}

		// Create and return new property with given name
		public static Property CreateProperty (string name)
		{
			Property newProperty = new Property();
			newProperty.Name = name;
			newProperty.Value = "Value";
			return newProperty;
		}

		// Search datalist for data with given name and returns
		public static Data GetData (string dataName)
		{
			foreach (Data data in GameObject.FindObjectOfType<Database>().DataList)
			{
				if (data.Name == dataName)
				{
					return data;
				}
			}
			return null;
		}

		// Search and return property
		public static object GetProperty (string dataName, string propertyName)
		{
			// Find data of given name
			Data data = GetData(dataName);

			// Loop through properties of returned data
			foreach (Property property in data.Properties)
			{
				// Check if element is property of given name
				if (propertyName == property.Name)
				{
					// Define int variable and check if property value is int and returns value
					int Int = 0;
					if (int.TryParse(property.Value,out Int))
					{
						return Int;
					}
					// Define float variable and check if property value is float and returns value
					float Float = 0;
					if (float.TryParse(property.Value,out Float))
					{
						return Float;
					}

					// If value is not int or float return as value as string
					return property.Name;
				}
			}
				
			Debug.LogWarning(data.Name + "does not contain property " + propertyName + ".");
			return null;
		}

		// Search and set value of property of given name within data of given name
		public static void SetProperty (string dataName, string propertyName,string value)
		{
			Data data = GetData(dataName);
			
			foreach (Property property in data.Properties)
			{
				if (propertyName == property.Name)
				{
					property.Value = value;
				}
			}

			Debug.LogWarning(data.Name + "does not contain property " + propertyName + ".");
		}
	}
}
