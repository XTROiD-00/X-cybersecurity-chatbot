using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Text;

namespace CyberSecurityChatbot
{
    class Program
    {
        const string MemoryFile = "memory.txt"; // File to store user name and last question

        static void Main(string[] args)
        {
            DisplayAsciiLogo("poeimg.jpg"); // Display ASCII logo converted from image
            PlayVoiceGreeting("audiosamp.wav"); // Play welcome audio

            string userName = "";
            string lastQuestion = "";

            // Load user memory if exists
            if (File.Exists(MemoryFile))
            {
                string[] memory = File.ReadAllLines(MemoryFile);
                if (memory.Length >= 2)
                {
                    userName = memory[0];
                    lastQuestion = memory[1];
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Welcome back, {userName}!");
                    Console.WriteLine($"Last time you asked: \"{lastQuestion}\"\n");
                    Console.ResetColor();
                }
            }

            // Prompt for name if not stored
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.Write("What is your name? ");
                userName = Console.ReadLine()?.Trim();
                while (string.IsNullOrEmpty(userName))
                {
                    Console.Write("Please enter a valid name: ");
                    userName = Console.ReadLine()?.Trim();
                }
            }

            DisplayWelcomeMessage(userName);
            Console.WriteLine($"\n_X_ > hey {userName}, click enter twice to continue");
            Console.ReadKey();
            Console.Write($"{userName} > ");
            Console.ReadLine();

            Console.WriteLine("_X_ > OK, Let's get to it...\n");

            var chatbot = new SplitFunction(); // Initialize chatbot logic class
            RunChatbot(chatbot, userName);     // Start chatbot interaction
        }

        // Main chat loop
        static void RunChatbot(SplitFunction chatbot, string userName)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n -X- => ");
                Console.ResetColor();
                Console.Write("got any questions on cybersecurity? Type 'exit' to leave.\n");
                Console.Write($"{userName} => ");
                string userInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(userInput)) continue;

                if (userInput.ToLower() == "exit")
                {
                    Console.WriteLine($"\nGoodbye {userName}! Stay safe online.");
                    break;
                }

                // Save current user input
                File.WriteAllLines(MemoryFile, new[] { userName, userInput });

                // Process the user's question
                chatbot.ProcessQuestion(userInput.ToLower());
            }
        }

        // Display ASCII art from image
        static void DisplayAsciiLogo(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                Bitmap image = new Bitmap(imagePath);
                int width = 80;
                int height = 40;
                Bitmap resized = new Bitmap(image, new Size(width, height));
                Console.WriteLine(ConvertImageToAscii(resized));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n Welcome to -X- Cybersecurity Assistant ChatBot.");
                Console.WriteLine(" Ask me about phishing, malware, strong passwords, and more!\n");
                Console.ResetColor();
            }
            else Console.WriteLine("[Error] ASCII logo image not found!");
        }

        // Convert bitmap to ASCII art string
        static string ConvertImageToAscii(Bitmap image)
        {
            StringBuilder result = new StringBuilder();
            string chars = "@#$%*-";
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    int gray = (int)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);
                    int index = gray * (chars.Length - 1) / 255;
                    result.Append(chars[index]);
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        // Play .wav audio file
        static void PlayVoiceGreeting(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                try
                {
                    using (SoundPlayer player = new SoundPlayer(path))
                    {
                        player.PlaySync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Unable to play audio: {ex.Message}");
                }
            }
            else Console.WriteLine("[Error] Audio file not found!");
        }

        // Display greeting message
        static void DisplayWelcomeMessage(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nWelcome, {userName}! I am -X-, your cybersecurity assistant.");
            Console.WriteLine("I'm here to help you stay safe online.\n");
            Console.ResetColor();
        }
    }

    // Core chatbot logic
    class SplitFunction
    {
        private Dictionary<string, List<string>> topics = new Dictionary<string, List<string>>();
        private List<string> ignores = new List<string>();
        private Dictionary<string, string> sentiments = new Dictionary<string, string>();
        private Random rand = new Random();

        // Constructor to initialize replies and keyword sets
        public SplitFunction()
        {
            StoreReplies();
            StoreIgnoreWords();
            StoreSentiments();
        }

        // Analyze and respond to input
        public void ProcessQuestion(string input)
        {
            string sentimentMsg = null;
            string topicMsg = null;

            // Detect sentiment keywords
            foreach (var pair in sentiments)
                if (input.Contains(pair.Key)) { sentimentMsg = pair.Value; break; }

            // Remove common/ignored words
            List<string> filteredWords = new List<string>();
            foreach (string word in input.Split(' '))
                if (!ignores.Contains(word)) filteredWords.Add(word);

            // Match input with known topic keywords
            foreach (string word in filteredWords)
                if (topics.ContainsKey(word))
                {
                    var responses = topics[word];
                    topicMsg = responses[rand.Next(responses.Count)];
                    break;
                }

            // Provide appropriate response
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (sentimentMsg != null && topicMsg != null)
                Console.WriteLine($"\n -X- => {sentimentMsg} {topicMsg}");
            else if (sentimentMsg != null)
                Console.WriteLine($"\n -X- => {sentimentMsg}");
            else if (topicMsg != null)
                Console.WriteLine($"\n -X- => {topicMsg}");
            else if (input.Contains("how are you"))
                Console.WriteLine("I'm just a bot, but I'm running securely! Thanks for asking.");
            else if (input.Contains("purpose"))
                Console.WriteLine("I'm here to teach you about cybersecurity and protect you online.");
            else if (input.Contains("what can i ask"))
                Console.WriteLine("Ask me about phishing, malware, browsing safely, passwords, MFA, and more.");
            else
                Console.WriteLine("Sorry, could you rephrase or ask a cybersecurity question?");
            Console.ResetColor();
        }

        // Populate cybersecurity topic replies
        private void StoreReplies()
        {
            // Each keyword has a list of possible replies
            topics["phishing"] = new List<string>
            {
                "• Phishing is a deceptive tactic where attackers impersonate legitimate institutions...",
                "• Many phishing scams use urgent or emotional language...",
                "• One of the best ways to avoid phishing attacks is to never provide personal information..."
            };

            topics["malware"] = new List<string>
            {
                "• Malware is software intentionally designed to cause damage...",
                "• To protect yourself from malware, install a reliable antivirus...",
                "• Some malware can operate silently in the background..."
            };

            topics["passwords"] = new List<string>
            {
                "• Creating a strong password involves using a mix...",
                "• Don’t reuse passwords across multiple accounts...",
                "• Updating your passwords regularly reduces the risk..."
            };

            topics["mfa"] = new List<string>
            {
                "• Multi-Factor Authentication (MFA) adds an extra layer of security...",
                "• Even if someone steals your password, MFA can prevent access...",
                "• Use MFA wherever possible — especially on sensitive accounts..."
            };

            topics["browsing"] = new List<string>
            {
                "• Safe browsing means being mindful of the websites you visit...",
                "• Many websites track your activity...",
                "• Public Wi-Fi networks can be risky..."
            };

            topics["ddos"] = new List<string>
            {
                "• A Distributed Denial-of-Service (DDoS) attack floods a server...",
                "• While individuals aren’t usually targeted by DDoS...",
                "• If you're managing a site, consider DDoS protection..."
            };

            topics["encryption"] = new List<string>
            {
                "• Encryption is a method of converting information into a code...",
                "• End-to-end encryption ensures only sender and receiver can read...",
                "• Always look for HTTPS in the address bar..."
            };

            topics["privacy"] = new List<string>
            {
                "• Online privacy involves controlling what you share...",
                "• Be cautious about apps asking for access...",
                "• Consider using privacy-focused tools..."
            };
        }

        // Define ignored common words
        private void StoreIgnoreWords()
        {
            ignores.AddRange(new string[]
            {
                "the", "is", "a", "an", "of", "on", "at", "to", "and", "or", "but", "in",
                "about", "with", "as", "by", "for", "if", "can", "i", "you", "we", "they",
                "are", "was", "were", "have", "has", "do", "does", "did", "how", "what",
                "who", "when", "where", "why", "which", "it", "this", "that", "these",
                "those", "any", "some", "just", "more", "less", "also", "only", "tell", "me", "please"
            });
        }

        // Define words that indicate sentiment
        private void StoreSentiments()
        {
            sentiments["worried"] = "It's okay to feel that way. Let's boost your security together.";
            sentiments["frustrated"] = "Frustration is valid — I'll keep it simple.";
            sentiments["curious"] = "Curiosity is good! Let's explore.";
            sentiments["confused"] = "Let’s clarify that for you.";
            sentiments["anxious"] = "No stress. I’m here to guide you.";
        }
    }
}
