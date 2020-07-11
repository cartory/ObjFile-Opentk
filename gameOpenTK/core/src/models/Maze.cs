using gameOpenTK.common;
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
        #region singleton
        public static Maze Instance { get => instance; }
        private static Maze instance = new Maze("Maze");
        #endregion
        //  -3f...3f => matrix6
        static float size = 3f;
        string maze, floor;
        Segment[] walls;
        public Maze(string name) : base(name)
        {
            maze = $"maze{name}";
            floor = $"floor{name}";
            walls = createWalls();
            Add(Loader.Instance.LoadFromFile(maze, "maze.obj", ShaderManager.Instance.textures["container"]));
            Add(Loader.Instance.LoadFromFile(floor, "floor.obj", ShaderManager.Instance.textures["metal"]));
        }

        public bool contains(float sqr, float dx, float dz)
        {
            return dx + sqr < size && 
                   dx - sqr > -size && 
                   dz + sqr < size && 
                   dz - sqr > -size;
        }

        public bool hitWall(float sqr, float px, float pz)
        {
            foreach (Segment wall in walls)
            {
                if (
                    wall.contains(px + sqr, pz + sqr) || wall.contains(px - sqr, pz - sqr) ||
                    wall.contains(px + sqr, pz - sqr) || wall.contains(px - sqr, pz + sqr) ||

                    wall.contains(px, pz + sqr) || wall.contains(px, pz - sqr) ||
                    wall.contains(px, pz - sqr) || wall.contains(px, pz + sqr)
                )
                {
                    Console.WriteLine("HIT!!!");
                    return true;
                }
            }
            return false;
        }
        private Segment[] createWalls(int type = 0)
        {
            return new Segment[] {
                //RANGE -3..+3
                //HORIZONTAL
                new Segment(new Vector3(-2, 0, -2), new Vector3(+2, 0, -2)),
                new Segment(new Vector3(-1, 0, -1), new Vector3(+0, 0, -1)),
                new Segment(new Vector3(-3, 0, +0), new Vector3(-2, 0, +0)),
                new Segment(new Vector3(-2, 0, +1), new Vector3(+2, 0, +1)),
                new Segment(new Vector3(-1, 0, +2), new Vector3(+1, 0, +2)),
                //VERTICAL
                new Segment(new Vector3(-2, 0, -1), new Vector3(-2, 0, -2)),
                new Segment(new Vector3(-2, 0, +2), new Vector3(-2, 0, +1)),
                new Segment(new Vector3(-1, 0, +3), new Vector3(-1, 0, +2)),
                new Segment(new Vector3(+0, 0, +1), new Vector3(+0, 0, -1)),
                new Segment(new Vector3(+1, 0, +2), new Vector3(+1, 0, -1)),
                new Segment(new Vector3(+2, 0, +3), new Vector3(+2, 0, +2)),
                new Segment(new Vector3(+2, 0, +1), new Vector3(+2, 0, -2)),

            };
        }
    }
}