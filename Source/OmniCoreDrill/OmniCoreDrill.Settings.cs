using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    class Settings : ModSettings {

        private GlobalDrillParameters _props = new GlobalDrillParameters();

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Deep.Look(ref _props, "props");
        }

        public GlobalDrillParameters GlobalParameters => _props;
    }
}
