using System.Reflection;
using DoctorVanGogh.OmniCoreDrill.Patches;
using Harmony;
using RimWorld;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    class OmniCoreDrillMod : Mod {
        public OmniCoreDrillMod(ModContentPack content) : base(content) {
            var harmonyInstance = HarmonyInstance.Create("DoctorVanGogh.OmniCoreDrill");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            
            Log.Message("Initialized OmniCoreDrill patches");
        }
    }
}
