using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class PropertyTests
    {
        [Test]
        public void available_for_purchase_is_false_for_basic_property()
        {
            var owner = new Trader();
            var property = new Property("Test Property", ref owner);

            Assert.IsFalse(property.AvailableForPurchase());
        }
    }
}
