using gameOpenTK.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK.models
{
    class Maze : Object
    {
        string maze, floor;
        public Maze(string name) : base(name) 
        {
            maze = $"maze{name}";
            floor = $"floor{name}";
            
            Add(Loader.Instance.LoadFromFile(maze, "maze.obj", ShaderManager.Instance.textures["container"]));
            Add(Loader.Instance.LoadFromFile(floor, "floor.obj", ShaderManager.Instance.textures["metal"]));
        }
    }
}