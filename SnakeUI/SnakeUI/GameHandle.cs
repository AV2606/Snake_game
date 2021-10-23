using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Snake;

namespace SnakeUI
{
    public static class GameHandle
    {
        public static Form GameUI;
        public static Map map { get; private set; }
        public static Snake.Snake snake { get; private set; }
        public static Size mapSize { get; private set; }= new Size(25, 25);
        public static Thread GameT { get; private set; }
        public static event EventHandler<Log> OnGameStops;
        public static event EventHandler<Land> OnSnakeEats;
        public static event EventHandler<Land> OnSnakeMoves;
        public static event EventHandler<Map> OnGameInitialize;
        public static event EventHandler GameFinished;
        public static bool GameRuning { get; private set; } = false;
        public static int interval
        {
            get
            {
                return INTERVAL;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentException("The interval (number of milliseconds between each crawl of the snake) cant be less than 1 millisecond.");
                else
                    INTERVAL = value;
            }
        }
        public static int Base_Number_of_food
        {
            get
            {
                return BNOF;
            }
            set
            {
                if (GameRuning)
                    throw new InvalidOperationException("Cant change the base number of food when the game is running.");
                if (value < 1)
                    throw new ArgumentException("The base number of food cant be less than 1!");
                else
                    BNOF = value;
            }
        }
        private static Func<int,int>[] FC;

        private static int BNOF=1;
        private static int INTERVAL=222;

        /// <summary>
        /// Initialize a new game.
        /// </summary>
        /// <param name="Caller"></param>
        /// <param name="map"></param>
        public static void Initialize(Form Caller,Map map,Func<int,int>[] foodChanger=null)
        {
            if (GameRuning)
                throw new InvalidOperationException("The Game Handle cant initialize when the game is running!");
            FC = foodChanger;
            GameUI = Caller;
            GameHandle.map = map;
            snake = new Snake.Snake(map.GetRandomLand(1));
            mapSize = new Size(map.Width, map.Height);
            GameRuning = true;
            GameT = new Thread(() => {
                Game_Handle();
            });
            GameT.Start();
            snake.OnSnakeMoves += (x,y)=> { OnSnakeMoves?.Invoke(snake, y); };
            snake.OnSnakeEats += (s,l)=> { OnSnakeEats?.Invoke(snake, l); };
            snake.SnakeHasCapturedTheWholeMap += (s,e)=>{ GameFinished?.Invoke(snake, e); };
            OnGameInitialize?.Invoke(Caller, map);
        }
        /// <summary>
        /// Stops the game and raises the <see cref="OnGameStops"/> event.
        /// </summary>
        public static void StopTheGame()
        {
            try
            {
                GameT.Abort();
            }
            catch(Exception e)
            {

            }
            finally
            {
                GameT = null;
            }
            BNOF = 1;
            INTERVAL = 250;
            GameUI = null;
            GameHandle.GameRuning = false;
            GameHandle.mapSize = Size.Empty;
            GameHandle.snake = null;
        }
        /// <summary>
        /// Changes the Direction the snake moves to and made him crawl.
        /// </summary>
        /// <param name="newDir"></param>
        public static void ChangeDirection(Directions newDir)
        {
            snake.ChangeDirection(newDir, true);
        }
        private static void Game_Handle()
        {
            for (int i = 0; i < BNOF; i++)
                map.AddFoodAtRandom();
            snake.OnSnakeEats += (s, land) =>
            {
                if(!(FC is null))
                foreach (var item in FC)
                {
                    int res = item(snake.Length);
                        if (res > 0)
                            for (int i = 0; i < res; i++) 
                                map.AddFoodAtRandom();
                }
                map.AddFoodAtRandom();
            };
            try
            {
                while (true)
                {
                    Thread.Sleep(interval);
                    Check:
                    if ((DateTime.Now - snake.Last_Move).TotalMilliseconds >= interval)
                        snake.Move();
                    else
                    {
                        Thread.Sleep(1);
                        goto Check;
                    }
                }
            }
            catch (AlreadyOccupiedLandException e)
            {
                Snake.Program.gamelog.AddLog("Finished the game because the snake touched himself");
                OnGameStops?.Invoke(e, Snake.Program.gamelog);
            }
            catch (NullReferenceException e)
            {
                Snake.Program.gamelog.AddLog("Ended the game because the snake went out of the map");
                OnGameStops?.Invoke(e, Snake.Program.gamelog);
            }
        }
    }
}
