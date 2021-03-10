using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class AlreadyOccupiedLandException:Exception
    {
        public Land FiredMe { get; private set; }
        public AlreadyOccupiedLandException(string message,Land land) : base(message)
        {
            FiredMe = land;
        }
    }
}
