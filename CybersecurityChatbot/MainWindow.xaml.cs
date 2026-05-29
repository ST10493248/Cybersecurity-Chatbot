using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CyberSecurityChatbot
{
    public partial class MainWindow : Window
    {
        // Memory variables
        private string currentTopic = "";
        private string userName = "";
        private int tipsGiven = 0;
        private Random rand = new Random();
        private DateTime sessionStart = DateTime.Now;
        private DispatcherTimer sessionTimer;
        private string currentMood = "Neutral";
        private List<string> userInterests = new List<string>();
        private List<string> conversationHistory = new List<string>();

        // Name collection state
        private bool waitingForName = true;

        // Response arrays
        private string[] phishingResponses = new string[]
        {
            "🎣 PHISHING ALERT: Always check the sender's email address carefully. Scammers use tricks like 'arnazon.com' instead of 'amazon.com'.",
            "🎣 Did you know? 91 percent of cyberattacks start with a phishing email. Never click links from unknown senders.",
            "🎣 RED FLAG: Urgent language like 'Your account will be closed in 24 hours' is a common phishing tactic to create panic.",
            "🎣 PRO TIP: Hover over links before clicking to see the actual URL. If it looks suspicious, do not click.",
            "🎣 In South Africa, banks will NEVER ask for your PIN or OTP via email or SMS. Hang up and call them directly!",
            "🎣 Look for spelling mistakes and poor grammar - legitimate companies proofread their communications.",
            "🎣 When in doubt, go directly to the website by typing the URL yourself rather than clicking email links.",
            "🎣 Phishers often pretend to be from SARS, your bank, or delivery services like Courier Guy.",
            "🎣 Never download attachments from suspicious emails - they may contain ransomware or keyloggers.",
            "🎣 Report phishing attempts to your IT department immediately."
        };

        private string[] passwordResponses = new string[]
        {
            "🔐 STRONG PASSWORD FORMULA: Use 12+ characters with uppercase, lowercase, numbers, AND symbols.",
            "🔐 Never reuse passwords. If one account gets hacked, ALL your accounts become vulnerable.",
            "🔐 Two-Factor Authentication (2FA) saves lives. Enable it everywhere possible!",
            "🔐 Use a Password Manager like Bitwarden, LastPass, or 1Password.",
            "🔐 Create a passphrase: Combine 4 random words like 'PurpleDinosaurCoffeeTable' - it's long and easy to remember!",
            "🔐 Avoid personal info - your birthday, pet's name, or 'password123' are the FIRST things hackers try.",
            "🔐 Fun Fact: '123456' and 'password' are still the most common passwords. Don't be an easy target!",
            "🔐 Change important passwords every 3 months, especially for banking, email, and social media.",
            "🔐 Never save passwords in your browser. Use a dedicated password manager instead.",
            "🔐 Enable biometric authentication (fingerprint or face ID) wherever available for extra security."
        };

        private string[] privacyResponses = new string[]
        {
            "👁️ PRIVACY TIP: Review your privacy settings on social media regularly. Set profiles to private!",
            "👁️ Never share your ID number or birth date online - identity thieves love this information.",
            "👁️ Cover your laptop webcam with a sliding cover when not in use. It's cheap physical security!",
            "👁️ Google yourself to see what personal information is publicly available about you.",
            "👁️ Check app permissions on your phone - many apps don't need access to your contacts or location.",
            "👁️ Use different email addresses for different purposes: banking, social media, newsletters.",
            "👁️ Delete old accounts you no longer use - forgotten accounts can be hacked without your knowledge.",
            "👁️ In South Africa, POPIA gives you rights over your data. Companies must protect it!",
            "👁️ Use a VPN when using public Wi-Fi to encrypt your internet traffic.",
            "👁️ Be careful with online quizzes - they often collect your personal information for marketing."
        };

        private string[] scamResponses = new string[]
        {
            "⚠️ SCAM SPOTTING: If it sounds too good to be true, it IS too good to be true!",
            "⚠️ The 'Microsoft Support' scam: Real Microsoft will NEVER call you about viruses. Hang up!",
            "⚠️ Romance scams are common in South Africa. Never send money to someone you haven't met in person!",
            "⚠️ SIM swap scam: If your phone suddenly loses signal, contact your bank IMMEDIATELY!",
            "⚠️ Job offer scams: Real jobs never ask for upfront payment for training or background checks.",
            "⚠️ 'You've won a prize you never entered' is a classic scam. They'll ask for 'fees' to release it.",
            "⚠️ WhatsApp scams: 'Hi Mom/Dad, I broke my phone' - always verify with a phone call!",
            "⚠️ SARS will NEVER ask for your credit card details or demand immediate payment via email.",
            "⚠️ The accidental payment scam: Someone sends you money 'by mistake' then asks you to return it.",
            "⚠️ Investment scams promising guaranteed returns - legitimate investments always carry risk."
        };

        private string[] safeBrowsingResponses = new string[]
        {
            "🌐 SAFE BROWSING: Look for 'https://' and the padlock icon before entering passwords or credit card info!",
            "🌐 Keep your browser updated - Chrome, Firefox, and Edge release critical security patches regularly.",
            "🌐 Clear your cookies and cache weekly. Cookies track your browsing habits across websites!",
            "🌐 Incognito mode does NOT make you anonymous - your ISP can still see everything you do.",
            "🌐 Install ad-blocker extensions like uBlock Origin to block malicious advertisements.",
            "🌐 Beware of fake download buttons on free software sites - the real button is NEVER the flashing one!",
            "🌐 Enable 'Do Not Track' in your browser settings. While not all sites respect it, some do!",
            "🌐 Use DuckDuckGo instead of Google for private searches - they don't track or profile you!",
            "🌐 Never save passwords in your browser - use a dedicated password manager instead.",
            "🌐 Be cautious of browser extensions - they can read everything you type including passwords."
        };

        private string[] quickTips = new string[]
        {
            "💡 Did you know? Using a password manager means you only need to remember ONE strong password!",
            "💡 Update your phone's software! Many security patches fix critical vulnerabilities.",
            "💡 Back up your photos and documents to the cloud or an external drive TODAY!",
            "💡 Use different PINs for your phone and your bank card.",
            "💡 Report cybercrime to the SAPS Cybercrime Unit or SAFPS in South Africa.",
            "💡 Never plug unknown USB drives into your computer - they can contain malware!",
            "💡 Turn off Bluetooth and Wi-Fi when not in use.",
            "💡 The best antivirus is YOU! No software can protect against human error.",
            "💡 Check haveibeenpwned.com to see if your email has been in a data breach.",
            "💡 Enable login alerts on all your social media accounts."
        };

        private string[] funFacts = new string[]
        {
            "📚 FUN FACT: The first computer virus was created in 1983 and spread via floppy disks!",
            "📚 FUN FACT: The most hacked password of 2024 was still '123456' - used by over 4 million people!",
            "📚 FUN FACT: South Africa ranks in the top 10 countries for cybercrime globally.",
            "📚 FUN FACT: A hacker tries to attack a computer every 39 seconds somewhere on the internet!",
            "📚 FUN FACT: 95% of cybersecurity breaches are caused by human error.",
            "📚 FUN FACT: Ethical hackers get paid to break into systems legally to find vulnerabilities!",
            "📚 FUN FACT: The first ransomware attack happened in 1989 via floppy disks!",
            "📚 FUN FACT: 30% of phishing emails are opened by targeted users - don't be one of them!"
        };

        // Sentiment responses
        private Dictionary<string, string[]> sentimentResponses = new Dictionary<string, string[]>
        {
            ["worried"] = new string[]
            {
                "💙 It's completely understandable to feel that way. I'm here to help you every step of the way.",
                "💙 Don't worry! The fact that you're concerned shows you care about your security.",
                "💙 I understand your concern. With the right knowledge, you can protect yourself effectively."
            },
            ["curious"] = new string[]
            {
                "🌟 That's wonderful! Curiosity is the first step to becoming cybersecurity aware!",
                "🌟 I love your enthusiasm for learning! Let me share something interesting with you.",
                "🌟 Great question! Your curiosity will keep you safer online than most people."
            },
            ["frustrated"] = new string[]
            {
                "🤝 I hear your frustration. Let me help simplify it for you.",
                "🤝 It can be frustrating when things aren't clear. Let me explain differently.",
                "🤝 I understand it's overwhelming. Take a deep breath - we'll go through this step by step."
            },
            ["confused"] = new string[]
            {
                "🤔 I understand this can be confusing at first. Let me break it down simply.",
                "🤔 No worries! Many people find this confusing initially. Here's an easier explanation:",
                "🤔 Let me explain that more clearly. Think of it this way:"
            },
            ["happy"] = new string[]
            {
                "😊 I'm so glad to hear that! A positive attitude makes learning cybersecurity more fun!",
                "😊 Your positive energy is contagious! Keep up the great work!",
                "😊 That's wonderful! A happy learner is an effective learner!"
            },
            ["thankful"] = new string[]
            {
                "🙏 You're very welcome! Staying safe online is my top priority for you.",
                "🙏 My pleasure! Knowledge shared is knowledge doubled.",
                "🙏 Anytime! That's what I'm here for. Stay vigilant and stay safe!"
            }
        };

        public MainWindow()
        {
            InitializeComponent();

            // Set timestamp
            txtChatTimestamp.Text = DateTime.Now.ToString("HH:mm");

            // Play voice greeting
            PlayVoiceGreeting();

            // Start session timer
            StartSessionTimer();

            // Ask for name INSIDE the chatbot (not popup)
            AskForNameInChat();

            // Button Events
            btnSend.Click += BtnSend_Click;
            btnPhishing.Click += BtnPhishing_Click;
            btnPasswords.Click += BtnPasswords_Click;
            btnPrivacy.Click += BtnPrivacy_Click;
            btnScams.Click += BtnScams_Click;
            btnSafeBrowsing.Click += BtnSafeBrowsing_Click;

            // Enter key support
            txtInput.KeyDown += TxtInput_KeyDown;

            // Set focus
            txtInput.Focus();
        }

        // NEW: Ask for name inside the chat instead of popup
        private void AskForNameInChat()
        {
            AddBotMessage("Welcome to Awareness Assistant! 👋", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("I'm your Cybersecurity Awareness Assistant here to help you stay safe online.", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("Before we begin, please tell me your name:", "CyberShield", DateTime.Now.ToString("HH:mm"));

            waitingForName = true;
            conversationHistory.Add("Bot: Asked for user name");
        }

        private void ProcessNameInput(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                userName = input.Trim().ToUpper();
                waitingForName = false;

                conversationHistory.Add($"User name stored: {userName}");
                UpdateMemoryDisplay();

                AddBotMessage($"Thank you, {userName}! 👋", "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("I'm your Cybersecurity Awareness Assistant. What would you like to explore today?", "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBulletPoints("Phishing tips", "Password safety", "What can you do?");
            }
            else
            {
                AddBotMessage("I didn't catch that. Could you please tell me your name?", "CyberShield", DateTime.Now.ToString("HH:mm"));
            }
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "audio",
                    "greeting.wav");

                if (File.Exists(audioPath))
                {
                    using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(audioPath))
                    {
                        player.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audio error: {ex.Message}");
            }
        }

        private void StartSessionTimer()
        {
            sessionTimer = new DispatcherTimer();
            sessionTimer.Interval = TimeSpan.FromSeconds(1);
            sessionTimer.Tick += (s, e) => UpdateStatistics();
            sessionTimer.Start();
        }

        private void UpdateStatistics()
        {
            TimeSpan sessionLength = DateTime.Now - sessionStart;
            txtSessionTime.Text = $"⏱️ Session: {sessionLength.Minutes}m {sessionLength.Seconds}s";
            txtTipsGiven.Text = $"💡 Tips given: {tipsGiven}";
            txtMood.Text = $"😊 Mood: {currentMood}";
        }

        private void AddBulletPoints(string point1, string point2, string point3)
        {
            try
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Margin = new Thickness(0, -10, 0, 10);

                Run run = new Run($"• {point1}\n• {point2}\n• {point3}");
                run.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CDD6F4");
                paragraph.Inlines.Add(run);

                rtbChat.Document.Blocks.Add(paragraph);
                rtbChat.ScrollToEnd();
            }
            catch { }
        }

        private void UpdateMemoryDisplay()
        {
            txtStoredName.Text = string.IsNullOrEmpty(userName) ? "Not set" : userName;
            txtStoredTopic.Text = string.IsNullOrEmpty(currentTopic) ? "None" : currentTopic;
        }

        private void StoreUserInterest(string topic)
        {
            if (!string.IsNullOrEmpty(topic) && !userInterests.Contains(topic))
            {
                userInterests.Add(topic);
                conversationHistory.Add($"User showed interest in: {topic}");
            }
        }

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            string input = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                txtInput.Clear();
                return;
            }

            // Display user message
            AddUserMessage(input, DateTime.Now.ToString("HH:mm"));
            conversationHistory.Add($"User: {input}");

            // If waiting for name, process name input first
            if (waitingForName)
            {
                ProcessNameInput(input);
                txtInput.Clear();
                return;
            }

            string lowerInput = input.ToLower();

            // Sentiment Detection
            string detectedSentiment = DetectSentiment(lowerInput);
            if (!string.IsNullOrEmpty(detectedSentiment))
            {
                currentMood = detectedSentiment;
                ProcessSentiment(detectedSentiment, lowerInput);
                UpdateMemoryDisplay();
                txtInput.Clear();
                return;
            }

            // Memory recall command
            if (lowerInput.Contains("what do you remember") || lowerInput.Contains("my interests"))
            {
                if (userInterests.Count > 0)
                {
                    AddBotMessage($"I remember you're interested in {string.Join(", ", userInterests)}. Would you like to learn more?", "CyberShield", DateTime.Now.ToString("HH:mm"));
                }
                else
                {
                    AddBotMessage("I haven't learned about your interests yet. Try asking about a cybersecurity topic!", "CyberShield", DateTime.Now.ToString("HH:mm"));
                }
                txtInput.Clear();
                return;
            }

            // Memory status command
            if (lowerInput.Contains("memory status") || lowerInput.Contains("what do you know about me"))
            {
                ShowMemoryStatus();
                txtInput.Clear();
                return;
            }

            // Process topics
            if (lowerInput.Contains("phishing"))
            {
                currentTopic = "Phishing";
                tipsGiven++;
                StoreUserInterest("Phishing");
                AddBotMessage(phishingResponses[rand.Next(phishingResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("Type 'more' for additional phishing protection tips! 🎣", "CyberShield", DateTime.Now.ToString("HH:mm"));
                conversationHistory.Add($"Bot: Provided phishing tip");
                UpdateMemoryDisplay();
                UpdateStatistics();
                txtInput.Clear();
                return;
            }

            if (lowerInput.Contains("password"))
            {
                currentTopic = "Passwords";
                tipsGiven++;
                StoreUserInterest("Password Safety");
                AddBotMessage(passwordResponses[rand.Next(passwordResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("Type 'more' for additional password safety tips! 🔐", "CyberShield", DateTime.Now.ToString("HH:mm"));
                conversationHistory.Add($"Bot: Provided password tip");
                UpdateMemoryDisplay();
                UpdateStatistics();
                txtInput.Clear();
                return;
            }

            if (lowerInput.Contains("privacy"))
            {
                currentTopic = "Privacy";
                tipsGiven++;
                StoreUserInterest("Privacy");
                AddBotMessage(privacyResponses[rand.Next(privacyResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("Type 'more' for additional privacy protection tips! 👁️", "CyberShield", DateTime.Now.ToString("HH:mm"));
                conversationHistory.Add($"Bot: Provided privacy tip");
                UpdateMemoryDisplay();
                UpdateStatistics();
                txtInput.Clear();
                return;
            }

            if (lowerInput.Contains("scam") || lowerInput.Contains("fraud"))
            {
                currentTopic = "Scams";
                tipsGiven++;
                StoreUserInterest("Scam Detection");
                AddBotMessage(scamResponses[rand.Next(scamResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("Type 'more' for additional scam prevention tips! ⚠️", "CyberShield", DateTime.Now.ToString("HH:mm"));
                conversationHistory.Add($"Bot: Provided scam tip");
                UpdateMemoryDisplay();
                UpdateStatistics();
                txtInput.Clear();
                return;
            }

            if (lowerInput.Contains("browsing") || lowerInput.Contains("safe browsing"))
            {
                currentTopic = "Safe browsing";
                tipsGiven++;
                StoreUserInterest("Safe Browsing");
                AddBotMessage(safeBrowsingResponses[rand.Next(safeBrowsingResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("Type 'more' for additional safe browsing tips! 🌐", "CyberShield", DateTime.Now.ToString("HH:mm"));
                conversationHistory.Add($"Bot: Provided browsing tip");
                UpdateMemoryDisplay();
                UpdateStatistics();
                txtInput.Clear();
                return;
            }

            // Random tip command
            if (lowerInput.Contains("random tip") || lowerInput.Contains("quick tip"))
            {
                AddBotMessage(quickTips[rand.Next(quickTips.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                tipsGiven++;
                UpdateStatistics();
                txtInput.Clear();
                return;
            }

            // Fun fact command
            if (lowerInput.Contains("fun fact") || lowerInput.Contains("fact"))
            {
                AddBotMessage(funFacts[rand.Next(funFacts.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                txtInput.Clear();
                return;
            }

            // Stats command
            if (lowerInput.Contains("stats") || lowerInput.Contains("statistics"))
            {
                ShowStatistics();
                txtInput.Clear();
                return;
            }

            // Clear command
            if (lowerInput.Contains("clear"))
            {
                rtbChat.Document.Blocks.Clear();
                AddBotMessage("Chat cleared! 🧹 Let's start fresh.", "CyberShield", DateTime.Now.ToString("HH:mm"));
                txtInput.Clear();
                return;
            }

            // Help command
            if (lowerInput.Contains("help") || lowerInput.Contains("what can i ask"))
            {
                ShowHelp();
                txtInput.Clear();
                return;
            }

            // More command
            if (lowerInput.Contains("more") || lowerInput.Contains("another tip"))
            {
                ContinueConversation();
                txtInput.Clear();
                return;
            }

            if (lowerInput == "quit" || lowerInput == "exit")
            {
                Close();
                return;
            }

            // Default response
            AddBotMessage("I can help with Phishing, Passwords, Privacy, Scams, and Safe browsing. Try typing one of these topics! 📚", "CyberShield", DateTime.Now.ToString("HH:mm"));
            txtInput.Clear();
        }

        private string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious") || input.Contains("concerned"))
                return "worried";
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("fascinated"))
                return "curious";
            if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("angry"))
                return "frustrated";
            if (input.Contains("confused") || input.Contains("don't understand") || input.Contains("unclear"))
                return "confused";
            if (input.Contains("happy") || input.Contains("great") || input.Contains("good") || input.Contains("excellent"))
                return "happy";
            if (input.Contains("thank") || input.Contains("appreciate") || input.Contains("grateful"))
                return "thankful";
            return null;
        }

        private void ProcessSentiment(string sentiment, string userInput)
        {
            string[] empathyOptions = sentimentResponses[sentiment];
            string empathyResponse = empathyOptions[rand.Next(empathyOptions.Length)];

            AddSentimentMessage($"Sentiment detected: {sentiment} — responding with extra reassurance");
            AddBotMessage(empathyResponse, "CyberShield", DateTime.Now.ToString("HH:mm"));
            conversationHistory.Add($"User mood detected: {sentiment}");

            if (userInput.Contains("phishing") || sentiment == "worried")
            {
                AddBotMessage("Here's a helpful tip to ease your mind:", "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("• Urgent language pressuring you to act fast is a red flag", "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("• Sender address that looks almost right but isn't", "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("• Links that don't match the stated destination", "CyberShield", DateTime.Now.ToString("HH:mm"));
            }
            else if (sentiment == "curious")
            {
                AddBotMessage("What topic would you like to explore? Try 'phishing', 'passwords', or 'privacy'!", "CyberShield", DateTime.Now.ToString("HH:mm"));
            }
            else if (sentiment == "confused")
            {
                AddBotMessage("Let's start with the basics: Use strong passwords, never click suspicious links, and enable two-factor authentication.", "CyberShield", DateTime.Now.ToString("HH:mm"));
            }
            else if (sentiment == "frustrated")
            {
                AddBotMessage("Take a deep breath. Here's a simple tip:", "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage(quickTips[rand.Next(quickTips.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
            }

            currentMood = sentiment;
            UpdateStatistics();
        }

        private void ShowMemoryStatus()
        {
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("📝 WHAT I REMEMBER ABOUT YOU", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"• Name: {userName}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"• Current topic: {(string.IsNullOrEmpty(currentTopic) ? "None" : currentTopic)}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"• Interests: {(userInterests.Count > 0 ? string.Join(", ", userInterests) : "None yet")}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"• Current mood: {currentMood}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"• Tips received: {tipsGiven}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"• Conversation exchanges: {conversationHistory.Count}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
        }

        private void ShowHelp()
        {
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("📚 COMMAND LIST", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'phishing' - Learn about phishing scams", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'password' - Get password safety tips", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'privacy' - Learn to protect your data", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'scam' - Recognize online scams", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'browsing' - Safe web browsing tips", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'more' - Get additional tips on current topic", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'random tip' - Get a random security tip", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'fun fact' - Learn interesting cybersecurity facts", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'stats' - View your learning statistics", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'memory status' - See what I remember about you", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'clear' - Clear chat history", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("• Type 'quit' or 'exit' - Close the application", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
        }

        private void ShowStatistics()
        {
            TimeSpan sessionLength = DateTime.Now - sessionStart;
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("📊 YOUR STATISTICS", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"👤 Name: {userName}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"💡 Tips received: {tipsGiven}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"🏷️ Current topic: {(string.IsNullOrEmpty(currentTopic) ? "None" : currentTopic)}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"⏱️ Session time: {sessionLength.Minutes}m {sessionLength.Seconds}s", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"😊 Current mood: {currentMood}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage($"📝 Interests: {(userInterests.Count > 0 ? string.Join(", ", userInterests) : "None")}", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("🏆 Keep learning! You're becoming more cyber-safe every day!", "CyberShield", DateTime.Now.ToString("HH:mm"));
        }

        private void ContinueConversation()
        {
            switch (currentTopic.ToLower())
            {
                case "phishing":
                    string[] morePhishing = { "📌 Generic greetings like 'Dear Customer' instead of your real name is a red flag.", "📌 Poor spelling and grammar errors - legitimate companies proofread.", "📌 Requests for personal data like passwords or PINs.", "📌 Fake urgency claiming your account will be closed - scammers create panic." };
                    AddBotMessage(morePhishing[rand.Next(morePhishing.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                    tipsGiven++;
                    break;
                case "passwords":
                    string[] morePasswords = { "📌 Use at least 12 characters - longer passwords are harder to crack.", "📌 Never use dictionary words or keyboard patterns.", "📌 Change passwords immediately after a data breach.", "📌 Use a password manager to generate unique passwords." };
                    AddBotMessage(morePasswords[rand.Next(morePasswords.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                    tipsGiven++;
                    break;
                case "privacy":
                    string[] morePrivacy = { "📌 Use different email addresses for different purposes.", "📌 Delete old accounts you no longer use.", "📌 Enable two-factor authentication on all important accounts.", "📌 Be careful with online quizzes - they collect your data." };
                    AddBotMessage(morePrivacy[rand.Next(morePrivacy.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                    tipsGiven++;
                    break;
                case "scams":
                    string[] moreScams = { "📌 Lottery scams: You can't win a prize you never entered.", "📌 Tech support scams: Legitimate companies never call unsolicited.", "📌 Report scams to SAFPS in South Africa.", "📌 Always verify urgent requests by calling the person directly." };
                    AddBotMessage(moreScams[rand.Next(moreScams.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                    tipsGiven++;
                    break;
                case "safe browsing":
                    string[] moreBrowsing = { "📌 Beware of fake download buttons on free software sites.", "📌 Enable 'Do Not Track' in your browser settings.", "📌 Use a VPN when connecting to public Wi-Fi.", "📌 Regularly clear your browser cache and cookies." };
                    AddBotMessage(moreBrowsing[rand.Next(moreBrowsing.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                    tipsGiven++;
                    break;
                default:
                    AddBotMessage("Please ask about a specific topic first: Phishing, Passwords, Privacy, Scams, or Safe browsing.", "CyberShield", DateTime.Now.ToString("HH:mm"));
                    break;
            }
            UpdateStatistics();
            conversationHistory.Add($"Bot: Provided additional tip on {currentTopic}");
        }

        // Suggestion Chip Handlers
        private void SuggestionChip_Click(object sender, RoutedEventArgs e)
        {
            Button chip = sender as Button;
            if (chip != null && chip.Tag != null)
            {
                string suggestion = chip.Tag.ToString();
                AddUserMessage(suggestion, DateTime.Now.ToString("HH:mm"));
                ProcessSuggestion(suggestion);
            }
        }

        private void ProcessSuggestion(string suggestion)
        {
            string lowerInput = suggestion.ToLower();

            if (lowerInput.Contains("phishing"))
            {
                currentTopic = "Phishing";
                tipsGiven++;
                StoreUserInterest("Phishing");
                AddBotMessage(phishingResponses[rand.Next(phishingResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("Type 'more' for additional phishing protection tips! 🎣", "CyberShield", DateTime.Now.ToString("HH:mm"));
                UpdateMemoryDisplay();
                UpdateStatistics();
            }
            else if (lowerInput.Contains("password"))
            {
                currentTopic = "Passwords";
                tipsGiven++;
                StoreUserInterest("Password Safety");
                AddBotMessage(passwordResponses[rand.Next(passwordResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                AddBotMessage("Type 'more' for additional password safety tips! 🔐", "CyberShield", DateTime.Now.ToString("HH:mm"));
                UpdateMemoryDisplay();
                UpdateStatistics();
            }
            else if (lowerInput.Contains("random tip"))
            {
                AddBotMessage(quickTips[rand.Next(quickTips.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
                tipsGiven++;
                UpdateStatistics();
            }
            else if (lowerInput.Contains("stats"))
            {
                ShowStatistics();
            }
            else if (lowerInput.Contains("fun fact"))
            {
                AddBotMessage(funFacts[rand.Next(funFacts.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
            }
            else if (lowerInput.Contains("help"))
            {
                ShowHelp();
            }
            else
            {
                AddBotMessage("I can help with Phishing, Passwords, Privacy, Scams, and Safe browsing. What would you like to learn about? 📚", "CyberShield", DateTime.Now.ToString("HH:mm"));
            }
        }

        // Sidebar button handlers
        private void BtnPhishing_Click(object sender, RoutedEventArgs e)
        {
            if (waitingForName)
            {
                AddBotMessage("Please tell me your name first!", "CyberShield", DateTime.Now.ToString("HH:mm"));
                return;
            }
            currentTopic = "Phishing";
            tipsGiven++;
            StoreUserInterest("Phishing");
            AddBotMessage(phishingResponses[rand.Next(phishingResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("Type 'more' for additional phishing protection tips! 🎣", "CyberShield", DateTime.Now.ToString("HH:mm"));
            UpdateMemoryDisplay();
            UpdateStatistics();
        }

        private void BtnPasswords_Click(object sender, RoutedEventArgs e)
        {
            if (waitingForName)
            {
                AddBotMessage("Please tell me your name first!", "CyberShield", DateTime.Now.ToString("HH:mm"));
                return;
            }
            currentTopic = "Passwords";
            tipsGiven++;
            StoreUserInterest("Password Safety");
            AddBotMessage(passwordResponses[rand.Next(passwordResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("Type 'more' for additional password safety tips! 🔐", "CyberShield", DateTime.Now.ToString("HH:mm"));
            UpdateMemoryDisplay();
            UpdateStatistics();
        }

        private void BtnPrivacy_Click(object sender, RoutedEventArgs e)
        {
            if (waitingForName)
            {
                AddBotMessage("Please tell me your name first!", "CyberShield", DateTime.Now.ToString("HH:mm"));
                return;
            }
            currentTopic = "Privacy";
            tipsGiven++;
            StoreUserInterest("Privacy");
            AddBotMessage(privacyResponses[rand.Next(privacyResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("Type 'more' for additional privacy protection tips! 👁️", "CyberShield", DateTime.Now.ToString("HH:mm"));
            UpdateMemoryDisplay();
            UpdateStatistics();
        }

        private void BtnScams_Click(object sender, RoutedEventArgs e)
        {
            if (waitingForName)
            {
                AddBotMessage("Please tell me your name first!", "CyberShield", DateTime.Now.ToString("HH:mm"));
                return;
            }
            currentTopic = "Scams";
            tipsGiven++;
            StoreUserInterest("Scam Detection");
            AddBotMessage(scamResponses[rand.Next(scamResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("Type 'more' for additional scam prevention tips! ⚠️", "CyberShield", DateTime.Now.ToString("HH:mm"));
            UpdateMemoryDisplay();
            UpdateStatistics();
        }

        private void BtnSafeBrowsing_Click(object sender, RoutedEventArgs e)
        {
            if (waitingForName)
            {
                AddBotMessage("Please tell me your name first!", "CyberShield", DateTime.Now.ToString("HH:mm"));
                return;
            }
            currentTopic = "Safe browsing";
            tipsGiven++;
            StoreUserInterest("Safe Browsing");
            AddBotMessage(safeBrowsingResponses[rand.Next(safeBrowsingResponses.Length)], "CyberShield", DateTime.Now.ToString("HH:mm"));
            AddBotMessage("Type 'more' for additional safe browsing tips! 🌐", "CyberShield", DateTime.Now.ToString("HH:mm"));
            UpdateMemoryDisplay();
            UpdateStatistics();
        }

        private void AddUserMessage(string message, string timestamp)
        {
            try
            {
                Paragraph paragraph = new Paragraph();
                Bold boldName = new Bold(new Run($"You · {timestamp}"));
                boldName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CDD6F4");
                Run messageRun = new Run($"\n{message}");
                messageRun.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CDD6F4");
                paragraph.Inlines.Add(boldName);
                paragraph.Inlines.Add(messageRun);
                rtbChat.Document.Blocks.Add(paragraph);
                rtbChat.ScrollToEnd();
            }
            catch { }
        }

        private void AddBotMessage(string message, string sender, string timestamp)
        {
            try
            {
                Paragraph paragraph = new Paragraph();
                Bold boldName = new Bold(new Run($"{sender} · {timestamp}"));
                boldName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#89B4FA");
                Run messageRun = new Run($"\n{message}");
                messageRun.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CDD6F4");
                paragraph.Inlines.Add(boldName);
                paragraph.Inlines.Add(messageRun);
                rtbChat.Document.Blocks.Add(paragraph);
                rtbChat.ScrollToEnd();
            }
            catch { }
        }

        private void AddSentimentMessage(string message)
        {
            try
            {
                Paragraph paragraph = new Paragraph();
                Italic italicMessage = new Italic(new Run($"--- {message} ---"));
                italicMessage.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#F9E2AF");
                paragraph.Inlines.Add(italicMessage);
                rtbChat.Document.Blocks.Add(paragraph);
                rtbChat.ScrollToEnd();
            }
            catch { }
        }
    }
}