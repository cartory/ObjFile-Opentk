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
        public static ShaderManager Instance { get => instance; }
        private static ShaderManager instance = new ShaderManager();
        #endregion

        string path = @"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\";

        public Dictionary<string, int> textures = new Dictionary<string, int>();
        public Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        //  values = {  default,    textured    }
        public string activeShader = "textured";

        private ShaderManager()
        {
            shaders.Add(
                key: "textured",
                value: new ShaderProgram(
                    GetPath(@"shaders\vs_tex.glsl"),
                    GetPath(@"shaders\fs_tex.glsl"),
                    fromFile: true
            ));

            textures.Add("wall", loadImage(GetPath(@"images\wall.png")));
            textures.Add("azul", loadImage(GetPath(@"images\azul_tex.jpg")));
            textures.Add("piel", loadImage(GetPath(@"images\piel.jpg")));
            textures.Add("tiger", loadImage(GetPath(@"images\tiger_tex.jpg")));
            textures.Add("metal", loadImage(GetPath(@"images\metal.jpg")));
            textures.Add("box", loadImage(GetPath(@"images\box.png")));
            textures.Add("container", loadImage(GetPath(@"images\container.png")));
            textures.Add("opentksquare", loadImage(GetPath(@"images\opentksquare.png")));
            textures.Add("opentksquare2", loadImage(GetPath(@"images\opentksquare2.png")));
        }

        string GetPath(string file) => $"{path}{file}";

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