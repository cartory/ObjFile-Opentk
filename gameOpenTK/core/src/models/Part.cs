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
    class Part : Volume<Vector3>, IGetters, ITransformations, IParamTransformations
    {
        public string name;

        public Vector3[] colors;
        public Vector3[] vertices;
        public Vector2[] texturecoords;

        public Part(string name) => this.name = name;

        public override int VertCount { get => vertices.Length; }
        public override int IndiceCount { get => list.Count * 3; }
        public override int ColorDataCount { get => colors.Length; }

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

        public void UpdateModelMatrix() => ModelMatrix = Matrix4.CreateScale(scale) * matrixRotation * matrixContainer;
        public override void Update(object camera, Size ClientSize, int vertCount = 0)
        {
            Camera cam = (Camera)camera;
            UpdateModelMatrix();
            ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
            ModelViewProjectionMatrix = ModelMatrix * ViewProjectionMatrix;
        }

        #region IGetters
        public int[] GetIndices(int offset = 0)
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
        public Vector3[] GetVerts() => vertices;
        public Vector3[] GetColorData() => colors;
        public Vector2[] GetTextureCoords() => texturecoords;
        #endregion

        #region ITransformations
        public void Scale(bool plus) => scale *= plus ? 1.1f : .9f;
        //  ROTATE
        public void RotateX(bool dir) => RotateX(dir ? theta : -theta);
        public void RotateY(bool dir) => RotateY(dir ? theta : -theta);
        public void RotateZ(bool dir) => RotateZ(dir ? theta : -theta);
        //  TRASLATE
        public void TraslateX(bool dir) => TraslateX(dir ? step : -step);
        public void TraslateY(bool dir) => TraslateY(dir ? step : -step);
        public void TraslateZ(bool dir) => TraslateZ(dir ? step : -step);
        #endregion

        #region IParamTransformations
        public void RotateX(float angle) => matrixRotation *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
        public void RotateY(float angle) => matrixRotation *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
        public void RotateZ(float angle) => matrixRotation *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));

        public void TraslateX(float distance) => matrixContainer *= Matrix4.CreateTranslation(new Vector3(distance, 0, 0));
        public void TraslateY(float distance) => matrixContainer *= Matrix4.CreateTranslation(new Vector3(0, distance, 0));
        public void TraslateZ(float distance) => matrixContainer *= Matrix4.CreateTranslation(new Vector3(0, 0, distance));
        #endregion

    }
}