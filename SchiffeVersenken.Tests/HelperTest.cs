using NUnit.Framework;
using static SchiffeVersenken.Helper;

namespace SchiffeVersenken.Tests
{
    [TestFixture]
    class HelperTest
    {

        [TestCase]
        public void PointTest()
        {
            Point p0 = new Point { };
            Point p1 = new Point { X = 5, Y = 5 };
            Point p2 = new Point { X = 5, Y = 5 };
            Point p3 = new Point { X = 5, Y = 6 };
            Point p4 = new Point { X = 6, Y = 5 };
            Point p5 = new Point { X = 6, Y = 6 };

            Assert.That(p0.Equals(new Point { }) == true);
            Assert.That(p0.Equals(new Point { X = 0, Y = 0 }) == true);
            Assert.That(p1.Equals(p1) == true);
            Assert.That(p1.Equals(p2) == true);
            Assert.That(p1.Equals(p3) == false);
            Assert.That(p1.Equals(p4) == false);
            Assert.That(p1.Equals(p5) == false);
            Assert.That(p0.Equals(null) == false);
            Assert.That(p0.Equals("Random String") == false);

            Assert.That(p0.Equals(0, 0) == true);
            Assert.That(p1.Equals(p2.X, p2.Y) == true);
            Assert.That(p1.Equals(p3.X, p3.Y) == false);
            Assert.That(p1.Equals(p4.X, p4.Y) == false);
            Assert.That(p1.Equals(p5.X, p5.Y) == false);
            Assert.That(p1.Equals(0, 0) == false);

        }
    }


}
