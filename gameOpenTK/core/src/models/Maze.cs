using gameOpenTK.controllers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.models
{
    class Maze : Object
    {
        static float limit = 3f;

        string maze, floor;
        public Maze(string name) : base(name) 
        {
            maze = $"maze{name}";
            floor = $"floor{name}";
            
            Add(Loader.Instance.LoadFromFile(maze, "maze.obj", ShaderManager.Instance.textures["container"]));
            Add(Loader.Instance.LoadFromFile(floor, "floor.obj", ShaderManager.Instance.textures["metal"]));
        }

        public static bool contains(float sqr, float dx, float dz, Vector3 pos)
        {
            return pos.X + dx + sqr < limit  && pos.X + dx - sqr > -limit 
                && pos.Z + dz + sqr < limit  && pos.Z + dz - sqr > -limit;
        }
    }
}