using UntitledTerrariaMod.Content.NPCs;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace UntitledTerrariaMod.Common.Systems
{
	public class TownNPCRespawnSystem : ModSystem
	{
		public static bool unlockedLuthierSpawn = false;

		public override void ClearWorld() {
			unlockedLuthierSpawn = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			tag[nameof(unlockedLuthierSpawn)] = unlockedLuthierSpawn;
		}

		public override void LoadWorldData(TagCompound tag) {
			unlockedLuthierSpawn = tag.GetBool(nameof(unlockedLuthierSpawn));

			unlockedLuthierSpawn |= NPC.AnyNPCs(ModContent.NPCType<Luthier>());
		}

		public override void NetSend(BinaryWriter writer) {
			writer.WriteFlags(unlockedLuthierSpawn);
		}

		public override void NetReceive(BinaryReader reader) {
			reader.ReadFlags(out unlockedLuthierSpawn);
		}
	}
}