using System;
/*******************************
 * Created By: Jon Yade
 * ****************************/

namespace CAA_Event_Management.Utilities
{
    /// <summary>
    /// Class to check Membership number to Luhn Algorithm
    /// </summary>
    internal class MemberNumberCheck
    {
        /// <summary>
        /// Method to Check Member Number
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <returns>boolean</returns>
        internal bool CheckMemberNumber(string memberNumber)
        {
            double[] number = new double[16];

            try
            {
                double numberCheck1 = Convert.ToDouble(memberNumber);
            }
            catch
            {
                return false;
            }

            int reverseNumberCount = 0;
            for (int i = 15; i >= 0; i--)
            {
                number[reverseNumberCount] = Convert.ToDouble(memberNumber.Substring(i, 1));
                reverseNumberCount++;
            }

            double runningTotal = 0;
            for (int i = 1; i < 16; i++)
            {
                if (i % 2 != 0)
                {
                    double doubleNumber = number[i] * 2;
                    if (doubleNumber > 9) doubleNumber -= 9;
                    runningTotal += doubleNumber;
                }
                else runningTotal += number[i];
            }
            //if (runningTotal % 10 != 0) return false;   <<Possibly add this or delete it later; ask CAA about it

            runningTotal = runningTotal * 9;
            double finalCheck = runningTotal % 10;

            if (finalCheck == number[0]) return true;
            else return false;
        }
    }
}
