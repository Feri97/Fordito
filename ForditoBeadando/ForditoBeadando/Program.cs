using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ForditoBeadando
{
    class Program
    {
        public static string[,] rules = new string[12, 7];

        public static List<string> number = new List<string>();
        public static string input;
        public static int index = 0;
        public static bool end = false;

        public static Stack stack;
        public static string sAct;

        public static void Evaluate(string data)
        {
            if (data.Trim().Length == 0)
            {
                Console.Write("Az input elutasítva.");
                end = true;
            }

            if (data.Trim() == "elfogad")
            {
                Console.Write("Az input elfogadva.");
                end = true;
            }

            if (data.Trim() == "pop")
            {
                index++;
            }

            if (data.Contains('(') && data.Contains(')'))
            {
                string datas = data.Substring(1).Split(',')[0];
                for (int i = datas.Length - 1; i >= 0; i--)
                {
                    if(datas[i].ToString() != "e")
                        stack.Push(datas[i].ToString());
                }
                number.Add(data.Substring(0, data.Length - 1).Split(',')[1]);
            }
        }

        public static void FileRead()
        {
            StreamReader sr = new StreamReader("rules.txt");
            string line = "";
            string[] lineArray = new string[7];
            int i = 0;
            while (!sr.EndOfStream)
            {
                line += sr.ReadLine();
                for (int k = 0; k < lineArray.Length; k++)
                {
                    lineArray[k] = line.Split('|')[k];
                }
                for (int j = 0; j < lineArray.Length; j++)
                {
                    rules[i, j] = lineArray[j];
                }
                line = "";
                i++;
            }
        }

        public static void PrintRules(string[,] rules)
        {
            Console.WriteLine("Szabályok: ");
            for (int i = 0; i < rules.GetLength(0); i++)
            {
                for (int j = 0; j < rules.GetLength(1); j++)
                {
                    Console.Write("\t" + rules[i, j] + "\t");
                    Console.Write("|");
                }
                Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------------");
            }
        }

        public static string ListStack(Stack stack)
        {
            string elements = "";
            string[] stackArray = new string[stack.Count];
            stack.CopyTo(stackArray, 0);

            for (int i = 0; i < stackArray.Length; i++)
            {
                elements += stackArray[i];
            }

            if (stack.Count == 0)
            {
                return "#";
            }
            else
            {
                return elements;
            }
        }

        public static string ListToString(List<string> list)
        {
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                result += list[i];
            }

            return result;
        }

        public static bool CheckInput(string input, string[,] rules)
        {
            string characters = "";
            for (int j = 0; j < rules.GetLength(1); j++)
            {
                characters += rules[0, j];
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (!characters.Contains(input[i]))
                {
                    return false;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {
            stack = new Stack();
            stack.Push("#");
            stack.Push("E");

            FileRead();

            Console.Write("Szeretne inputot megadni?(I/N): ");
            string answer = Console.ReadLine().Trim().ToLower();

            Console.Clear();
            switch (answer)
            {
                case "i":
                    PrintRules(rules);
                    Console.Write("Adja meg az inputot: ");
                    input = Console.ReadLine().Trim() + "#";
                    input = Regex.Replace(input, @"([0-9]+)", "i");
                    break;
                case "n":
                    input = "(i+i)*i#";
                    Console.WriteLine("Az alapértelmezett input: {0}", input);
                    break;
                default:
                    input = "(i+i)*i#";
                    Console.WriteLine("Az alapértelmezett input: {0}", input);
                    break;
            }

            if (CheckInput(input, rules))
            {
                do
                {
                    for (int i = 0; i < rules.GetLength(1); i++)
                    {
                        if (input[index].ToString() == rules[0, i])
                        {
                            sAct = stack.Pop().ToString();
                            for (int j = 0; j < rules.GetLength(0); j++)
                            {
                                if (sAct == rules[j, 0])
                                {
                                    Evaluate(rules[j, i]);
                                    if (!end)
                                        Console.WriteLine("({0}, \t{1}, \t{2})", input.Substring(index), ListStack(stack), ListToString(number));
                                    break;
                                }
                            }
                        }
                    }
                } while (index < input.Length - 1 || stack.Count > 0);
            }
            else
            {
                Console.WriteLine("Az input helytelen karaktert tartalmaz");
                input = "#";
            }

            Console.ReadKey();
        }
    }
}
