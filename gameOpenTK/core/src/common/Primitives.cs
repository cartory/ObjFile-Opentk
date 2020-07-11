using gameOpenTK.common;
using gameOpenTK.controllers;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.ES11;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.common
{
    public class HashList<T>
    {
        public Hashtable list { get; }
        public int Count { get => list.Count; }

        public HashList() => list = new Hashtable();

        public T Get(object key) => (T)list[key];
        public void Del(string key) => list.Remove(key);
        public void Set(string key, T element) => list[key] = element;
        public void Add(string key, T element) => list.Add(key, element);

        public IDictionaryEnumerator GetEnumerator() => list.GetEnumerator();
    }
    public abstract class Volume<T>
    {
        protected string shader = ShaderManager.Instance.activeShader;
        public HashList<T> list = new HashList<T>();

        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;

        public float scale = 1;
        public float step = .1f;
        public float theta = 3f;

        public virtual int VertCount { get; set; }
        public virtual int IndiceCount { get; set; }
        public virtual int ColorDataCount { get; set; }

        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 matrixRotation = Matrix4.Identity;
        public Matrix4 matrixContainer = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public abstract void Draw(int indiceat = 0);
        public abstract void Update(object camera, Size ClientSize, int vertCount = 0);

        public int TextureID;
        public int TextureCoordsCount;
        public bool IsTextured = false;
    }

    public class Segment {
        private static float width = .1f;
        public Vector3 p1, p2;
        public float m;
        //  Ax + By + C = 0
        public float A, B, C;
        public Segment(Vector3 p1, Vector3 p2) 
        {
            this.p1 = p1;
            this.p2 = p2;
            this.calculateABC();
        }

        private void calculateABC() 
        {
            this.A = p2.Z - p1.Z;
            this.B = p1.X - p2.X;
            this.C = -(A * p1.X + B * p1.Z);
        }

        private float distance(float px, float pz) 
        {
            return Math.Abs(A * px + B * pz + C) / (float)Math.Sqrt(A * A + B * B);
        }

        public bool contains(float px, float pz) {
            //horizontal
            if (p1.Z == p2.Z)
            {
                return Math.Abs(A * px + B * pz + C) / Math.Sqrt(A * A + B * B) < width && p1.X < px && px < p2.X;
            }
            //vertical
            if (p1.X == p2.X)
            {
                return Math.Abs(A * px + B * pz + C) / Math.Sqrt(A * A + B * B) < width && p2.Z < pz && pz < p1.Z;
            }
            return Math.Abs(A * px + B * pz + C) / Math.Sqrt(A * A + B * B) < width && p1.X < px && px < p2.X && p1.Z < pz && pz < p2.Z;
        }

        //public override string ToString() => $"A: {a}\tB:{b}";
    }
}