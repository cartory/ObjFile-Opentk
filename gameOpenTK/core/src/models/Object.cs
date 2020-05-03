using gameOpenTK.common;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using gameOpenTK.controllers;

namespace gameOpenTK.models
{
    class Object : Volume<Vector3>
    {
        Vector3[] colors;
        Vector3[] vertices;
        Vector2[] texturecoords;
        string shader = ShaderManager.Instance.activeShader;

        public override int VertCount { get => vertices.Length; }
        public override int IndiceCount { get => list.Count * 3; }
        public override int ColorDataCount { get => colors.Length; }

        public override Vector3[] GetVerts() => vertices;
        public override int[] GetIndices(int offset = 0)
        {
            List<int> temp = new List<int>();
            foreach (DictionaryEntry entry in list)
            {
                Vector3 e = (Vector3)entry.Value;
                temp.Add((int)e.X + offset);
                temp.Add((int)e.Y + offset);
                temp.Add((int)e.Z + offset);
            }
            return temp.ToArray();
        }

        public override Vector3[] GetColorData() => colors;
        public override Vector2[] GetTextureCoords() => texturecoords;
        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) *
                Matrix4.CreateRotationX(Rotation.X) *
                Matrix4.CreateRotationY(Rotation.Y) *
                Matrix4.CreateRotationZ(Rotation.Z) *
                Matrix4.CreateTranslation(Position);
        }

        public static Object LoadFromFile(string filename)
        {
            Object obj = new Object();
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd());
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

        public static Object LoadFromString(string obj)
        {
            // Seperate lines from the file
            List<String> lines = new List<string>(obj.Split('\n'));

            // Lists to hold model data
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();

            //List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();
            Hashtable list = new Hashtable();
            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(' ');

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success |= float.TryParse(vertparts[1], out vec.Y);
                        success |= float.TryParse(vertparts[2], out vec.Z);

                        // Dummy color/texture coordinates for now
                        colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        texs.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<int, int, int> face = new Tuple<int, int, int>(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success |= int.TryParse(faceparts[1], out i2);
                        success |= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
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
            Object vol = new Object();
            vol.vertices = verts.ToArray();
            vol.list = list;
            vol.colors = colors.ToArray();
            vol.texturecoords = texs.ToArray();
            vol.TextureID = ShaderManager.Instance.textures["piel.jpg"];
            return vol;
        }

        public override Vector3 GetT(string key) => (Vector3)list[key];

        public override void DelT(string key) => list.Remove(key);

        public override void SetT(string key, Vector3 element) => list[key] = element;

        public override void AddT(string key, Vector3 element) => list.Add(key, element);

        public override void Draw(int indiceat = 0)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);

            GL.UniformMatrix4(
                ShaderManager.Instance.shaders[shader].GetUniform("modelview"),
                false, ref ModelViewProjectionMatrix);
            if (ShaderManager.Instance.shaders[shader].GetUniform("maintexture") != -1)
            {
                GL.Uniform1(ShaderManager.Instance.shaders[shader].GetUniform("maintexture"), 0);
            }

            GL.DrawElements(PrimitiveType.Triangles, IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
        }

        public void Update(Camera cam, Size ClientSize)
        {
            CalculateModelMatrix();
            ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
            ModelViewProjectionMatrix = ModelMatrix * ViewProjectionMatrix;
        }
    }
}