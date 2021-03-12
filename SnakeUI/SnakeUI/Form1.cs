using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Snake;
using System.Windows.Forms;

namespace SnakeUI
{
    public partial class Form1 : Form
    {
        LandUI[,] mapUI;
        Map decoyMap;
        public bool IsBotAlive = false;
        SnakeBot bot = null;
        public Form1()
        {
            ///row left => right
            ///col top=>down
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Start_btn.Click += start_Click;
            this.PreviewKeyDown += (sender, e) => {
                e.IsInputKey = true;
            };
            ColPanel.Padding = new Padding(0);
            ColPanel.Margin = new Padding(0);
            decoyMap = new Map(new Size(25, 25));
            mapUI = new LandUI[25, 25];
            for (int j = 0; j < mapUI.GetLength(1); j++)
            {
                FlowLayoutPanel nextRow;
                if (j == 0)
                    nextRow = rowPanel;
                else
                {
                    nextRow = new FlowLayoutPanel();
                    nextRow.BackColor = rowPanel.BackColor;
                    nextRow.AutoSize = rowPanel.AutoSize;
                    nextRow.AutoSizeMode = rowPanel.AutoSizeMode;
                    nextRow.FlowDirection = rowPanel.FlowDirection;
                }
                nextRow.Location = new Point(0, 0);
                nextRow.Margin = new Padding(0);
                nextRow.Padding = new Padding(0);
                this.ColPanel.Controls.Add(nextRow);
                for (int i = 0; i < mapUI.GetLength(0); i++)
                {
                    var land = decoyMap.Locations[i, j];
                    mapUI[i, j] = new LandUI(land);
                    mapUI[i, j].Location = new Point(0, 0);
                    nextRow.Controls.Add(mapUI[i, j]);
                    mapUI[i, j].Click += (sender, e) => {
                        this.Text = ((LandUI)sender).Land.Location.ToString();
                    };
                }
            }
            foreach (Control c in this.Controls)
            {
                c.PreviewKeyDown += (sender, e) => {
                    e.IsInputKey = true;
                };
                c.KeyDown += (sender, e) =>
                {
                    if (IsBotAlive)
                        return;
                    var snake = GameHandle.snake;
                    if (snake is null)
                        return;
                    if (e.KeyData == Keys.A || e.KeyData == Keys.Left)
                    {
                        if (snake.HeadsTo == Directions.Left)
                            return;
                        GameHandle.ChangeDirection(Directions.Left);
                    }///////////////////////////////////////////////////
                    if (e.KeyData == Keys.D || e.KeyData == Keys.Right)
                    {
                        if (snake.HeadsTo == Directions.Right)
                            return;
                        GameHandle.ChangeDirection(Directions.Right);
                    }//////////////////////////////////////////////////////
                    if (e.KeyData == Keys.W || e.KeyData == Keys.Up)
                    {
                        if (snake.HeadsTo == Directions.Up)
                            return;
                        GameHandle.ChangeDirection(Directions.Up);
                    }//////////////////////////////////////////////////////
                    if (e.KeyData == Keys.S || e.KeyData == Keys.Down)
                    {
                        if (snake.HeadsTo == Directions.Down)
                            return;
                        GameHandle.ChangeDirection(Directions.Down);
                    }
                };
            }
        }
        private void StartANewGame(bool Bot=false,dynamic Settings=null)
        {
            int width = mapUI.GetLength(0);
            int height = mapUI.GetLength(1);
            Map map = new Map(new Size(width,height));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    mapUI[i, j].Land = map.Locations[i, j];
                }
            }
            if (!Bot)
            {
                GameHandle.Initialize(this, map);
                GameHandle.OnSnakeMoves += GameHandle_OnSnakeMoves;
                GameHandle.OnSnakeEats += GameHandle_OnSnakeEats;
                GameHandle.OnGameStops += GameHandle_OnGameStops;
            }
            else
            {
                SnakeBot bot = new SnakeBot(this, map, Settings.Food);
                bot.Interval = Settings.Time;
                this.bot = bot;
                bot.Start();
                bot.snake.OnSnakeMoves += GameHandle_OnSnakeMoves;
                bot.snake.OnSnakeEats += GameHandle_OnSnakeEats;
            }
        }

        private void GameHandle_OnGameStops(object sender, Log e)
        {
            for (int i = 0; i < decoyMap.Width; i++)
            {
                for (int j = 0; j < decoyMap.Height; j++)
                {
                    mapUI[i, j].Land = decoyMap.Locations[i, j];
                }
            }
            GameHandle.OnSnakeMoves -= GameHandle_OnSnakeMoves;
            GameHandle.OnSnakeEats -= GameHandle_OnSnakeEats;
            GameHandle.OnGameStops -= GameHandle_OnGameStops;
        }

        private void GameHandle_OnSnakeEats(object sender, Land e)
        {
        }

        private void GameHandle_OnSnakeMoves(object sender, Land e)
        {
            if (!(sender is null))
            {
                Text = ((Snake.Snake)sender).Head.Location.ToString();
                LengthL.Text = "Length: " + ((Snake.Snake)sender).Length;
            }
        }

        private void CancelGame()
        {
            GameHandle.StopTheGame();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void start_Click(object sender, EventArgs e)
        {
            string startstr = "Start a new game";
            string restartstr = "End Game";

            if (Start_btn.Text == startstr)
            {
                StartANewGame();
                Start_btn.Text = restartstr;
            }
            else
            {
                CancelGame();
                Start_btn.Text = startstr;
            }

        
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Buttons_layout.Location =new Point { X=ColPanel.Location.X+ColPanel.Width+10, Y=ColPanel.Location.Y};
            CenterVertical(Start_btn);
            CenterVertical(Bot_Game_btn);
            CenterVertical(LengthL);
            PutBelow(Start_btn, Bot_Game_btn, 10);
            PutBelow(Bot_Game_btn, LengthL, 10);
        }
        /// <summary>
        /// <see cref="Point.X"/>=middle (considering width)
        /// </summary>
        /// <param name="control"></param>
        private void CenterVertical(Control control)
        {
            var parent = control.Parent;
            control.Location = new Point(parent.Width / 2 - control.Width / 2,control.Location.Y);
        }
        /// <summary>
        /// <see cref="Point.Y"/>=middle (considering height)
        /// </summary>
        /// <param name="control"></param>
        private void CenterHorizontal(Control control)
        {
            var parent = control.Parent;
            control.Location = new Point(control.Location.X,parent.Height / 2 - control.Height / 2);
        }
        private void PutBelow(Control upper, Control lower, int padding)
        {
            lower.Location = new Point
            {
                Y = upper.Location.Y + padding+upper.Height,
                X = lower.Location.X
            };
        }
        private void PutAbove(Control lower, Control upper, int padding)
        {
            upper.Location = new Point
            {
                Y = lower.Location.Y - padding-upper.Height,
                X = upper.Location.X
            };
        }
        private void PutToTheRight(Control left, Control right, int padding)
        {
            right.Location = new Point
            {
                Y = right.Location.Y,
                X = left.Location.X + padding + left.Width
            };
        }
        private void PutToTheLeft(Control right, Control left, int padding)
        {
            left.Location = new Point
            {
                Y = left.Location.Y,
                X = right.Location.X-padding-left.Width
            };
        }

        private void Bot_Game_btn_Click(object sender, EventArgs e)
        {
            string StartBotText = "Bot Game";
            string StopBotText = "Stop bot";
            if (Bot_Game_btn.Text == StopBotText)
            {
                bot.Stop();
                Bot_Game_btn.Text = StartBotText;
                return;
            }
            Form SettingsConfigure = new Form { Size = new Size(192, 108),AutoSize=true, AutoSizeMode=AutoSizeMode.GrowAndShrink };
            Label TimeL = new Label { Font = new Font("Arial", 11), ForeColor = Color.Black, Text="Time between moves in milliseconds: ", AutoSize=true, };
            Label FoodL = new Label { Font = TimeL.Font, ForeColor = Color.Blue, Text = "The base number of food in the map: ", AutoSize = true };
            TextBox TimeTB = new TextBox { Font = TimeL.Font, Width=TimeL.Width};
            TextBox FoodTB = new TextBox { Font = TimeL.Font, Width = FoodL.Width };
            Button Start_btn = new Button { Font = TimeL.Font, Text = "Start" };
            dynamic settings=null;
            SettingsConfigure.Controls.Add(TimeL);
            SettingsConfigure.Controls.Add(FoodL);
            SettingsConfigure.Controls.AddRange(new Control[]{ FoodTB,TimeTB,Start_btn});
            CenterVertical(TimeL);
            TimeL.Location = new Point { X = TimeL.Location.X, Y = 0 };
            CenterVertical(FoodL);
            CenterVertical(FoodTB);
            CenterVertical(TimeTB);
            CenterVertical(Start_btn);
            PutBelow(TimeL, TimeTB, 2);
            PutBelow(TimeTB, FoodL, 6);
            PutBelow(FoodL, FoodTB, 2);
            PutBelow(FoodTB, Start_btn, 6);
            Start_btn.Click += (x, y) => {
                settings = new { Food = Convert.ToInt32(FoodTB.Text), Time = Convert.ToInt32(TimeTB.Text) };
                SettingsConfigure.Close();
            };
            SettingsConfigure.ShowDialog();
            if (settings is null)
                return;
            else
            {
                GameHandle.Base_Number_of_food = settings.Food;
                GameHandle.interval = settings.Time;
                StartANewGame(true,settings);
                if (IsBotAlive)
                    Bot_Game_btn.Text = StopBotText;
            }

        }
    }
}
