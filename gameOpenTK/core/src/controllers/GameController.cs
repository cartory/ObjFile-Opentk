using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using gameOpenTK.common;
using gameOpenTK.models;

namespace gameOpenTK.controllers
{
    class GameController
    {
        #region Singleton
        private static GameController instance;
        public static GameController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameController();
                }
                return instance;
            }
        }
        #endregion

        Camera cam = new Camera();
        Scene scene = new Scene();

        Vector2 lastMousePos = new Vector2();

        void initProgram()
        {
            cam.MouseSensitivity = 0.0025f;
            cam.Position += new Vector3(0f, 0f, 5f);
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            Object cow = Object.LoadFromFile(@"C:\Users\cartory\source\repos\appOpentk\appOpentk\core\objs\cow.obj");
            Object teapot = Object.LoadFromFile(@"C:\Users\cartory\source\repos\appOpentk\appOpentk\core\objs\teapot.obj");
            //Object pumpkin = Object.LoadFromFile(@"C:\Users\cartory\source\repos\appOpentk\appOpentk\core\objs\pumpkin.obj");

            teapot.TextureID = ShaderManager.Instance.textures["azul_tex.jpg"];
            teapot.Position = new Vector3(5f, -1, 3);

            scene.AddT("cow", cow);
            scene.AddT("teapot", teapot);
            //scene.AddT("pumpkin", pumpkin);
        }

        public void OnLoad()
        {
            initProgram();
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(3f);
        }
        public void OnRenderFrame(FrameEventArgs e, int Width, int Height)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            scene.Draw();
            GL.Flush();
        }

        public void OnUpdateFrame(FrameEventArgs e, Size ClientSize, bool Focused)
        {
            ProcessInput(Focused);
            scene.Update(cam, ClientSize);
        }

        private void ProcessInput(bool Focused)
        {
            if (Keyboard.GetState().IsKeyDown(Key.W))
            {
                cam.Move(0f, 0.1f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Key.S))
            {
                cam.Move(0f, -0.1f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Key.A))
            {
                cam.Move(-0.1f, 0f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Key.D))
            {
                cam.Move(0.1f, 0f, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Key.Q))
            {
                cam.Move(0f, 0f, 0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Key.E))
            {
                cam.Move(0f, 0f, -0.1f);
            }

            if (Focused)
            {
                Vector2 delta = lastMousePos - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                lastMousePos += delta;

                cam.AddRotation(delta.X, delta.Y);
                lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        public void OnFocusedChanged() => lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
    }
}
