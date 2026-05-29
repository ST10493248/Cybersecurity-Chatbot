using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class Chatbot
    {
        private string userName;
        private ResponseHandler responseHandler;
        private bool isRunning;

        public Chatbot()
        {
            responseHandler = new ResponseHandler();
            isRunning = true;
        }

        public void Start()
        {
            // Play voice greeting
            PlayVoiceGreeting();

            // Welcome message with decorative border
            Utils.PrintColoredText("\n" + new string('=', 60), ConsoleColor.Cyan);
            Utils.PrintColoredText("   WELCOME TO THE CYBERSECURITY AWARENESS CHATBOT", ConsoleColor.Green);
            Utils.PrintColoredText(new string('=', 60), ConsoleColor.Cyan);

            // Get user name with validation
            GetUserName();

            // Personalized greeting with typing effect
            string greeting = $"Hello {userName}! I'm your Cybersecurity Awareness Assistant. ";
            Utils.TypewriterEffect(greeting, 30);

            Utils.TypewriterEffect("I'm here to help you stay safe online. ", 30);
            Utils.TypewriterEffect("What would you like to know about today?\n\n", 30);

            // Show available topics
            ShowHelp();

            // Main conversation loop
            RunConversationLoop();
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "audio",
                    "greeting.wav"
                );

                if (System.IO.File.Exists(audioPath))
                {
                    using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(audioPath))
                    {
                        player.PlaySync(); // Waits for audio to complete
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio error: {ex.Message}");
            }
        }

        private void GetUserName()
        {
            while (string.IsNullOrWhiteSpace(userName))
            {
                Utils.PrintColoredText("\nMay I have your name? ", ConsoleColor.Yellow);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    userName = input.Trim();
                }
                else
                {
                    Utils.PrintColoredText("I didn't catch that. Please enter a valid name.\n", ConsoleColor.Red);
                }
            }
        }

        private void ShowHelp()
        {
            Utils.PrintColoredText("\n" + new string('-', 60), ConsoleColor.DarkGray);
            Utils.PrintColoredText("Here's what you can ask me about:", ConsoleColor.Cyan);
            Utils.PrintColoredText("• Password safety tips", ConsoleColor.White);
            Utils.PrintColoredText("• Phishing attacks", ConsoleColor.White);
            Utils.PrintColoredText("• Safe browsing habits", ConsoleColor.White);
            Utils.PrintColoredText("• General cybersecurity questions", ConsoleColor.White);
            Utils.PrintColoredText("\nYou can also ask: 'How are you?', 'What's your purpose?', 'Help'", ConsoleColor.DarkYellow);
            Utils.PrintColoredText(new string('-', 60), ConsoleColor.DarkGray);
            Utils.PrintColoredText("\nType 'quit' or 'exit' to end the conversation.\n", ConsoleColor.DarkGray);
        }

        private void RunConversationLoop()
        {
            while (isRunning)
            {
                Utils.PrintColoredText($"{userName}: ", ConsoleColor.Yellow, false);
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    HandleEmptyInput();
                    continue;
                }

                string response = ProcessInput(userInput);
                Utils.TypewriterEffect(response, 40);
                Console.WriteLine();

                // Check if user wants to quit
                if (userInput.ToLower() == "quit" || userInput.ToLower() == "exit")
                {
                    SayGoodbye();
                    break;
                }
            }
        }

        private string ProcessInput(string input)
        {
            string lowerInput = input.ToLower();

            // Check for quit command first
            if (lowerInput == "quit" || lowerInput == "exit")
            {
                return "";
            }

            // Check for help
            if (lowerInput.Contains("help") || lowerInput.Contains("what can i ask"))
            {
                ShowHelp();
                return "Is there a specific topic you'd like to learn about?";
            }

            // Check for "how are you"
            if (lowerInput.Contains("how are you"))
            {
                return responseHandler.GetHowAreYouResponse();
            }

            // Check for "purpose"
            if (lowerInput.Contains("purpose") || lowerInput.Contains("what do you do"))
            {
                return responseHandler.GetPurposeResponse(userName);
            }

            // Check for cybersecurity topics
            return responseHandler.GetCybersecurityResponse(lowerInput, userName);
        }

        private void HandleEmptyInput()
        {
            string[] responses = {
                "I didn't quite understand that. Could you rephrase?",
                "Hmm, I didn't catch that. Can you try again?",
                "Please enter a question or statement so I can help you!"
            };
            Random rand = new Random();
            Utils.PrintColoredText($"\nBot: {responses[rand.Next(responses.Length)]}\n", ConsoleColor.Red);
        }

        private void SayGoodbye()
        {
            Utils.PrintColoredText("\n" + new string('=', 60), ConsoleColor.Cyan);
            Utils.TypewriterEffect($"Goodbye {userName}! Stay safe online and remember to practice good cybersecurity habits! ", 40);
            Utils.PrintColoredText("\n" + new string('=', 60), ConsoleColor.Cyan);
        }
    }
}