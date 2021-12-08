using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            Day8PartTwo();
        }

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
        
        struct Point
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

