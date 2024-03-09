using System.Text;
namespace LearninigC_
{
    public class Program
    {
        private static readonly Dictionary<string, string> _passwordEntries = new Dictionary<string, string>();

        public static void Main(string[] args)
        {
            
            ReadPasswords();
            CheckOrCreatePasswordForApp();
            while (true)
            {
                Console.WriteLine("Please select an option: ");
                Console.WriteLine("[1] List all passwords");
                Console.WriteLine("[2] Add/Change password");
                Console.WriteLine("[3] Get Password");
                Console.WriteLine("[4] Delete Password");
                int _selectedOption = int.Parse(Console.ReadLine());
                if (_selectedOption == 1)
                    ListPassword();
                else if (_selectedOption == 2)
                    AddOrChangePassword();
                else if (_selectedOption == 3)
                    GetPassword();
                else if (_selectedOption == 4)
                    DeletePassword();
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Option..");
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine("===============================================");
            }

        }

        public static void CheckOrCreatePasswordForApp()
        {
            if (_passwordEntries.ContainsKey("masterKey"))
            {
                Console.Write("Please enter your password: ");
                string _pass = Console.ReadLine();
                int _count = 1;
                while (_pass != (_passwordEntries["masterKey"]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid password...");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("try again: ");
                    _pass = Console.ReadLine();
                    _count++;
                    if (_count > 3) 
                        break;
                }
                if(_count > 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    DecrementingTimer(60, 0, 7); // Start a one-minute timer at row 10
                    Console.ForegroundColor= ConsoleColor.Green;
                    CheckOrCreatePasswordForApp(); // Recall the verification function
                    return;
                }
                else
                    Console.WriteLine("Welcome to our services..");
            }
            else
            {
                Console.Write("Enter the password: ");
                string _pass = Console.ReadLine();
                _passwordEntries.Add("masterKey",(_pass));
                string _master = "masterKey="+EncryptionUtility.Encrypt(_passwordEntries["masterKey"]);
                var _filePath = "D:\\passwords.txt";
                File.WriteAllText(_filePath, _master);
            }
        }
        private static void ListPassword()
        {
            foreach (var entry in _passwordEntries)
                Console.WriteLine($"{entry.Key}={entry.Value}");
        }

        private static void AddOrChangePassword()
        {
            Console.Write("Please enter Website/App name: ");
            var _appName = Console.ReadLine();
            Console.Write("Please enter your password: ");
            var _password = Console.ReadLine();
            if (_passwordEntries.ContainsKey(_appName))
                _passwordEntries[_appName] = _password;
            else
                _passwordEntries.Add(_appName, _password);

            SavePassword();
        }

        private static void GetPassword()
        {
            Console.Write("Please enter Website/App name: ");
            var _appName = Console.ReadLine();
            if (_passwordEntries.ContainsKey(_appName))
                Console.WriteLine($"Your password is: {_passwordEntries[_appName]}");
            else
                Console.WriteLine("Password not Found");
        }

        private static void DeletePassword()
        {
            Console.Write("Please enter Website/App name: ");
            var _appName = Console.ReadLine();
            if (_passwordEntries.ContainsKey(_appName))
            {
                _passwordEntries.Remove(_appName);
                SavePassword();
            }
            else
                Console.WriteLine("Password not Found");
        }
        private static void ReadPasswords()
        {
            // this is the path of the file in your device
            var _filePath = "D:\\passwords.txt";
            if (File.Exists(_filePath))
            {
                var _passowrdLine = File.ReadAllText(_filePath);
                string _appName;
                string _password;
                int _equalIndex;
                foreach (var line in _passowrdLine.Split(Environment.NewLine))
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        _equalIndex = line.IndexOf('=');
                        _appName = line.Substring(0, _equalIndex);
                        _password = line.Substring(_equalIndex + 1);
                        _passwordEntries.Add(_appName, EncryptionUtility.Encrypt(_password));
                    }
                }
            }
        }

        private static void SavePassword()
        {
            StringBuilder _sb = new StringBuilder();
            foreach (var entry in _passwordEntries)
                _sb.AppendLine($"{entry.Key}={EncryptionUtility.Decrypt(entry.Value)}");
            var _filePath = "D:\\passwords.txt";
            File.WriteAllText(_filePath, _sb.ToString());
        }

        public static void DecrementingTimer(int seconds, int column, int row)
        {
            for (int i = seconds; i >= 0; i--)
            {
                Console.WriteLine($"Please try again in {i} seconds...");
                Thread.Sleep(1000); // Pause for one second
                Console.SetCursorPosition(column, row); // Move cursor back to the beginning of the line
                Console.Write(new string(' ', Console.WindowWidth)); // Clear the line
            }
        }
    }


    public class EncryptionUtility
    {
        private static readonly string _originalChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly string _fakeTable = "6pXF7rKeu2N5SlEPBnAiQyGf8UvJzg3d1bhxCtTL04YkOW9DMsRaZmVcjHqoIw";

        public static string Encrypt(string password)
        {
            StringBuilder _sb = new StringBuilder();
            int _charIndex;
            foreach(var ch in password)
            {
                _charIndex = _originalChars.IndexOf(ch);
                _sb.Append(_fakeTable[_charIndex]);
            }
            return _sb.ToString();
        }

        public static string Decrypt(string password)
        {
            StringBuilder _sb = new StringBuilder();
            int _charIbdex;
            foreach (var ch in password)
            {
                _charIbdex = _fakeTable.IndexOf(ch);
                _sb.Append(_originalChars[_charIbdex]);
            }
            return _sb.ToString();
        }
    }
}
