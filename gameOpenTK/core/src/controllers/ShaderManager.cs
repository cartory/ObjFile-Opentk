using gameOpenTK.common;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.controllers
{
    public class ShaderManager
    {
        #region Singleton
        private static ShaderManager instance;


        public static ShaderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShaderManager();
                }
                return instance;
            }
        }
        #endregion

        public Dictionary<string, int> textures = new Dictionary<string, int>();
        public Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        //  values = {  default,    textured    }
        public string activeShader = "textured";

        private ShaderManager()
        {
            //shaders.Add("default", new ShaderProgram("vs.glsl", "fs.glsl", true));
            shaders.Add("textured", new ShaderProgram(
                @"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\shaders\vs_tex.glsl",
                @"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\shaders\fs_tex.glsl", true));

            textures.Add("opentksquare.png",
                loadImage(@"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\images\opentksquare.png"));
            textures.Add("opentksquare2.png",
                loadImage(@"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\images\opentksquare2.png"));
            textures.Add("piel.jpg",
                loadImage(@"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\images\piel.jpg"));
            textures.Add("azul_tex.jpg",
                loadImage(@"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\images\azul_tex.jpg"));
            textures.Add("tiger_tex.jpg",
                loadImage(@"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\images\tiger_tex.jpg"));
        }

        int loadImage(Bitmap image)
        {
            int texID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }

        int loadImage(string filename)
        {
            try
            {
                Bitmap file = new Bitmap(filename);
                return loadImage(file);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public void deleteShaders()
        {
            foreach (KeyValuePair<string, ShaderProgram> entry in shaders)
            {
                entry.Value.DeleteShader();
            }
        }
    }
}