using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class iOSPostProcessBuild
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("BuildTarget " + target);
        Debug.Log(pathToBuiltProject);
        if (target == BuildTarget.iOS)
        {
            string plistPath = pathToBuiltProject + "/Info.plist";
#if UNITY_IOS
			Debug.Log("add photo write permission");
			PlistDocument plist = new PlistDocument();
			plist.ReadFromFile(plistPath);

			PlistElementDict root = plist.root;
			
			const string photoWritePermissionKey = "NSPhotoLibraryAddUsageDescription";
			const string photoWritePermissionValue = "for snap shot";

			PlistElementString e = new PlistElementString(photoWritePermissionValue);
			root[photoWritePermissionKey] = e;
#endif
			plist.WriteToFile(plistPath);
        }
    }
}
