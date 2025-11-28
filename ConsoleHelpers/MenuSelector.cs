using System;
using System.Collections.Generic;

namespace ClinicPatientRegister_v2.ConsoleHelpers
{
    public static class MenuSelector
    {
        public static int Show(string title, List<string> options)
        {
            int index = 0;

            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine(title);
                Console.WriteLine("==========================================\n");

                for (int i = 0; i < options.Count; i++)
                {
                    if (i == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"> {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {options[i]}");
                    }
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.DownArrow)
                    index = (index + 1) % options.Count;

                if (key == ConsoleKey.UpArrow)
                    index = (index - 1 + options.Count) % options.Count;

            } while (key != ConsoleKey.Enter);

            return index; // return chosen index
        }
    }
}
