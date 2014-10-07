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

            SetLocation(GetLocation() + moveDistance);

            _lastMove = moveDistance;
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

        #region Getters/Setters

        public int GetLocation()
        {
            return _location;
        }

        public void SetLocation(int newLocation)
        {
            var boardSize = Board.Access().GetSquares();

            if (newLocation >= boardSize)
            {
                _location = newLocation - boardSize;

                if (PlayerPassGo != null)
                {
                    PlayerPassGo(this, new EventArgs());
                    Receive(200);
                }
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
