using gameOpenTK.common;
using gameOpenTK.models;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.controllers
{
    class Loader
    {
        #region singleton
        private Loader() { }
        private static Loader instance = new Loader();
        public static Loader Instance { get => instance; }
        #endregion

        string path = @"C:\Users\USUARIO\Source\Repos\ObjFile-Opentk\gameOpenTK\core\files\objs\";

        public Part LoadFromFile(string name, string filename, int textureID = 0)
        {
            Part obj = null;
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream($"{path}{filename}", FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd(), name);
                    if (textureID != 0)
                    {
                        obj.TextureID = textureID;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found: {0}", filename);
            }
            catch (Exception)
            {
                Console.WriteLine("Error loading file: {0}", filename);
            }
            return obj;
        }
        private Part LoadFromString(string obj, string name)
        {
            List<string> lines = new List<string>(obj.Split('\n'));

            List<Vector2> texs = new List<Vector2>();
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();

            HashList<Vector3> list = new HashList<Vector3>();
            foreach (string line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    string temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        string[] vertparts = temp.Split(' ');

                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success |= float.TryParse(vertparts[1], out vec.Y);
                        success |= float.TryParse(vertparts[2], out vec.Z);

                        colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        texs.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    string temp = line.Substring(2);

                    Tuple<int, int, int> face = new Tuple<int, int, int>(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        string[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        bool success = int.TryParse(faceparts[0], out i1);
                        success |= int.TryParse(faceparts[1], out i2);
                        success |= int.TryParse(faceparts[2], out i3);

                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            // Decrement to get zero-based vertex numbers
                            list.Add(Guid.NewGuid().ToString(), new Vector3(i1 - 1, i2 - 1, i3 - 1));
                        }
                    }
                }
            }

            // Create the Object
            return new Part(name)
            {
                list = list,
                colors = colors.ToArray(),
                vertices = verts.ToArray(),
                texturecoords = texs.ToArray(),
                TextureID = ShaderManager.Instance.textures["container"]
            };
        }
    }
}