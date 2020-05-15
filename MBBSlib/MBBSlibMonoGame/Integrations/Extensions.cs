using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.MonoGame.Integrations
{
    public static class Extensions
    {
        public static Microsoft.Xna.Framework.Vector2 ToXnaVector(this Math.Vector2 vector) => new Microsoft.Xna.Framework.Vector2(vector.x, vector.y);
        public static Math.Vector2 ToMBBSVector(this Microsoft.Xna.Framework.Vector2 vector) => new Math.Vector2(vector.X, vector.Y);
    }
}
