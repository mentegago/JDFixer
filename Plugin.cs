using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using JDFixer.Installers;
using SiraUtil.Zenject;

namespace JDFixer
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public sealed class Plugin
    {
        public static Harmony harmony;
        //internal static string game_version = "";

        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Plugin.Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();

            zenjector.Install<JDFixerMenuInstaller>(Location.Menu);
            //TimeSetup.Inject(zenjector);
        }


        [OnEnable]
        public void OnApplicationStart()
        {
            //Plugin.Log.Debug("OnApplicationStart()");
            //game_version = IPA.Utilities.UnityGame.GameVersion.ToString();
            //Plugin.Log.Debug(game_version);

            harmony = new Harmony("com.zephyr.BeatSaber.JDFixer");
            //TimeSetup.Patch();
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            UI.Donate.Refresh_Text();
        }


        [OnDisable]
        public void OnApplicationQuit()
        {
            PluginConfig.Instance.Changed();
            harmony.UnpatchSelf();
        }


    }
}
