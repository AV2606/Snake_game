using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake;
using Image = Tools.Imageing.GenericImage;

namespace SnakeUI
{
    class LandUI:PictureBox
    {
        public Land Land { get { return land; }
            set
            {
                //land.StateChanged -= Land_State_Changed;
                land = value;
                land.StateChanged += Land_State_Changed;
                Land_State_Changed(land, land.OnIt);
            }
        }
        public Color SnakeColor { get; set; } = Color.Green;
        public Color FoodColor { get; set; } = Color.Red;

        private Image DrawImage;
        private Land land;

        public LandUI(Land land) : base()
        {
            this.land = land;
            this.Land.StateChanged += Land_State_Changed;
            this.DrawImage = new Image(17, 17);
            DrawImage.Wipe(Color.White);
            for (int i = 0; i < DrawImage.Width; i++)
            {
                DrawImage.SetPixel(Color.Black, i, 0);
                DrawImage.SetPixel(Color.Black, i, DrawImage.Height-1);
                DrawImage.SetPixel(Color.Black, 0, i);
                DrawImage.SetPixel(Color.Black, DrawImage.Width-1,i);
            }
            this.Size = new Size(DrawImage.Width, DrawImage.Height);
            this.Image = DrawImage.ToBitmap();
            this.Margin = new Padding(0);
            this.Padding = new Padding(0);
            this.Click += OnClick;
        }

        private void OnClick(object sender, EventArgs e)
        {
        }

        private void Land_State_Changed(object sender, Occupition occupition)
        {
            if(occupition==Occupition.Food)
            {
                this.DrawImage.Replace(DrawImage[2, 2], FoodColor);
            }
            if (occupition == Occupition.Snake)
            {
                this.DrawImage.Replace(DrawImage[2, 2], SnakeColor);
            }
            if (occupition == Occupition.Empty)
            {
                this.DrawImage.Replace(DrawImage[2, 2], Color.White);
            }
            Image = DrawImage.ToBitmap();
        }

    }
}
