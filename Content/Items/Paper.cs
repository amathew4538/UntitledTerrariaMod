using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UntitledTerrariaMod.Content.Items
{
    public class Paper : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;

            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(copper: 5);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3)
				.AddRecipeGroup(RecipeGroupID.Wood, 1)
				.AddTile(TileID.Sawmill)
				.Register();
        }
    }
}