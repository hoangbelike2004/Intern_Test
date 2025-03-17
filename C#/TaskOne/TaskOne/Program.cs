using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nhap chuoi: ");
            string str = Console.ReadLine();
            Solution(str);
        }
        public static void Solution(string str)
        {
            while (true)
            {
                if (str.Length < 2 || str.Length > 100000 || Regex.IsMatch(str, @"[\u00C0-\u1EF9]") || Isnumber(str))
                {
                    Console.Write("Nhap sai! nhap lai: ");
                    str = Console.ReadLine();
                }
                else
                {
                    str = str.ToLower();
                    break;
                }
            }
            for (int i = 0; i < str.Length - 1; i++)
            {
                if ((int)str[i] > (int)str[i + 1])
                {
                    str = str.Remove(i, 1);
                    break;
                }
                else if (i + 1 == str.Length - 1)
                {
                    str = str.Remove(i + 1, 1);
                }
            }
            Console.WriteLine(str);
        }
        public static bool Isnumber(string str)
        {
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
