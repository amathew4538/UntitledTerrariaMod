using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace UntitledTerrariaMod.Content
{
	public class CustomRecipeGroups : ModSystem
	{
		// A place to store the recipe group so we can easily use it later
		public static RecipeGroup hammerRecipeGroup;

		public override void Unload() {
			hammerRecipeGroup = null;
		}

		public override void AddRecipeGroups() {
			hammerRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Hammer",
                ItemID.WoodenHammer,
                ItemID.RichMahoganyHammer,
                ItemID.PalmWoodHammer,
                ItemID.BorealWoodHammer,
                ItemID.CopperHammer,
                ItemID.TinHammer,
                ItemID.IronHammer,
                ItemID.EbonwoodHammer,
                ItemID.ShadewoodHammer,
                ItemID.LeadHammer,
                ItemID.AshWoodHammer,
                ItemID.SilverHammer,
                ItemID.TungstenHammer,
                ItemID.GoldHammer,
                ItemID.TheBreaker,
                ItemID.PearlwoodHammer,
                ItemID.FleshGrinder,
                ItemID.PlatinumHammer,
                ItemID.MeteorHamaxe,
                ItemID.Rockfish,
                ItemID.MoltenHamaxe,
                ItemID.Pwnhammer,
                4317, // Haemorraxe
                ItemID.Hammush,
                ItemID.ChlorophyteWarhammer,
                ItemID.ChlorophyteJackhammer,
                ItemID.SpectreHamaxe,
                3522, // Solar Flare Hamaxe
                3523, // Vortex Hamaxe
                3524, // Nebula Hamaxe
                3525, // Stardust Hamaxe
                ItemID.TheAxe
            );

			RecipeGroup.RegisterGroup("AnyHammer", hammerRecipeGroup);
		}
    }
}