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
using System.Threading;

namespace gameOpenTK.controllers
{
    class GameController
    {
        #region Singleton
        private GameController() { }
        public static GameController Instance { get => instance; }
        private static GameController instance = new GameController();
        #endregion

        Tank tank;
        Maze maze;
        Scene scene;
        Camera camera;
        Vector2 lastMousePos;

        void initProgram()
        {
            tank = new Tank("player1");
            scene = new Scene();
            camera = new Camera();
            maze = new Maze("maze");
            lastMousePos = new Vector2();

            camera.MouseSensitivity = 0.005f;
            camera.Position += new Vector3(0f, 0f, 5f);
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            tank.setScale(.0000001f);
            maze.setScale(.000001f);

            scene.Add(tank);
            scene.Add(maze);
        }

        public void OnLoad()
        {
            initProgram();
            GL.ClearColor(Color.Gray);
            GL.PointSize(3f);
        }
        public void OnRenderFrame(int Width, int Height)
        {
            //GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            scene.Draw();
            GL.Flush();
        }

        public void OnUpdateFrame(Size ClientSize, bool Focused)
        {
            InputController.Instance.ProcessKeyBoard(Keyboard.GetState(), camera, tank);
            if (Focused)
            {
                InputController.Instance.UpdateMouseSlide(Mouse.GetState(), camera, ref lastMousePos);
            }
            scene.Update(camera, ClientSize);
        }

        public void OnFocusedChanged(MouseState mouseState) => lastMousePos = new Vector2(mouseState.X, mouseState.Y);
    }
}
