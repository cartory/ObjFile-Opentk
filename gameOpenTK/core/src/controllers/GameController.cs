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
using System;

namespace gameOpenTK.controllers
{
    class GameController
    {
        #region Singleton
        private GameController() { }
        public static GameController Instance { get => instance; }
        private static GameController instance = new GameController();
        #endregion

        Tank tank, tank2;
        Maze maze;
        Scene scene;
        
        Camera camera;
        Vector2 lastMousePos;
        bool endGame = false;
        void initProgram()
        {
            tank = new Tank("player1", true);
            tank2 = new Tank("player2", false);

            scene = new Scene();
            camera = new Camera();
            maze = Maze.Instance;
            lastMousePos = new Vector2();

            camera.MouseSensitivity = 0.005f;
            camera.Position += new Vector3(0f, 0f, 5f);
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            tank.setScale(.0000001f);
            tank.setPos(-2.5f, .5f);
            tank.RotateAngle(90);

            tank2.setScale(.0000001f);
            tank2.setPos(.5f, 2.6f);
            tank2.RotateAngle(180);

            maze.setScale(.000001f);
            scene.Add(tank);
            scene.Add(tank2);
            scene.Add(maze);
            //scene.Add(tank.bullet);
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
            if (!endGame) 
            {
                endGame = InputController.Instance.ProcessKeyBoard(Keyboard.GetState(), camera, tank, tank2);
            }
            else 
            {
                //Console.WriteLine("PRESS SPACE TO RESTART THE GAME !!");
                if (Keyboard.GetState().IsKeyDown(Key.Space)) 
                {
                    tank.reset(true);
                    tank2.reset(false);
                    endGame = false;
                }
            }
            if (Focused)
            {
                InputController.Instance.UpdateMouseSlide(Mouse.GetState(), camera, ref lastMousePos);
            }

            scene.Update(camera, ClientSize);
        }

        public void OnFocusedChanged(MouseState mouseState) => lastMousePos = new Vector2(mouseState.X, mouseState.Y);
    }
}
