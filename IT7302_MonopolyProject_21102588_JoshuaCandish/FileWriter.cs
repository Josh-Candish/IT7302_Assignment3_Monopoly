﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class FileWriter
    {
        /// <summary>
        /// Serializes and saves the current state of the supplied board to a .bin file
        /// </summary>
        /// <param name="board"></param>
        public void SaveGame(Board board)
        {
            Stream testFileStream = File.Create("SavedBoard.bin");
            var serializer = new BinaryFormatter();
            serializer.Serialize(testFileStream, board);
            testFileStream.Close();

            SaveBanker();
        }

        // Need to save the banker's current state as well
        private static void SaveBanker()
        {
            Stream testFileStream = File.Create("SavedBanker.bin");
            var serializer = new BinaryFormatter();
            serializer.Serialize(testFileStream, Banker.Access());
            testFileStream.Close();
        }
    }
}
