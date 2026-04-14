using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UntitledTerrariaMod.Content.Items.Weapons
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class BentGun : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.UntitledTerrariaMod.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 22;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(gold: 4, silver: 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe()
				.AddIngredient(ItemID.FlintlockPistol, 1)
				.AddRecipeGroup(CustomRecipeGroups.HammerRecipeGroup, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
