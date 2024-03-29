﻿using System;
using System.Collections;
using System.Linq;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{

    /// <summary>
    /// Main class for monoploy game that implements abstract class game
    /// </summary>
    
    public class Monopoly : Game
    {
        private readonly ConsoleColor[] _colors = new ConsoleColor[8] { ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Gray, ConsoleColor.Blue, ConsoleColor.DarkYellow};
        private bool _gameSetUp;
        private Creator _creator = new Creator();

        public override void InitialiseGame()
        {
            DisplayMainChoiceMenu();

        }

        public override void MakePlay(int iPlayerIndex)
        {
            while (true)
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

                if (player.IsInJail && player.RolledDoubles && (player.RollDoublesFailureCount < 3))
                {
                    player.SetFreeFromJail();
                    Console.WriteLine("You rolled doubles! You are out of Jail.");
                }

                if (player.IsInJail && player.RollDoublesFailureCount == 3)
                {
                    Console.WriteLine("You've failed to roll doubles three times while in Jail and must now settle it.");
                    GetOutOfJail(player, true);
                }

                //Display making move
                Console.WriteLine("*****Move for {0}:*****", player.GetName());
                //Display rolling
                Console.WriteLine("{0}{1}\n", PlayerPrompt(iPlayerIndex), player.DiceRollingToString());

                Property propertyLandedOn = Board.Access().GetProperty(player.GetLocation());

                HandleLanding(propertyLandedOn, player);
                
                //Display player details
                Console.WriteLine("\n{0}{1}", PlayerPrompt(iPlayerIndex), player.BriefDetailToString());

                if (player.IsCriminal())
                {
                    Console.WriteLine("You are a criminal, go to Jail and lose your turn.");
                    // Need to reset this here so when they come to get out of jail they aren't seen as a ciminal again
                    // Thrice in a row means jail!
                    if (player.RolledDoublesCount > 2)
                    {
                        Console.WriteLine("This is because you rolled doubles thrice in a row.");
                    }

                    // Reset the count as they've been sent to jail
                    player.RolledDoublesCount = 0;
                    return;
                }

                //display player choice menu
                DisplayPlayerChoiceMenu(player);

                // Player's get another turn when they roll doubles
                if (player.RolledDoubles && !player.IsInJail)
                {
                    Console.WriteLine("You rolled doubles and get another turn");
                    continue;
                }

                // When turn ends reset their rolled doubles count
                player.RolledDoublesCount = 0;
                break;
            }
        }

        private void HandleLanding(Property propertyLandedOn , Player player)
        {
            // If it's a residential property we need to parse it so we can get the colour and display it to the user
            // otherwise they won't be aware of colour the property is at any point.
            var propertyAsRes = propertyLandedOn.GetType() == typeof (Residential) ? (Residential) propertyLandedOn : null;
           
            // When landing on chance or community chest we need the behavour to be
            // slightly different, i.e. get a card and display what the card is
            if (propertyLandedOn.GetName().Contains("Chance") && (Board.Access().GetChanceCards().Count > 0))
            {
                Console.WriteLine(Board.Access().GetChanceCard().LandOn(ref player));
            }
            else if (propertyLandedOn.GetName().Contains("Community Chest") && (Board.Access().GetCommunityChestCards().Count > 0))
            {
                Console.WriteLine(Board.Access().GetCommunityChestCard().LandOn(ref player));
            }
            else
            {
                // Landon property and output to console
                // In the case that they've landed on a chance or community chest but there 
                // aren't any cards left we must make them aware of this.
                var isChance = propertyLandedOn.GetName().Contains("Chance");
                var isCommunityChest = propertyLandedOn.GetName().Contains("Community Chest");

                Console.WriteLine("{0}{1}{2}",
                    propertyLandedOn.LandOn(ref player),/*{0}*/
                    isChance ? " No more Chance cards." : isCommunityChest ? " No more Community Chest cards." : "",/*{1}*/
                    propertyAsRes != null ? " (Colour: " + propertyAsRes.GetHouseColour() + ")" : "");/*{2}*/
            }
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
            Console.WriteLine("3. Load Game");
            Console.WriteLine("4. Exit");
            Console.Write("(1-4)>");
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
                        if (LoadGame())// Only play if a game has been loaded
                        {
                            PlayGame();
                            break;
                        }
                        DisplayMainChoiceMenu();
                        break;
                    case 4:
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
            SetUpProperties();
            //add players
            SetUpPlayers();
            //add community and chance cards
            SetUpCards();
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
            _creator.CreateProperties();
        }

        /// <summary>
        /// Sets up players for the new game of Monopoly
        /// </summary>
        /// <param name="testInput">Required so we can test this method without relying on console input, shouldn't be used outside of tests</param>
        /// <param name="testName">Required so we can test this method without relying on console input, shouldn't be used  outside of tests</param>
        public void SetUpPlayers(int? testInput = null, string testName = null)
        {
            //Add players to the board
            Console.WriteLine("How many players are playing?");
            Console.Write("(2-8)>");
            var playerCount = testInput ?? InputInteger();

            //if it is out of range then display msg and redo this method
            if ((playerCount < 2) || (playerCount > 8))
            {
                Console.WriteLine("That is an invalid amount. Please try again.");

                // Don't recall if it's a test
                if (testInput == null)
                {
                    SetUpPlayers();
                }
            }

            //Ask for players names
            for (int i = 0; i < playerCount; i++)
            {
                Console.WriteLine("Please enter the name for Player {0}:", i + 1);
                Console.Write(">");
                string sPlayerName = testName ?? Console.ReadLine();
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

        public void SetUpCards()
        {
            _creator.CreateCards();
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
            Console.WriteLine("5. Sell house");
            Console.WriteLine("6. Trade Property with Player");
            Console.WriteLine("7. Mortgage Options");
            Console.WriteLine("8. Save Game");
            if (player.IsInJail) Console.WriteLine("9. Get Out of Jail");

            Console.Write(player.IsInJail ? "(1-9)>" : "(1-8)>");
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
                        DisplayPlayerChoiceMenu(player);
                        break;
                    case 3:
                        PurchaseProperty(player);
                        DisplayPlayerChoiceMenu(player);
                        break;
                    case 4:
                        BuyHouse(player);
                        DisplayPlayerChoiceMenu(player);
                        break;
                    case 5:
                        SellHouse(player);
                        DisplayPlayerChoiceMenu(player);
                        break;
                    case 6:
                        TradeProperty(player);
                        DisplayPlayerChoiceMenu(player);
                        break;
                    case 7:
                        MortgageOptions(player);
                        DisplayPlayerChoiceMenu(player);
                        break;
                    case 8:
                        SaveGame();
                        DisplayPlayerChoiceMenu(player);
                        break;
                    case 9:
                        GetOutOfJail(player, false);
                        DisplayPlayerChoiceMenu(player);
                        break;
                    default:
                        Console.WriteLine("That option is not avaliable. Please try again.");
                        DisplayPlayerChoiceMenu(player);
                        break;
                }
        }

        public void PurchaseProperty(Player player, bool? testAnswer = null)
        {
            if (player.IsInJail)
            {
                Console.WriteLine("You are in Jail and can not purchase property.");
            }
            //if property available give option to purchase else so not available
            else if (Board.Access().GetProperty(player.GetLocation()).AvailableForPurchase())
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

        public void BuyHouse(Player player, Property testProperty = null, bool? testAnswer = null)
        {
            if (Board.Access().Houses < 1)
            {
                Console.WriteLine("Sorry, the bank doesn't have any houses left.");
                return;
            }

            if (player.IsInJail)
            {
                Console.WriteLine("Sorry, you are in jail.");
                return;
            }

            //create prompt
            var sPrompt = String.Format("{0}Please select a property to buy a house for:", PlayerPrompt(player));

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
            var property = testProperty ?? DisplayPropertyChooser(player.GetPropertiesOwnedFromBoard(), sPrompt);
            //if dont own any properties
            
            //check that it is a residential
            if (property.GetType() == (new Residential().GetType()))
            {
                //cast to residential property
               propertyToBuyFor = (Residential) property;
            }
            else //else display msg 
            {
                Console.WriteLine("{0}A house can not be bought for {1} because it is not a Residential Property.", this.PlayerPrompt(player), property.GetName());
                return;
            }

            if (propertyToBuyFor.IsMortgaged)
            {
                Console.WriteLine("{0}A house can not be bought for {1} because it is currently mortgaged.", PlayerPrompt(player), property.GetName());
                return;
            }
            
            // Player must own all the propeties in the colour group
            if (!player.OwnsAllPropertiesOfColour(propertyToBuyFor.GetHouseColour()))
            {
                Console.WriteLine(
                    "You must own all the properties within this colour group ({0}) to buy houses for this property.",
                    propertyToBuyFor.GetHouseColour());
                return;
            }

            // We've checked if they own the properties of the colour, now
            // check if each property is equally developed
            if (!player.CanDevelopPropertyFurther(propertyToBuyFor))
            {
                Console.WriteLine("Each property in this colour group needs to have houses uniformly added");
            }
            else if (propertyToBuyFor.HasHotel)// If it has a hotel they can't add more houses.
            {
                Console.WriteLine("This property has a hotel and can not be developed further.");
            }
            else
            {
                // Can't buy a fifth house if there are not hotels left
                if (propertyToBuyFor.GetHouseCount() == 4 && Board.Access().Hotels < 1)
                {
                    Console.WriteLine("You can't buy another house as this would result in a hotel and there aren't any hotels left.");
                    return;
                }

                //confirm
                var doBuyHouse = testAnswer ?? GetInputYn(player, string.Format("You chose to buy a house for {0}. Are you sure you want to purchase a house for ${1}?", 
                                                                                propertyToBuyFor.GetName(), propertyToBuyFor.GetHouseCost()));

                //if confirmed
                if (doBuyHouse)
                {
                    //buy the house
                    propertyToBuyFor.AddHouse();
                    Console.WriteLine("{0}A new house for {1} has been bought successfully", PlayerPrompt(player),
                        propertyToBuyFor.GetName());
                }
            }
        }

        public void SellHouse(Player player, Property testProperty = null)
        {
            var playersProperties = player.GetPropertiesOwnedFromBoard();

            if (playersProperties.Count == 0)
            {
                Console.WriteLine("{0}You do not own any properties.", PlayerPrompt(player));
                return;
            }

            // Get properties that are residential and have at least one house to be sold
            var developedResidentials =
                playersProperties.ToArray()
                    .Where(x => x.GetType() == typeof (Residential))
                    .Cast<Residential>()
                    .Where(x => x.GetHouseCount() >= 1)
                    .ToArray();

            if (!developedResidentials.Any())
            {
                Console.WriteLine("You don't have any properties with houses to be sold!");
                return;
            }

            // Add the correct propeties to an arraylist, this is because
            // the property chooser takes an arraylist not an IEnumarable
            var propertiesToChooseFrom = new ArrayList();
            propertiesToChooseFrom.AddRange(developedResidentials);

            var playerPrompt = String.Format("{0}Please select a property to sell the house for:", PlayerPrompt(player));

            // Get the property to buy house for
            var property = testProperty ?? DisplayPropertyChooser(propertiesToChooseFrom, playerPrompt);

            var propertyToSellHouse = (Residential) property;

            propertyToSellHouse.SellHouse();
            Console.WriteLine("House sold for ${0}", propertyToSellHouse.GetHouseCost()/2);
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

            bool agreesToTrade;
            var mortgageInterest = Decimal.Zero;

            //get player response
            /*We need to do this to check and find if the property is mortgaged 
             and in the case that it is, notify the buyer that the price
             of the property includes the original owner's mortgage interest*/
            if (propertyToTrade.GetType() == typeof (Residential))
            {
                var residentialPropertyToTrade = (Residential) propertyToTrade;

                if (residentialPropertyToTrade.IsMortgaged)
                {
                    mortgageInterest = residentialPropertyToTrade.GetMortgageValue()*10/100;

                    agreesToTrade = GetInputYn(playerToTradeWith,
                        string.Format(
                            "{0} wants to trade '{1}' with you for ${2} (including seller's mortgage interest for this property). Do you agree to pay {2} for '{1}'",
                            player.GetName(),/*{0}*/
                            propertyToTrade.GetName(),/*{1}*/
                            amountWanted + mortgageInterest));/*{2}*/
                }
                else // If it's not mortgaged do the normal trading behaviour
                {
                    agreesToTrade = GetInputYn(playerToTradeWith,
                        string.Format("{0} wants to trade '{1}' with you for ${2}. Do you agree to pay {2} for '{1}'",
                            player.GetName(), propertyToTrade.GetName(), amountWanted));
                }
            }
            else 
            {
                agreesToTrade = GetInputYn(playerToTradeWith,
                    string.Format("{0} wants to trade '{1}' with you for ${2}. Do you agree to pay {2} for '{1}'",
                        player.GetName(), propertyToTrade.GetName(), amountWanted));
            }

            //resent console color
            Console.ForegroundColor = origColor;
            if (agreesToTrade)
            {
                Player playerFromBoard = Board.Access().GetPlayer(playerToTradeWith.GetName());
                //player trades property
                player.TradeProperty(ref propertyToTrade, ref playerFromBoard, amountWanted, mortgageInterest);
                Console.WriteLine("{0} has been traded successfully. {0} is now owned by {1}", propertyToTrade.GetName(), playerFromBoard.GetName());
            }
            else
            {
                //display rejection message
                Console.WriteLine("{0}{1} does not agree to trade {2} for ${3}", PlayerPrompt(player), playerToTradeWith.GetName(), propertyToTrade.GetName(), amountWanted);
            }     
        }

        public Property DisplayPropertyChooser(ArrayList properties, String sPrompt, bool forMortgages = false)
        {
            var residentialProps = properties.ToArray().Where(property => property.GetType() == typeof(Residential)).ToArray();

            //if for mortgages and no residential properties return null
            if(forMortgages && !residentialProps.Any())
                return null;

            //if no properties return null
            if (properties.Count == 0)
                return null;

            Console.WriteLine(sPrompt);

            if (!forMortgages)
            {
                for (var i = 0; i < properties.Count; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, properties[i].ToString());
                }
            }
            else// We only want to show them residential properties if this is for mortgaging
            {
                // clear the properties list and add only the residential properties to it
                properties.Clear();
                properties.AddRange(residentialProps);

                for (var i = 0; i < properties.Count; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, properties[i].ToString());
                }
            }

            //display prompt
            Console.Write("({0}-{1})>", 1, properties.Count);
            //get input
            var resp = this.InputInteger();

            //if outside of range
            if ((resp < 1) || (resp > properties.Count))
            {
                Console.WriteLine("That option is not avaliable. Please try again.");
                return DisplayPropertyChooser(properties, sPrompt, forMortgages);
            }

            //return the appropriate property
            return (Property) properties[resp - 1];
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

        public void MortgageOptions(Player player)
        {
            Console.WriteLine();
            Console.WriteLine("1. Mortgage property");
            Console.WriteLine("2. Pay mortgage on property");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("(1-3)>");

            //read response
            var resp = InputInteger();

            Residential property;
            switch (resp)
            {
                case 1:
                    property =
                        (Residential)
                            DisplayPropertyChooser(player.GetPropertiesOwnedFromBoard(), "Property to mortgage: ", true);
                    if (property == null)
                    {
                        Console.WriteLine("You don't own any properties!");
                    }
                    else if (property.MortgageProperty())
                    {
                        Console.WriteLine("Property has been mortgaged for {0}", property.GetMortgageValue());
                    }
                    else // If it wasn't mortgaged it must be developed or already be mortgaged
                    {
                        Console.WriteLine(
                            "Property can't be mortgaged. Either it is already mortgaged or has been developed.");
                    }
                    break;
                case 2:
                    property =
                        (Residential)
                            DisplayPropertyChooser(player.GetPropertiesOwnedFromBoard(), "Property to unmortgage: ",
                                true);

                    if (property == null)
                    {
                        Console.WriteLine("You don't own any properties!");
                    }
                    else if (property.UnmortgageProperty())
                    {
                        Console.WriteLine("Mortgage has been paid for property {0}", property.GetName());
                    }
                    else // If it wasn't unmortgaged it's because it wasn't mortgaged in the first place
                    {
                        Console.WriteLine("Property must be mortgaged for you to pay the mortgage");
                    }
                    break;
                case 3:
                    break;
                default:
                    Console.WriteLine("That option is not avaliable. Please try again.");
                    MortgageOptions(player);
                    break;
            }
        }

        public void SaveGame()
        {
            var fileWriter = new FileWriter();

            // Save the current state of the board
            fileWriter.SaveGame(Board.Access());

            Console.WriteLine("Game Saved!");
        }

        public bool LoadGame()
        {
            var fileReader = new FileReader();

            var banker = fileReader.ReadBankerFromBin();
            // Set the banker to be the banker loaded from the bin file.
            // We need to set the banker before loading the board because
            // we set the banker to be the owner of properties from the board
            // when loading the properties that the banker owned originally
            Banker.SetBankerFromLoadedBanker(banker);

            var board = fileReader.ReadBoardFromBin();

            // The board will be null if the file doesn't exist so we need to cover that case
            // and we need to make sure the board has players otherwise it's an empty board
            if (board != null && board.GetPlayerCount() >= 2)
            {
                // Set the board to be the the board instance loaded from the bin file
                Board.Access().SetBoardFromLoadedBoard(board);


                Console.WriteLine("Game Loaded!");
                return true;
            }

            Console.WriteLine("No game to load!");
            return false;
        }

        private void GetOutOfJail(Player player, bool mustSettle)
        {
            Console.WriteLine();
            Console.WriteLine("1. Pay $50 fine");
            Console.WriteLine("2. Use 'Get Out of Jail Free' card");
            if(!mustSettle) Console.WriteLine("3. Back to Main Menu");
            Console.Write(mustSettle ? "(1-2)>" : "(1-3)>");

            //read response
            var resp = InputInteger();

            //if response is invalid redisplay menu
            if (resp == 0)
                GetOutOfJail(player, mustSettle);

            if (mustSettle && resp != 1 && resp != 2)
            {
                Console.WriteLine("You must get out of Jail");
                GetOutOfJail(player, true);
            }

            switch (resp)
            {
                case 1:
                    Console.WriteLine(player.PayJailFee()
                        ? "You are now out of jail"
                        : "You have insufficient funds and are still in jail");
                    break;
                case 2:
                    if (player.GetOutOfJailCardCount > 0)
                    {
                        player.SetFreeFromJail();
                        player.GetOutOfJailCardCount--;
                        Console.WriteLine("You are now free from jail.");
                        break;
                    }
                    Console.WriteLine("You don't have any Get out of Jail free cards.");
                    if (mustSettle) GetOutOfJail(player, true);
                    break;
                case 3:
                    break;
                default:
                    Console.WriteLine("That option is not avaliable. Please try again.");
                    GetOutOfJail(player, mustSettle);
                    break;
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