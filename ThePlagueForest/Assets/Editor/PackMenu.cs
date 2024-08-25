using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using System;
using System.IO;

public class CustomBuild : MonoBehaviour
{
    [MenuItem("Forest/Build Android")]
    public static void PerformAndroidBuild()
    {
        BuildPlatform(BuildTarget.Android);
    }

    [MenuItem("Forest/Build iOS")]
    public static void PerformiOSBuild()
    {
        BuildPlatform(BuildTarget.iOS);
    }

    [MenuItem("Forest/Build Windows")]
    public static void PerformWindowsBuild()
    {
        BuildPlatform(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Forest/Build OSX")]
    public static void PerformOSXBuild()
    {
        BuildPlatform(BuildTarget.StandaloneOSX);
    }
    
    private static void BuildPlatform(BuildTarget buildTarget)
    {
        Debug.Log($"building " + buildTarget.ToString());
        PlayerSettings.bundleVersion = GameConfig.Version;
        PlayerSettings.companyName = GameConfig.CompanyName;
        PlayerSettings.productName = GameConfig.AppName;
        PlayerSettings.SplashScreen.show = false;

        BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
        string targetDir = Application.dataPath + "/../BuildDist/" + buildTarget.ToString();

        if (buildTarget == BuildTarget.Android)
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.bundleVersionCode = GameConfig.BuildNumber;
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = "theplagueforest.keystore";
            PlayerSettings.Android.keystorePass = "123456";
            PlayerSettings.Android.keyaliasName = "theplagueforest";
            PlayerSettings.Android.keyaliasPass = "123456";
            PlayerSettings.Android.targetSdkVersion = PlayerSettings.Android.targetSdkVersion;
            targetDir += "/" + GameConfig.AppName + ".apk";
        }
        else if (buildTarget == BuildTarget.iOS)
        {
            buildTargetGroup = BuildTargetGroup.iOS;
            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            PlayerSettings.iOS.applicationDisplayName = GameConfig.AppName;
            PlayerSettings.iOS.buildNumber = GameConfig.BuildNumber.ToString();
            targetDir += "/" + GameConfig.AppName;
        }
        else if (buildTarget == BuildTarget.StandaloneWindows64)
        {
            buildTargetGroup = BuildTargetGroup.Standalone;
            targetDir += "/" + GameConfig.AppName + ".exe";
        }

        else if (buildTarget == BuildTarget.StandaloneOSX)
        {
            buildTargetGroup = BuildTargetGroup.Standalone;
            targetDir += "/" + GameConfig.AppName + ".app";
        }
        
        if (File.Exists(targetDir))
        {
            File.Delete(targetDir);
        }
        if (Directory.Exists(targetDir))
        {
            Directory.Delete(targetDir, true);
        }

        List<string> EditorScenes = new List<string>();
        Debug.Log("Collect Scene");
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            EditorScenes.Add(scene.path);
            Debug.Log("Scene.Path:" + scene.path);
        }
        GenericBuild(EditorScenes.ToArray(), targetDir, buildTargetGroup, buildTarget);
    }

    private static void GenericBuild(string[] scenes, string targetDir, BuildTargetGroup buildTargetGroup,  BuildTarget buildTarget)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        if (buildTarget == BuildTarget.Android)
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        }
        PlayerSettings.applicationIdentifier = GameConfig.AppId;
        PlayerSettings.SetApplicationIdentifier(buildTargetGroup, GameConfig.AppId);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = targetDir;
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.targetGroup = buildTargetGroup;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + report.summary.totalSize + " bytes");
        }
        else if (report.summary.result == BuildResult.Failed)
        {
            throw new Exception("BuildPlayer failure.");
        }
    }
}
