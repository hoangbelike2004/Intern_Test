using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTwo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Nhap so hang: ");
            int n = int.Parse(Console.ReadLine());
            Console.Write("Nhap so cot: ");
            int m = int.Parse(Console.ReadLine());
            while(n < 2 || m < 2)
            {
                Console.Write("Nhap so hang: ");
                n = int.Parse(Console.ReadLine());
                Console.Write("Nhap so cot: ");
                m = int.Parse(Console.ReadLine());
            }
            int[,] arr = new int[n, m];
            Console.WriteLine("-----------------------INPUT------------------------");
            InputArr(n, m, arr);
            Console.WriteLine("-----------------------SOLUTION------------------------");
            Console.WriteLine("Tong lon nhat: " +Solution(arr));
            Console.WriteLine("-----------------------OUTPUT------------------------");
            OutputArr(arr);
        }
        public static int Solution(int[,] arr)
        {
            int[] danhdau = new int[arr.GetLength(0) + arr.GetLength(1)];
            int sumRow = 0,sumCol = 0;
            //danh dau ca cot hay hang
            Console.WriteLine("-------CAC GIA TRI DUOC CHON KHI DUYET THEO HANG-----------");
            for (int i = 0; i < arr.GetLength(0); i++)//duyet theo hang
            {
                int tmp = 0;
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (tmp <= arr[i, j] && IsAttack(j,i, danhdau))
                    {
                        danhdau[i] = j;//save pos
                        tmp = arr[i, j];
                    }
                }
                Console.WriteLine(tmp);
                sumRow += tmp;
            }
            
            Console.WriteLine("-------CAC GIA TRI DUOC CHON KHI DUYET THEO COT-----------");
            for (int i = 0; i < arr.GetLength(1); i++)//duyet theo cot
            {
                int tmp = 0;
                for (int j = 0; j < arr.GetLength(0); j++)
                {
                    if (tmp <= arr[j,i] && IsAttack(j, i, danhdau))
                    {
                        danhdau[i] = j;//save pos
                        tmp = arr[j,i];
                    }
                }
                Console.WriteLine(tmp);
                sumCol += tmp;
            }
            return sumRow >= sumCol ? sumRow : sumCol;
        }
        public static bool IsAttack(int pos,int index, int[] danhdau)
        {
            for (int i = 0; i < index; i++)
            {
                if (danhdau[i] == pos)//khi duyet theo hang ma ton tai 2 xe tren 1 cot
                {
                    return false;
                }
            }
            return true;
        }
        public static void InputArr(int row, int column, int[,] arr)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    arr[i, j] = int.Parse(Console.ReadLine());
                }
            }
        }

        public static void OutputArr(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
