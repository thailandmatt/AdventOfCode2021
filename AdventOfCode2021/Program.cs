using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            Day15PartTwo();
        }

        #region Day15

        public static void Day15PartOne()
        {
            var lines = System.IO.File.ReadAllLines(@"Day15.txt");
            var grid = lines.Select(one => one.Select(two => int.Parse(two.ToString())).ToArray()).ToArray();
            var cols = grid[0].Length;
            var rows = grid.Length;
            var newgrid = new int[rows, cols];

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    newgrid[row, col] = grid[row][col];
                }
            }

            Day15DijjkstrasPartOne(newgrid);
        }

        public static void Day15DijjkstrasPartOne(int[,] grid)
        {
            //do dijkstras shortest path
            var cols = grid.GetUpperBound(1) + 1;
            var rows = grid.GetUpperBound(0) + 1;

            //i'm not going to do a full priority queue and hope its fast enough
            var distance = new int[rows, cols];
            var parent = new (int, int)[rows, cols];
            var remainingNodes = new List<(int, int)>();

            //initialize distance
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    distance[row, col] = int.MaxValue;
                    remainingNodes.Add((row, col));
                }
            }

            distance[0, 0] = 0;

            //do dijkstras
            while (remainingNodes.Count > 0)
            {
                //find the smallest one in queue
                (int, int) minNode = (-1, -1);
                var min = int.MaxValue;

                foreach (var x in remainingNodes)
                {
                    if (distance[x.Item1, x.Item2] < min)
                    {
                        min = distance[x.Item1, x.Item2];
                        minNode = x;
                    }
                }

                remainingNodes.Remove(minNode);

                //ok now go add update distance for all the neighbors
                if (minNode.Item1 > 0)
                {
                    //can go up
                    if (distance[minNode.Item1 - 1, minNode.Item2] > grid[minNode.Item1 - 1, minNode.Item2] + min)
                    {
                        distance[minNode.Item1 - 1, minNode.Item2] = grid[minNode.Item1 - 1, minNode.Item2] + min;
                        parent[minNode.Item1 - 1, minNode.Item2] = minNode;
                    }
                }

                if (minNode.Item2 > 0)
                {
                    //can go left
                    if (distance[minNode.Item1, minNode.Item2 - 1] > grid[minNode.Item1, minNode.Item2 - 1] + min)
                    {
                        distance[minNode.Item1, minNode.Item2 - 1] = grid[minNode.Item1, minNode.Item2 - 1] + min;
                        parent[minNode.Item1, minNode.Item2 - 1] = minNode;
                    }
                }

                if (minNode.Item1 < rows - 1)
                {
                    //can go down
                    if (distance[minNode.Item1 + 1, minNode.Item2] > grid[minNode.Item1 + 1, minNode.Item2] + min)
                    {
                        distance[minNode.Item1 + 1, minNode.Item2] = grid[minNode.Item1 + 1, minNode.Item2] + min;
                        parent[minNode.Item1 + 1, minNode.Item2] = minNode;
                    }
                }

                if (minNode.Item2 < cols - 1)
                {
                    //can go right
                    if (distance[minNode.Item1, minNode.Item2 + 1] > grid[minNode.Item1, minNode.Item2 + 1] + min)
                    {
                        distance[minNode.Item1, minNode.Item2 + 1] = grid[minNode.Item1, minNode.Item2 + 1] + min;
                        parent[minNode.Item1, minNode.Item2 + 1] = minNode;
                    }
                }
            }

            //now trace shortest path from the bottom left corner
            Console.WriteLine("Total Cost = " + distance[rows - 1, cols - 1]);
        }

        public static void Day15PartTwo()
        {
            var lines = System.IO.File.ReadAllLines(@"Day15.txt");
            var grid = lines.Select(one => one.Select(two => int.Parse(two.ToString())).ToArray()).ToArray();
            var cols = grid[0].Length;
            var rows = grid.Length;

            var newGrid = new int[rows * 5, cols * 5];

            //Console.Write("\n");
            for (var row = 0; row < rows * 5; row++)
            {
                for (var col = 0; col < cols * 5; col++)
                {
                    var rowDiv = row / rows;
                    var rowMod = row % rows;
                    var colDiv = col / cols;
                    var colMod = col % cols;

                    var newVal = grid[rowMod][colMod] + rowDiv + colDiv;
                    if (newVal > 9) newVal = newVal - 9;
                    newGrid[row, col] =  newVal;
                    //Console.Write(newVal);
                }
                //Console.Write("\n");
            }

            //duplicate this 5 times in each direction
            Day15DijjkstrasPartTwo(newGrid);
        }

        public static void Day15DijjkstrasPartTwo(int[,] grid)
        {
            //do dijkstras shortest path
            var cols = grid.GetUpperBound(1) + 1;
            var rows = grid.GetUpperBound(0) + 1;
            
            //have to do a queue - but don't have to update priority because of the nature of the problem
            var distance = new int[rows, cols];
            var parent = new (int, int)[rows, cols];
            var queue = new PriorityQueue<(int, int), int>();

            //initialize distance
            //initialize distance
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    distance[row, col] = int.MaxValue;             
                }
            }
            distance[0, 0] = 0;

            queue.Enqueue((0, 0), 0);

            //do dijkstras
            while (queue.Count > 0)
            {
                //find the smallest one in queue
                var minNode = queue.Dequeue();
                var min = distance[minNode.Item1, minNode.Item2];

                //ok now go add update distance for all the neighbors
                if (minNode.Item1 > 0)
                {
                    
                    //can go up
                    if (distance[minNode.Item1 - 1, minNode.Item2] > grid[minNode.Item1 - 1, minNode.Item2] + min)
                    {
                        distance[minNode.Item1 - 1, minNode.Item2] = grid[minNode.Item1 - 1, minNode.Item2] + min;
                        parent[minNode.Item1 - 1, minNode.Item2] = minNode;
                        queue.Enqueue((minNode.Item1 - 1, minNode.Item2), distance[minNode.Item1 - 1, minNode.Item2]);
                    }
                    
                }

                if (minNode.Item2 > 0)
                {
                    //can go left
                    if (distance[minNode.Item1, minNode.Item2 - 1] > grid[minNode.Item1, minNode.Item2 - 1] + min)
                    {
                        distance[minNode.Item1, minNode.Item2 - 1] = grid[minNode.Item1, minNode.Item2 - 1] + min;
                        parent[minNode.Item1, minNode.Item2 - 1] = minNode;
                        queue.Enqueue((minNode.Item1, minNode.Item2 - 1), distance[minNode.Item1, minNode.Item2 - 1]);
                    }
                }

                if (minNode.Item1 < rows - 1)
                {
                    //can go down
                    if (distance[minNode.Item1 + 1, minNode.Item2] > grid[minNode.Item1 + 1, minNode.Item2] + min)
                    {
                        distance[minNode.Item1 + 1, minNode.Item2] = grid[minNode.Item1 + 1, minNode.Item2] + min;
                        parent[minNode.Item1 + 1, minNode.Item2] = minNode;
                        queue.Enqueue((minNode.Item1 + 1, minNode.Item2), distance[minNode.Item1 + 1, minNode.Item2]);
                    }
                }

                if (minNode.Item2 < cols - 1)
                {
                    //can go right
                    if (distance[minNode.Item1, minNode.Item2 + 1] > grid[minNode.Item1, minNode.Item2 + 1] + min)
                    {
                        distance[minNode.Item1, minNode.Item2 + 1] = grid[minNode.Item1, minNode.Item2 + 1] + min;
                        parent[minNode.Item1, minNode.Item2 + 1] = minNode;
                        queue.Enqueue((minNode.Item1, minNode.Item2 + 1), distance[minNode.Item1, minNode.Item2 + 1]);
                    }
                }
            }

            //now trace shortest path from the bottom left corner
            Console.WriteLine("Total Cost = " + distance[rows - 1, cols - 1]);
        }

        #endregion

        #region Day14

        public static void Day14PartOne()
        {
            //parse
            var lines = System.IO.File.ReadAllLines("Day14.txt").ToList();
            string start = lines[0];

            lines.RemoveAt(0);
            lines.RemoveAt(0);

            Dictionary<string, string> rules = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                rules.Add(line.Substring(0, 2), line.Substring(6, 1));
            }

            List<char> polymer = new List<char>(start.ToArray());

            for (int i = 0; i < 10; i++)
            {
                List<char> next = new List<char>();
                for (int j = 0; j < polymer.Count - 1; j++)
                {
                    next.Add(polymer[j]);
                    var key = polymer[j].ToString() + polymer[j + 1].ToString();
                    if (rules.ContainsKey(key))
                    {
                        next.Add(rules[key][0]);
                    }
                }
                next.Add(polymer[polymer.Count - 1]);
                string x = String.Concat(next);
                polymer = next;
            }

            //group by
            var groups = polymer.GroupBy(one => one).OrderByDescending(one => one.Count()).ToList();
            Console.WriteLine("Most common = " + groups[0].Key + " count " + groups[0].Count());
            Console.WriteLine("Least common = " + groups[groups.Count - 1].Key + " count " + groups[groups.Count - 1].Count());
            Console.WriteLine("Answer = " + (groups[0].Count() - groups[groups.Count - 1].Count()));
        }

        public static void Day14PartTwo()
        {
            //parse
            var lines = System.IO.File.ReadAllLines("Day14.txt").ToList();
            string start = lines[0];

            lines.RemoveAt(0);
            lines.RemoveAt(0);

            Dictionary<string, string> rules = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                rules.Add(line.Substring(0, 2), line.Substring(6, 1));
            }

            //a different way to think about this
            //we can count how many of each two letter combo we have
            //and then just add counts to a dictionary instead
            Dictionary<string, Int64> keyValuePairs = new Dictionary<string, Int64>();

            for (int i = 0; i < start.Length - 1; i++)
            {
                if (!keyValuePairs.ContainsKey(start.Substring(i, 2)))
                    keyValuePairs.Add(start.Substring(i, 2), 0);

                keyValuePairs[start.Substring(i, 2)]++;
            }

            for (int i = 0; i < 40; i++)
            {
                Dictionary<string, Int64> newKV = new Dictionary<string, long>();
                foreach (var key in keyValuePairs.Keys)
                {
                    if (rules.ContainsKey(key))
                    {
                        var k1 = key[0] + rules[key];
                        var k2 = rules[key] + key[1];

                        if (!newKV.ContainsKey(k1))
                            newKV.Add(k1, 0);

                        if (!newKV.ContainsKey(k2))
                            newKV.Add(k2, 0);

                        newKV[k1] += keyValuePairs[key];
                        newKV[k2] += keyValuePairs[key];
                    }
                }
                keyValuePairs = newKV;
            }

            //add up
            Dictionary<char, Int64> letters = new Dictionary<char, Int64>();
            foreach (string key in keyValuePairs.Keys)
            {
                if (!letters.ContainsKey(key[0])) letters.Add(key[0], 0);
                letters[key[0]] += keyValuePairs[key];
            }
            letters[start[start.Length - 1]]++;
            var max = letters.MaxBy(one => one.Value);
            var min = letters.MinBy(one => one.Value);

            Console.WriteLine("Most common = " + max.Key + " count " + max.Value);
            Console.WriteLine("Least common = " + min.Key + " count " + min.Value);
            Console.WriteLine("Answer = " + (max.Value - min.Value));
        }

        #endregion

        #region Day13

        public static void Day13PartOne()
        {
            //parse input
            string[] lines = System.IO.File.ReadAllLines("Day13.txt");
            List<Point> points = new List<Point>();
            List<Point> folds = new List<Point>();

            foreach (string line in lines)
            {
                if (line != "" && !line.StartsWith("fold along"))
                {
                    var segments = line.Split(',');
                    points.Add(new Point() { X = int.Parse(segments[0]), Y = int.Parse(segments[1]) });
                }
                else if (line.StartsWith("fold along x="))
                {
                    folds.Add(new Point() { X = int.Parse(line.Replace("fold along x=", "")), Y = 0 });
                }
                else if (line.StartsWith("fold along y="))
                {
                    folds.Add(new Point() { Y = int.Parse(line.Replace("fold along y=", "")), X = 0 });
                }
            }

            points = points.OrderBy(one => one.Y).ThenBy(one => one.X).ToList();

            foreach (var fold in folds)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var p = points[i];
                    if (p.X > fold.X && fold.X > 0) p.X = p.X - ((p.X - fold.X) * 2);
                    if (p.Y > fold.Y && fold.Y > 0) p.Y = p.Y - ((p.Y - fold.Y) * 2);
                    points[i] = p;
                }

                //part 1
                //break;
            }

            //part 1
            //points = points.GroupBy(one => one.ToString()).Select(one => one.First()).ToList();
            //Console.WriteLine("Number of points = " + points.Count());

            //print it out for part 2

            for (var y = 0; y <= points.Max(one => one.Y); y++)
            {
                for (var x = 0; x <= points.Max(one => one.X); x++)
                {
                    var test = points.Find(one => one.X == x && one.Y == y);
                    if (test != null)
                        Console.Write("X");
                    else
                        Console.Write(" ");
                }
                Console.Write("\n");
            }
        }

        #endregion

        #region Day12

        public static void Day12PartOne()
        {
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();

            //parse input
            string[] lines = System.IO.File.ReadAllLines("Day12.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('-');
                if (!map.ContainsKey(parts[0]))
                    map.Add(parts[0], new List<string>());
                if (!map.ContainsKey(parts[1]))
                    map.Add(parts[1], new List<string>());

                if (!map[parts[0]].Contains(parts[1]))
                    map[parts[0]].Add(parts[1]);


                if (!map[parts[1]].Contains(parts[0]))
                    map[parts[1]].Add(parts[0]);
            }

            //DFS from start - only mark "small" caves (lowercase) as visited
            //and pray there isn't a loop of large caves or else it will go on forever
            List<List<string>> paths = DFSFromNode("start", map, new List<string>());
            List<string> codedPaths = new List<string>();
            foreach (var path in paths)
            {
                codedPaths.Add(String.Join("-", path));
            }

            codedPaths = codedPaths.Distinct().ToList();

            Console.WriteLine("Paths = " + codedPaths.Count);
        }

        public static List<List<string>> DFSFromNode(string nodeName, Dictionary<string, List<string>> map, List<string> pathSoFar)
        {
            List<List<string>> paths = new List<List<string>>();

            pathSoFar.Add(nodeName);
            bool hasDoubleSmallCave = false;
            if (pathSoFar.Where(one => char.IsLower(one[0])).GroupBy(one => one).Where(one => one.Count() > 1).Count() > 0)
                hasDoubleSmallCave = true;

            var nodeEndpoints = map[nodeName];
            foreach (var other in nodeEndpoints)
            {
                if (other == "start")
                    continue;
                else if (other == "end")
                {
                    pathSoFar.Add("end");
                    paths.Add(new List<string>(pathSoFar));
                    pathSoFar.RemoveAt(pathSoFar.Count - 1);
                }
                else if (!pathSoFar.Contains(other))
                {
                    paths.AddRange(DFSFromNode(other, map, pathSoFar));
                }
                else
                {
                    if (char.IsUpper(other[0]))
                        paths.AddRange(DFSFromNode(other, map, pathSoFar));

                    //part 2
                    if (pathSoFar.Count(one => one == other) == 1 && !hasDoubleSmallCave)
                        paths.AddRange(DFSFromNode(other, map, pathSoFar));
                }
            }

            pathSoFar.RemoveAt(pathSoFar.Count - 1);
            return paths;
        }

        #endregion

        #region Day11

        public class Grid: List<List<int>>
        {
            public override string ToString()
            {
                string s = "";
                foreach (var x in this)
                {
                    s += String.Concat(x) + "\n";
                }
                return s.Trim();
            }
        }

        public static void Day11PartOne()
        {
            string[] lines = System.IO.File.ReadAllLines("Day11.txt");
            Grid grid = new Grid();
            foreach (string line in lines)
            {
                List<int> t = new List<int>(line.ToCharArray().Select(one => int.Parse(one.ToString())));
                grid.Add(t);
            }

            int flashes = 0;

            for (int i = 0; i < 1000; i++)
            {
                //increment by 1
                foreach (List<int> line in grid)
                {
                    for (int x = 0; x < line.Count; x++)
                    {
                        line[x]++;
                    }
                }

                while (true)
                {
                    bool didFlash = false;

                    for (int y = 0; y < grid.Count; y++)
                    {
                        var line = grid[y];
                        for (int x = 0; x < line.Count; x++)
                        {
                            if (line[x] > 9 && line[x] < 100)
                            {
                                //flash
                                flashes++;
                                didFlash = true;
                                if (x > 0 && y > 0) grid[y - 1][x - 1]++;
                                if (y > 0) grid[y - 1][x]++;
                                if (x < line.Count - 1&& y > 0) grid[y - 1][x + 1]++;
                                if (x > 0) line[x - 1]++;
                                if (x < line.Count - 1) line[x + 1]++;
                                if (x > 0 && y < grid.Count - 1) grid[y + 1][x - 1]++;
                                if (y < grid.Count - 1) grid[y + 1][x]++;
                                if (x < line.Count - 1 && y < grid.Count - 1) grid[y + 1][x + 1]++;
                                line[x] = 100;
                            }
                        }
                    }

                    if (!didFlash) break;
                }

                foreach (List<int> line in grid)
                {
                    for (int x = 0; x < line.Count; x++)
                    {
                        if (line[x] >= 100) line[x] = 0;
                    }
                }

                //part 2
                bool AllZero = !grid.Any(one => one.Any(two => two != 0));
                if (AllZero)
                {
                    Console.WriteLine("Part 2: " + (i + 1).ToString());
                    return;
                }
            }
                        
            Console.WriteLine("Part 1: " + flashes);
        }

        #endregion

        #region Day10

        public static void Day10PartOne()
        {
            string[] lines = System.IO.File.ReadAllLines("Day10.txt");
            Stack<char> stack = new Stack<char>();
            List<char> illegals = new List<char>();
            foreach (string line in lines)
            {
                stack.Clear();

                foreach (char c in line)
                {
                    if (c == '(' || c == '[' || c == '{' || c == '<')
                    {
                        stack.Push(c);
                    }
                    else if (c == ')')
                    {
                        char next = stack.Pop();
                        if (next != '(')
                        {
                            illegals.Add(c);
                            break;
                        }
                    }
                    else if (c == '}')
                    {
                        char next = stack.Pop();
                        if (next != '{')
                        {
                            illegals.Add(c);
                            break;
                        }
                    }
                    else if (c == ']')
                    {
                        char next = stack.Pop();
                        if (next != '[')
                        {
                            illegals.Add(c);
                            break;
                        }
                    }
                    else if (c == '>')
                    {
                        char next = stack.Pop();
                        if (next != '<')
                        {
                            illegals.Add(c);
                            break;
                        }
                    }
                }
            }

            int score = illegals.Select(one => one == ')' ? 3 : one == ']' ? 57 : one == '}' ? 1197 : one == '>' ? 25137 : 0).Sum();
            Console.WriteLine("Score: " + score);
        }

        public static void Day10PartTwo()
        {
            string[] lines = System.IO.File.ReadAllLines("Day10.txt");
            Stack<char> stack = new Stack<char>();
            List<string> finishes = new List<string>();
            foreach (string line in lines)
            {
                bool isIllegal = false;
                stack.Clear();

                foreach (char c in line)
                {
                    if (c == '(' || c == '[' || c == '{' || c == '<')
                    {
                        stack.Push(c);
                    }
                    else if (c == ')')
                    {
                        char next = stack.Pop();
                        if (next != '(')
                        {
                            isIllegal = true;
                            break;
                        }
                    }
                    else if (c == '}')
                    {
                        char next = stack.Pop();
                        if (next != '{')
                        {
                            isIllegal = true;
                            break;
                        }
                    }
                    else if (c == ']')
                    {
                        char next = stack.Pop();
                        if (next != '[')
                        {
                            isIllegal = true;
                            break;
                        }
                    }
                    else if (c == '>')
                    {
                        char next = stack.Pop();
                        if (next != '<')
                        {
                            isIllegal = true;
                            break;
                        }
                    }
                }

                if (!isIllegal)
                {
                    //incomplete
                    string finish = "";
                    while (stack.Count > 0)
                    {
                        var x = stack.Pop();
                        finish += (x == '<' ? '>' : x == '[' ? ']' : x == '{' ? '}' : x == '(' ? ')' : ' ').ToString();
                    }
                    finishes.Add(finish);
                }
            }

            List<Int64> scores = new List<Int64>();
            foreach (string finish in finishes)
            {
                Int64 score = 0;
                foreach (char c in finish)
                {
                    score *= 5;
                    score += (c == ')' ? 1 : c == ']' ? 2 : c == '}' ? 3 : c == '>' ? 4 : 0);
                }
                scores.Add(score);
            }

            scores.Sort();
            Console.WriteLine(scores[scores.Count / 2]);
        }

        #endregion

        #region Day9

        static void Day9PartOne()
        {
            string[] lines = System.IO.File.ReadAllLines("Day9.txt");
            List<int> lowSpots = new List<int>();

            for (int row = 0; row < lines.Length; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    var x = int.Parse(lines[row][col].ToString());
                    var a = row == 0 ? 10 : int.Parse(lines[row - 1][col].ToString());
                    var b = row == lines.Length - 1 ? 10 : int.Parse(lines[row + 1][col].ToString());
                    var c = col == 0 ? 10 : int.Parse(lines[row][col - 1].ToString());
                    var d = col == lines[row].Length - 1 ? 10 : int.Parse(lines[row][col + 1].ToString());

                    if (x < a && x < b && x < c && x < d)
                        lowSpots.Add(x);
                }
            }

            Console.WriteLine(lowSpots.Count + " " + lowSpots.Sum() + " " + (lowSpots.Count  + lowSpots.Sum()));
        }

        static void Day9PartTwo()
        {
            string[] lines = System.IO.File.ReadAllLines("Day9.txt");
            //this is a variation on the island problem
            var islandSizes = new List<int>();

            for (int row = 0; row < lines.Length; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    if (lines[row][col] != '9')
                    {
                        islandSizes.Add(ClearIslandSpot(lines, row, col));
                    }
                }
            }

            islandSizes.Sort();
            islandSizes.Reverse();

            Console.WriteLine(islandSizes[0] + " " + islandSizes[1] + " " + islandSizes[2] + " = " + (islandSizes[0] * islandSizes[1] * islandSizes[2]));

        }

        static int ClearIslandSpot(string[] lines, int row, int col)
        {            
            var x = int.Parse(lines[row][col].ToString());
            if (x == 9) return 0;

            var a = row == 0 ? 10 : int.Parse(lines[row - 1][col].ToString());
            var b = row == lines.Length - 1 ? 10 : int.Parse(lines[row + 1][col].ToString());
            var c = col == 0 ? 10 : int.Parse(lines[row][col - 1].ToString());
            var d = col == lines[row].Length - 1 ? 10 : int.Parse(lines[row][col + 1].ToString());

            int count = 1;
            lines[row] = lines[row].Substring(0, col) + "9" + lines[row].Substring(col + 1);

            if (a < 9) count += ClearIslandSpot(lines, row - 1, col);
            if (b < 9) count += ClearIslandSpot(lines, row + 1, col);
            if (c < 9) count += ClearIslandSpot(lines, row, col - 1);
            if (d < 9) count += ClearIslandSpot(lines, row, col + 1);

            return count;
        }

        #endregion

        #region Day8

        //Part one I did in excel

        static bool ContainsAllChars(string parent, string substring)
        {
            foreach (char c in substring)
            {
                if (!parent.Contains(c))
                    return false;
            }

            return true;
        }

        public static void Day8PartTwo()
        {
            string[] lines = System.IO.File.ReadAllLines("Day8.txt");

            List<string> translations = new List<string>();

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                var mappings = parts[0].Trim().Split(' ').ToList();

                string[] answers = new string[10];

                for (int i = 0; i < mappings.Count; i++)
                    mappings[i] = String.Concat(mappings[i].OrderBy(c => c));

                mappings = mappings.OrderBy(one => one.Length).ToList();

                foreach (string map in mappings)
                {
                    if (map.Length == 2)
                        answers[1] = map;
                    else if (map.Length == 3)
                        answers[7] = map;
                    else if (map.Length == 4)
                        answers[4] = map;
                    else if (map.Length == 7)
                        answers[8] = map;
                }

                string fourLessOne = answers[4];
                foreach (char c in answers[1])
                {
                    fourLessOne = fourLessOne.Replace(c.ToString(), "");
                }

                foreach (string map in mappings)
                {
                    if (map.Length == 5)
                    {
                        if (ContainsAllChars(map, answers[1]))
                            answers[3] = map;
                        else if (ContainsAllChars(map, fourLessOne))
                            answers[5] = map;
                        else
                            answers[2] = map;
                    }
                    if (map.Length == 6)
                    {
                        if (ContainsAllChars(map, answers[4]))
                            answers[9] = map;
                        else if (ContainsAllChars(map, fourLessOne))
                            answers[6] = map;
                        else
                            answers[0] = map;
                    }
                }

                var question = parts[1].Trim().Split(' ').ToList();
                for (int i = 0; i < question.Count; i++)
                    question[i] = String.Concat(question[i].OrderBy(c => c));

                string translation = "";
                foreach (var q in question)
                {
                    for (var i = 0; i < answers.Length; i++)
                    {
                        if (answers[i] == q)
                        {
                            translation += i.ToString();
                            break;
                        }
                    }
                }

                if (translation.Length != 4)
                    throw new System.Exception("Bad Length");

                translations.Add(translation);
            }

            int answer = translations.Select(one => int.Parse(one)).Sum();
            Console.WriteLine(answer);
        }

        #endregion

        #region Day7

        public static void Day7PartOne()
        {
            List<int> crabs = new List<int>(System.IO.File.ReadAllText("Day7.txt").Split(',').Select(one => int.Parse(one)));

            var min = crabs.Min();
            var max = crabs.Max();

            var answer = -1;
            var position = -1;

            for (var i = min; i <= max; i++)
            {
                var x = crabs.Select(one => Math.Abs(one - i)).Sum();
                if (x < answer || answer == -1)
                {
                    answer = x;
                    position = i;
                }
            }

            Console.WriteLine($"Answer: {answer}, Position: {position}");
            Console.ReadLine();
        }

        public static void Day7PartTwo()
        {
            List<int> crabs = new List<int>(System.IO.File.ReadAllText("Day7.txt").Split(',').Select(one => int.Parse(one)));

            var min = crabs.Min();
            var max = crabs.Max();

            var answer = -1;
            var position = -1;

            for (var i = min; i <= max; i++)
            {
                var x = crabs.Select(one => Math.Abs(one - i) * (Math.Abs(one - i) + 1) / 2).Sum();
                if (x < answer || answer == -1)
                {
                    answer = x;
                    position = i;
                }
            }

            Console.WriteLine($"Answer: {answer}, Position: {position}");
            Console.ReadLine();
        }

        #endregion

        #region Day6

        public static void Day6PartOne()
        {
            List<int> fish = new List<int>(System.IO.File.ReadAllText("Day6.txt").Split(',').Select(one => int.Parse(one)));
            //List<int> fish = new List<int>("3,4,3,1,2".Split(',').Select(one => int.Parse(one)));

            for (int day = 0; day < 80; day++)
            {
                int newFish = 0;
                for (int i = 0; i < fish.Count; i++)
                {
                    fish[i]--;
                    if (fish[i] == -1)
                    {
                        fish[i] = 6;
                        newFish++;
                    }
                }

                for (int i = 0; i < newFish; i++)
                    fish.Add(8);
            }

            Console.WriteLine($"Fish count:{fish.Count}");
            Console.ReadLine();
        }

        public static void Day6PartTwo()
        {
            List<Int64> fish = new List<Int64>(System.IO.File.ReadAllText("Day6.txt").Split(',').Select(one => Int64.Parse(one)));

            Dictionary<Int64, Int64> fishCount = new Dictionary<Int64, Int64>();
            foreach (var f in fish)
            {
                if (!fishCount.ContainsKey(f)) fishCount.Add(f, 0);
                fishCount[f]++;
            }

            for (int day = 0; day < 256; day++)
            {
                Dictionary<Int64, Int64> newFishCount = new Dictionary<Int64, Int64>();
                for (int i = 8; i >= 0; i--)
                {
                    newFishCount[i - 1] = fishCount.ContainsKey(i) ? fishCount[i] : 0;
                }

                newFishCount[8] = newFishCount[-1];
                newFishCount[6] += newFishCount[-1];

                if (newFishCount.ContainsKey(-1))
                    newFishCount.Remove(-1);

                fishCount = newFishCount;
            }

            Console.WriteLine($"Fish count:{fishCount.Values.Sum()}");
            Console.ReadLine();
        }

        #endregion

        #region Day5
        
        class Point
        {
            public int X;
            public int Y;

            public override string ToString()
            {
                return $"({X}:{Y})";
            }
        }
        public static void Day5PartOne()
        {
            var data = File.ReadAllLines("Day5.txt");

            List<(Point, Point)> lines = new List<(Point, Point)>();

            foreach (var line in data)
            {
                var parts = line.Split(new char[] { ',', ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
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

        #endregion

        #region Day4

        internal class bingoSpot
        {
            internal int num;
            internal bool matched;
        }

        static void Day4PartOne()
        {
            //parse file
            var fileLines = System.IO.File.ReadAllLines("Day4.txt");
            fileLines = fileLines.Where(one => one != "").ToArray();
            int[] numbers = fileLines[0].Split(new char[] { ',' }).Select(one => int.Parse(one)).ToArray();

            List<List<bingoSpot>> boards = new List<List<bingoSpot>>();
            List<bingoSpot> board = null;

            for (var i = 1; i < fileLines.Length; i++)
            {
                if ((i - 1) % 5 == 0)
                {
                    board = new List<bingoSpot>();
                    boards.Add(board);
                }

                var x = fileLines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(one => int.Parse(one)).ToArray();
                foreach (var y in x)
                {
                    board.Add(new bingoSpot() { num = y, matched = false });
                }
            }

            //play bingo
            foreach (var num in numbers)
            {
                foreach (var b in boards)
                {
                    var spot = b.Find(one => one.num == num);
                    if (spot != null)
                    {
                        spot.matched = true;
                    }

                    if (isWinner(b))
                    {
                        var sum = 0;
                        foreach (var x in b)
                        {
                            if (!x.matched) sum += x.num;
                        }

                        Console.WriteLine($"Winner! sum = {sum}, num = {num}, multiple={sum * num}");
                        Console.ReadLine();
                        return;
                    }
                }
            }
        }

        static bool isWinner(List<bingoSpot> board)
        {
            for (var i = 0; i < 5; i++)
            {
                if (board[i].matched &&
                    board[i + 5].matched &&
                    board[i + 10].matched &&
                    board[i + 15].matched &&
                    board[i + 20].matched)
                {
                    return true;
                }
            }

            for (var i = 0; i < 25; i = i + 5)
            {
                if (board[i].matched &&
                    board[i + 1].matched &&
                    board[i + 2].matched &&
                    board[i + 3].matched &&
                    board[i + 4].matched)
                {
                    return true;
                }
            }

            return false;
        }

        static void Day4PartTwo()
        {
            //parse file
            var fileLines = System.IO.File.ReadAllLines("Day4.txt");
            fileLines = fileLines.Where(one => one != "").ToArray();
            int[] numbers = fileLines[0].Split(new char[] { ',' }).Select(one => int.Parse(one)).ToArray();

            List<List<bingoSpot>> boards = new List<List<bingoSpot>>();
            List<bingoSpot> board = null;

            for (var i = 1; i < fileLines.Length; i++)
            {
                if ((i - 1) % 5 == 0)
                {
                    board = new List<bingoSpot>();
                    boards.Add(board);
                }

                var x = fileLines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(one => int.Parse(one)).ToArray();
                foreach (var y in x)
                {
                    board.Add(new bingoSpot() { num = y, matched = false });
                }
            }

            //play bingo
            foreach (var num in numbers)
            {
                for (var i = 0; i < boards.Count; i++)
                {
                    var b = boards[i];
                    var spot = b.Find(one => one.num == num);
                    if (spot != null)
                    {
                        spot.matched = true;
                    }

                    if (isWinner(b))
                    {
                        if (boards.Count > 1)
                        {
                            boards.Remove(b);
                            i--;
                        }
                        else
                        {
                            var sum = 0;
                            foreach (var x in boards[0])
                            {
                                if (!x.matched) sum += x.num;
                            }

                            Console.WriteLine($"Last Winner! sum = {sum}, num = {num}, multiple={sum * num}");
                            Console.ReadLine();
                            return;
                        }
                    }
                }
            }
        }

        #endregion

        #region Day3

        static string[] FilterOnBit(string[] input, int bitNo, bool common)
        {
            if (input.Length == 1) return input;

            int ones = 0;
            int zeroes = 0;

            foreach (string line in input)
            {
                if (line[bitNo] == '0')
                {
                    zeroes++;
                }
                else
                {
                    ones++;
                }
            }

            if ((common && ones >= zeroes) || (!common && ones < zeroes))
            {
                return input.Where(one => one[bitNo] == '1').ToArray();
            }
            else
            {
                return input.Where(one => one[bitNo] == '0').ToArray();
            }
        }

        static void Day3PartTwo()
        {
            string[] Commoninput = System.IO.File.ReadAllLines(@"C:\temp\day3input.txt");
            string[] Uncommoninput = System.IO.File.ReadAllLines(@"C:\temp\day3input.txt");

            for (int i = 0; i < Commoninput[0].Length; i++)
            {
                Commoninput = FilterOnBit(Commoninput, i, true);
                Uncommoninput = FilterOnBit(Uncommoninput, i, false);
            }

            int a = Convert.ToInt32(Commoninput[0], 2);
            int b = Convert.ToInt32(Uncommoninput[0], 2);
            Console.WriteLine($"a = {Commoninput[0]} {a}, b = {Uncommoninput[0]} {b}, Product = {a * b}");
            Console.ReadLine();
        }

        static void Day3PartOne()
        {
            string[] input = System.IO.File.ReadAllLines(@"C:\temp\day3input.txt");
            List<int> ones = new List<int>();
            List<int> zeros = new List<int>();

            for (int i = 0; i < input[0].Length; i++)
            {
                ones.Add(0);
                zeros.Add(0);
            }

            foreach (string line in input)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '0')
                    {
                        zeros[i]++;
                    }
                    else
                    {
                        ones[i]++;
                    }
                }
            }

            string gammaCommon = "";
            string epsilonStr = "";
            for (int i = 0; i < input[0].Length; i++)
            {
                if (ones[i] > zeros[i])
                {
                    gammaCommon += "1";
                    epsilonStr += "0";
                }
                else
                {
                    gammaCommon += "0";
                    epsilonStr += "1";
                }
            }

            int gamma = Convert.ToInt32(gammaCommon, 2);
            int epsilon = Convert.ToInt32(epsilonStr, 2);
            Console.WriteLine($"Gamma = {gammaCommon} {gamma}, Epsilon = {epsilonStr} {epsilon}, Product = {gamma * epsilon}");
            Console.ReadLine();
        }

        #endregion
    
        //Day 2 and 1 I did in excel
    }

}

