using Terraria;
using Terraria.ModLoader;
using System.Linq;

namespace UntitledTerrariaMod.Content.NPCs
{
    public class LuthierMusicScene : ModSceneEffect
    {
        // This tells the game which music to play
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/AmongAShiningNight");

        // This determines when the music should play
        public override bool IsSceneEffectActive(Player player) {
            // Find the Luthier NPC
            int luthierType = ModContent.NPCType<Luthier>();
            NPC luthier = Main.npc.FirstOrDefault(n => n.active && n.type == luthierType);

            if (luthier == null) return false;

            // Check conditions: Lantern Night + Sky + Distance
            bool isLanternNight = !Main.dayTime && Terraria.GameContent.Events.LanternNight.LanternsUp;
            bool inSky = luthier.position.Y < Main.worldSurface * 16f * 0.5f;
            bool playerNear = player.Distance(luthier.Center) < 1200f;
            bool randomSucceeded = Random.NextDouble() < 0.025;
            bool afterMidnight = !Main.dayTime && Main.time >= 16200;


            // Add the "near another NPC" check here later

            return inSky && playerNear && isLanternNight && randomSucceeded && afterMidnight;
        }

        // Sets the priority so it overrides biome music
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
    }
}