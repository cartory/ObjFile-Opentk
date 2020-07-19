using gameOpenTK.controllers;
using gameOpenTK.models;
using OpenTK;
using OpenTK.Graphics.ES10;
using OpenTK.Graphics.ES20;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gameOpenTK.models
{
    class Tank : Object
    {
        static float sqr = .3f;
        static float radio = .1f;
        static float width = .17f;

        private int life = 3;
        private int lastTime = 0;

        private float angle = 0;
        private float bangle = 0;
        private float bx = 0, bz = 0;

        private bool endGame = false;

        private string gun, body, bullet;
        public Tank(string name, bool player1) : base(name)
        {
            gun = $"gun{name}";
            body = $"body{name}";
            bullet = $"bullet{name}";
            if (player1)
            {
                Add(Loader.Instance.LoadFromFile(body, "base.obj", ShaderManager.Instance.textures["box"]));
                Add(Loader.Instance.LoadFromFile(gun, "canion.obj", ShaderManager.Instance.textures["tiger"]));
                Add(Loader.Instance.LoadFromFile(bullet, "sphere.obj", ShaderManager.Instance.textures["azul"]));
            }
            else 
            {
                Add(Loader.Instance.LoadFromFile(body, "base.obj", ShaderManager.Instance.textures["container"]));
                Add(Loader.Instance.LoadFromFile(gun, "canion.obj", ShaderManager.Instance.textures["tiger"]));
                Add(Loader.Instance.LoadFromFile(bullet, "sphere.obj", ShaderManager.Instance.textures["azul"]));
            }
        }
        public void Move(Tank tank, bool forward)
        {
            float dz = step * (float)Math.Cos(MathHelper.DegreesToRadians(angle));
            float dx = step * (float)Math.Sin(MathHelper.DegreesToRadians(angle));

            //  forward (-) Z, backward (+)
            if (forward)
            {
                if (Maze.Instance.hitWall(tank, sqr, width, -dx + pos.X, dz + pos.Z)) return;
                Traslate(new Vector3(-dx, 0, dz));
                updatePos(-dx, dz);
            }
            else
            {
                if (Maze.Instance.hitWall(tank, sqr, width, dx + pos.X, -dz + pos.Z)) return;
                Traslate(new Vector3(dx, 0, -dz));
                updatePos(dx, -dz);
            }
            //Console.WriteLine(pos);
        }

        internal void RotateAngle(float angle)
        {
            RotateY(angle);
            this.angle += -angle;
            this.bangle += -angle;
        }

        internal void Shoot(Tank tank)
        {
            float dz = step * (float)Math.Cos(MathHelper.DegreesToRadians(bangle));
            float dx = step * (float)Math.Sin(MathHelper.DegreesToRadians(bangle));
            var pos = parts.Get(bullet).Position;
            if (Math.Abs(DateTime.Now.Second % 10 - lastTime) > 0)
            {
                Console.WriteLine($"{name} SHOOT !!");
                lastTime = DateTime.Now.Second % 10;
                while (!Maze.Instance.hitWall(tank, radio, radio, pos.X, pos.Z))
                {
                    bz += dz;
                    bx += -dx;
                    pos += new Vector3(-dx / 3, 0, dz / 3);
                    parts.Get(bullet).Traslate(new Vector3(-dx / 3, 0, dz / 3));
                }

                if (tank.isAlive())
                {
                    resetBulletPos();
                }
                else 
                {
                    Console.WriteLine($"{name} HAS WON !!!");
                    tank.endGame = endGame = true;
                }
            }
        }

        public void RotateGun(bool left)
        {
            parts.Get(gun).RotateY(left);
            bangle = (bangle + ((left) ? -theta : theta)) % 360;
        }

        public void RotateBody(bool left)
        {
            parts.Get(body).RotateY(left);
            UpdateAngle(left);
        }

        public bool hitTank(float px, float pz)
        { 
            bool h = Math.Abs(pos.X - px) < width && Math.Abs(pos.Z - pz) < width;
            if (h && isAlive()) 
            {
                Console.WriteLine($"{name} life : {life--}");
            }
            return h;
        }

        public bool isAlive() => life > 0;

        public void Rotate(bool left) 
        {
            RotateY(left);
            UpdateAngle(left);
        }
        private void UpdateAngle(bool dir)
        {
            angle = (angle + ((dir) ? -theta : theta)) % 360;
            bangle = (bangle + ((dir) ? -theta : theta)) % 360;
        }

        internal void resetBulletPos()
        {
            parts.Get(bullet).Traslate(new Vector3(-bx / 3, 0, -bz / 3));
            bx = bz = 0;
        }

        internal void reset(bool tank1)
        {
            Console.Clear();
            if (tank1)
            {
                setPos(-2.5f, .5f);
                resetBulletPos();
            }
            else
            {
                setPos(.5f, 2.6f);
                resetBulletPos();
            }
            life = 3;
            endGame = false;
        }
    }
}