using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class FileReader
    {
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

                    propertyDetailsFromCSV.Add(propertyDetailsRow);

                }

                return propertyDetailsFromCSV;
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong importing the property details file.");
                return null;
            }
        }
    }
}
