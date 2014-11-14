using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTerra
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Launching game...");
            new Game(new Vector2(640, 480)).Run(20, 50);
        }
    }
}