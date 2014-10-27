using System.IO;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class FileWriterTests
    {
        [Test]
        public void file_is_written()
        {
            var fileWriter = new FileWriter();
            fileWriter.SaveGame(Board.Access());

            Assert.IsTrue(File.Exists("SavedBoard.bin"));
        }
    }
}
