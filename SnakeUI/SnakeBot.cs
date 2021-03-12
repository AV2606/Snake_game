using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Snake;

namespace SnakeUI
{
    class SnakeBot
    {
        public Form1 Parent { get; private set; }
        public Map map { get; private set; }
        public Snake.Snake snake { get; private set; }
        public int Interval { get; set; } = 1;
        public int Food { get; private set; }

        private Thread GameT;


        public SnakeBot(Form1 caller,Map map, int food)
        {
            this.Parent = caller;
            this.Food = food;
            this.map = map;
            foreach (Land l in map.Locations)
                if (l.OnIt == Occupition.Snake)
                    throw new ArgumentException("The map needs to be empty!");
            snake = new Snake.Snake(map.Locations[0, 0]);
        }

        public void Start()
        {
            snake.SnakeHasCapturedTheWholeMap += Won;
            for (int i = 0; i < Food; i++)
                map.AddFoodAtRandom();
            snake.OnSnakeEats += (sender, l) => {
                map.AddFoodAtRandom();
            };
            GameT = new Thread(() => { 
            while (true)
            {
                snake.ChangeDirection(NextDirection(), true);
                Thread.Sleep(Interval);
            }
            });
            GameT.Start();
            Parent.IsBotAlive=true;
        }
        public void Stop()
        {
            try
            {
                GameT?.Abort();
            }
            catch(Exception e)
            {
            }
            finally
            {
                GameT = null;
                Parent.IsBotAlive = false;
            }
        }
        public void Won(object sender, EventArgs e)
        {
            Stop();
            MessageBox.Show("I have won the game!", "I am the winner", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        public Directions NextDirection()
        {
            if (snake.Head.Location.X == 24)
                if (snake.Head.Location.Y == 0)
                    return Directions.Left;
                else
                    return Directions.Up;
            if (snake.Head.Location.X == 0)
                if (snake.Head.Location.Y == 24)
                    return Directions.Left;
            if (snake.Head.Location.Y % 2 == 1)
                if (snake.Head.Location.X == 23)
                    return Directions.Down;
                else
                    return Directions.Right;
            if (snake.Head.Location.Y % 2 == 0)
                if (snake.Head.Location.X == 0)
                    return Directions.Down;
                else
                    return Directions.Left;
            return Directions.Up;
        }
    }
}
