using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using IT7302_MonopolyProject_21102588_JoshuaCandish;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ResidentialTests
    {
        private Residential _residentialProperty;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            var residentialFactory = new ResidentialFactory();
            _residentialProperty = residentialFactory.create("Cape Reinga Lighthouse", 140, 14, 100);
            _residentialProperty.GetHouseCost();

            // Add one house to the property
            _residentialProperty.AddHouse();
        }

        [Test]
        public void property_has_house_after_adding_house()
        {

            Assert.IsTrue(_residentialProperty.GetHouseCount() == 1);
        }

        [Test]
        public void rent_for_property_with_one_house_is_correct()
        {
            const decimal rentPlusOneHouse = 28;
            var rentWithHouse = _residentialProperty.GetRent();

            Assert.AreEqual(rentPlusOneHouse, rentWithHouse);
        }

        [Test]
        public void max_houses_should_always_be_4()
        {
            // This is the case for standard monopoly rules
            Assert.AreEqual(4, Residential.GetMaxHouses());
        }

        [Test]
        public void to_string_is_correct()
        {
            var toString = _residentialProperty.ToString();

            Assert.IsTrue(toString.Contains("\t Houses: 1"));
        }

    }
}
