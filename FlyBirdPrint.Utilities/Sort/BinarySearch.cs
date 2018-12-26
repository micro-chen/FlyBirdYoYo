using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
    /// 二叉树检索
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinarySearch<T>
    {
        /// <summary>
        /// Find the index of value close to.
        /// The index of value is equal or great then the value's index;
        /// Array = 1,3,5 value = 2, index = 1
        /// This function used for cycle buffer that from start to start - 1.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="start"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static int FindClosedIndex(T[] array, int start, T value, IComparer<T> comparer)
        {
            T[] objArray = array;
            int length = array.Length;
            int lo = 0; //Relative position
            int hi = length - 1; //Relative position

            while (lo <= hi)
            {
                // i might overflow if lo and hi are both large positive numbers.
                int i = lo + ((hi - lo) >> 1);

                int ai = start + i;
                if (ai >= length)
                {
                    ai = ai - length;
                }

                int c = comparer.Compare(objArray[ai], value);

                if (c == 0)
                {
                    return ai;
                }

                if (c < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            lo += start;

            if (lo >= length)
            {
                return lo - length;
            }
            else
            {
                return lo;
            }
        }

        /// <summary>
        /// Find the index of value close to.
        /// The index of value is equal or great then the value's index;
        /// Array = 1,3,5 value = 2, index = 1
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static int FindClosedIndex(T[] array, int index, int length, T value, IComparer<T> comparer)
        {
            T[] objArray = array;

            int lo = index;
            int hi = index + length - 1;

            if (index + length > array.Length)
            {
                throw new System.ArgumentException("length + index > array.Length");
            }

            while (lo <= hi)
            {
                // i might overflow if lo and hi are both large positive numbers.
                int i = lo + ((hi - lo) >> 1);

                int c = comparer.Compare(objArray[i], value);

                if (c == 0) return i;
                if (c < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            return lo;
        }

    }
}
