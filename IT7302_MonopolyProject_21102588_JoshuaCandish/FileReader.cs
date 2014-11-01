using System.Runtime.Serialization.Formatters.Binary;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using System;
using System.Collections.Generic;
using System.IO;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class FileReader
    {
        /// <summary>
        /// Reads property details about each property from a csv
        /// </summary>
        /// <returns>A list of PropertyDetails objects, each representing a specific property to be added to the board</returns>
        public List<PropertyDetails> ReadPropertyDetailsFromCSV()
        {
            try
            {
                var reader = new StreamReader(File.OpenRead("propertydetails.csv"));
                var propertyDetailsFromCSV = new List<PropertyDetails>();

                while (!reader.EndOfStream)
                {
                    var propertyDetailsRow = new PropertyDetails();

                    var line = reader.ReadLine();
                    if (line.Contains("Type")) continue;// Ignore the column headings

                    var values = line.Split(',');

                    // Each type of property will not have a value for every field so
                    // we must check that before assigning it
                    if (!string.IsNullOrEmpty(values[0])) propertyDetailsRow.Type = values[0];
                    if (!string.IsNullOrEmpty(values[1])) propertyDetailsRow.Name = values[1];
                    if (!string.IsNullOrEmpty(values[2])) propertyDetailsRow.Price = Convert.ToDecimal(values[2]);
                    if (!string.IsNullOrEmpty(values[3])) propertyDetailsRow.Rent = Convert.ToDecimal(values[3]);
                    if (!string.IsNullOrEmpty(values[4])) propertyDetailsRow.HouseCost = Convert.ToDecimal(values[4]);
                    if (!string.IsNullOrEmpty(values[5])) propertyDetailsRow.IsPenalty = Convert.ToBoolean(values[5]);
                    if (!string.IsNullOrEmpty(values[6])) propertyDetailsRow.Amount = Convert.ToDecimal(values[6]);
                    if (!string.IsNullOrEmpty(values[7])) propertyDetailsRow.HouseColour = values[7];

                    propertyDetailsFromCSV.Add(propertyDetailsRow);

                }

                return propertyDetailsFromCSV;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Reads the details for chance and community chest cards from a csv file
        /// </summary>
        /// <returns>A list of Luck objects that represent each card</returns>
        public List<Luck> ReadCardDetailsFromCSV()
        {
            try
            {
                var luckFactory = new LuckFactory();
                var reader = new StreamReader(File.OpenRead("carddetails.csv"));
                var cardDetailsFromCSV = new List<Luck>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains("Name")) continue; // Ignore the column headings

                    var values = line.Split(',');

                    var card = luckFactory.create(values[0], Convert.ToBoolean(values[1]), Convert.ToDecimal(values[2]));

                    cardDetailsFromCSV.Add(card);
                }

                return cardDetailsFromCSV;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Loads a saved instance of Board from the SavedBoard.bin file
        /// </summary>
        /// <returns>The board instance</returns>
        public Board ReadBoardFromBin()
        {
            const string fileName = "SavedBoard.bin";
            Board board = null;

            if (File.Exists(fileName))
            {
                Stream testFileStream = File.OpenRead(fileName);
                var deserializer = new BinaryFormatter();
                board = (Board) deserializer.Deserialize(testFileStream);
                testFileStream.Close();

                /*
                 * We need to set the properties on the board that 
                 * were owned by the banker originally to be the banker 
                 * again when reloading the properties
                 */
                foreach (var property in board.GetProperties())
                {
                    var prop = (Property) property;
                    var banker = Banker.Access();

                    if (prop.GetOwner().GetName() != "Leeroy Jenkins") continue;

                    // If it's attached to leeroy who is the banker then
                    // set that propety to be owned by the actual banker
                    var banksProperty = board.GetProperty(prop.GetName());
                    banksProperty.SetOwner(ref banker);
                }
            }

            return board;
        }

        /// <summary>
        /// Loads a saved instance of Banker from the SavedBankerbin file
        /// </summary>
        /// <returns>The banker instance</returns>
        public Banker ReadBankerFromBin()
        {
            const string fileName = "SavedBanker.bin";
            Banker banker = null;

            if (File.Exists(fileName))
            {
                Stream testFileStream = File.OpenRead(fileName);
                var deserializer = new BinaryFormatter();
                banker = (Banker)deserializer.Deserialize(testFileStream);
                testFileStream.Close();
            }

            return banker;
        }

        /// <summary>
        /// Reads the two initial starting values for the banker and player's the from a csv file
        /// </summary>
        /// <returns>An array containing the 2 values, by default it contains 0</returns>
        public decimal[] ReadInitialValuesFromCSV()
        {
            var values = new decimal[] {0, 0};

            try
            {
                var reader = new StreamReader(File.OpenRead("initialvalues.csv"));

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line.Contains("Banker")) continue; // Ignore the column headings

                    var csvValues = line.Split(',');

                    values[0] = Convert.ToDecimal(csvValues[0]);
                    values[1] = Convert.ToDecimal(csvValues[1]);
                }

                return values;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: {0}", ex.Message);
            }

            return values;
        }
    }
}
