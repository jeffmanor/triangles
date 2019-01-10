using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class TrianglesService
    {
        /// <summary>
        /// I'm not sure if the coords are supposed to be ordered or not... I guess I'll assume they can be in any order.
        /// </summary>
        public string GetTriangleFromVertices(List<Tuple<int, int>> coords)
        {
            coords = coords.OrderBy(coord => coord.Item1).ThenBy(coord => coord.Item2).ToList();

            // Now they're in order.  The upper left one will be first, followed by the right angle, followed by the lower right.

            // ... ought to do some validation
            Assert(coords[2].Item1 - coords[0].Item1 == 10);
            Assert(coords[2].Item2 - coords[0].Item2 == 10);

            // Been an age since I've used an xor operator.  We need either x or y from the right angle coord to be 10 less than the coord in the lower right,
            // and the other one has to be equal to the x/y in the lower right pair.  We'll just figure out now if this is the lower left half of 
            // the square(A1, A3, A5, A7, A9, A11, B1...) or if we have the triangle in the upper right (A2, A4, A6, A8, A10, A12, B2...)

            var isLowerLeft = coords[2].Item1 - coords[1].Item1 == 10
                && coords[2].Item2 - coords[1].Item2 == 0;
            var isUpperRight = coords[2].Item1 - coords[1].Item1 == 0
                && coords[2].Item2 - coords[1].Item2 == 10;

            Assert(isLowerLeft ^ isUpperRight);

            // Not only do all the differences have to line up, we also need to make sure the upper left coord's x/y are <= (50, 50) and evenly
            // divisible by 10:

            Assert(coords[0].Item1 <= 50);
            Assert(coords[0].Item1 % 10 == 0);
            Assert(coords[0].Item2 <= 50);
            Assert(coords[0].Item2 % 10 == 0);

            // If we made it this far, we have a valid set of data

            // Turning ints into chars.  Suddenly this feels like C++.  Maybe I should have used a dictionary.
            char row = Convert.ToChar(Convert.ToInt32('A') + (coords[0].Item2 / 10));

            // Divide by 10 to change pixels to 'squares'
            // Multiply by two since there are two triangles (columns) per square
            // Add one to get out of the land of zero-based
            // Add one if the square is in the upper right (determined earlier)
            int col = (coords[0].Item1 / 10) * 2 + (isUpperRight ? 1 : 0) + 1;


            return $"{row}{col}";
        }

        private void Assert(bool condition)
        {
            if (!condition)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Given a row (A-F) and a column (1-12), return the vertices of the triangle.
        /// </summary>
        public TriangleResult GetVertices(string rowText, int column)
        {

            // Turn the row variable into a number:
            int row = Convert.ToInt32(rowText[0]) - Convert.ToInt32('A');

            // The square that we're inside of is shared by A1/A2, A3/A4, so we can get the column of the box by dividing by 2.
            // "col" refers to the 6 rectangular columns now.  This is useful because A1/A2 share their upper left and lower right coordinates
            int col = (column - 1) / 2;

            if (row < 0 || row > 5 || col < 0 || col > 5)
            {
                // Maybe log something here if needed.  This is kind of a lazy way of keeping the view model from being polluted w/ success/fail flags and handling them.
                throw new Exception();
            }

            // So now we have enough to get the upper left and lower right coords.  We'll put those in (x2, y2) and (x3, y3) in order to match
            // the naming convention established on page 3 of the pdf:

            var result = new TriangleResult
            {
                x2 = col * 10,
                y2 = row * 10,

                x3 = (col + 1) * 10,
                y3 = (row + 1) * 10
            };

            // The last thing to do is establish if we're looking at a triangle that occupies the lower left half of the square (A1, A3, A5, A7, A9, A11, B1...)
            // or if we're looking at the triangle in the upper right (A2, A4, A6, A8, A10, A12, B2...).  If it's odd, we want x1 to be set to the same value as x2 and y1 
            // to be equal to y3.  If it's even, we want x1 to equal x3 and y1 to equal y2.

            result.x1 = column % 2 == 1 ? result.x2 : result.x3;
            result.y1 = column % 2 == 1 ? result.y3 : result.y2;

            return result;
        }
    }
}