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
    }
}
