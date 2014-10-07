using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IT7302_MonopolyProject_21102588_JoshuaCandish;
using NUnit.Framework;

namespace Tests
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
