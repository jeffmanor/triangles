using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Services;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private TrianglesService TrianglesService => new TrianglesService();

        /// <summary>
        /// The two services are basically inverses...
        /// </summary>
        [TestMethod]
        public void TestServiceIntegrity()
        {
            // Create the full set of all 72 triangles (A1...F12), run them through the service that converts row/col to coords, then
            // take the coords and run them through the second service.  We should get the original A1...F12 string back in the end.
            for (int i = 0; i < 6; i++)
            {
                for (int j = 1; j < 13; j++)
                {
                    // Turn i into A-F
                    var row = Convert.ToChar(Convert.ToInt32('A') + i).ToString();

                    // This will get us the three vertices
                    var result = TrianglesService.GetVertices(row, j);

                    // This is the string we expect to get back
                    var expected = $"{row}{j}";

                    // Run the vertices through the GetTriangleFromVertices service
                    var actual = TrianglesService.GetTriangleFromVertices(
                        new List<Tuple<int, int>> { new Tuple<int, int>(result.x1, result.y1),
                            new Tuple<int, int>(result.x2, result.y2),
                            new Tuple<int, int>(result.x3, result.y3)
                        });

                    // We should be back where we started (A1...F12) now
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        private class TestCase
        {
            public string Row { get; set; }
            public int Col { get; set; }
            public int x1 { get; set; }
            public int y1 { get; set; }
            public int x2 { get; set; }
            public int y2 { get; set; }
            public int x3 { get; set; }
            public int y3 { get; set; }
            public string ExpectedRowCol { get; set; }
        }


        /// <summary>
        /// GetVertices UT
        /// </summary>
        [TestMethod]
        public void TestGetVertices()
        {
            // ... I miss my [TestCase] attribute right now

            var testCases = new List<TestCase>() {
                new TestCase() { Row = "A", Col = 1, x1 = 0, y1 = 10, x2 = 0, y2 = 0, x3 = 10, y3 = 10 },
                new TestCase() { Row = "D", Col = 3, x1 = 10, y1 = 40, x2 = 10, y2 = 30, x3 = 20, y3 = 40 },
                new TestCase() { Row = "B", Col = 10, x1 = 50, y1 = 10, x2 = 40, y2 = 10, x3 = 50, y3 = 20 },
                new TestCase() { Row = "F", Col = 12, x1 = 60, y1 = 50, x2 = 50, y2 = 50, x3 = 60, y3 = 60 }
            };

            // ...Honestly, if I wanted to put more time into this I'd probably just add them all.  I think you get the idea.

            foreach (var testCase in testCases)
            {
                var result = TrianglesService.GetVertices(testCase.Row, testCase.Col);
                Assert.AreEqual(testCase.x1, result.x1);
                Assert.AreEqual(testCase.y1, result.y1);
                Assert.AreEqual(testCase.x2, result.x2);
                Assert.AreEqual(testCase.y2, result.y2);
                Assert.AreEqual(testCase.x3, result.x3);
                Assert.AreEqual(testCase.y3, result.y3);
            }

            Assert.ThrowsException<Exception>(() => TrianglesService.GetVertices("H", 1));
            Assert.ThrowsException<Exception>(() => TrianglesService.GetVertices("A", 13));
        }

        /// <summary>
        /// GetTriangleFromVertices UT
        /// </summary>
        [TestMethod]
        public void TestGetTriangleFromVertices()
        {
            var testCases = new List<TestCase>()
            {
                new TestCase() {x1 = 0, y1 = 10, x2 = 0, y2 = 0, x3 = 10, y3 = 10, ExpectedRowCol="A1" },
                new TestCase() {x1 = 20, y1 = 20, x2 = 10, y2 = 20, x3 = 20, y3 = 30, ExpectedRowCol="C4" },
                new TestCase() {x1 = 40, y1 = 50, x2 = 40, y2 = 40, x3 = 50, y3 = 50, ExpectedRowCol="E9" },
                new TestCase() {x1 = 60, y1 = 50, x2 = 50, y2 = 50, x3 = 60, y3 = 60, ExpectedRowCol="F12" },
            };

            foreach (var testCase in testCases)
            {
                Assert.AreEqual(testCase.ExpectedRowCol,
                    TrianglesService.GetTriangleFromVertices(new List<Tuple<int, int>>() {
                        new Tuple<int, int>(testCase.x1, testCase.y1),
                        new Tuple<int, int>(testCase.x2, testCase.y2),
                        new Tuple<int, int>(testCase.x3, testCase.y3)
                    }));
            }


            // Failure cases aren't very exhaustive either... but I hope you get the idea.
            Assert.ThrowsException<Exception>(() => TrianglesService.GetTriangleFromVertices(new List<Tuple<int, int>>() {
                        new Tuple<int, int>(1, 2),
                        new Tuple<int, int>(3, 4),
                        new Tuple<int, int>(5, 6)
                    }));

            Assert.ThrowsException<Exception>(() => TrianglesService.GetTriangleFromVertices(new List<Tuple<int, int>>() {
                        new Tuple<int, int>(0, 10),
                        new Tuple<int, int>(10, 0),
                        new Tuple<int, int>(10, 10)
                    }));
        }
    }
}
