using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public static class GeneralExtensions
    {
        public static Dictionary<T, Keys> ConvertToKeySet<T>(Dictionary<string, string> keymappings) where T : struct, IConvertible
        {
            return keymappings.ToDictionary(kvp => (T)Enum.Parse(typeof(T), kvp.Key), kvp => (Keys)Enum.Parse(typeof(Keys), kvp.Value));
        }

        public static Vector2 AngledVectorFromDegrees(float angleInDegrees, int length = 1)
        {
            // our unit vector as a reference 
            // length/Magnitude should be constant
            var unitV = new Vector2(0, length);
            var theta = (double)MathHelper.ToRadians(angleInDegrees);
            var cs = Math.Cos(theta);
            var sn = Math.Sin(theta);
            // WE are calculating the end point of a line.
            // This result will be added to the start position.
            var endX = (unitV.X * cs) - (unitV.Y * sn);
            var endY = (unitV.X * sn) - (unitV.Y * cs);
            return new Vector2((float)Math.Floor(-endX), (float)endY);
        }
    }
}
