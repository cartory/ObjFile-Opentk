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
        public bool ProcessKeyBoard(KeyboardState state, Camera camera, Tank tank, Tank tank2) 
        {
            if (state.IsKeyDown(Key.Keypad8))
            {
                camera.Move(0f, 0.1f, 0f);
            }
            if (state.IsKeyDown(Key.Keypad2))
            {
                camera.Move(0f, -0.1f, 0f);
            }
            if (state.IsKeyDown(Key.Keypad4))
            {
                camera.Move(-0.1f, 0f, 0f);
            }

            if (state.IsKeyDown(Key.Keypad6))
            {
                camera.Move(0.1f, 0f, 0f);
            }
            // MOVING THE TANK
            if (state.IsKeyDown(Key.Q))
            {
                tank.RotateGun(left: true);
            }
            if (state.IsKeyDown(Key.ShiftLeft))
            {
                tank.Shoot(tank2);
            }
            if (state.IsKeyDown(Key.E))
            {
                tank.RotateGun(left: false);
            }
            if (state.IsKeyDown(Key.D))
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
            if (state.IsKeyDown(Key.A))
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
            if (state.IsKeyDown(Key.W))
            {
                tank.Move(tank2, forward: true);
            }
            if (state.IsKeyDown(Key.S))
            {
                tank.Move(tank2, forward: false);
            }

            if (state.IsKeyDown(Key.Keypad7))
            {
                camera.Move(0f, 0f, 0.1f);
            }

            if (state.IsKeyDown(Key.Keypad9))
            {
                camera.Move(0f, 0f, -0.1f);
            }

            if (state.IsKeyDown(Key.J)) 
            {
                if (state.IsKeyDown(Key.AltRight))
                {
                    tank2.RotateBody(left: true);
                }
                else 
                {
                    tank2.Rotate(left: true);
                }
            }
            if (state.IsKeyDown(Key.L))
            {
                if (state.IsKeyDown(Key.AltRight))
                {
                    tank2.RotateBody(left: false);
                }
                else 
                {
                    tank2.Rotate(left: false);
                }
            }
            if (state.IsKeyDown(Key.I))
            {
                tank2.Move(tank, forward: true);
            }
            if (state.IsKeyDown(Key.K))
            {
                tank2.Move(tank, forward: false);
            }
            if (state.IsKeyDown(Key.Semicolon))
            {
                tank2.Shoot(tank);
            }
            if (state.IsKeyDown(Key.U))
            {
                tank2.RotateGun(left: true);
            }
            if (state.IsKeyDown(Key.O))
            {
                tank2.RotateGun(left: false);
            }
            return !tank.isAlive() || !tank2.isAlive();
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
