using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Player : Trader
    {
        private int _location;
        private int _lastMove;
        private bool _isActive = true;
        private Die _die1 = new Die();
        private Die _die2 = new Die();

        #region Events

        public event EventHandler PlayerPassGo;
        public event EventHandler PlayerBankrupt;

        #endregion

        public Player()
        {
            
        }

        public Player(string name)
        {
            Name = name;
            Balance = InitialValueAccessor.GetPlayerStartingBanker();
            _location = 0;
        }

        public void Move()
        {  
            var moveDistance = _die1.Roll() + _die2.Roll();

            SetLocation(GetLocation() + moveDistance);

            _lastMove = moveDistance;
        }

        public override string ToString()
        {
            return GetName();
        }

        public string BriefDetailToString()
        {
            return string.Format("Player: {0}.\nBalance: ${1}\nLocation:{2} (Square {3})\nProperties Owned: \n{4}",
                GetName(), GetBalance(), Board.Access().GetProperty(GetLocation()), GetLocation(),
                GetPropertiesOwnedToString());
        }

        public string FullDetailToString()
        {
            return string.Format("You are on {0}. \t You have ${1}", Board.Access().GetProperty(GetLocation()).GetName(),
                GetBalance());
        }

        public string DiceRollingToString()
        {
            return string.Format("Rolling dice: \t Dice 1: {0} \t Dice 2: {1}", _die1, _die2);
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
