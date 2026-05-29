using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class ResponseHandler
    {
        private List<string> generalGreetings;
        private Dictionary<string, string[]> topicResponses;
        private Random random;

        public ResponseHandler()
        {
            random = new Random();
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            // Initialize general responses
            generalGreetings = new List<string>
            {
                "I'm doing great, thanks for asking! Always excited to help people stay safe online!",
                "I'm functioning well! Ready to share some cybersecurity knowledge with you.",
                "Doing my best! Cybersecurity is serious business, but I'm always happy to chat about it!"
            };

            // Initialize topic-specific responses
            topicResponses = new Dictionary<string, string[]>
            {
                ["password"] = new string[]
                {
                    "Create strong passwords with at least 12 characters, including uppercase, lowercase, numbers, and symbols!",
                    "Never reuse passwords across different accounts. Use a password manager instead!",
                    "Enable two-factor authentication whenever possible - it adds an extra layer of security!",
                    "Avoid using personal information like birthdays or pet names in your passwords."
                },
                ["phish"] = new string[]
                {
                    "Always check the sender's email address carefully - scammers often use slight misspellings!",
                    "Never click on suspicious links. Hover over them first to see where they really go!",
                    "Legitimate companies never ask for personal information via email!",
                    "Look for urgent or threatening language - it's a common phishing tactic!"
                },
                ["scam"] = new string[]
                {
                    "If something sounds too good to be true, it probably is! Trust your instincts.",
                    "Never share your OTP or PIN with anyone - not even 'bank officials'!",
                    "Verify unexpected calls by contacting the company directly through official channels.",
                    "Be wary of prize winnings you never entered - it's a classic scam technique!"
                },
                ["privacy"] = new string[]
                {
                    "Review your privacy settings on social media regularly!",
                    "Be mindful of what personal information you share online - it can be used against you.",
                    "Use a VPN when connecting to public Wi-Fi to protect your data!",
                    "Regularly check which apps have access to your accounts and remove unnecessary ones."
                },
                ["browsing"] = new string[]
                {
                    "Look for 'https://' and the padlock icon in the address bar before entering sensitive information!",
                    "Keep your browser and extensions updated to the latest versions!",
                    "Use ad-blockers and privacy-focused browser extensions!",
                    "Clear your browsing cache and cookies regularly to protect your privacy!"
                }
            };
        }

        public string GetHowAreYouResponse()
        {
            return generalGreetings[random.Next(generalGreetings.Count)];
        }

        public string GetPurposeResponse(string userName)
        {
            return $"My purpose is to help you, {userName}, stay safe online! I provide education about cybersecurity threats like phishing, password theft, and online scams. In South Africa, cyberattacks are increasing, and I'm here to help protect you!";
        }

        public string GetCybersecurityResponse(string input, string userName)
        {
            // Check each topic
            foreach (var topic in topicResponses)
            {
                if (input.Contains(topic.Key))
                {
                    string[] responses = topic.Value;
                    string response = responses[random.Next(responses.Length)];

                    // Personalize the response
                    if (random.Next(2) == 0)
                    {
                        response = $"Great question, {userName}! {response}";
                    }

                    return response;
                }
            }

            // Default response for unrecognized queries
            return GetDefaultResponse();
        }

        private string GetDefaultResponse()
        {
            string[] defaultResponses = {
                "I'm not sure I understand that. Could you ask about password safety, phishing, scams, privacy, or safe browsing?",
                "Hmm, I didn't quite get that. Would you like to learn about password safety, phishing protection, or safe browsing habits?",
                "I'm still learning! Try asking me about 'password safety', 'phishing', or 'safe browsing' for helpful tips."
            };
            return defaultResponses[random.Next(defaultResponses.Length)];
        }
    }
}