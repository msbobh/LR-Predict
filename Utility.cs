using System;
using Accord.Math;

namespace Functions
{
    class funcs
    {
        static public int[] BoolToInt(bool[] input)
        {
            int[] result = new int[input.Rows()];
            int count = 0;
            foreach (var value in input)
            {
                result[count] = Convert.ToInt32(value);
                count++;
            }

            return result;
        }

        static public int[] convetToJaggedArray(double[,] inputmatrix)
        {
            int numOfRows = inputmatrix.Rows();
            int[] converted = new int[numOfRows];
            for (int r = 0; r < numOfRows; r++)
            {
                converted[r] = (int)inputmatrix[r, 0];
            }
            return converted;
        }
    }
}