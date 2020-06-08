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
            float distanceZ = step * (float)Math.Cos(MathHelper.DegreesToRadians(angle));
            float distanceX = step * (float)Math.Sin(MathHelper.DegreesToRadians(angle));
            
            //  forward (-) Z, backward (+) Z
            if (forward)
            {
                TraslateZ(-distanceZ);
                TraslateX(distanceX);
            }
            else 
            {
                TraslateZ(distanceZ);
                TraslateX(-distanceX);
            }
        }

        internal void Shoot() => Console.WriteLine("SHOOT!!");

        public void RotateGun(bool left) => RotateYChild(gun, left);
        public void RotateBody(bool left) 
        { 
            RotateYChild(body, left);
            UpdateAngle(left);
            Console.WriteLine(angle);
        }

        public void TurnAround() => RotateY(180);
        public void Rotate(bool left) 
        {
            RotateY(left);
            UpdateAngle(left);
            Console.WriteLine(angle);
        }
        private void UpdateAngle(bool dir) => angle = (angle + ((dir)? -theta: theta)) % 360;
    }
}