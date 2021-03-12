using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    public enum Directions { Up, Left, Down, Right};
    public class Snake
    {
        //Head of queue (dequeue) = head of snake
        //Tail of queue (enqueue) = tail of snake
        private Queue<Land> Space;
        /// <summary>
        /// The last cell the snake has been to and left.
        /// </summary>
        private Land LastOccupied = null;
        /// <summary>
        /// The next move direction.
        /// </summary>
        private Directions Moves = Directions.Right;
        private int NumberOfTailsToAdd = 0;

        public Land Head { get
            {
                return Space.Peek();

            } }
        public Land Tail { get
            {
                return Space.ElementAt(Space.Count - 1);
            } }
        public int Length {
            get {
                return Space.Count;
            } }
        /// <summary>
        /// The direction in which this <see cref="Snake"/> is moving to.
        /// </summary>
        public Directions HeadsTo { get
            {
                return Moves;
            } }
        /// <summary>
        /// The map this <see cref="Snake"/> is crwaling on.
        /// </summary>
        public Map map { get; private set; }
        public DateTime Last_Move { get; private set; }

        /// <summary>
        /// <para>
        /// Raises when this <see cref="Snake"/> crawl.
        /// </para>
        /// Holds the information of the new <see cref="Land"/> this <see cref="Snake"/> crwaled to.
        /// </summary>
        public event EventHandler<Land> OnSnakeMoves;
        /// <summary>
        /// Raises when this <see cref="Snake"/> eats some food.
        /// </summary>
        public event EventHandler<Land> OnSnakeEats;
        /// <summary>
        /// Raises when the snake is the length of the entaire map.
        /// </summary>
        public event EventHandler SnakeHasCapturedTheWholeMap;

        /// <summary>
        /// Creates a snake with just a head.
        /// </summary>
        /// <param name="firsLocation"></param>
        public Snake(Land firsLocation)
        {
            Space = new Queue<Land>();
            Space.Enqueue(firsLocation);
            firsLocation.MoveSnakeIn(this);
            map = firsLocation.Parent;
            Program.gamelog.AddLog("Added a new snake in the location " + firsLocation.Location.ToString());
        }

        /// <summary>
        /// Feeds the snake and makes its length bigger by 1.
        /// </summary>
        public void AddFood()
        {
            // Space.Enqueue(LastOccupied);
            //var temp = LastOccupied;
            //LastOccupied.MoveSnakeOut(this);
            //LastOccupied = null;
            Program.gamelog.AddLog("Fed the snake");
            //OnSnakeEats?.Invoke(this, temp);
            NumberOfTailsToAdd++;
            OnSnakeEats?.Invoke(this, LastOccupied);
        }
        /// <summary>
        /// Makes this <see cref="Snake"/> crawl to the next <see cref="Land"/>.
        /// </summary>
        public void Move()
        {
            var NewHead = map.GetNextTo(Head, HeadsTo);
            try
            {
                NewHead.MoveSnakeIn(this);
            }
            catch(AlreadyOccupiedLandException e)
            {
                if (e.FiredMe == Space.ElementAt(0) || e.FiredMe == Space.ElementAt(1))
                    return;
                if(Length==map.Locations.Length)
                {
                    this.SnakeHasCapturedTheWholeMap?.Invoke(this, null);
                    return;
                }    
                throw e;
            }
            Queue<Land> NewSpace = new Queue<Land>();
            NewSpace.Enqueue(NewHead);
            if (NumberOfTailsToAdd > 0)
            {
                while (Space.Count > 0)
                    NewSpace.Enqueue(Space.Dequeue());
                Space = NewSpace;
                --NumberOfTailsToAdd;
                Program.gamelog.AddLog("Snake moved to " + NewHead.Location.ToString());
                OnSnakeMoves?.Invoke(this, Head);
                return;
            }
            while (Space.Count > 1)
                NewSpace.Enqueue(Space.Dequeue());
            LastOccupied = Space.Dequeue();
            Space = NewSpace;
            if (!(LastOccupied is null))
                LastOccupied.MoveSnakeOut(this);
            Program.gamelog.AddLog("Snake moved to " + NewHead.Location.ToString());
            Last_Move = DateTime.Now;
            OnSnakeMoves?.Invoke(this, Head);
        }
        /// <summary>
        /// Changes this <see cref="Snake"/> direction.
        /// </summary>
        /// <param name="newDir">The new direction.</param>
        /// <param name="ForceCrawl">Indicates whether to foce this <see cref="Snake"/> to make a move (<see cref="true"/>) or not (false).</param>
        public void ChangeDirection(Directions newDir, bool ForceCrawl = false)
        {
            Moves = newDir;
            Program.gamelog.AddLog("Snake changed direction to " + newDir.ToString());
            if (ForceCrawl)
                Move();
        }
    }
}
