using System.Collections;
using System;

namespace split
{
    public class split_function
    {
        private ArrayList reply = new ArrayList(); // List of predefined chatbot responses
        private ArrayList ignores = new ArrayList(); // List of words to ignore in user input

        // Constructor to initialize responses and ignored words
        public split_function()
        {
            store_replies(); // Store the predefined chatbot responses
            store_ignore();  // Store the words to ignore in the input
        }

        // Method to process the user's question and generate a response
        public void ProcessQuestion(string question)
        {
            // Split the question into words
            string[] store_word = question.Split(' ');
            ArrayList filter = new ArrayList();

            // Filter out the words that should be ignored
            foreach (string word in store_word)
            {
                if (!ignores.Contains(word))
                {
                    filter.Add(word);
                }
            }

            string message = ""; // Store the chatbot's response
            bool found = false; // Flag to track if a response was found

            // Check if any of the filtered words match the predefined responses
            foreach (string word in filter)
            {
                foreach (string response in reply)
                {
                    // If the response contains the word, add it to the message
                    if (response.ToLower().Contains(word))
                    {
                        found = true; // Mark that a response was found
                        message += response + "\n"; // Add the response to the message
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            // Provide specific responses for certain questions
            if (question.Contains("how are you"))
            {
                // Special response for "How are you?" question
                Console.WriteLine("I'm just a bot, but I'm running securely! Thanks for asking.");
            }
            else if (question.Contains("what’s your purpose?") || question.Contains("what is your purpose?"))
            {
                // Special response for "What is your purpose?" question
                Console.WriteLine("My purpose is to educate you about cybersecurity and help you stay safe online.");
            }
            else
            {
                // If no specific response was found, print a default message
                Console.WriteLine(found ? message : "Write something related to cybersecurity.");
            }
            Console.ForegroundColor = ConsoleColor.White; // Reset the console color after printing the response
        }

        // Method to store predefined chatbot responses
        private void store_replies()
        {
            // Full, detailed responses for various cybersecurity-related topics
            reply.Add("• Cybersecurity is a comprehensive practice dedicated to safeguarding systems, networks, and programs from the growing number of digital attacks. These attacks can range from unauthorized data access and malware infections to more sophisticated threats like ransomware. Effective cybersecurity involves various strategies, tools, and processes designed to protect both the integrity and confidentiality of data, as well as ensuring the availability of services in the face of potential threats. It is a crucial component in both personal and organizational digital safety, as cybercriminals continuously evolve their tactics.");
            reply.Add("• Phishing is a deceptive cyberattack method where attackers attempt to trick individuals into divulging sensitive information, such as usernames, passwords, or credit card details, by masquerading as trustworthy sources. These attacks typically occur through emails, social media, or even text messages, with attackers creating fake websites or messages that closely resemble legitimate ones. Phishing scams often prey on individuals' trust or fear, urging them to act quickly or providing a sense of urgency, making it vital for people to be cautious when handling unsolicited communication.");
            reply.Add("• A strong password is essential for protecting your online accounts and personal information. It should be long, typically at least 12 characters, and contain a mix of uppercase and lowercase letters, numbers, and special characters. This complexity makes it much harder for attackers to crack using brute-force methods. Avoid using common words, phrases, or easily guessable information, such as your name or birthdate. Additionally, using unique passwords for each account ensures that a breach in one does not compromise all your other accounts.");
            reply.Add("• Multi-factor authentication (MFA) adds an important extra layer of security to your online accounts. Instead of relying solely on a password, MFA requires users to provide multiple forms of verification to confirm their identity. These can include something you know (like a password), something you have (like a smartphone or hardware token), or something you are (such as a fingerprint or facial recognition). By requiring more than just a password, MFA significantly reduces the likelihood of unauthorized access even if a password is compromised.");
            reply.Add("• Malware, short for malicious software, refers to any software specifically designed to harm, exploit, or disrupt computer systems and networks. It includes a wide range of threats such as viruses, worms, Trojans, ransomware, and spyware. Malware can be used to steal sensitive information, damage or delete files, take control of systems, or even hold data hostage for ransom.");
            reply.Add("• Safe browsing is a set of practices and habits designed to protect users while navigating the internet. It involves staying cautious about the websites you visit, the links you click, and the personal information you share online. To ensure safe browsing, always check for the presence of HTTPS in the website's URL, which indicates a secure connection. Be wary of suspicious websites, particularly those that look unprofessional, contain unusual pop-ups, or ask for sensitive information without proper justification.");
            reply.Add("• A Denial-of-Service (DoS) attack is a type of cyberattack where attackers attempt to make a system, service, or network unavailable by overwhelming it with a flood of internet traffic. In a Distributed Denial-of-Service (DDoS) attack, the traffic comes from multiple sources, making it even harder to stop. These attacks can cause websites or services to become slow or unresponsive, potentially leading to significant financial loss for organizations. DoS attacks can be launched for various reasons, including political motives, competitive sabotage, or simply for the thrill of disruption.");
            reply.Add("• A zero-day attack occurs when a cybercriminal exploits a previously unknown vulnerability in software or hardware. Because the vulnerability is not yet recognized by the vendor or security community, there are no patches or defenses available to mitigate the attack. Zero-day vulnerabilities are highly valuable on the black market, as they can provide attackers with undetected access to systems. Regular software updates and patch management can help mitigate the risk of zero-day attacks.");
            reply.Add("• A Man-in-the-Middle (MITM) attack occurs when a cybercriminal intercepts and potentially alters the communication between two parties who believe they are directly communicating with each other. MITM attacks often occur on unsecured public Wi-Fi networks, where attackers can capture and manipulate data being sent between users and websites. This could include stealing login credentials, injecting malicious code, or redirecting users to fake websites. Using encryption (e.g., HTTPS) and VPNs can help protect against MITM attacks.");
        }

        // Method to store words that should be ignored in the input
        private void store_ignore()
        {
            ignores.Add("tell"); ignores.Add("how");
            ignores.Add("me"); ignores.Add("a");
            ignores.Add("about"); ignores.Add("and");
            ignores.Add("is"); ignores.Add("what");
            ignores.Add("or"); ignores.Add("when");
            ignores.Add("an"); ignores.Add("you");
            ignores.Add("by"); ignores.Add("of");
            ignores.Add("can"); ignores.Add("i");
            ignores.Add("if"); ignores.Add("possible");
            ignores.Add("what's"); ignores.Add("possibility");
            ignores.Add("possibilities"); ignores.Add("are");
            ignores.Add("the"); ignores.Add("they");
            ignores.Add("them"); ignores.Add("there");
            ignores.Add("then"); ignores.Add("car");
            ignores.Add("but"); ignores.Add("so");
            ignores.Add("with"); ignores.Add("without");
            ignores.Add("for"); ignores.Add("in");
            ignores.Add("on"); ignores.Add("at");
            ignores.Add("to"); ignores.Add("from");
            ignores.Add("as"); ignores.Add("that");
            ignores.Add("which"); ignores.Add("who");
            ignores.Add("whom"); ignores.Add("this");
            ignores.Add("these"); ignores.Add("those");
            ignores.Add("where"); ignores.Add("why");
            ignores.Add("yes"); ignores.Add("no");
            ignores.Add("not"); ignores.Add("will");
            ignores.Add("do"); ignores.Add("was");
            ignores.Add("were"); ignores.Add("be");
            ignores.Add("been"); ignores.Add("being");
            ignores.Add("have"); ignores.Add("has");
            ignores.Add("had"); ignores.Add("having");
            ignores.Add("it"); ignores.Add("its");
            ignores.Add("my"); ignores.Add("your");
            ignores.Add("his"); ignores.Add("her");
            ignores.Add("their"); ignores.Add("our");
            ignores.Add("we"); ignores.Add("you");
            ignores.Add("us"); ignores.Add("one");
            ignores.Add("two"); ignores.Add("three");
            ignores.Add("four"); ignores.Add("five");
            ignores.Add("first"); ignores.Add("second");
            ignores.Add("third"); ignores.Add("last");
            ignores.Add("next"); ignores.Add("previous");
            ignores.Add("only"); ignores.Add("some");
            ignores.Add("all"); ignores.Add("few");
            ignores.Add("many"); ignores.Add("much");
            ignores.Add("more"); ignores.Add("less");
            ignores.Add("least"); ignores.Add("most");
            ignores.Add("several"); ignores.Add("each");
            ignores.Add("every"); ignores.Add("another");
            ignores.Add("any"); ignores.Add("none");
            ignores.Add("such"); ignores.Add("either");
            ignores.Add("neither"); ignores.Add("both");
            ignores.Add("ten"); ignores.Add("hundred");
            ignores.Add("thousand"); ignores.Add("million");
            ignores.Add("billion"); ignores.Add("year");
            ignores.Add("month"); ignores.Add("day");
            ignores.Add("hour"); ignores.Add("minute");
            ignores.Add("second"); ignores.Add("now");
            ignores.Add("soon"); ignores.Add("later");
            ignores.Add("yesterday"); ignores.Add("today");
            ignores.Add("tomorrow"); ignores.Add("always");
            ignores.Add("usually"); ignores.Add("sometimes");
            ignores.Add("rarely"); ignores.Add("never");



        }
    }
}
