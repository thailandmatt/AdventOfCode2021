using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        struct Point
        {
            public int X;
            public int Y;

            public override string ToString()
            {
                return $"({X}:{Y})";
            }
        }

        public static void Main(string[] args)
        {
            Day5PartTwo();
        }

        public static void Day5PartOne()
        { 
            var data = File.ReadAllLines("Day5.txt");

            List<(Point, Point)> lines = new List<(Point, Point)>();

            foreach (var line in data)
            {
                var parts = line.Split(new char[] { ',', ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries) ;
                lines.Add((new Point() { X = int.Parse(parts[0]), Y = int.Parse(parts[1]) }, new Point() { X = int.Parse(parts[2]), Y = int.Parse(parts[3]) }));
            }

            //filter only horizontal lines
            lines = lines.Where(one => one.Item1.X == one.Item2.X || one.Item1.Y == one.Item2.Y).ToList();

            //set up map
            var hits = new Dictionary<Point, int>();

            foreach (var line in lines)
            {
                var start = 0;
                var end = 0;

                if (line.Item1.X == line.Item2.X)
                {
                    start = line.Item1.Y > line.Item2.Y ? line.Item2.Y : line.Item1.Y;
                    end = line.Item1.Y < line.Item2.Y ? line.Item2.Y : line.Item1.Y;

                    for (int i = start; i <= end; i++)
                    {
                        var x = new Point() { X = line.Item1.X, Y = i };
                        if (!hits.ContainsKey(x)) hits.Add(x, 0);
                        hits[x]++;
                    }
                } 
                else
                {
                    start = line.Item1.X > line.Item2.X ? line.Item2.X : line.Item1.X;
                    end = line.Item1.X < line.Item2.X ? line.Item2.X : line.Item1.X;

                    for (int i = start; i <= end; i++)
                    {
                        var x = new Point() { X = i, Y = line.Item1.Y };
                        if (!hits.ContainsKey(x)) hits.Add(x, 0);
                        hits[x]++;
                    }
                }
            }

            Console.WriteLine("Answer = " + hits.Where(one => one.Value > 1).Count());
            Console.ReadLine();
        }

        public static void Day5PartTwo()
        {
            var data = File.ReadAllLines("Day5.txt");

            List<(Point, Point)> lines = new List<(Point, Point)>();

            foreach (var line in data)
            {
                var parts = line.Split(new char[] { ',', ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
                lines.Add((new Point() { X = int.Parse(parts[0]), Y = int.Parse(parts[1]) }, new Point() { X = int.Parse(parts[2]), Y = int.Parse(parts[3]) }));
            }

            //set up map
            var hits = new Dictionary<Point, int>();

            foreach (var line in lines)
            {
                var curPoint = line.Item1;

                while (true)
                {
                    if (!hits.ContainsKey(curPoint))
                        hits.Add(curPoint, 0);

                    hits[curPoint]++;

                    if (curPoint.Equals(line.Item2))
                        break;

                    if (line.Item1.Y != line.Item2.Y)                    
                        curPoint.Y = curPoint.Y + (line.Item1.Y < line.Item2.Y ? 1 : -1);
                    
                    if (line.Item1.X != line.Item2.X)
                        curPoint.X = curPoint.X + (line.Item1.X < line.Item2.X ? 1 : -1);
                }
            }

            Console.WriteLine("Answer = " + hits.Where(one => one.Value > 1).Count());
            Console.ReadLine();
        }
    }
    
}

