using gameOpenTK.common;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.models
{
    public abstract class Volume<T>
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public virtual Hashtable list { get; set; }
        public virtual int VertCount { get; set; }
        public virtual int IndiceCount { get; set; }
        public virtual int ColorDataCount { get; set; }

        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public abstract void Draw(int indiceat = 0);
        public abstract T GetT(String key);
        public abstract void DelT(String key);
        public abstract void SetT(String key, T element);
        public abstract void AddT(String key, T element);
        public abstract Vector3[] GetVerts();
        public abstract Vector3[] GetColorData();
        public abstract void CalculateModelMatrix();
        public abstract int[] GetIndices(int offset = 0);

        public int TextureID;
        public int TextureCoordsCount;
        public bool IsTextured = false;
        public abstract Vector2[] GetTextureCoords();
    }
}
