using SevenDaysProfileEditor.Inventory;
using System.Linq;

namespace SevenDaysProfileEditor
{
    class Util
    {
        public static int normalize(int input, int min, int max)
        {
            if (input > max)
            {
                return max;
            }

            if (input < min)
            {
                return min;
            }

            return input;
        }

        public static string[] quicksort(string[] array, int start, int end)
        {
            if (start < end)
            {
                int pivotPosition = partition(array, start, end);
                quicksort(array, start, pivotPosition);
                quicksort(array, pivotPosition + 1, end);
            }

            return array;
        }

        private static int partition(string[] array, int start, int end)
        {
            int i = start + 1;
            string piv = array[start];

            for (int j = start + 1; j < end; j++)
            {
                int length = array.Length;

                if (compare(array[j], piv))
                {
                    swap(array, i, j);
                    i += 1;
                }
            }

            swap(array, start, i - 1);
            return i - 1;
        }

        private static void swap(string[] array, int one, int two)
        {
            string temp = array[one];
            array[one] = array[two];
            array[two] = temp;
        }

        private static bool compare(string smaller, string bigger)
        {
            smaller += " ";
            bigger += " ";

            int i = 0;

            while (true)
            {
                int big = bigger.ElementAt(i);
                if (big > 64 && big < 91)
                {
                    big += 32;
                }

                int small = smaller.ElementAt(i);
                if (small > 64 && small < 91)
                {
                    small += 32;
                }

                if (big > small)
                {
                    return true;
                }

                else if (big < small)
                {
                    return false;
                }

                else
                {
                    i++;
                }
            }
        }

        public static string[] pulverizeString(string toPulverize)
        {
            int count = 1;

            for (int i = 0; i < toPulverize.Length; i++)
            {
                if (toPulverize[i] == ',')
                {
                    count++;
                }
            }

            string[] stringBits = new string[count];

            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    stringBits[i] = toPulverize;
                }

                else
                {
                    stringBits[i] = toPulverize.Substring(0, toPulverize.IndexOf(','));
                    toPulverize = toPulverize.Substring(toPulverize.IndexOf(',') + 2);
                }
            }

            return stringBits;
        }

    }
}
