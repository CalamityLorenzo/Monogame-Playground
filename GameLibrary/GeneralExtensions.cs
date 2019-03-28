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
            var unitV = new Vector2(0, -length);
            var theta = (double)MathHelper.ToRadians(angleInDegrees);
            var cs = Math.Cos(theta);
            var sn = Math.Sin(theta);
            // WE are calculating the end point of a line.
            // This result will be added to the start position.
            var endX = (float)(cs * unitV.X - sn * unitV.Y); // unitV.X * sn - unitV.Y * cs;
            var endY = (float)(sn * unitV.X + cs * unitV.Y);
            return new Vector2(endX, endY);
        }
        // This kinda answers the questions... I think..
        // https://www.physicsclassroom.com/mmedia/vectors/vd.cfm
        // -1 to mean means 270, we starting widdershins
        public static Vector2 UnitAngleVector(float angleInDegrees)
        {
            var unitV = new Vector2(0, -1);
            var theta = (double)MathHelper.ToRadians(angleInDegrees);
            var cs = Math.Cos(theta);
            var sn = Math.Sin(theta);
            
            var endX = (float)(cs * unitV.X - sn * unitV.Y); // unitV.X * sn - unitV.Y * cs;
            var endY = (float)(sn * unitV.X + cs * unitV.Y);
            //return new Vector2((float)ca * unitV.X - (float)sa * unitV.Y, (float)sa * unitV.X + (float)ca * unitV.Y);
            return new Vector2(endX, endY);
        }
    }
}
