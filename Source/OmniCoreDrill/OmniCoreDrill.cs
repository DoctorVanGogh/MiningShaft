using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DoctorVanGogh.OmniCoreDrill.Patches;
using Harmony;

using RimWorld;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    class OmniCoreDrillMod : Mod {

        protected Settings _settings;

        private const float multiplierMaxScale = 4f;

        private static float minMultiplier = (float) Math.Log(1/ multiplierMaxScale);
        private static float maxMultiplier = (float)Math.Log(multiplierMaxScale);

        private static string labelLeft = (1/multiplierMaxScale).ToStringPercent();
        private static string labelRight = (multiplierMaxScale).ToStringPercent();

        private Vector2 _scrollPosition = new Vector2();


        public OmniCoreDrillMod(ModContentPack content) : base(content) {
            _settings = GetSettings<Settings>();

            var harmonyInstance = HarmonyInstance.Create("DoctorVanGogh.OmniCoreDrill");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            
            Log.Message("Initialized OmniCoreDrill patches");
        }

        public Settings Settings => _settings;

        public GameMaterialParameters MaterialParams { get; set; }

        public override string SettingsCategory() {
            return LanguageKeys.keyed.ocd.Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect) {
            Listing_Standard list = new Listing_Standard(GameFont.Small) {
                ColumnWidth = inRect.width
            };
            list.Begin(inRect);

            Text.Font = GameFont.Medium;
            list.Label(LanguageKeys.keyed.ocd_global.Translate());
            list.GapLine();
            Text.Font = GameFont.Small;

            const float sliderHeight = 30f;
            const float gapSize = 4f;
            const float iconSize = 24f;


            DoModifier(
                list.GetRect(sliderHeight),
                LanguageKeys.keyed.ocd_density.Translate(),
                _settings.GlobalParameters.Density,
                LanguageKeys.keyed.ocd_density_tip.Translate(),
                1);
            DoModifier(
                list.GetRect(sliderHeight),
                LanguageKeys.keyed.ocd_commonality.Translate(),
                _settings.GlobalParameters.Commonality,
                LanguageKeys.keyed.ocd_commonality_tip.Translate(),
                2);
            DoModifier(
                list.GetRect(sliderHeight), 
                LanguageKeys.keyed.ocd_drillwork.Translate(), 
                _settings.GlobalParameters.DrillWork,
                LanguageKeys.keyed.ocd_drillwork_tip.Translate(),
                3);

            list.Gap();

            var gmp = MaterialParams;

            if (Current.ProgramState != ProgramState.Playing || gmp == null) {
                list.Label(LanguageKeys.keyed.ocd_gameneeded.Translate());
            }
            else {
                GUIStyle headerStyle = new GUIStyle(Text.fontStyles[2]) {
                                         alignment = TextAnchor.UpperCenter
                                     };
                const float scrollbarSize = 16f;

                // calc colum widths
                var w = list.ColumnWidth - scrollbarSize;

                var w4 = w * 0.4f;
                var w3 = w * 0.3f;
                var w2 = w * 0.2f;
                var w1 = w * 0.1f;

                var gcMaterial = new GUIContent(LanguageKeys.keyed.ocd_material.Translate());
                var gcWork = new GUIContent(LanguageKeys.keyed.ocd_work.Translate());
                var gcYield = new GUIContent(LanguageKeys.keyed.ocd_yield.Translate());


                // setup header
                var maxHeight = new[] {
                            headerStyle.CalcHeight(gcMaterial, w4),
                            headerStyle.CalcHeight(gcWork, w3),
                            headerStyle.CalcHeight(gcYield, w3)
                        }.Max();


                const float dividerPadding = 4f;
                const float dividerSize = 2*dividerPadding + 1f;

                var line = list.GetRect(maxHeight + dividerSize);

                // DRAW HEADER
                GUI.Label(new Rect(line.x, line.y, w4 - dividerPadding, maxHeight), gcMaterial, headerStyle);
                Widgets.DrawLineVertical(line.x + w4, line.y, maxHeight + dividerSize);
                var rectHeaderWork = new Rect(line.x + w4 + dividerPadding, line.y, w3 - dividerPadding*2, maxHeight);
                GUI.Label(rectHeaderWork, gcWork, headerStyle);
                TooltipHandler.TipRegion(rectHeaderWork, new TipSignal(LanguageKeys.keyed.ocd_work_tip.Translate()));
                Widgets.DrawLineVertical(line.x + w4 +w3, line.y, maxHeight + dividerSize);
                var rectHeaderYield = new Rect(line.x + w4 + w3 + dividerPadding, line.y, w3 - dividerPadding, maxHeight);
                GUI.Label(rectHeaderYield, gcYield, headerStyle);
                TooltipHandler.TipRegion(rectHeaderYield, new TipSignal(LanguageKeys.keyed.ocd_yield_tip.Translate()));

                Widgets.DrawLineHorizontal(line.x, line.y + maxHeight + dividerPadding +1f, line.width);

                // prepare rows
                var recipes = ThingDefGenerator.AllDrillRecipes;
                var count = recipes.Count();

                var r = list.GetRect(inRect.height - list.CurHeight);

                Widgets.BeginScrollView(r, ref _scrollPosition, new Rect(0, 0, r.width - scrollbarSize, count*(sliderHeight + gapSize)));
                Vector2 position = new Vector2();


                GUIStyle resultStyle = new GUIStyle(Text.fontStyles[1]) {
                                           alignment = TextAnchor.MiddleRight,
                                           fontStyle = FontStyle.Italic
                                       };

                // render each row
                foreach (var entry in recipes) {
                    position.y = position.y + gapSize;

                    line = new Rect(0, position.y, r.width -scrollbarSize, sliderHeight);;

                    Widgets.ThingIcon(new Rect(line.x, line.y + (sliderHeight - iconSize) /2, iconSize, iconSize), entry.Key);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Widgets.Label(new Rect(line.x + iconSize + gapSize, line.y, w4 - iconSize - gapSize, line.height), entry.Key.LabelCap);

                    var mp = gmp[entry.Key];

                    Widgets.DrawLineVertical(line.x + w4, line.y - gapSize, line.height + gapSize);
                    DoMultiplierContentsShortened(new Rect(line.x + w4 + dividerSize, line.y, w2 - dividerSize, line.height), mp.Work );
                    GUI.Label(new Rect(line.x + w4 + w2 + gapSize, line.y, w1 - 2* gapSize - 8f, line.height), entry.Value.workAmount.ToStringWorkAmount(), resultStyle);

                    Widgets.DrawLineVertical(line.x + w4 + w3, line.y - gapSize, line.height + gapSize);
                    DoMultiplierContentsShortened(new Rect(line.x + w4 + w3 + dividerSize, line.y, w2 - dividerSize, line.height), mp.Yield);
                    GUI.Label(new Rect(line.x + w4 + w2 + w3 + gapSize, line.y, w1 - 2*gapSize - 8f, line.height), entry.Value.products[0].count.ToString(), resultStyle);

                    position.y = position.y + sliderHeight;

                }

                Widgets.EndScrollView();
            }

            Text.Anchor = TextAnchor.UpperLeft;
   

            list.End();
        }

        private static void DoMultiplierContentsShortened(Rect rect, Multiplier m) {
            string leftLabel = null;
            string rightLabel = null;

            if (Mouse.IsOver(rect)) {
                leftLabel = labelLeft;
                rightLabel = labelRight;
            }
            m.DoWindowContents(rect, minMultiplier, maxMultiplier, leftLabel, rightLabel);
        }

        private void DoModifier(Rect inRect, string label, Multiplier m, string tooltip, int uniqueId) {
            var half = inRect.width/2f;

            var rectLabel = new Rect(0, inRect.y, half, inRect.height);
            GUI.Label(rectLabel, label);
            TooltipHandler.TipRegion(rectLabel, () => tooltip, uniqueId);

            m.DoWindowContents(new Rect(half, inRect.y, half, inRect.height), minMultiplier, maxMultiplier, labelLeft, labelRight);
        }
    }    
}
