using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Nhap so luong phan tu: ");
            int n = int.Parse(Console.ReadLine());

            int[] arr = new int[n];
            InputArr(arr);
            Console.WriteLine("---------SOLUTION------");
            Solution(arr);

            Console.WriteLine("---------OUTPUT------");
            OutputArr(arr);
        }

        public static void Solution(int[] arr)
        {
            int numberOfMove = 0;
            List<int> list = new List<int>();//luu lai ca phan tu khong ton tai
            for (int i = 0; i < arr.Length; i++)
            {
                list.Add(i + 1);
            }
            List<savePos> savs = new List<savePos>();//luu lai cac phan tu trung va vi tri cua chung trong mang
            int count = list.Count - 1;
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    if(count < list.Count && list[count] == arr[j])
                    {
                        list.RemoveAt(count);
                    }
                    if (i < j && arr[i] == arr[j])
                    {
                        savePos temp = new savePos();
                        temp.index = j;
                        temp.value = arr[j];
                        savs.Add(temp);
                    }
                }
                count--;
            }
            SelectionSort(savs);
            for (int i = 0;i < savs.Count; i++)
            {
                numberOfMove += Math.Abs(savs[i].value - list[i]);
                savePos temp = new savePos();
                temp.value = list[i];
                temp.index = savs[i].index;
                savs[i]= temp;
            }
            for (int i = 0; i < savs.Count; i++)
            {
                arr[savs[i].index] = savs[i].value;
            }
            Console.WriteLine("So lan di chuyen: "+numberOfMove);
        }

        public static void SelectionSort(List<savePos> saves)
        {
            for (int i = 0; i < saves.Count - 1; i++)
            {
                for (int j = i + 1; j < saves.Count; j++)
                {
                    if (saves[i].value >= saves[j].value)
                    {
                        savePos temp = saves[i];
                        saves[i] = saves[j];
                        saves[j] = temp;
                    }
                }
            }
        }
        public static void InputArr(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write("Nhap pham tu {0} la: ", i + 1);
                arr[i] = int.Parse(Console.ReadLine());
                while (arr[i] < 1 || arr[i] > arr.Length)
                {
                    Console.Write("Nhap lai pham tu thu {0}: ", i + 1);
                    arr[i] = int.Parse(Console.ReadLine());
                }
            }
        }

        public static void OutputArr(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i]);
            }
        }
    }
    public struct savePos
    {
        public int index;
        public int value;
    }
}
