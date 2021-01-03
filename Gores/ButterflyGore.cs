using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SolemnLament.Gores
{
	public class ButterflyGore : ModGore
	{
		public override void OnSpawn(Gore gore)
		{
			gore.scale = 0.3f + 0.5f * Main.rand.NextFloat();
			gore.alpha = 0;
			gore.velocity = (MathHelper.Pi * Main.rand.NextFloat() - MathHelper.Pi).ToRotationVector2() * (5 * Main.rand.NextFloat() + 2);
			gore.numFrames = 4;
			gore.behindTiles = false;
			gore.rotation = gore.velocity.ToRotation() + MathHelper.Pi / 2;
			gore.sticky = false;
			gore.timeLeft = Gore.goreTime * 3;
		}

		public override bool Update(Gore gore)
		{

			gore.velocity *= 0.99f;
			gore.rotation = gore.velocity.ToRotation() + MathHelper.Pi / 2;
            if (++gore.frameCounter > 25)
            {
				gore.frame = (byte)((gore.frame + 1) % 4);
            }
			gore.position += gore.velocity;
			gore.alpha += 3;
            if (gore.alpha > 250)
            {
				gore.active = false;
            }
			return false;
		}
	}
}