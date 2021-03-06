﻿using OpenTK;
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
    class Scene : Volume<Part>
    {
        HashList<Object> objs;
        int ibo_elements;

        public Scene() 
        {
            objs = new HashList<Object>();
            GL.GenBuffers(1, out ibo_elements);
        }

        public override void Draw(int indiceat = 0)
        {
            ShaderManager.Instance.shaders[shader].EnableVertexAttribArrays();
            foreach (DictionaryEntry e in list)
            {
                Part p = list.Get(e.Key);
                p.Draw(indiceat);
                indiceat += p.IndiceCount;
            }
            ShaderManager.Instance.shaders[shader].DisableVertexAttribArrays();
        }

        public void Add(Object obj) 
        {
            Part[] parts = obj.GetArray();
            foreach (Part part in parts)
            {
                list.Add(part.name, part);
            }
            objs.Add(obj.name, obj);
        }

        public void Set(Object obj) 
        {
            objs.Set(obj.name, obj);
        }

        public override void Update(object camera, Size ClientSize, int vertCount = 0)
        {
            List<int> inds = new List<int>();
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();

            foreach (DictionaryEntry e in list)
            {
                Part p = list.Get(e.Key);
                verts.AddRange(p.GetVerts().ToList());
                inds.AddRange(p.GetIndices(vertCount).ToList());
                colors.AddRange(p.GetColorData().ToList());
                texcoords.AddRange(p.GetTextureCoords());
                vertCount += p.VertCount;
            }

            var coldata = colors.ToArray();
            var vertdata = verts.ToArray();
            var indicedata = inds.ToArray();
            var texcoorddata = texcoords.ToArray();

            var shaders = ShaderManager.Instance.shaders;

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[shader].GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (System.IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(shaders[shader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (shaders[shader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[shader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (System.IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(shaders[shader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Buffer texture coordinates if shader supports it
            if (shaders[shader].GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[shader].GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (System.IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.DynamicDraw);
                GL.VertexAttribPointer(shaders[shader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            foreach (DictionaryEntry e in list)
            {
                list.Get(e.Key).Update(camera, ClientSize);
            }

            GL.UseProgram(shaders[shader].ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (System.IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.DynamicDraw);
        }
    }
}