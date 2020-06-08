using gameOpenTK.models;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.controllers
{
    class InputController
    {
        #region singleton
        private InputController() { }
        private static InputController instance = new InputController();
        public static InputController Instance { get => instance; }
        #endregion
        public void ProcessKeyBoard(KeyboardState state, Camera camera, Tank tank) 
        {
            if (state.IsKeyDown(Key.W))
            {
                camera.Move(0f, 0.1f, 0f);
            }
            if (state.IsKeyDown(Key.S))
            {
                camera.Move(0f, -0.1f, 0f);
            }
            if (state.IsKeyDown(Key.A))
            {
                camera.Move(-0.1f, 0f, 0f);
            }

            if (state.IsKeyDown(Key.D))
            {
                camera.Move(0.1f, 0f, 0f);
            }
            // MOVING THE TANK
            if (state.IsKeyDown(Key.Z))
            {
                tank.RotateGun(left: true);
            }
            if (state.IsKeyDown(Key.X))
            {
                tank.Shoot();
            }
            if (state.IsKeyDown(Key.C))
            {
                tank.RotateGun(left: false);
            }
            if (state.IsKeyDown(Key.Right))
            {
                if (state.IsKeyDown(Key.ControlLeft))
                {
                    tank.RotateBody(left: false);
                }
                else 
                { 
                    tank.Rotate(left: false);
                }

            }
            if (state.IsKeyDown(Key.Left))
            {
                if (state.IsKeyDown(Key.ControlLeft))
                {
                    tank.RotateBody(left: true);
                }
                else
                {
                    tank.Rotate(left: true);
                }
            }
            if (state.IsKeyDown(Key.Up))
            {
                tank.Move(forward: true);
            }
            if (state.IsKeyDown(Key.Down))
            {
                tank.Move(forward: false);
            }

            if (state.IsKeyDown(Key.Q))
            {
                camera.Move(0f, 0f, 0.1f);
            }

            if (state.IsKeyDown(Key.E))
            {
                camera.Move(0f, 0f, -0.1f);
            }
        }

        internal void UpdateMouseSlide(MouseState mouseState, Camera camera, ref Vector2 lastMousePos)
        {
            Vector2 delta = lastMousePos - new Vector2(mouseState.X, mouseState.Y);
            //lastMousePos += delta;

            camera.AddRotation(delta.X, delta.Y);
            lastMousePos = new Vector2(mouseState.X, mouseState.Y);
        }
    }
}
