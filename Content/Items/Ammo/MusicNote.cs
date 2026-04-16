using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UntitledTerrariaMod.Content.Items.Ammo
{
    public class MusicNote : ModItem
    {
        public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}

        public override void SetDefaults() {
			Item.width = 14;
			Item.height = 14;

			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;

			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 2f;
			Item.value = Item.buyPrice(copper: 5);
			Item.rare = ItemRarityID.Yellow;

            Item.shoot = ModContent.ProjectileType<Projectiles.MusicNoteProjectile>();
			Item.ammo = Item.type;
		}

		public override void AddRecipes() {
			CreateRecipe(50)
				.AddIngredient(ItemID.BlackInk, 1)
                .AddIngredient<Paper>(10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }
}