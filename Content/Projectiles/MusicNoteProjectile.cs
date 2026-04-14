using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace UntitledTerrariaMod.Content.Projectiles
{
    public class MusicNoteProjectile : ModProjectile
    {

        private NPC HomingTarget {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
            set => Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
        }

        public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults() {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI() {
            if (Main.rand.NextBool(5)) {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Gold);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            float maxDetectRadius = 400f;

            if (DelayTimer < 15) {
                DelayTimer++;
                return;
            }

            HomingTarget ??= FindClosestNPC(maxDetectRadius);

            if (HomingTarget != null && !IsValidTarget(HomingTarget)) {
                HomingTarget = null;
            }

            if (HomingTarget != null) {
                float speed = 8f;
                Vector2 desiredVelocity = Projectile.DirectionTo(HomingTarget.Center) * speed;
                

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.08f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void OnKill(int timeLeft) {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            for (int i = 0; i < 10; i++) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Gold);
            }
        }

        public NPC FindClosestNPC(float maxDetectDistance) {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            foreach (var target in Main.ActiveNPCs) {
                if (IsValidTarget(target)) {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                    if (sqrDistanceToTarget < sqrMaxDetectDistance) {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }
            return closestNPC;
        }

        public bool IsValidTarget(NPC target) => target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
    }
}