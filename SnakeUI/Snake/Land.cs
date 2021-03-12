using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Snake
{
    public enum Occupition { Empty, Snake, Food};
    public class Land
    {
        public Map Parent { get; private set; }
        public Point Location { get
            {
                return Parent.GetLocation(this);
            } }
        public Occupition OnIt { get; private set; } = Occupition.Empty;

        public event EventHandler<Occupition> StateChanged;

        public Land(Map parent)
        {
            this.Parent = parent;
        }

        public void AddFood()
        {
            if (OnIt != Occupition.Empty)
                throw new AlreadyOccupiedLandException("The land alread has " + OnIt + " on it.", this);
            else
                OnIt = Occupition.Food;
            StateChanged?.Invoke(this, OnIt);
        }
        public void MoveSnakeIn(Snake snake)
        {
            if (OnIt == Occupition.Snake)
                throw new AlreadyOccupiedLandException("The snake has touched himself!", this);
            if (OnIt == Occupition.Food)
                snake.AddFood();
            OnIt = Occupition.Snake;
            StateChanged?.Invoke(this, OnIt);
        }
        public void MoveSnakeOut(Snake snake)
        {
            OnIt = Occupition.Empty;
            StateChanged?.Invoke(this, OnIt);
        }
    }
}
