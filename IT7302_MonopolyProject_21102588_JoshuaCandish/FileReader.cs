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
            }

            return board;
        }
    }
}
