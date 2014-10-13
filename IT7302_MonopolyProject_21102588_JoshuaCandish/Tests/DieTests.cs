using System.Linq;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class DieTests
    {
        [Test]
        public void when_rolling_die_it_should_return_a_random_number_between_1_and_6()
        {
            var die = new Die();

            var result = die.Roll();

            Assert.LessOrEqual(result, 6);
            Assert.GreaterOrEqual(result, 1);
        }

        [Test]
        public void when_rolling_die_it_should_always_return_a_random_number_between_1_and_6()
        {
            var die = new Die();

            for (var i = 0; i < 1000000; i++)
            {
                var result = die.Roll();

                Assert.LessOrEqual(result, 6);
                Assert.GreaterOrEqual(result, 1);
            }
        }

        [Test]
        public void each_die_roll_should_occur_with_the_same_chance()
        {
            const int minimumOccurences = 160000;
            const int maximumOccurences = 170000;
            var results = new int[1000000];


            var die = new Die();

            for (var i = 0; i < 1000000; i++)
            {
                results[i] = die.Roll();
            }

            var testResults = results.Where(num => num.Equals(6)).Select(num => num);

            Assert.GreaterOrEqual(testResults.Count(), minimumOccurences);
            Assert.LessOrEqual(testResults.Count(), maximumOccurences);
        }

        [Test]
        public void test_to_string()
        {
            var die = new Die();
            var roll = die.Roll();

            Assert.AreEqual(roll.ToString(), die.ToString());
        }

        [Test]
        public void should_return_the_last_number_rolled()
        {
            var die = new Die();
            var numberRolled = die.Roll();
            var result = die.NumberLastRolled();

            Assert.AreEqual(result, numberRolled);
        }        
    }
}
