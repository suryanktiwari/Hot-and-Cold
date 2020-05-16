using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
public class MenuItems : MonoBehaviour {
	[MenuItem("Tools/Clear PlayerPrefs")]
	private static void NewMenuOption(){
		PlayerPrefs.DeleteAll ();
		Debug.Log ("Reseting");
	}

    

}
