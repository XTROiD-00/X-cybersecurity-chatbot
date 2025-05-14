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
        const string MemoryFile = "memory.txt";

        static void Main(string[] args)
        {
            DisplayAsciiLogo("poeimg.jpg");
            PlayVoiceGreeting("audiosamp.wav");

            string userName = "";
            string lastQuestion = "";

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

            var chatbot = new SplitFunction();
            RunChatbot(chatbot, userName);
        }

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

                // Update memory
                File.WriteAllLines(MemoryFile, new[] { userName, userInput });

                chatbot.ProcessQuestion(userInput.ToLower());
            }
        }

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

        static void DisplayWelcomeMessage(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nWelcome, {userName}! I am -X-, your cybersecurity assistant.");
            Console.WriteLine("I'm here to help you stay safe online.\n");
            Console.ResetColor();
        }
    }

    class SplitFunction
    {
        private Dictionary<string, List<string>> topics = new Dictionary<string, List<string>>();
        private List<string> ignores = new List<string>();
        private Dictionary<string, string> sentiments = new Dictionary<string, string>();
        private Random rand = new Random();

        public SplitFunction()
        {
            StoreReplies();
            StoreIgnoreWords();
            StoreSentiments();
        }

        public void ProcessQuestion(string input)
        {
            string sentimentMsg = null;
            string topicMsg = null;

            foreach (var pair in sentiments)
                if (input.Contains(pair.Key)) { sentimentMsg = pair.Value; break; }

            List<string> filteredWords = new List<string>();
            foreach (string word in input.Split(' '))
                if (!ignores.Contains(word)) filteredWords.Add(word);

            foreach (string word in filteredWords)
                if (topics.ContainsKey(word))
                {
                    var responses = topics[word];
                    topicMsg = responses[rand.Next(responses.Count)];
                    break;
                }

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

private void StoreReplies()
{
            topics["phishing"] = new List<string>
    {
        "• Phishing is a deceptive tactic where attackers impersonate legitimate institutions via email or messages to trick you into revealing sensitive information like passwords or credit card numbers. Always double-check the sender's address and avoid clicking suspicious links.",
        "• Many phishing scams use urgent or emotional language to trick users into acting quickly. If an email claims your account will be locked or you're entitled to a reward, stop and verify it directly through the company’s official website.",
        "• One of the best ways to avoid phishing attacks is to never provide personal information through email. Banks and legitimate companies will never ask for passwords or login details via email."
    };

            topics["malware"] = new List<string>
    {
        "• Malware is software intentionally designed to cause damage to devices, steal data, or gain unauthorized access. It includes viruses, worms, Trojans, and ransomware. Regularly update your software and avoid downloading files from unknown sources.",
        "• To protect yourself from malware, install a reliable antivirus program and enable real-time protection. Also, be cautious about clicking on pop-ups and never open unexpected email attachments.",
        "• Some malware can operate silently in the background, collecting your keystrokes or files. Running regular scans and monitoring your system performance can help detect and remove such threats."
    };

    topics["passwords"] = new List<string>
    {
        "• Creating a strong password involves using a mix of uppercase and lowercase letters, numbers, and special characters. Avoid using personal details like birthdays or pet names, as these can be easily guessed.",
        "• Don’t reuse passwords across multiple accounts. If one site is breached, hackers can try the same credentials elsewhere. Consider using a password manager to securely store and generate unique passwords for each site.",
        "• Updating your passwords regularly reduces the risk of unauthorized access. Make it a habit to change critical passwords like those for your email or banking every few months."
    };

    topics["mfa"] = new List<string>
    {
        "• Multi-Factor Authentication (MFA) adds an extra layer of security by requiring you to provide something beyond your password — like a code from your phone or a fingerprint scan. It's one of the best ways to protect your accounts.",
        "• Even if someone steals your password, MFA can prevent unauthorized access by asking for a second factor only you have. Enabling MFA on your accounts makes it significantly harder for hackers to get in.",
        "• Use MFA wherever possible — especially on sensitive accounts like your email, financial apps, and cloud storage. It takes just a few seconds but drastically increases your security."
    };

    topics["browsing"] = new List<string>
    {
        "• Safe browsing means being mindful of the websites you visit and the links you click. Avoid clicking on pop-ups, don’t download random files, and always check the URL to make sure it's secure (look for HTTPS).",
        "• Many websites track your activity to show ads or gather data. Use privacy-focused browsers like Brave or Firefox and consider installing ad blockers or tracker blockers to enhance your privacy.",
        "• Public Wi-Fi networks can be risky — avoid logging into personal accounts while on them, and use a VPN if you need to access sensitive data over an open network."
    };

    topics["ddos"] = new List<string>
    {
        "• A Distributed Denial-of-Service (DDoS) attack floods a server with so much traffic that it becomes unavailable to users. These attacks are often launched using botnets — networks of infected devices controlled by attackers.",
        "• While individuals aren’t usually targeted by DDoS, it’s important to understand that websites and online services can go down because of them. Businesses often use special firewalls and load balancers to help defend against such attacks.",
        "• If you're managing a site or service, consider working with your hosting provider to implement DDoS protection. Being prepared in advance can minimize disruption if you are targeted."
    };

    topics["encryption"] = new List<string>
    {
        "• Encryption is a method of converting information into a code to prevent unauthorized access. It protects your messages, files, and transactions online from being read by hackers or third parties.",
        "• End-to-end encryption ensures that only the sender and receiver can read a message. Services like WhatsApp and Signal use this technology to keep your conversations private, even from the service providers.",
        "• Always look for HTTPS in the address bar of websites — the 'S' means your connection is encrypted and secure. Avoid entering sensitive information on sites without it."
    };

    topics["privacy"] = new List<string>
    {
        "• Online privacy involves controlling what personal information you share and with whom. Review your social media privacy settings to make sure only trusted people can see your posts.",
        "• Be cautious about apps and websites asking for access to your location, camera, or contacts. Only grant permissions when absolutely necessary and uninstall apps you no longer use.",
        "• Consider using tools like privacy-focused browsers, search engines like DuckDuckGo, and browser extensions that block trackers to help reduce digital footprints."
    };
}

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
