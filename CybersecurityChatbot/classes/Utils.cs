using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public static class Utils
    {
        public static void PrintColoredText(string text, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.ResetColor();
        }

        public static void TypewriterEffect(string text, int delayMilliseconds = 30)
        {
            PrintColoredText("Bot: ", ConsoleColor.Green, false);

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMilliseconds);
            }
            Console.WriteLine();
        }
    }
}
