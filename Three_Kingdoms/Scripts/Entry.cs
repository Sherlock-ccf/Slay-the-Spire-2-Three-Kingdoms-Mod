using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
namespace Three_Kingdoms.Scripts;
[ModInitializer("Init")]
public class Entry
{
    public static void Init()
    {
        var harmony = new Harmony("sts2.reme.ThreeKingdomsMod");
        harmony.PatchAll();
        ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
        Log.Info("Mod initialized!");
    }
}