using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameOpenTK
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1024, 720, "GAME OPENTK"))
            {
                game.Run(30.0, 30.0);
            }
        }
    }
}
