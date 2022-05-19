using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace GameForAndroid.Elements
{
    public static class Collision
    {
		public static bool PerPixelCollision(Rectangle rectA, Texture2D textureA, Rectangle rectB, Texture2D textureB)
		{
			Color[] dataA = new Color[textureA.Width * textureA.Height];
			Color[] dataB = new Color[textureB.Width * textureB.Height];

			textureA.GetData(dataA);
			textureB.GetData(dataB);

			int top = Math.Max(rectA.Top, rectB.Top);
			int bottom = Math.Min(rectA.Bottom, rectB.Bottom);
			int left = Math.Max(rectA.Left, rectB.Left);
			int right = Math.Min(rectA.Right, rectB.Right);

			for (int y = top; y < bottom; y++)
			{
				for (int x = left; x < right; x++)
				{
					Color a = dataA[(y-rectA.Top) * (rectA.Width)+(x - rectA.Left)];
					Color b = dataB[(y - rectB.Top) * (rectB.Width) + (x - rectB.Left)];

					if (a.A != 0 && b.A != 0)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}