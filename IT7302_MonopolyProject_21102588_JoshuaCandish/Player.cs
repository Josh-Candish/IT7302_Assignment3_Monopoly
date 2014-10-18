using System;
using System.Collections;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Player : Trader
    {
        private int _location;
        private int _lastMove;
        private bool _isActive = true;
        private Die _die1 = new Die();
        private Die _die2 = new Die();
        private int _die1Result;
        private int _die2Result;
        public int RollDoublesFailureCount;
        public int RolledDoublesCount;
        public bool RolledDoubles;
        public int GetOutOfJailCardCount;

        public bool IsInJail { get; set; }

        #region Events

        public event EventHandler PlayerPassGo;
        public event EventHandler PlayerBankrupt;

        #endregion

        public Player()
        {
            
        }

        public Player(string name)
        {
            Name = name.Trim();
            Balance = InitialValueAccessor.GetPlayerStartingBanker();
            _location = 0;
        }

        public Player(string name, decimal balance)
            : base(name, balance)
        {
            _location = 0;
        }

        public void Move()
        {
            _die1Result = _die1.Roll();
            _die2Result = _die2.Roll();

            var moveDistance = _die1Result + _die2Result;
            RolledDoubles = _die1Result == _die2Result;

            // Don't set a new location if they're in jail
            if (IsInJail)
            {
                if (!RolledDoubles)
                {
                    // If they fail to roll doubles thrice, they have no choice 
                    // but to pay the fine or use a get out of jail free card
                    RollDoublesFailureCount++;
                }

                return;
            }

            if (RolledDoubles)
            {
                RolledDoublesCount++;
            }

            SetLocation(GetLocation() + moveDistance);

            _lastMove = moveDistance;
        }

        public bool IsCriminal()
        {
            const int goToJailLocation = 30;
            
            var goToJailProperty = Board.Access().GetProperty(goToJailLocation);
            var currentLocation = Board.Access().GetProperty(_location);

            var isCriminal = currentLocation.Equals(goToJailProperty) || (RolledDoublesCount > 2);

            if (isCriminal) GoToJail();

            // If they are on go to jail, they are a criminal
            return isCriminal;
        }

        public void GoToJail()
        {
            SetLocation(10);// Jail's location on the board
            IsInJail = true;
        }

        public override string ToString()
        {
            return GetName();
        }

        public string BriefDetailToString()
        {
            return String.Format("You are on {0}.\tYou have ${1}.", Board.Access().GetProperty(GetLocation()).GetName(), GetBalance());
        }

        public string FullDetailToString()
        {
            return String.Format("Player:{0}.\nBalance: ${1}\nLocation: {2} (Square {3}) \nProperties Owned:\n{4}", GetName(), GetBalance(), Board.Access().GetProperty(GetLocation()), GetLocation(), PropertiesOwnedToString());
        }

        public string PropertiesOwnedToString()
        {
            string owned = "";
            //if none return none
            if (GetPropertiesOwnedFromBoard().Count == 0)
                return "None";
            //for each property owned add to string owned
            for (int i = 0; i < GetPropertiesOwnedFromBoard().Count; i++)
            {
                owned += GetPropertiesOwnedFromBoard()[i].ToString() + "\n";
            }
            return owned;
        }

        public string DiceRollingToString()
        {
            return string.Format("Rolling dice: \t Dice 1: {0} \t Dice 2: {1}", _die1Result, _die2Result);
        }

        public override void CheckBankrupt()
        {
            if (GetBalance() <= 0)
            {
                if (PlayerBankrupt != null)
                {
                    PlayerBankrupt(this, new EventArgs());
                    var banker = Banker.Access();

                    foreach (Property property in GetPropertiesOwnedFromBoard())
                    {
                        property.SetOwner(ref banker);
                    }
                }

                _isActive = false;
            }
        }

        public ArrayList GetPropertiesOwnedFromBoard()
        {
            var propertiesOwned = new ArrayList();

            for (var i = 0; i < Board.Access().GetProperties().Count; i++)
            {
                if (Board.Access().GetProperty(i).GetOwner() == this)
                {
                    propertiesOwned.Add(Board.Access().GetProperty(i));
                }
            }

            return propertiesOwned;
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public bool PayJailFee()
        {
            if (Balance >= 50)
            {
                Balance -= 50;
                SetFreeFromJail();
                return true;
            }

            return false;
        }

        public void SetFreeFromJail()
        {
            IsInJail = false;
            RollDoublesFailureCount = 0;
        }

        #region Getters/Setters

        public int GetLocation()
        {
            return _location;
        }

        public void SetLocation(int newLocation)
        {
            var boardSize = Board.Access().GetSquares();

            if (newLocation > boardSize)
            {
                // Because the properties arraylist is zero based index, 
                // we have to add one to the board size to get the correct location
                _location = newLocation - (boardSize + 1);

                if (PlayerPassGo != null)
                {
                    PlayerPassGo(this, new EventArgs());
                    Receive(200);
                }

                return;
            }

            if (newLocation == boardSize)
            {
                // The last index is 39 (0-39 = 40) therefore
                // if the new location is 40, as in the last
                // square, we must substract 1 from the new location
                // of square 40 to place the player at index 39.
                _location = newLocation - 1;
                return;
            }

            _location = newLocation;
        }

        public int GetLastMove()
        {
            return _lastMove;
        }

        #endregion
    }
}
