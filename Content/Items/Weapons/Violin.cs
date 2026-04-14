using UntitledTerrariaMod.Content.Items.Ammo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace UntitledTerrariaMod.Content.Items.Weapons
{
	public class Violin : ModItem
	{
		public override void SetDefaults() {
			Item.width = 42;
			Item.height = 30;

			Item.autoReuse = true;
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.rare = ItemRarityID.Yellow;
			Item.shootSpeed = 10f;
			Item.useAnimation = 35;
			Item.useTime = 35;
			Item.UseSound = null;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.buyPrice(gold: 2, silver: 50);

			Item.shoot = ModContent.ProjectileType<Projectiles.MusicNoteProjectile>();
			Item.useAmmo = ModContent.ItemType<MusicNote>();
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            SoundStyle violinNote = SoundID.Item26 with {
                Pitch = Main.rand.NextFloat(-0.75f, 0.75f),
                PitchVariance = 0.2f,
                Volume = 0.6f
            };

            SoundEngine.PlaySound(violinNote, player.Center);
        }
		public override void AddRecipes() {
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.Wood, 13)
                .AddIngredient(ItemID.Silk, 1)
                .AddRecipeGroup(CustomRecipeGroups.SilverBarRecipeGroup, 1)
				.AddTile(TileID.Sawmill)
				.Register();
		}
	}
}