using GameLibrary.AppObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameVector = Microsoft.Xna.Framework.Vector2;
namespace GameLibrary.Extensions
{
    public static class GeneralExtensions
    {
        // Turn strings in to  monogame keys
        public static Dictionary<T, Keys> ConvertToKeySet<T>(Dictionary<string, string> keymappings) where T : struct, IConvertible
        {
            return keymappings.ToDictionary(kvp => (T)Enum.Parse(typeof(T), kvp.Key), kvp => (Keys)Enum.Parse(typeof(Keys), kvp.Value));
        }
        /// effectivel returns only the width and heigth of a rectangle.
        /// so it's different to a point...no really
        public static Dimensions ToDimensions(this Rectangle @this) => new Dimensions(@this.Width, @this.Height);

        public static List<int> LoadCsvMapData(string fileName)
        {
            var map = File.ReadAllLines(fileName).ToList();
            return map.SelectMany(r => r.Split(',').Select(s=>String.IsNullOrEmpty(s)?-1:int.Parse(s)).ToList()).ToList();
        }
        /// <summary>
        /// Returns a UNit 
        /// </summary>
        /// <param name="angleInDegrees"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Vector2 UnitAngleVector(float angleInDegrees, int length)
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
        // -1 to mean means 270degrees angle, we starting widdershins
        public static Vector2 UnitAngleVector(float angleInDegrees) => UnitAngleVector(angleInDegrees, 1);
        

    }
}
