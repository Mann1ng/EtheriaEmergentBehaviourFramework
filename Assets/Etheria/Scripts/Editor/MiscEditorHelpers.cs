using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class MiscEditorHelpers {



    static public string SaveScriptableObjectAtCurrentFolder<T>(T sObj, string fileNameAndExt) where T:ScriptableObject {

        return MiscEditorHelpers.SaveScriptableObjectAtFolder<T>(sObj, MiscEditorHelpers.GetActiveProjectFolderPath(), fileNameAndExt);


    }

    static public string SaveScriptableObjectAtFolder<T>(T sObj, string parentFolder, string fileNameAndExt) where T:ScriptableObject {

        if (!fileNameAndExt.Contains(".")) fileNameAndExt += ".asset";

        string rawPath = parentFolder.Trim('/', '\\') + "/" + fileNameAndExt;
        //Debug.Log("generating unique asset name from: '" + rawPath + "'...");

        string uPath = AssetDatabase.GenerateUniqueAssetPath(rawPath);
        //Debug.Log("creating asset at: " + uPath);

        AssetDatabase.CreateAsset(sObj, uPath);

        AssetDatabase.SaveAssets();
        Selection.activeObject = sObj;
        EditorGUIUtility.PingObject(sObj);

        return uPath;


    }

    static public string GetActiveProjectFolderPath() {

        //attempt to get active ProjectView folder else Assets root..
        string pwDir = "Assets";
        UnityEngine.Object obj = Selection.activeObject;
        if (obj != null) {
            pwDir = GetUnityObjectFolderPath(obj);
            if (!pwDir.StartsWith("Assets", StringComparison.CurrentCulture)) pwDir = "Assets"; // check incase the active object was a system object outside assets folder (illegal)..
        }

        return pwDir;

    }

    static public string GetUnityObjectFolderPath(UnityEngine.Object obj) {

        string path = "";
        string selPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
        if (!string.IsNullOrEmpty(selPath)) {
            if (Directory.Exists(selPath)) {
                path = selPath;
            } else {
                //trim file name..
                path = selPath.Substring(0, selPath.LastIndexOf('/'));
            }
        }
        return path;

    }

}
