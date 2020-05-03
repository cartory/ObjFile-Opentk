using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using gameOpenTK.controllers;
using gameOpenTK.common;

namespace gameOpenTK
{
    public class Game : GameWindow
    {
        public Game(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CursorVisible = false;
            GameController.Instance.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GameController.Instance.OnRenderFrame(e, Width, Height);
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
            {
                Exit();
            }
            GameController.Instance.OnUpdateFrame(e, ClientSize, Focused);
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            GameController.Instance.OnFocusedChanged();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            ShaderManager.Instance.deleteShaders();
            base.OnClosed(e);
        }
    }
}