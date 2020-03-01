# Omni Core Drill

[![RimWorld 1.0](https://img.shields.io/badge/RimWorld-1.0-green.svg?style=popout-square)](http://rimworldgame.com/) 
[![RimWorld 1.1](https://img.shields.io/badge/RimWorld-1.0-green.svg?style=popout-square)](http://rimworldgame.com/) 
[![License: CC BY-NC-SA 4.0](https://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-lightgrey.svg?style=popout-square)](https://creativecommons.org/licenses/by-nc-sa/4.0/) ![Github Total Downloads](https://img.shields.io/github/downloads-pre/doctorvangogh/MiningShaft/total.svg?style=popout-square)  [![GitHub Latest Release Version](https://img.shields.io/github/release/doctorvangogh/MiningShaft.svg?style=popout-square)](../../releases/latest) [![GitHub Latest Pre-Release Version](https://img.shields.io/github/release/doctorvangogh/MiningShaft/all.svg?style=popout-square)](../../releases)


<p align="center">
<em>"Because OCD can just screw up everything"</em>
</p>

A Rimworld mod offering a deep core drill that supports _any_ mineable material (vanilla or mod-added) out of the box.

Supports _any_ material that _could_ be found in a map as a mineable rock.

Depending on material rarity and properties, drilling for that material will take more or less work and produce higher or lower yields.

For example drilling for _Components_ is quite inefficient due to the small cluster size and low yield of compacted machinery.
_Plasteel_ on the other hand - while much rarer and harder to mine (and drill) - will yield significantly higher amounts of material because of the large cluster size and high yields.

## (Un-)Install

Can be installed into existing savegames. Can be safely uninstalled once all bills on the core drill(s) and the drills themselves are removed/deconstructed.

TheUbie's original caveat still applies:
> This mod does add a new jobgiver into the game. There is a known issue where adding a new jobgiver will break pawns who are active in jobs already. If you are running mods on your saved game, the chance of this happening increases. I suggest starting a fresh save game, but if you wish to use this with an old save and you do have a pawn (or pawns) who start behaving very oddly, one reported work around is to use dev mode (turn it on in options before loading the colony). Check your pawns to see if they are acting wierd (well, more wierd than normal...most of them are nuts to begin with). You can reset their little pawn brains by using the Tool:Down tool to down your pawn. After that, you can either heal them in game normally (hey, free experience for your doctor!) or you can use Tool:Apply Damage... select restore body part, and then select torso to completely heal your pawn (warning: This will also fix old injuries, if you with to preserve those, heal the specific body parts in order). This will get them back on their feet. They will have a negative mood because of the shock of pain. If you fully healed them it should wear off quickly.

### (Somewhat) long winded balancing explanation

Basically any mineable rock in RimWorld can have 'rarity', 'cluster sizes', 'hitpoints' & 'yield' set. I'm crunching those numbers together to get a consistent spread of work required and material yielded for the drill recipes.
Higher hitpoints per lump (Plasteel is tough and requires a lot of hacking/drilling) mean more work to get at the material. Bigger clusters of material mean higher yields (think Steel vs Jade).

Also there's a "find the stuff" component built into the drilling work required (Except for stone - that's literally everywhere). The less common stuff is, the higher that amount will be.

This all turns out to the following values for vanilla materials (as of B18):

Material | work to Drill | Yield | $ / Work
--- | ---: | ---: | ---:
Steel | 75 | 26 | 0.665
Silver | 140 | 7 | 0.050
Gold | 158 | 3 | 0.193
Plasteel | 434 | 13 | 0.423
Components | 400 | 1 | 0.012
Uranium | 233 | 3 | 0.079
Jade | 163 | 3 | 0.094
Granite | 27 | 1 | -
Limestone | 26 | 1 | -
Slate | 15 | 1 | -
Marble | 14 | 1 | -
Sandstone | 12 | 1 | -

The generalized formula also allows inclusion of any custom mod-materials.

## Third Party Notices

Drill artwork by pointcache - licensed under "The Unlicense".

Based on TheUbie's "[Deep Core Miner](https://ludeon.com/forums/index.php?topic=25346.45).

Drilling plan icon derived from [Fluffy's Blueprint mod](https://github.com/FluffierThanThou/Blueprints) - licensed under [CC-BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/).

### Powered by ![Harmony](https://github.com/pardeike/Harmony)

<p align="center">
<img alt="Powered by Harmony" src="https://raw.githubusercontent.com/pardeike/Harmony/master/HarmonyLogo.png" />
</p>

Harmony is lisenced under a [MIT license](https://raw.githubusercontent.com/pardeike/Harmony/master/LICENSE).


