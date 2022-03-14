using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels.Tests
{
    [TestClass()]
    public class ScheduleDistanceFinderTests
    {
        private static IDistanceFinder<Schedules> distanceFinder;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            distanceFinder = new ScheduleDistanceFinder();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            distanceFinder = null;
        }

        [TestMethod()]
        public void GetNumberOfStops_FromSchedule92ToSchedule93_Returns1()
        {
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                #region arrange
                int expected = 1;
                #endregion

                #region act
                int actual = distanceFinder
                    .GetNumberOfStops(context.Schedules.Find(92),
                                      context.Schedules.Find(93));
                #endregion

                #region assert
                Assert.AreEqual(expected, actual);
                #endregion
            }
        }

        [TestMethod()]
        public void ToString_FromSchedule92ToSchedule93_Returns92And107()
        {
            #region arrange
            string expected = "[92] - [107]";
            #endregion

            #region act
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                distanceFinder
                    .GetNumberOfStops(context.Schedules.Find(92),
                                      context.Schedules.Find(93));
            }
            string actual = distanceFinder.ToString();
            #endregion

            #region assert
            Assert.AreEqual(expected, actual);
            #endregion
        }
    }
}