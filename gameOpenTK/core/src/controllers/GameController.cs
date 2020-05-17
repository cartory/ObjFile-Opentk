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
            Object teddy = Object.LoadFromFile(@"C:\Users\cartory\source\repos\gameOpenTK\gameOpenTK\core\files\objs\teddy.obj");
            teddy.TextureID = ShaderManager.Instance.textures["tiger_tex.jpg"];
            teddy.Scale = .1f;
            scene.AddT("teddy", teddy);
        }

        public void OnLoad()
        {
            initProgram();
            GL.ClearColor(Color.Gray);
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
            // MOVING THE TEDDY BEAR
            if (Keyboard.GetState().IsKeyDown(Key.Z))
            {
                scene.rotateTX("teddy", true);
            }
            if (Keyboard.GetState().IsKeyDown(Key.X))
            {
                scene.rotateTY("teddy", true);
            }
            if (Keyboard.GetState().IsKeyDown(Key.C))
            {
                scene.rotateTZ("teddy", true);
            }
            if (Keyboard.GetState().IsKeyDown(Key.M))
            {
                scene.scaleT("teddy", true);
            }
            if (Keyboard.GetState().IsKeyDown(Key.N))
            {
                scene.scaleT("teddy", false);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Right))
            {
                scene.traslateTX("teddy", true);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Left))
            {
                scene.traslateTX("teddy", false);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Up))
            {
                scene.traslateTY("teddy", true);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Down))
            {
                scene.traslateTY("teddy", false);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Number1))
            {
                scene.traslateTZ("teddy", true);
            }
            if (Keyboard.GetState().IsKeyDown(Key.Number3))
            {
                scene.traslateTZ("teddy", false);
            }
            //  //  //

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
