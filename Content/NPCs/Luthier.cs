using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using UntitledTerrariaMod.Content.EmoteBubbles;
using UntitledTerrariaMod.Common.Systems;
using Microsoft.Xna.Framework;

namespace UntitledTerrariaMod.Content.NPCs
{
    [AutoloadHead]
    public class Luthier : ModNPC
    {
        public const string ShopName = "LuthierShop";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26;
            NPCID.Sets.AttackFrameCount[Type] = 0;
            NPCID.Sets.ExtraFramesCount[Type] = 5;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackAverageChance[Type] = 0;

            NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<LuthierEmote>();

            ContentSamples.NpcBestiaryRarityStars[Type] = 3;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f,
				Direction = 1
			};

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike)
            ;


        }
        public override void SetDefaults()
        {
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 150;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("Mods.UntitledTerrariaMod.Bestiary.Luthier_1"),
			]);
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
			}

			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				string variant = "";
				if (NPC.IsShimmerVariant)
					variant += "_Shimmer";
				if (NPC.altTexture == 1)
					variant += "_Party";
				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

				if (hatGore > 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}
		}
        public override void OnSpawn(IEntitySource source)
        {
			if (source is EntitySource_SpawnNPC) {
				TownNPCRespawnSystem.unlockedLuthierSpawn = true;
			}
		}
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
			if (TownNPCRespawnSystem.unlockedLuthierSpawn) {
				return true;
			}

			foreach (var player in Main.ActivePlayers) {
				if (player.inventory.Any(item => item.type == ModContent.ItemType<Items.Weapons.Violin>() || item.type == ModContent.ItemType<Items.Ammo.MusicNote>())) {
					return true;
				}
			}

			return false;
		}

        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            bool hasSawmill = false;
            bool hasTreesNearby = false;
            int searchDistance = 10; // 10 blocks away from the house

            // Scan the house for the Sawmill
            for (int x = left; x <= right; x++) {
                for (int y = top; y <= bottom; y++) {
                    if (Main.tile[x, y].TileType == TileID.Sawmill) {
                        hasSawmill = true;
                        break;
                    }
                }
            }
        
            // Scan outside the house for trees
            // Search from (left - 10) to (right + 10)
            for (int x = left - searchDistance; x <= right + searchDistance; x++) {
                for (int y = top - searchDistance; y <= bottom + searchDistance; y++) {
                    // Ensure we don't check coordinates outside the map
                    if (WorldGen.InWorld(x, y)) {
                        Tile tile = Main.tile[x, y];
                        // TileID.Trees covers standard trees; you can add TileID.PalmTree etc.
                        if (tile.HasTile && (tile.TileType == TileID.Trees || tile.TileType == TileID.PalmTree)) {
                            hasTreesNearby = true;
                            break;
                        }
                    }
                }
                if (hasTreesNearby) break;
            }

            // The house is only valid if it has a Sawmill and trees nearby
            return hasSawmill && hasTreesNearby;
        }
        public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Olaf Grawert",
				"Antonio Stradivari",
				"Giuseppe Guarneri del Gesù",
				"Nicolò Amati",
                "Gasparo da Salò",
                "Andrea Amati",
                "Giovanni Paolo Maggini",
                "Matteo Goffriller",
                "Domenico Montagnana",
                "François Tourte",
                "Dominique Peccatte",
                "Eugène Sartory",
                "Nicolas Maline",
                "Charles Peccatte",
                "Jean-Baptiste Vuillaume",
                "Charles Jean Baptiste Collin-Mezin",
                "Matthias Klotz",
                "Sebastian Klotz",
                "Leopold Widhalm",
                "Gasparo Duiffopruggar",
                "Giovanni Battista Guadagnini",
                "Alessandro Gagliano",
                "Carlo Giuseppe Testore",
                "Francesco Ruger",
                "Abraham Prescott"
			};
		}
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28"); // This sets the button to say "Shop"
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton) {
                shopName = ShopName;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemID.Wood, 3));
		}

        public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

        public override void OnGoToStatue(bool toKingStatue) {
			if (Main.netMode == NetmodeID.Server) {
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)NPC.whoAmI);
				packet.Send();
			}
			else {
				StatueTeleport();
			}
		}

        
        public void StatueTeleport()
        {
            for (int i = 0; i < 30; i++) {
                Vector2 position = Main.rand.NextVector2Square(-20, 21);
                if (Math.Abs(position.X) > Math.Abs(position.Y)) {
                    position.X = Math.Sign(position.X) * 20;
                }
                else {
                    position.Y = Math.Sign(position.Y) * 20;
                }

                Dust.NewDustPerfect(NPC.Center + position, DustID.GoldFlame, Vector2.Zero).noGravity = true;
            }
        }
        public override void AddShops() {
            var npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.Wood)
                .Add(ItemID.Sawmill)
                .Add(ModContent.ItemType<Items.Ammo.MusicNote>())
                .Add(ModContent.ItemType<Items.Weapons.Violin>());
            npcShop.Register();
        }
        public override string GetChat() {
    		WeightedRandom<string> chat = new WeightedRandom<string>();

    		chat.Add(Language.GetTextValue("Mods.UntitledTerrariaMod.NPCs.Luthier.TownNPCMood.Content"));
    		chat.Add("Have you ever heard the soul of an maple? It comes out in a tone unrivaled once it's carved.");
    		chat.Add("Careful with that sword near my workbench! One scratch and it'll ruin my instrument!");
			chat.Add("Have you brought me any special wood?");
			chat.Add("How are your lessons going? Have you been practicing?");
			chat.Add("I practiced 40 hours yesterday. How about you?");
			chat.Add("You need a bow rehair! Look at how many hairs are missing!");
			chat.Add("Have you replaced your strings recently?");
			chat.Add("You look beautiful today! Huh? Oh, I was talking to the instrument, not you.");
			chat.Add("Quickly, please. I'm almost done and this is a masterpiece!");
			chat.Add("Have you seen the lanterns at night? I'm creating a song for a night like that.");

    		if (Main.bloodMoon) {
        		chat.Add("The temperatures tonight are devestating. I can't get this violin in tune!");
    		}

    		return chat;
		}
    }
}