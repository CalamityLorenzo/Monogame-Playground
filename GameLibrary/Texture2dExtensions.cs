using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameLibrary.Extensions
{
    public static class Texture2d
    {
        public static Texture2D FromFileName(GraphicsDevice graphicsDevice, string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            var newTexture = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Dispose();
            return newTexture;
        }

        public static Texture2D TextureFromFileName(this GraphicsDevice graphicsDevice, string fileName)
        {
            return FromFileName(graphicsDevice, fileName);   
        }
    }


}