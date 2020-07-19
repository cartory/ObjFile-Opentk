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
        string maze, floor;
        Segment[] walls;
        public Maze(string name) : base(name)
        {
            maze = $"maze{name}";
            floor = $"floor{name}";
            walls = this.createWalls();
            this.sortWallsByDistance();

            Add(Loader.Instance.LoadFromFile(floor, "floor.obj", ShaderManager.Instance.textures["metal"]));
            Add(Loader.Instance.LoadFromFile(maze, "maze.obj", ShaderManager.Instance.textures["container"]));
        }

        private bool hit(Segment wall, float sqr, float width, float px, float pz) 
        {
            return
                wall.contains(px + sqr, pz + width) || wall.contains(px - sqr, pz - width) ||
                wall.contains(px + sqr, pz - width) || wall.contains(px - sqr, pz + width);
                //wall.contains(px, pz + width) || wall.contains(px + sqr, pz) ||
                //wall.contains(px, pz - width) || wall.contains(px - sqr, pz);
        }

        private bool hitWall(int a, int b, float sqr, float width, float px, float pz)
        {
            int m = (a + b) / 2;
            float d = px * px + pz * pz;
            if (d > walls[m].d)
            {
                if (m < b)
                {
                    if (d < walls[m + 1].d)
                    {
                        return hit(walls[m], sqr, width, px, pz) || hit(walls[m + 1], sqr, width, px, pz);
                    }
                    return hitWall(m + 1, b, sqr, width, px, pz);
                }
            }
            else 
            {
                if (m > a) 
                {
                    if (d > walls[m - 1].d) 
                    {
                        return hit(walls[m - 1], sqr, width, px, pz) || hit(walls[m], sqr, width, px, pz);
                    }
                    return hitWall(a, m - 1, sqr, width, px, pz);
                }
            }
            return hit(walls[m], sqr, width, px, pz);
        }

        private bool hitTank(Tank tank, float sqr, float width, float px, float pz, bool isBullet)
        {
            return
                tank.hitTank(px + sqr, pz + width, isBullet) || tank.hitTank(px + sqr, pz - width, isBullet) ||
                tank.hitTank(px - sqr, pz + width, isBullet) || tank.hitTank(px - sqr, pz - width, isBullet);
        }

        public bool hitWall(Tank tank, float sqr, float width, float px, float pz, bool isBullet)
        {
            if (!hitTank(tank, sqr, width, px, pz, isBullet)) 
            {
                foreach (Segment wall in walls)
                {
                    if (hit(wall, sqr, width, px, pz))
                    {
                        return true;
                    }
                }
                return false;
            }
            //Console.WriteLine("TANK DAMAGED!!!");
            return true;
        }

        private void sortWallsByDistance() 
        {
            int n = walls.Length;
            for (int i = 0; i < n - 1; i++) 
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (walls[j].d > walls[j + 1].d) 
                    {
                        var tmp = walls[j];
                        walls[j] = walls[j + 1];
                        walls[j + 1] = tmp;
                    }
                }
            }
        }

        private Segment[] createWalls(int type = 0)
        {
            return new Segment[] {
                //RANGE -3..+3
                //HORIZONTAL
                new Segment(new Vector3(-3, 0, -3), new Vector3(+3, 0, -3)),
                new Segment(new Vector3(-3, 0, +3), new Vector3(+3, 0, +3)),

                new Segment(new Vector3(-2, 0, -2), new Vector3(+2, 0, -2)),
                new Segment(new Vector3(-1, 0, -1), new Vector3(+0, 0, -1)),
                new Segment(new Vector3(-3, 0, +0), new Vector3(-2, 0, +0)),
                new Segment(new Vector3(-2, 0, +1), new Vector3(+2, 0, +1)),
                new Segment(new Vector3(-1, 0, +2), new Vector3(+1, 0, +2)),
                //VERTICAL
                new Segment(new Vector3(-3, 0, +3), new Vector3(-3, 0, -3)),
                new Segment(new Vector3(+3, 0, +3), new Vector3(+3, 0, -3)),

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