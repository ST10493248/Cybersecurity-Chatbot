using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    namespace CybersecurityChatbot
    {
        public static class AsciiArt
        {
            public static void DisplayLogo()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                string logo = @"
    ╔══════════════════════════════════════════════════════════════╗
    ║                                                              ║
    ║                                                              ║
    ║                    ██████╗    ███████╗     ██████╗           ║     
    ║                   ██╔════╝    ██╔════╝    ██╔════╝           ║     
    ║                   ██║         ███████╗    ██║                ║     
    ║                   ██║     █║ ╚═══  ██    ║██║    ██ ║        ║     
    ║                   ╚████ ██╔╝  ███████║     ██████ ╔╝         ║    
    ║                    ╚═════╝   ╚══════╝     ╚═════ ╝           ║
    ║                                                              ║
    ║             CYBERSECURITY AWARENESS CHATBOT                  ║
    ║                   Protecting South Africa                    ║
    ║                      Stay Safe Online!                       ║
    ╚══════════════════════════════════════════════════════════════╝
";
                Console.WriteLine(logo);
                Console.ResetColor();
            }
        }
    }
}
