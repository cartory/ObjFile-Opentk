using gameOpenTK.common;
using gameOpenTK.controllers;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.common
{
    public abstract class HashList<T>
    {
        public Hashtable list = new Hashtable();
        protected T Get(string key) => (T)list[key];
        protected void Del(string key) => list.Remove(key);
        protected void Set(string key, T element) => list[key] = element;
        protected void Add(string key, T element) => list.Add(key, element);
    }
    public abstract class Volume<T> : HashList<T>
    {
        protected string shader = ShaderManager.Instance.activeShader;

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
}
