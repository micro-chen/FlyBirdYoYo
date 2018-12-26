using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class SortObjects
    {

        #region  调用示例：
        //{ int[] test7 = { 21, 13, 321, 231, 43, 7, 65, 18, 48, 6 };  
    //        heapsort(test7, 0, 9);                                         //堆排序  
    //        foreach (int a in test7)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  
  
    //        int[] test6 = { 21, 13, 321, 231, 43, 7, 65, 18, 48, 6 };  
    //        radixsort(test6, 0, 9, 2);                                         //基数排序（第4个参数是数组中最大数的10的最大次瞑）  
    //        foreach (int a in test6)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  
  
    //        int[] test0 = { 21, 13, 321, 231, 43, 7, 65, 18, 48, 6 };           
    //        insertsort(test0, 10);                                             //插入排序  
    //        foreach (int a in test0)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  
  
    //       int[] test1 = { 21, 13,321, 231, 43, 7, 65, 18, 48, 6 };  
    //        newinsertsort(test1, 10);                                          //折半插入排序  
    //        foreach (int a in test1)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  
  
    //             int[] test2 = { 21, 13,321, 231, 43, 7, 65, 18, 48, 6 };  
    //        shellsort(test2, 10);                                             //希尔排序  
    //        foreach (int a in test2)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  
  
    //        int[] test3 = { 21, 13, 321, 231, 43, 7, 65, 18, 48, 6 };  
    //        paopaosort(test3, 10);                                            //冒泡排序  
    //        foreach (int a in test3)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  
  
    //        int[] test4 = { 21, 13, 321, 231, 43, 7, 65, 18, 48, 6 };  
    //        fastsort(test4, 0, 9);                                            //快速排序  
    //        foreach (int a in test4)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  
  
    //        int[] test5 = { 21, 13, 321, 231, 43, 7, 65, 18, 48, 6 };  
    //        selectsort(test5, 10);                                           //选择排序  
    //        foreach (int a in test5)  
    //            Console.Write(a.ToString().PadRight(4));  
    //        Console.WriteLine();  

        //        Console.Read();  

        #endregion


        /// <summary>
        /// 堆排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public  static void HeapSort(int[] array, int begin, int end)          
        {
            int temp, i, j, length;
            for (i = (end - begin) / 2; i >= begin; i--)                                             //建初堆  
            {
                j = i;
                while (j <= (end - begin) / 2)
                {
                    if ((2 * j + 2) <= end)
                    {
                        if (array[2 * j + 1] > array[2 * j + 2] && array[2 * j + 1] > array[j])
                        {
                            temp = array[2 * j + 1];
                            array[2 * j + 1] = array[j];
                            array[j] = temp;
                            j = 2 * j + 1;
                        }
                        else if (array[2 * j + 1] < array[2 * j + 2] && array[2 * j + 2] > array[j])
                        {
                            temp = array[j];
                            array[j] = array[2 * j + 2];
                            array[2 * j + 2] = temp;
                            j = 2 * j + 2;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (array[2 * j + 1] > array[j])
                        {
                            temp = array[2 * j + 1];
                            array[2 * j + 1] = array[j];
                            array[j] = temp;
                            j = 2 * j + 1;
                        }
                        break;
                    }
                }
            }
            for (length = end; length > begin; length--)                              //首尾交换  
            {
                temp = array[length];
                array[length] = array[0];
                array[0] = temp;
                j = 0;
                while (j < (length - begin - 1) / 2)                                   //调整堆  
                {
                    if ((2 * j + 2) <= end)
                    {
                        if (array[2 * j + 1] > array[2 * j + 2] && array[2 * j + 1] > array[j])
                        {
                            temp = array[2 * j + 1];
                            array[2 * j + 1] = array[j];
                            array[j] = temp;
                            j = 2 * j + 1;
                        }
                        else if (array[2 * j + 1] < array[2 * j + 2] && array[2 * j + 2] > array[j])
                        {
                            temp = array[j];
                            array[j] = array[2 * j + 2];
                            array[2 * j + 2] = temp;
                            j = 2 * j + 2;
                        }
                        else
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 直接插入排序  
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        public static void InsertSort(int[] array, int length)   
        {
            int i, j, temp;
            for (i = 1; i < length; i++)
            {
                temp = array[i];
                j = i - 1;
                while (temp < array[j])
                {
                    array[j + 1] = array[j];
                    j = j - 1;
                    if (j == -1)
                    {
                        break;
                    }
                }
                array[j + 1] = temp;
            }
        }

        /// <summary>
        /// 折半插入排序  
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        static public void NewInsertSort(int[] array, int length)  
        {
            int low, high, i, j, temp;
            for (i = 1; i < length; i++)
            {
                temp = array[i];
                low = 0; high = i - 1;
                j = (high - low) / 2;
                while (low <= high)
                {
                    if (low == high)
                    {
                        if (array[0] > temp)
                            j = 0;
                        else
                            j = 1;
                        break;
                    }
                    else if (low == high - 1)
                    {
                        if (array[j + 1] < temp)
                            j += 2;
                        else if (array[j] < temp)
                            j++;
                        break;
                    }
                    else if (array[j] < temp)
                    {
                        low = j;
                        j += (high - low) / 2;
                    }
                    else if (array[j] > temp)
                    {
                        high = j;
                        j = low + (high - low) / 2;
                    }
                    else
                        break;
                }
                for (int n = i - 1; n >= j; n--)
                    array[n + 1] = array[n];
                array[j] = temp;
            }
        }

        /// <summary>
        /// 希尔排序（基于直接插入排序）  
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        static public void ShellSort(int[] array, int length)                 
        {
            int i, j, k, delta = length / 2, temp;
            while (delta != 0)                                                 //delte为分组递增值  
            {
                for (i = 0; i < delta; i++)
                {
                    for (j = i + delta; j < length; j += delta)
                    {
                        temp = array[j];
                        k = j - delta;
                        while (temp < array[k])                             //对每组进行直接插入排序  
                        {
                            array[k + delta] = array[k];
                            k = k - delta;
                            if (k == i - delta)
                            {
                                break;
                            }
                        }
                        array[k + delta] = temp;
                    }
                    j -= delta;
                    if (array[j] < array[i])                              //2组之间首位进行交换排序  
                    {
                        temp = array[j];
                        array[j] = array[j];
                        array[j] = temp;
                    }
                }
                delta /= 2;
            }
        }

        /// <summary>
        /// 冒泡排序  
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        public static void PaopaoSort(int[] array, int length)          
        {
            int i, j, temp;
            j = length;
            while (j != 0)
            {
                for (i = 0; i < j - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;
                    }
                }
                j--;
            }
        }

        /// <summary>
        /// 快速排序  
        /// </summary>
        /// <param name="array"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        static public void FastSort(int[] array, int begin, int end)     
        {
            if (begin < 0 || end < 0 || begin > end)
                return;
            int left = begin, right = end, temp;                            //经典的快排  
            temp = array[left];
            while (right != left)
            {
                while (temp < array[right] && right > left)
                    right--;
                if (right > left)
                {
                    array[left] = array[right];
                    left++;
                }
                while (temp > array[left] && right > left)
                    left++;
                if (right > left)
                {
                    array[right] = array[left];
                    right--;
                }
            }
            array[right] = temp;
            FastSort(array, right + 1, end);
            FastSort(array, begin, right - 1);
        }


        /// <summary>
        /// 选择排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        static public void SelectSort(int[] array, int length)        
        {
            int i = 0, j, min, temp_array, temp;
            while (i < length - 1)
            {
                min = array[i];
                temp = i;
                for (j = i + 1; j < length; j++)
                {
                    if (array[j] < min)
                    {
                        min = array[j];
                        temp = j;
                    }
                }
                temp_array = array[i];
                array[i] = array[temp];
                array[temp] = temp_array;
                i++;
            }
        }

        /// <summary>
        /// 基数排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="begin"></param>
        /// <param name="last"></param>
        /// <param name="pow"></param>
        public static void RadixSort(int[] array, int begin, int last, int pow)      
        {
            Queue<int>[] queue = new Queue<int>[10];                               //利用泛型队列来存储筛选分组  
            queue[0] = new Queue<int>();
            queue[1] = new Queue<int>();
            queue[2] = new Queue<int>();
            queue[3] = new Queue<int>();
            queue[4] = new Queue<int>();
            queue[5] = new Queue<int>();
            queue[6] = new Queue<int>();
            queue[7] = new Queue<int>();
            queue[8] = new Queue<int>();
            queue[9] = new Queue<int>();
            int[] nn = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x, p = pow, n, i;
            while (p >= 0)
            {
                for (i = begin; i <= last; i++)
                {
                    int sum = array[i];
                    n = pow - p;
                    while (n != 0)
                    {
                        sum = sum / 10;
                        n--;
                    }
                    sum = sum % 10;
                    switch (sum)
                    {
                        case 0: queue[0].Enqueue(array[i]); nn[0]++; break;
                        case 1: queue[1].Enqueue(array[i]); nn[1]++; break;
                        case 2: queue[2].Enqueue(array[i]); nn[2]++; break;
                        case 3: queue[3].Enqueue(array[i]); nn[3]++; break;
                        case 4: queue[4].Enqueue(array[i]); nn[4]++; break;
                        case 5: queue[5].Enqueue(array[i]); nn[5]++; break;
                        case 6: queue[6].Enqueue(array[i]); nn[6]++; break;
                        case 7: queue[7].Enqueue(array[i]); nn[7]++; break;
                        case 8: queue[8].Enqueue(array[i]); nn[8]++; break;
                        case 9: queue[9].Enqueue(array[i]); nn[9]++; break;
                    }
                }  //for  
                x = n = 0;
                for (i = 0; i < 10; i++)
                {
                    n = n + x;
                    x = nn[i];
                    while (nn[i] != 0)
                    {
                        array[n + x - nn[i]] = queue[i].Peek();
                        queue[i].Dequeue();
                        nn[i]--;
                    }
                }
                p--;
            }    //while  
        }





    }

}
