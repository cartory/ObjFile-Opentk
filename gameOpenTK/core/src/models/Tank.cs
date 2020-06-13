using gameOpenTK.controllers;
using gameOpenTK.models;
using OpenTK;
using OpenTK.Graphics.ES10;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.models
{
    class Tank: Object
    {
        static float sqr = .3f;
        private float angle = 0;
        private string gun, body;
        public Tank(string name) : base(name)
        {
            gun = $"gun{name}";
            body = $"body{name}";
            Add(Loader.Instance.LoadFromFile(body, "base.obj", ShaderManager.Instance.textures["box"]));
            Add(Loader.Instance.LoadFromFile(gun, "canion.obj", ShaderManager.Instance.textures["tiger"]));
        }
        public void Move(bool forward)
        {
            float dz = step * (float)Math.Cos(MathHelper.DegreesToRadians(angle));
            float dx = step * (float)Math.Sin(MathHelper.DegreesToRadians(angle));

            //  forward (-) Z, backward (+)
            if (forward)
            {
                if (!Maze.contains(sqr, -dx, dz, position)) return;
                TraslateZ(dz);
                TraslateX(-dx);
                this.position += new Vector3(-dx, 0, dz);
            }
            else 
            {
                if (!Maze.contains(sqr, dx, -dz, position)) return;
                TraslateZ(-dz);
                TraslateX(dx);
                this.position += new Vector3(dx, 0, -dz);
            }
            Console.WriteLine(position);
        }

        internal void Shoot() => Console.WriteLine("SHOOT!!");
        public void RotateGun(bool left) => parts.Get(gun).RotateY(left);
        public void RotateBody(bool left) 
        {
            parts.Get(body).RotateY(left);
            UpdateAngle(left);
            Console.WriteLine(angle);
        }

        public void Rotate(bool left) 
        {
            RotateY(left);
            UpdateAngle(left);
            Console.WriteLine(angle);
        }
        private void UpdateAngle(bool dir) => angle = (angle + ((dir)? -theta: theta)) % 360;
    }
}