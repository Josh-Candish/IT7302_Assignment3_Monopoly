using System;
using System.Collections;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{

    /// <summary>
    /// Main class for monoploy game that implements abstract class game
    /// </summary>
    
    public class Monopoly : Game
    {
        readonly ConsoleColor[] _colors = new ConsoleColor[8] { ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Gray, ConsoleColor.Blue, ConsoleColor.DarkYellow};
        bool _gameSetUp;

        public override void InitialiseGame()
        {
            DisplayMainChoiceMenu();

        }

        public override void MakePlay(int iPlayerIndex)
        {
            //make variable for player
            Player player = Board.Access().GetPlayer(iPlayerIndex);
            //Change the colour for the player
            Console.ForegroundColor = this._colors[iPlayerIndex];

            //if inactive skip turn
            if (!player.IsActive())
            {
                Console.WriteLine("\n{0} is inactive.\n", player.GetName());
                //check players to continue
                //check that there are still two players to continue
                int activePlayerCount = 0;
                foreach (Player p in Board.Access().GetPlayers())
                {
                    //if player is active
                    if (p.IsActive())
                        //increment activePlayerCount
                        activePlayerCount++;
                }

                //if less than two active players display winner
                if (activePlayerCount < 2)
                {
                    this.PrintWinner();
                }
               
                return;
            }
            
            

            //prompt player to make move
            Console.WriteLine("{0}Your turn. Press Enter to make move", PlayerPrompt(iPlayerIndex));
            Console.ReadLine();
            //move player
            player.Move();

            //Display making move
            Console.WriteLine("*****Move for {0}:*****", player.GetName());
            //Display rolling
           Console.WriteLine("{0}{1}\n", PlayerPrompt(iPlayerIndex), player.DiceRollingToString());

            Property propertyLandedOn = Board.Access().GetProperty(player.GetLocation());
            //landon property and output to console
            Console.WriteLine(propertyLandedOn.LandOn(ref player));
            //Display player details
            Console.WriteLine("\n{0}{1}", PlayerPrompt(iPlayerIndex), player.BriefDetailToString());
            //display player choice menu
            DisplayPlayerChoiceMenu(player);

            
        }

        public override bool EndOfGame()
        {
            //display message
            Console.WriteLine("The game is now over. Please press enter to exit.");
            Console.ReadLine();
            //exit the program
            Board.Access().SetGameOver(true);
            return true;
        }

        public override void PrintWinner()
        {
            Player winner = null;
            //get winner who is last active player
            foreach (Player p in Board.Access().GetPlayers())
            {
                //if player is active
                if (p.IsActive())
                    winner = p;
            }
             //display winner
            Console.WriteLine("\n\n{0} has won the game!\n\n" , winner.GetName());
            //end the game
            this.EndOfGame();
        }

        public void DisplayMainChoiceMenu()
        {
            int resp = 0;
            Console.WriteLine("Please make a selection:\n");
            Console.WriteLine("1. Setup Monopoly Game");
            Console.WriteLine("2. Start New Game");
            Console.WriteLine("3. Exit");
            Console.Write("(1-3)>");
            //read response
            resp = InputInteger();
            //if response is invalid redisplay menu
            if (resp == 0)
                this.DisplayMainChoiceMenu();

            //perform choice according to number input
            try
            {
                switch (resp)
                {
                    case 1:
                        this.SetUpGame();
                        this._gameSetUp = true;
                        this.DisplayMainChoiceMenu();
                        break;
                    case 2:
                        if (this._gameSetUp)
                            this.PlayGame();
                        else
                        {
                            Console.WriteLine("The Game has not been set up yet.\n");
                            this.DisplayMainChoiceMenu();
                        }
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        throw new ApplicationException("That option is not avaliable. Please try again.");
                }
            }
            catch(ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void SetUpGame()
        {
            //setup properties
            this.SetUpProperties();

            //add players
            this.SetUpPlayers();
            
        }

        public void PlayGame()
        {
            while (Board.Access().GetPlayerCount() >= 2 && !Board.Access().GetGameOver())
            {
                for (int i = 0; i < Board.Access().GetPlayerCount(); i++)
                {
                    // If the game is over don't make them play
                    if (Board.Access().GetGameOver()) break;

                    MakePlay(i);
                }
            }
        }

        public int InputInteger() //0 is invalid input
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Please enter a number such as 1 or 2. Please try again.");
                return 0;
            }
        }

        public decimal InputDecimal() //0 is invalid input
        {
            try
            {
                return decimal.Parse(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Please enter a decimal number such as 25.54 or 300. Please try again.");
                return 0;
            }
        }

        public decimal InputDecimal(string msg)
        {
            Console.WriteLine(msg);
             Console.Write(">");
            decimal amount = this.InputDecimal();

            //if response is invalid redisplay 
            if (amount == 0)
            {
                Console.WriteLine("That was not a valid amount. Please try again");
                this.InputDecimal(msg);
            }
            return amount;
        }

        public void SetUpProperties()
        {
            var luckFactory = new LuckFactory();
            var resFactory = new ResidentialFactory();
            var transFactory = new TransportFactory();
            var utilFactory = new UtilityFactory();
            var genericFactory = new PropertyFactory();

            try
            {
                var fileReader = new FileReader();
                var propertyDetails = fileReader.ReadPropertyDetailsFromCSV();

                // Add the properties to the board
                foreach (var propertyDetail in propertyDetails)
                {
                    switch (propertyDetail.Type.ToLower())
                    {
                        case "luck":
                            Board.Access()
                                .AddProperty(luckFactory.create(propertyDetail.Name, propertyDetail.IsPenalty,
                                    propertyDetail.Amount));
                            break;
                        case "residential":
                            Board.Access()
                                .AddProperty(resFactory.create(propertyDetail.Name, propertyDetail.Price,
                                    propertyDetail.Rent, propertyDetail.HouseCost));
                            break;
                        case "transport":
                            Board.Access().AddProperty(transFactory.create(propertyDetail.Name));
                            break;
                        case "utility":
                            Board.Access().AddProperty(utilFactory.create(propertyDetail.Name));
                            break;
                        case "generic":
                            Board.Access().AddProperty(genericFactory.Create(propertyDetail.Name));
                            break;
                    }
                }

                Console.WriteLine("Properties have been setup");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops, something went wrong setting up the properties: {0}", ex.Message); 
            }
        }

        public void SetUpPlayers()
        {
            //Add players to the board
            Console.WriteLine("How many players are playing?");
            Console.Write("(2-8)>");
            int playerCount = this.InputInteger();

            //if it is out of range then display msg and redo this method
            if ((playerCount < 2) || (playerCount > 8))
            {
                Console.WriteLine("That is an invalid amount. Please try again.");
                this.SetUpPlayers();
            }

            //Ask for players names
            for (int i = 0; i < playerCount; i++)
            {
                Console.WriteLine("Please enter the name for Player {0}:", i + 1);
                Console.Write(">");
                string sPlayerName = Console.ReadLine();
                Player player = new Player(sPlayerName);
                //subscribe to events
                player.PlayerBankrupt += playerBankruptHandler;
                player.PlayerPassGo += playerPassGoHandler;
                //add player 
                Board.Access().AddPlayer(player);
                Console.WriteLine("{0} has been added to the game.", Board.Access().GetPlayer(i).GetName());
            }

            Console.WriteLine("Players have been setup");
        }

        public string PlayerPrompt(int playerIndex)
        {
            return string.Format("{0}:\t", Board.Access().GetPlayer(playerIndex).GetName());
        }

        public string PlayerPrompt(Player player)
        {
            return string.Format("{0}:\t", player.GetName());
        }

        public bool GetInputYn(Player player, string sQuestion)
        {
            Console.WriteLine(PlayerPrompt(player) + sQuestion);
            Console.Write("y/n>");
            string resp = Console.ReadLine().ToUpper();

            switch (resp)
            {
                case "Y":
                    return true;
                case "N":
                    return false;
                default:
                    Console.WriteLine("That answer cannot be understood. Please answer 'y' or 'n'.");
                    this.GetInputYn(player, sQuestion);
                    return false;
            }
        }

        public void DisplayPlayerChoiceMenu(Player player)
        {
            int resp = 0;
            Console.WriteLine("\n{0}Please make a selection:\n", PlayerPrompt(player));
            Console.WriteLine("1. Finish turn");
            Console.WriteLine("2. View your details");
            Console.WriteLine("3. Purchase This Property");
            Console.WriteLine("4. Buy House for Property");
            Console.WriteLine("5. Trade Property with Player");
            Console.Write("(1-5)>");
            //read response
            resp = InputInteger();
            //if response is invalid redisplay menu
            if (resp == 0)
                this.DisplayPlayerChoiceMenu(player);

            //perform choice according to number input
                switch (resp)
                {
                    case 1:
                        break;
                    case 2:
                        Console.WriteLine("==================================");
                        Console.WriteLine(player.FullDetailToString());
                        Console.WriteLine("==================================");
                        this.DisplayPlayerChoiceMenu(player);
                        break;
                    case 3:
                        this.PurchaseProperty(player);
                        this.DisplayPlayerChoiceMenu(player);
                        break;
                    case 4:
                        this.BuyHouse(player);
                        this.DisplayPlayerChoiceMenu(player);
                        break;
                    case 5:
                        this.TradeProperty(player);
                        this.DisplayPlayerChoiceMenu(player);
                        break;
                    default:
                        Console.WriteLine("That option is not avaliable. Please try again.");
                        this.DisplayPlayerChoiceMenu(player);
                        break;
                }
        }

        public void PurchaseProperty(Player player, bool? testAnswer = null)
        {
            //if property available give option to purchase else so not available
            if (Board.Access().GetProperty(player.GetLocation()).AvailableForPurchase())
            {
                TradeableProperty propertyLocatedOn = (TradeableProperty)Board.Access().GetProperty(player.GetLocation());
                bool? respYN = testAnswer ?? GetInputYn(player, string.Format("'{0}' is available to purchase for ${1}. Are you sure you want to purchase it?", propertyLocatedOn.GetName(), propertyLocatedOn.GetPrice()));
                if ((bool) respYN)
                {
                    propertyLocatedOn.Purchase(ref player);//purchase property
                    Console.WriteLine("{0}You have successfully purchased {1}.", PlayerPrompt(player), propertyLocatedOn.GetName());
                }
            }
            else
            {
                Console.WriteLine("{0}{1} is not available for purchase.", PlayerPrompt(player), Board.Access().GetProperty(player.GetLocation()).GetName());
            }
        }

        public void BuyHouse(Player player)
        {
            //create prompt
            string sPrompt = String.Format("{0}Please select a property to buy a house for:", this.PlayerPrompt(player));
            //create variable for propertyToBuy
            Residential propertyToBuyFor = null;
            if (player.GetPropertiesOwnedFromBoard().Count == 0)
            {
                //write message
                Console.WriteLine("{0}You do not own any properties.", PlayerPrompt(player));
                //return from method
                return;
            }
            //get the property to buy house for
            Property property = this.DisplayPropertyChooser(player.GetPropertiesOwnedFromBoard(), sPrompt);
            //if dont own any properties
            
            //check that it is a residential
            if (property.GetType() == (new Residential().GetType()))
            {
                //cast to residential property
               propertyToBuyFor = (Residential) property;
            }
            else //else display msg 
            {
                Console.WriteLine("{0}A house can no be bought for {1} because it is not a Residential Property.", this.PlayerPrompt(player), propertyToBuyFor.GetName());
                return;
            }
            
            //check that max houses has not been reached
            if (propertyToBuyFor.GetHouseCount() >= Residential.GetMaxHouses())
            {
                Console.WriteLine("{0}The maximum house limit for {1} of {2} houses has been reached.", PlayerPrompt(player), propertyToBuyFor.GetName(), Residential.GetMaxHouses());
            }
            else
            {
                //confirm
                bool doBuyHouse = this.GetInputYn(player, String.Format("You chose to buy a house for {0}. Are you sure you want to purchase a house for ${1}?", propertyToBuyFor.GetName(), propertyToBuyFor.GetHouseCost()));
                //if confirmed
                if (doBuyHouse)
                {
                    //buy the house
                    propertyToBuyFor.AddHouse();
                    Console.WriteLine("{0}A new house for {1} has been bought successfully", PlayerPrompt(player), propertyToBuyFor.GetName());
                }
            }
        }

        public void TradeProperty(Player player)
        {
            //create prompt
            string sPropPrompt = String.Format("{0}Please select a property to trade:", this.PlayerPrompt(player));
            //create prompt
            string sPlayerPrompt = String.Format("{0}Please select a player to trade with:", this.PlayerPrompt(player));

            //get the property to trade
            TradeableProperty propertyToTrade = (TradeableProperty)this.DisplayPropertyChooser(player.GetPropertiesOwnedFromBoard(), sPropPrompt);

            //if dont own any properties
            if (propertyToTrade == null)
            {
                //write message
                Console.WriteLine("{0}You do not own any properties.", PlayerPrompt(player));
                //return from method
                return;
            }

            //get the player wishing to trade with
            Player playerToTradeWith = this.DisplayPlayerChooser(Board.Access().GetPlayers(), player, sPlayerPrompt);

            //get the amount wanted

            string inputAmtMsg = string.Format("{0}How much do you want for this property?", PlayerPrompt(player));

            decimal amountWanted = InputDecimal(inputAmtMsg);

            //confirm with playerToTradeWith
                //set console color
            ConsoleColor origColor = Console.ForegroundColor;
            int i = Board.Access().GetPlayers().IndexOf(playerToTradeWith);
            Console.ForegroundColor = this._colors[i];
                //get player response
            bool agreesToTrade = GetInputYn(playerToTradeWith, string.Format("{0} wants to trade '{1}' with you for ${2}. Do you agree to pay {2} for '{1}'", player.GetName(), propertyToTrade.GetName(), amountWanted));
            //resent console color
            Console.ForegroundColor = origColor;
            if (agreesToTrade)
            {
                Player playerFromBoard = Board.Access().GetPlayer(playerToTradeWith.GetName());
                //player trades property

                player.TradeProperty(ref propertyToTrade, ref playerFromBoard, amountWanted);
                Console.WriteLine("{0} has been traded successfully. {0} is now owned by {1}", propertyToTrade.GetName(), playerFromBoard.GetName());
            }
            else
            {
                //display rejection message
                Console.WriteLine("{0}{1} does not agree to trade {2} for ${3}", PlayerPrompt(player), playerToTradeWith.GetName(), propertyToTrade.GetName(), amountWanted);
            }     
        }

        public Property DisplayPropertyChooser(ArrayList properties, String sPrompt)
        {
            //if no properties return null
            if (properties.Count == 0)
                return null;
            Console.WriteLine(sPrompt);
            for (int i = 0; i < properties.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, properties[i].ToString());
            }
            //display prompt
            Console.Write("({0}-{1})>", 1, properties.Count);
            //get input
            int resp = this.InputInteger();

            //if outside of range
            if ((resp < 1) || (resp > properties.Count))
            {
                Console.WriteLine("That option is not avaliable. Please try again.");
                this.DisplayPropertyChooser(properties, sPrompt);
                return null;
            }
            else
            {
                //return the appropriate property
                return (Property) properties[resp - 1];
            }
        }

        public Player DisplayPlayerChooser(ArrayList players, Player playerToExclude, String sPrompt)
        {
            //if no players return null
            if (players.Count == 0)
                return null;
            Console.WriteLine(sPrompt);
            //Create a new arraylist to display
            ArrayList displayList = new ArrayList(players);

            //remove the player to exlude
            displayList.Remove(playerToExclude);

            //go through and display each
            for (int i = 0; i < displayList.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, displayList[i].ToString());
            }
            //display prompt
            Console.Write("({0}-{1})>", 1, displayList.Count);
            //get input
            int resp = this.InputInteger();

            //if outside of range
            if ((resp < 1) || (resp > displayList.Count))
            {
                Console.WriteLine("That option is not avaliable. Please try again.");
                this.DisplayPlayerChooser(players, playerToExclude, sPrompt);
                return null;
            }
            else
            {
                Player chosenPlayer = (Player) displayList[resp - 1];
                //find the player to return
                foreach (Player p in players)
                {
                    if(p.GetName() == chosenPlayer.GetName())
                        return p;
                }
                return null;
            }
        }

        public static void playerBankruptHandler(object obj, EventArgs args)
        {
            //cast to player
            Player p = (Player) obj;
            //display bankrupt msg
            Console.WriteLine("{0} IS BANKRUPT!", p.GetName().ToUpper());

        }

        public static void playerPassGoHandler(object obj, EventArgs args)
        {
            Player p = (Player)obj;
            Console.WriteLine("{0} has passed go.{0} has received $200", p.GetName());
        }
   }
}

