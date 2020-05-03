using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using gameOpenTK.common;
using System.Drawing;
using gameOpenTK.controllers;

namespace gameOpenTK.models
{
    class Scene : Volume<Object>
    {
        private int ibo_elements;
        private string shader = ShaderManager.Instance.activeShader;
        public Scene() 
        {
            this.list = new Hashtable();
            GL.GenBuffers(1, out ibo_elements);
        }

        public override void DelT(string key) => list.Remove(key);
        public override Object GetT(string key) => (Object)list[key];
        public override void SetT(string key, Object element) => list[key] = element;
        public override void AddT(string key, Object element) => list.Add(key, element);
        public override void Draw(int indiceat = 0)
        {
            ShaderManager.Instance.shaders[shader].EnableVertexAttribArrays();

            foreach (DictionaryEntry entry in list)
            {
                Object v = (Object)entry.Value;
                v.Draw(indiceat);
                indiceat += v.IndiceCount;
            }
            ShaderManager.Instance.shaders[shader].DisableVertexAttribArrays();
        }

        public void Update(Camera cam, Size ClientSize, int vertCount = 0)
        {
            List<int> inds = new List<int>();
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();

            foreach (DictionaryEntry entry in list)
            {
                Object v = (Object)entry.Value;
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertCount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                texcoords.AddRange(v.GetTextureCoords());
                vertCount += v.VertCount;
            }

            var coldata = colors.ToArray();
            var vertdata = verts.ToArray();
            var indicedata = inds.ToArray();
            var texcoorddata = texcoords.ToArray();

            var shaders = ShaderManager.Instance.shaders;

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[shader].GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (System.IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[shader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (shaders[shader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[shader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (System.IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[shader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Buffer texture coordinates if shader supports it
            if (shaders[shader].GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[shader].GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (System.IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[shader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            foreach (DictionaryEntry entry in list)
            {
                Object v = (Object)entry.Value;
                v.Update(cam, ClientSize);
            }

            GL.UseProgram(shaders[shader].ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (System.IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);
        }

        public override Vector3[] GetVerts()
        {
            throw new NotImplementedException();
        }

        public override Vector3[] GetColorData()
        {
            throw new NotImplementedException();
        }

        public override void CalculateModelMatrix()
        {
            throw new NotImplementedException();
        }

        public override int[] GetIndices(int offset = 0)
        {
            throw new NotImplementedException();
        }

        public override Vector2[] GetTextureCoords()
        {
            throw new NotImplementedException();
        }
    }
}
