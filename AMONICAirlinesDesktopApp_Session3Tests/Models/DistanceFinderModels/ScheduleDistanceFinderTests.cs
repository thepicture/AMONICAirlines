using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels.Tests
{
    [TestClass()]
    public class ScheduleDistanceFinderTests
    {
        private static IDistanceFinder<string> distanceFinder;

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
        public void GetNumberOfStops_FromBahToDohFromYYYY2017MM10dd04_Returns1()
        {
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                #region arrange
                int expected = 1;
                #endregion

                #region act
                int actual = distanceFinder
                    .GetNumberOfStops("BAH",
                                      "DOH",
                                      DateTime.Parse("2017-10-04"));
                #endregion

                #region assert
                Assert.AreEqual(expected, actual);
                #endregion
            }
        }

        [TestMethod()]
        public void GetNumberOfStops_FromBahToDohFromYYYY2017MM10dd04_ReturnsNegativeOne()
        {
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                #region arrange
                int expected = -1;
                #endregion

                #region act
                int actual = distanceFinder
                    .GetNumberOfStops("BAH",
                                      "DOH",
                                      DateTime.Parse("2017-10-28"));
                #endregion

                #region assert
                Assert.AreEqual(expected, actual);
                #endregion
            }
        }

        [TestMethod()]
        public void GetNumberOfStops_FromRuhToAuhFromYYYY2017MM10dd04_Returns0()
        {
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                #region arrange
                int expected = 0;
                #endregion

                #region act
                int actual = distanceFinder
                    .GetNumberOfStops("RUH",
                                      "AUH",
                                      DateTime.Parse("2017-10-04"));
                #endregion

                #region assert
                Assert.AreEqual(expected, actual);
                #endregion
            }
        }

        [TestMethod()]
        public void GetNumberOfStops_FromAuhToBahFromYYYY2017MM10dd29_ReturnsNegativeOne()
        {
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                #region arrange
                int expected = -1;
                #endregion

                #region act
                int actual = distanceFinder
                    .GetNumberOfStops("AUH",
                                      "BAH",
                                      DateTime.Parse("2017-10-29"));
                #endregion

                #region assert
                Assert.AreEqual(expected, actual);
                #endregion
            }
        }

        [TestMethod()]
        public void ToString_FromBahToDohFromYYYY2017MM10dd04_Returns50And89()
        {
            #region arrange
            string expected = "[50] - [89]";
            #endregion

            #region act
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                distanceFinder
                    .GetNumberOfStops("BAH",
                                      "DOH",
                                      DateTime.Parse("2017-10-04"));
            }
            string actual = distanceFinder.ToString();
            #endregion

            #region assert
            Assert.AreEqual(expected, actual);
            #endregion
        }

        [TestMethod()]
        public void ToString_FromBahToDohFromYYYY2017MM10dd28_ReturnsSquareBrackets()
        {
            #region arrange
            string expected = "[]";
            #endregion

            #region act
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                distanceFinder
                    .GetNumberOfStops("BAH",
                                      "DOH",
                                      DateTime.Parse("2017-10-28"));
            }
            string actual = distanceFinder.ToString();
            #endregion

            #region assert
            Assert.AreEqual(expected, actual);
            #endregion
        }

        [TestMethod()]
        public void ToString_FromRuhToAuhFromYYYY2017MM10dd04_Returns76()
        {
            #region arrange
            string expected = "[76]";
            #endregion

            #region act
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                distanceFinder
                    .GetNumberOfStops("RUH",
                                      "AUH",
                                      DateTime.Parse("2017-10-04"));
            }
            string actual = distanceFinder.ToString();
            #endregion

            #region assert
            Assert.AreEqual(expected, actual);
            #endregion
        }
    }
}