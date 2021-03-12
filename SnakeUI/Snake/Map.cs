using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Snake
{
    /// <summary>
    /// A class to handle all the <see cref="Land"/>s to a map.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Just a random instance for this class.
        /// </summary>
        static Random rnd = new Random();
        /// <summary>
        /// All the lands of the map
        /// </summary>
        public Land[,] Locations { get; private set; }
        public int Width
        {
            get
            {
                return Locations.GetLength(0);
            }
        }
        public int Height { get
            {
                return Locations.GetLength(1);
            } }
        public char DrawChar { get; set; } = '*';
        public ConsoleColor FoodColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor SnakeColor { get; set; } = ConsoleColor.Green;
        public bool WrapBorders { get; private set; }

        /// <summary>
        /// Creates a Map of size <see cref="Size"/>
        /// </summary>
        /// <param name="size"></param>
        public Map(Size size, bool wrapBorders=true)
        {
            Program.gamelog.AddLog("Initialize a game map");
            Locations = new Land[size.Width, size.Height];
            for (int i = 0; i < size.Width; i++)
                for (int j = 0; j < size.Height; j++)
                    Locations[i, j] = new Land(this);
            this.WrapBorders = wrapBorders;
        }

        /// <summary>
        /// Draws this <see cref="Map"/> to the console stream.
        /// </summary>
        /// <param name="Clear">Indicates wether to clear the console before writing to it.</param>
        public void DrawToConsole(bool Clear = false)
        {
            Program.gamelog.AddLog("Drawing the map to Console");
            if (Clear)
                Console.Clear();
            List<CString> strings = new List<CString>();
            strings.Add(new CString());
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    /*if (Locations[i, j].OnIt == Occupition.Food)
                        Console.ForegroundColor = FoodColor;
                    else
                    if (Locations[i, j].OnIt == Occupition.Snake)
                        Console.ForegroundColor = SnakeColor;
                    else
                        if(Console.ForegroundColor!= ConsoleColor.White)
                        Console.ForegroundColor = ConsoleColor.White;*/
                    var color = GetColor(Locations[i, j].OnIt);
                    if (strings[strings.Count - 1].Color == color)
                        strings[strings.Count - 1] += DrawChar;
                    else
                        strings.Add(new CString(DrawChar, color));
                }
                strings.Add(new CString("*\n", ConsoleColor.Blue));
            }
            strings.Add(new CString(Multiply('*', Width + 1), ConsoleColor.Blue));
            CString.Write(strings);
            Program.gamelog.AddLog("Finished drawing the map");
        }
        private string Multiply(char c, int times)
        {
            string str = "";
            for (int i = 0; i < times; i++)
            {
                str += c;
            }
            return str;
        }
        private ConsoleColor GetColor(Occupition occ)
        {
            if (occ == Occupition.Food)
                return FoodColor;
            if (occ == Occupition.Snake)
                return SnakeColor;
            return ConsoleColor.White;
        }

        /// <summary>
        /// Returns the <see cref="Land"/> which Located next to <paramref name="from"/> in the <paramref name="direction"/>
        /// </summary>
        /// <param name="from">The land to look from.</param>
        /// <param name="direction">The direction to look at.</param>
        /// <returns></returns>
        public Land GetNextTo(Land from, Directions direction)
        {
            var loc = GetLocation(from);
            try
            {
                if (direction == Directions.Right)
                    return FromLocation(loc.X + 1, loc.Y);
                if (direction == Directions.Down)
                    return FromLocation(loc.X, loc.Y + 1);
                if (direction == Directions.Left)
                    return FromLocation(loc.X - 1, loc.Y);
                return FromLocation(loc.X, loc.Y - 1);
            }
            catch (IndexOutOfRangeException e)
            {
                if (direction == Directions.Right)
                    return FromLocation(0, loc.Y);
                if (direction == Directions.Down)
                    return FromLocation(loc.X, 0);
                if (direction == Directions.Left)
                    return FromLocation(this.Width-1, loc.Y);
                return FromLocation(loc.X, this.Height-1);
            }
        }
        /// <summary>
        /// Returns the location of the <paramref name="land"/>
        /// </summary>
        /// <param name="land">The land to look for its location.</param>
        /// <returns></returns>
        public Point GetLocation(Land land)
        {
            for (int i = 0; i < Locations.GetLength(0); i++)
                for (int j = 0; j < Locations.GetLength(1); j++)
                    if (Locations[i, j] == land)
                        return new Point(i, j);
            return new Point(-1,-1);
        }
        /// <summary>
        /// Returns the <see cref="Land"/> which is at the <paramref name="Location"/>
        /// </summary>
        /// <param name="Location">The location of the desired <see cref="Land"/></param>
        /// <returns></returns>
        public Land FromLocation(Point Location)
        {
            return Locations[Location.X, Location.Y];
        }
        /// <summary>
        /// Returns the <see cref="Land"/> which is at the coordinates x and y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Land FromLocation(int x, int y)
        {
            return FromLocation(new Point(x, y));
        }
        /// <summary>
        /// Returns all the free lands in this map.
        /// </summary>
        /// <returns></returns>
        public Land[] AllFreeLands()
        {
            Queue<Land> lands = new Queue<Land>();
            foreach (Land land in Locations)
                if (land.OnIt == Occupition.Empty)
                    lands.Enqueue(land);
            return lands.ToArray();
        }
        /// <summary>
        /// Tries to add food to the <paramref name="Location"/>, returns true if succed, false if not.
        /// </summary>
        /// <param name="Location">The location to try to put the food at.</param>
        /// <returns></returns>
        public bool TryAddFood(Point Location)
        {
            var l = FromLocation(Location);
            if (l.OnIt == Occupition.Empty)
                l.AddFood();
            else
                return false;
            return true;
        }
        /// <summary>
        /// Adds food in a random place in the map and returns its location.
        /// </summary>
        /// <returns></returns>
        public Point AddFoodAtRandom()
        {
            var lands = AllFreeLands();
            if (lands.Length == 0)
            {
                Program.gamelog.AddLog("Tried to add a food but the map was full! ");
                return new Point(-1, -1);
                
            }
            var land = lands[rnd.Next(lands.Length)];
            land.AddFood();
            Program.gamelog.AddLog("Added a new food at location " + land.Location.ToString());
            return GetLocation(land);
        }
        /// <summary>
        /// Returns a random land
        /// </summary>
        /// <param name="margin">The margin from the boundries of the map which the given land need to be in.</param>
        /// <returns></returns>
        public Land GetRandomLand(int margin=0)
        {
            if (margin > Width / 2 || margin > Height / 2)
                throw new ArgumentException("Margin cant be grater than half the width or height");
            return Locations[rnd.Next(margin, Width - margin), rnd.Next(margin, Height - margin)];
        }
        /// <summary>
        /// Returns the amount of food that currently in the map
        /// </summary>
        /// <returns></returns>
        public int NumberOfFood()
        {
            int ret = 0;
            foreach (Land l in Locations)
                if (l.OnIt == Occupition.Food)
                    ret++;
            return ret;
        }

        
    }
}
