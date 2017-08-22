using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    class GameMaterialParameters : GameComponent {

        private Dictionary<ThingDef, MaterialParameters> _materialModifiers = new Dictionary<ThingDef, MaterialParameters>();

        public GameMaterialParameters() {            
        }

        public GameMaterialParameters(Game game) : this() {
        }


        public override void ExposeData() {
            base.ExposeData();

            Scribe_Collections.Look(ref _materialModifiers, "materials");
        }

        public override void LoadedGame() {
            base.LoadedGame();

            PrepareDrill();
        }

        public override void StartedNewGame() {
            base.StartedNewGame();

            PrepareDrill();
        }

        private void PrepareDrill() {
            UpsertMaterialModifiers();
            ThingDefGenerator.UpdateAllGeneratedDefs();
            LoadedModManager.GetMod<OmniCoreDrillMod>().MaterialParams = this;
        }

        private void UpsertMaterialModifiers() {
            foreach (var coreMiningDef in ThingDefGenerator.GetCoreMiningDefs()) {
                var key = coreMiningDef.products.First().thingDef;

                if (!_materialModifiers.ContainsKey(key))
                    _materialModifiers.Add(key, new MaterialParameters(key));
            }
        }

        public MaterialParameters this[ThingDef def] {
            get {
                MaterialParameters mp;
                return _materialModifiers.TryGetValue(def, out mp) ? mp : null;
            }
        }
    }
}
