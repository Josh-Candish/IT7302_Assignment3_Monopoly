using System;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Creator
    {
        public void CreateProperties()
        {
            var resFactory = new ResidentialFactory();
            var transFactory = new TransportFactory();
            var utilFactory = new UtilityFactory();
            var genericFactory = new PropertyFactory();
            var luckFactory = new LuckFactory();


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
                                    propertyDetail.Rent, propertyDetail.HouseCost, propertyDetail.HouseColour));
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

        public void CreateCards()
        {
            try
            {
                var fileReader = new FileReader();
                var cards = fileReader.ReadCardDetailsFromCSV();

                foreach (var card in cards)
                {
                    if (card.GetName().Contains("Chance"))
                    {
                        Board.Access().AddChanceCard(card);
                    }

                    if (card.GetName().Contains("Community Chest"))
                    {
                        Board.Access().AddCommunityChestCard(card);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops, something went wrong setting up the cards: {0}", ex.Message);
            }
        }
    }
}
