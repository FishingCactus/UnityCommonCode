using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAutomation : EditorWindow
{
    enum BuildType
    {
        BaseGame
    }

    enum BuildPlatform
    {
        Windows = 1 << 0,
        Linux = 1 << 1,
        Mac = 1 << 2
    }

    static string BaseFolder = "../../AlgobotSide/Builds/";
    static string[] GetNormalReleaseScenes()
    {
        string[] ScenesList = new string[] {
                                                "Assets/__Algobot/Scenes/spash_screen.unity",

                                                "Assets/__Algobot/Scenes/level_scene.unity",
                                                "Assets/__Algobot/Scenes/level_selection_scene.unity",
                                                "Assets/__Algobot/Scenes/main_menu_scene.unity",

                                                "Assets/__Algobot/Scenes/scenery/sc1-1.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-2.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-3.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-4.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-5.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-6.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-7.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-8.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc1-9.unity",

                                                "Assets/__Algobot/Scenes/scenery/sc2-1.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-2.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-3.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-4.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-5.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-6.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-7.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-8.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc2-9.unity",

                                                "Assets/__Algobot/Scenes/scenery/sc3-1.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc3-2.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc3-3.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc3-4.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc3-5.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc3-6.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc3-7.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc3-8.unity",

                                                "Assets/__Algobot/Scenes/scenery/sc4-1.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-2.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-3.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-4.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-5.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-6.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-7.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-8.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc4-9.unity",

                                                "Assets/__Algobot/Scenes/scenery/sc5-1.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc5-2.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc5-3.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc5-4.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc5-5.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc5-6.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc5-7.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc5-8.unity",

                                                "Assets/__Algobot/Scenes/scenery/sc6-1.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc6-2.unity",
                                                "Assets/__Algobot/Scenes/scenery/sc6-3.unity",
                                                };

        return ScenesList;
    }

    static string[] GetScenes( BuildType type )
    {
        string[]
            SceneArray;

        switch ( type )
        {
            case BuildType.BaseGame:
                SceneArray = GetNormalReleaseScenes();
                break;

            default:
                SceneArray = null;
                break;
        }

        return SceneArray;
    }

    [MenuItem("FishingCactus/Build/Build_as_is/BuildWindowsBaseGame")]
    static void BuildWindowsBaseGame()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();

        options.scenes = GetNormalReleaseScenes();
        options.locationPathName = BaseFolder + "as_is_base_game/Windows/Algobot.exe";
        options.target = BuildTarget.StandaloneWindows64;

        BuildPipeline.BuildPlayer( options );
    }

    static void Build( string base_path, BuildType type, BuildPlatform platform )
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = GetScenes( type );

        if ( ( platform & BuildPlatform.Windows ) == BuildPlatform.Windows )
        {
            options.locationPathName = BaseFolder + base_path + "Windows64/Algobot.exe";
            options.target = BuildTarget.StandaloneWindows64;

            BuildPipeline.BuildPlayer( options );

            options.locationPathName = BaseFolder + base_path + "Windows32/Algobot.exe";
            options.target = BuildTarget.StandaloneWindows;

            BuildPipeline.BuildPlayer( options );
        }

        if ( ( platform & BuildPlatform.Linux ) == BuildPlatform.Linux )
        {
            options.locationPathName = BaseFolder + base_path + "Linux/Algobot.x86";
            options.target = BuildTarget.StandaloneLinuxUniversal;

            BuildPipeline.BuildPlayer( options );
        }

        if ( ( platform & BuildPlatform.Mac ) == BuildPlatform.Mac )
        {
            options.locationPathName = BaseFolder + base_path + "Mac/Algobot.app";
            options.target = BuildTarget.StandaloneOSX;

            BuildPipeline.BuildPlayer( options );
        }

        string version_number = File.ReadAllText( "Assets\\__Algobot\\Various\\version_number.txt" );

        string[] version_numbers = version_number.Split( '.' );

        int build_number = int.Parse( version_numbers[ 2 ] );
        build_number++;

        version_number = version_numbers[ 0 ] + "." + version_numbers[ 1 ] + "." + build_number;

        File.WriteAllText( "Assets\\__Algobot\\Various\\version_number.txt", version_number );
    }


    [MenuItem("FishingCactus/Build/BuildEveryRetail")]
    static void BuildEveryRetail()
    {
        BuildAllSteamRetail();
        BuildAllRetail();
    }

    [MenuItem("FishingCactus/Build/BuildAll/BuildAllSteam")]
    static void BuildAllSteamRetail()
    {
        DefineSteamRetail();
        Build( "Steam_Retail/", BuildType.BaseGame, BuildPlatform.Windows );
    }

    [MenuItem("FishingCactus/Build/BuildAll_Debug/BuildAllSteam")]
    static void BuildAllSteamNonRetail()
    {
        DefineSteam();
        Build( "Steam_Debug/", BuildType.BaseGame, BuildPlatform.Windows );
    }

    [MenuItem("FishingCactus/Build/BuildAll/BuildAllDRMFree")]
    static void BuildAllRetail()
    {
        DefineRetail();
        Build( "DRMFree_Retail/", BuildType.BaseGame, BuildPlatform.Windows );
    }

    [MenuItem("FishingCactus/Build/BuildAll_Debug/BuildAllDRMFree")]
    static void BuildAll()
    {
        DefineNothing();
        Build( "DRMFree_Debug/", BuildType.BaseGame, BuildPlatform.Windows );
    }

    [MenuItem("FishingCactus/Defines/DefineRetail")]
    static void DefineRetail()
    {
        BuildTargetGroup target_group = BuildTargetGroup.Standalone;
        string symbols = "AMPLIFY_SHADER_EDITOR;TextMeshPro;RETAIL_VERSION";
        PlayerSettings.SetScriptingDefineSymbolsForGroup( target_group, symbols );
    }

    [MenuItem("FishingCactus/Defines/DefineSteamRetail")]
    static void DefineSteamRetail()
    {
        BuildTargetGroup target_group = BuildTargetGroup.Standalone;
        string symbols = "AMPLIFY_SHADER_EDITOR;TextMeshPro;STEAM_VERSION;RETAIL_VERSION";
        PlayerSettings.SetScriptingDefineSymbolsForGroup( target_group, symbols );
    }

    [MenuItem("FishingCactus/Defines/Defines_Debug/DefineNothing")]
    static void DefineNothing()
    {
        BuildTargetGroup target_group = BuildTargetGroup.Standalone;
        string symbols = "AMPLIFY_SHADER_EDITOR;TextMeshPro";
        PlayerSettings.SetScriptingDefineSymbolsForGroup( target_group, symbols );
    }

    [MenuItem("FishingCactus/Defines/Defines_Debug/DefineSteam")]
    static void DefineSteam()
    {
        BuildTargetGroup target_group = BuildTargetGroup.Standalone;
        string symbols = "AMPLIFY_SHADER_EDITOR;TextMeshPro;STEAM_VERSION";
        PlayerSettings.SetScriptingDefineSymbolsForGroup( target_group, symbols );
    }
}