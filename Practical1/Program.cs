using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace Practical1
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                try
                {
                    Console.Clear();

                    int ch;
                    Console.WriteLine("Enter number of ex to check(from 1 to 7)\nEnter '0' to exit");
                    Console.WriteLine("Ex 4, 5 do the same stuff");
                    Console.Write("Check Ex# ");
                    ch = Int32.Parse(Console.ReadLine());

                    if (ch == 0)
                        break;

                    else if (ch == 1)
                    {
                        // 1
                        /*Создайте консольное приложение, порождающее
                            поток. Этот поток должен отображать в консоль числа
                            от 0 до 50. */

                        ThreadStart threadStart = new ThreadStart(PrintFrom0to50UsingThread);
                        Thread thread = new Thread(threadStart);
                        thread.Start();
                        thread.Join();
                    }

                    else if (ch == 2)
                    {
                        // 2
                        /*Добавьте в первое задание возможность передачи
                            начала и конца диапазона чисел. Границы определяет
                            пользователь.*/
                        
                        Console.Write("Start number: ");
                        int start = Int32.Parse(Console.ReadLine());
                        Console.Write("End number: ");
                        int end = Int32.Parse(Console.ReadLine());

                        ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(PrintNumsInRangle);
                        Thread thread = new Thread(parameterizedThreadStart);
                        thread.Start(new Tuple<int, int>(start, end));
                        thread.Join();
                    }

                    else if (ch == 3)
                    {

                        // 3
                        /*Добавьте к первому заданию возможность определения
                            пользователем количества потоков. Границы диапазона
                            чисел также выбираются пользователем.*/

                        Console.Write("# of threads: ");
                        int t = Int32.Parse(Console.ReadLine());
                        Console.Write("Start number: ");
                        int start = Int32.Parse(Console.ReadLine());
                        Console.Write("End number: ");
                        int end = Int32.Parse(Console.ReadLine());

                        Tuple<int, int, int> tuple = new Tuple<int, int, int>(t, start, end);

                        ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(PrintNumsInRangeUsingMultipleThreads);
                        Thread thread = new Thread(parameterizedThreadStart);
                        thread.Start(tuple);
                        thread.Join();
                    }

                    else if (ch == 4 || ch == 5)
                    {
                        // 4
                        /*Консольное приложение генерирует набор чисел,
                            состоящий из 10000 элементов. С помощью механизма
                            потоков нужно найти максимум, минимум, среднее в этом
                            наборе.
                            Для каждой из задач выделите поток.*/

                        // 5
                        /*К четвертому заданию добавьте поток, выводящий
                            набор чисел и результаты вычислений в файл.*/

                        Random rnd = new Random();
                        List<int> nums = new List<int>();
                        for (int i = 0; i < 10000; i++)
                            nums.Add(rnd.Next(0, 25000));

                        ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(FindMinMaxAverageInList);
                        Thread thread = new Thread(parameterizedThreadStart);
                        thread.Start(nums);
                        thread.Join();
                    }

                    else
                        Console.WriteLine("No such exercise");


                    Thread.Sleep(1000);
                    Console.WriteLine();
                    Console.WriteLine("Enter any key to continue");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n\n\nError message: {ex.Message}\n\nError Stack Trace: {ex.StackTrace}");
                }

            }
        }

        static void PrintFrom0to50UsingThread()
        {
            for (int i = 0; i < 50; i++)
            {
                Console.Write(i + " ");
            }
        }

        static void PrintNumsInRangle(object obj)
        {
            Tuple<int, int> t = obj as Tuple<int, int>;

            for (int i = t.Item1; i < t.Item2; i++)
            {
                Console.WriteLine(i);
            }
        }

        static void PrintNumsInRangeUsingMultipleThreads(object obj)
        {
            Tuple<int, int, int> t = obj as Tuple<int, int, int>;
            List<Thread> threads = new List<Thread>();

            int start = t.Item2;
            for (int i = 0; i < t.Item1; i++)
            {
                Console.WriteLine($"\n\t\tThread: {i}");
                ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(PrintNumsInRangle);
                Thread thread = new Thread(parameterizedThreadStart);

                thread.Start(new Tuple<int, int>(start, start + (t.Item3 - t.Item2) / t.Item1));
                start += (t.Item3 - t.Item2) / t.Item1;
                Thread.Sleep(1000);
            }
        }

        static void FindMinMaxAverageInList(object obj)
        {
            List<int> nums = obj as List<int>;

            ParameterizedThreadStart parameterizedThreadStart1 = new ParameterizedThreadStart(FindMin);
            Thread thread1 = new Thread(parameterizedThreadStart1);
            thread1.Start(nums);
            thread1.Join();

            ParameterizedThreadStart parameterizedThreadStart2 = new ParameterizedThreadStart(FindMax);
            Thread thread2 = new Thread(parameterizedThreadStart2);
            thread2.Start(nums);
            thread2.Join();

            ParameterizedThreadStart parameterizedThreadStart3 = new ParameterizedThreadStart(FindAverage);
            Thread thread3 = new Thread(parameterizedThreadStart3);
            thread3.Start(nums);
            thread3.Join();
        }

        static void FindMin(object obj)
        {
            int min = Int32.MaxValue;
            List<int> nums = obj as List<int>;

            for (int i = 0; i < nums.Count; i++)
            {
                if (nums[i] < min) min = nums[i];
            }

            Thread.Sleep(600);
            Console.WriteLine($"Min value: {min}");


            Tuple<string, List<int>> tuple = new Tuple<string, List<int>>("Min: ", new List<int>() { min });

            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(SaveToFile);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.Start(tuple);
            thread.Join();
        }

        static void FindMax(object obj)
        {
            int max = Int32.MinValue;
            List<int> nums = obj as List<int>;

            for (int i = 0; i < nums.Count; i++)
            {
                if (nums[i] > max) max = nums[i];
            }

            Thread.Sleep(600);
            Console.WriteLine($"Max value: {max}");


            Tuple<string, List<int>> tuple = new Tuple<string, List<int>>("Max: ", new List<int>() { max });

            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(SaveToFile);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.Start(tuple);
            thread.Join();
        }

        static void FindAverage(object obj)
        {
            int average = 0;
            List<int> nums = obj as List<int>;

            for (int i = 0; i < nums.Count; i++)
            {
                average += nums[i];
            }

            average /= nums.Count;

            Thread.Sleep(600);
            Console.WriteLine($"Average value: {average}");


            Tuple<string, List<int>> tuple = new Tuple<string, List<int>>("Average: ", new List<int>() { average });

            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(SaveToFile);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.Start(tuple);
            thread.Join();
        }

        static void SaveToFile(object obj)
        {

            try
            {
                string prevText = String.Empty;

                if (File.Exists("result.txt"))
                {
                    using (StreamReader sr = new StreamReader("result.txt"))
                    {
                        prevText = sr.ReadToEnd();
                    }
                }
             
                Tuple<string, List<int>> t = obj as Tuple<string, List<int>>;

                using (StreamWriter sw = new StreamWriter("result.txt"))
                {
                    sw.WriteLine(prevText);
                    
                    sw.Write(t.Item1);

                    foreach (int i in t.Item2)
                    {
                        sw.Write($"{i} ");
                    }

                    sw.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\n\nError message: {ex.Message}\n\nError Stack Trace: {ex.StackTrace}");
            }

        }
    }
}
