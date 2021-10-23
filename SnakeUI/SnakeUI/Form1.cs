using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Speech;
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
        SizedQueue<char> BotCode = new SizedQueue<char>(7);
        System.Speech.Synthesis.SpeechSynthesizer Speech = new System.Speech.Synthesis.SpeechSynthesizer { Volume = 100 };

        public Form1()
        {
            ///row left => right
            ///col top=>down
            InitializeComponent();
            Speech.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Female, System.Speech.Synthesis.VoiceAge.Teen);
            Speech.SpeakAsync("Hi there");
            BotCode.OnEnqueued += (sender, e) => {
                var s = (SizedQueue<char>)sender;
                string str = "";
                foreach(char c in s.Value)
                    str += c;
                if (str.ToLower() == "botgame")
                { 
                    Bot_Game_btn.Visible = true;
                    Bot_Game_btn.Enabled = true;
                }

            };
            this.Text = "Snake";
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
            this.PreviewKeyDown += Preview_key_down;
            this.KeyDown += Key_Down;
            foreach(Control c in Buttons_layout.Controls)
            {
                c.PreviewKeyDown += Preview_key_down;
                c.KeyDown += Key_Down;
            }
            foreach (Control c in this.Controls)
            {
                c.PreviewKeyDown += Preview_key_down;
                c.KeyDown += Key_Down;
            }
        }

        private void Preview_key_down(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {
            char ec = e.KeyData.ToString()[0];
            BotCode.Enqueue(ec);
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
        }
        private void StartANewGame(bool Bot=false, dynamic Settings=null)
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
                GameHandle.Initialize(this, map,new Func<int, int>[] { AddFood});
                GameHandle.OnSnakeMoves += GameHandle_OnSnakeMoves;
                GameHandle.OnSnakeEats += GameHandle_OnSnakeEats;
                GameHandle.OnGameStops += GameHandle_OnGameStops;
                GameHandle.GameFinished += (sender, e) => {
                    Speech.SpeakAsync("You are officially, an expert");
                    MessageBox.Show("The game has finished due to your fenomenal skillz!","Astonishing!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                };
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
        private int AddFood(int length)
        {
            if (length == 1)
            {
                Speech.SpeakAsync("Nice start!");
                return 1;
            }
            if (length == 10)
            {
                Speech.SpeakAsync("You have got ten points");
                return 1;
            }
            if (length == 20)
            {
                Speech.SpeakAsync("yeepie kay ayy");
                return 2;
            }
            if (length == 50)
            {
                Speech.SpeakAsync("nicely done, fifty points dude");
                return 1;
            }
            if (length == 100)
            {
                Speech.SpeakAsync("you've got nice moves");
                return 3;
            }
            if (length == 150)
            {
                Speech.SpeakAsync("an hundre and fifty points there");
                return 2;
            }
            if (length == 200)
            {
                Speech.SpeakAsync("two hundreds points");
                return 4;
            }
            if (length == 250)
            {
                Speech.SpeakAsync("You are on fire!");
                return 5;
            }
            if (length == 300)
            {
                Speech.SpeakAsync("ten more food on the table");
                return 10;
            }
            if (length == 375)
            {
                Speech.SpeakAsync("three hundred and seventy five points!");
                return 7;
            }
            if (length == 450)
            {
                Speech.SpeakAsync("Better than pub jee right?");
                return 12;
            }
            if (length == 550)
            {
                return 1 + 25;
                Speech.SpeakAsync("You have insane skils there friend");
            }
            return 0;
        }

        private void GameHandle_OnGameStops(object sender, Log e)
        {
            MessageBox.Show("You have touched yourself! your length was: " + LengthL.Text.Substring(LengthL.Text.IndexOf(' ')), "oops?!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            LengthL.Text = "Length: " + ((Snake.Snake)sender).Length;
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            int tries = 0;
            Start:
            try
            {
                GameHandle.StopTheGame();
                bot.Stop();
            }
            catch(Exception exc)
            {
                tries++;
                if (tries < 10)
                    goto Start;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        class SizedQueue<T>
        {
            public System.Collections.Generic.Queue<T> Value { get; private set; }
            public int Size
            {
                get
                {
                    return size;
                }
                set
                {
                    if (size < 0)
                        throw new ArgumentException("The size cant be less than 0!");
                    OnSizeChanged?.Invoke(this, EventArgs.Empty);
                    size = value;
                }
            }
            public event EventHandler<T> OnEnqueued;
            public event EventHandler<T> OnDequeued;
            public event EventHandler OnSizeChanged;

            private int size;

            public SizedQueue(int size)
            {
                Value = new System.Collections.Generic.Queue<T>(7);
                Size = size;
            }
            public void Enqueue(T item)
            {
                Value.Enqueue(item);
                if (Value.Count > size)
                    Value.Dequeue();
                OnEnqueued?.Invoke(this, item);
            }
            public T Dequeue()
            {
                var item= Value.Dequeue();
                OnDequeued?.Invoke(this, item);
                return item;
            }
            public T Peek()
            {
                return Value.Peek();
            }
            public T[] ToArray()
            {
                return Value.ToArray();
            }
        }
    }
}