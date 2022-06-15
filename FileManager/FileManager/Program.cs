using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Text.RegularExpressions;
using System.Xml;


namespace ConsoleApp1
{
    /// <summary>
    /// Класс, отвечающий за работу файлового менеджера.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Запускает программу, а также считывает и передает на
        /// обработку введенную пользователем команду.
        /// </summary>
        static void Main()
        {
            string comand;
            PrintBlue("Для справки введите команду help и нажмите Enter");
            while (true)
            {
                Console.Write(Environment.NewLine + Directory.GetCurrentDirectory() + ">");
                comand = ParseComand(Console.ReadLine());

                if (string.IsNullOrEmpty(comand))
                {
                    PrintRed("Неизвестная команда.");
                    continue;
                }

                if (!Process(comand))
                    break;
            }
        }

        /// <summary>
        /// Разбивает строку по пробелам и табуляции в массив строк и
        /// возвращает первый элемент этого массива.
        /// </summary>
        /// <param name="comand">Строка, которую необходимо разбить по пробелам и табуляции.</param>
        /// <returns>Возвращает первый элемент массива разбитых по пробелам и табуляции строк.
        /// Если массив пустой, возвращает пустую строку.</returns>
        public static string ParseComand(string comand)
        {
            char[] separators = { ' ', '\t' };
            string[] parts = comand.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 1 ? parts[0] : "";
        }

        /// <summary>
        /// Обрабатывает введенную команду и запускает методы, соответствующие
        /// введенной команде.
        /// </summary>
        /// <param name="comand">Введенная пользователем команда.</param>
        /// <returns>Возвращает false, если пользователь ввел exit, иначе true.</returns>
        public static bool Process(string comand)
        {
            switch (comand)
            {
                case "drives":
                    PrintAllDrives();
                    return true;
                case "files":
                    PrintAllFiles();
                    return true;
                case "folders":
                    PrintFolders();
                    return true;
                case "drive":
                    GoToDisk();
                    return true;
                case "abs":
                    GoToAbsolutePath();
                    return true;
                case "cd":
                    GoToRelativePath();
                    return true;
                case "read":
                    ReadFile();
                    return true;
                case "copy":
                    CopyFile();
                    return true;
                case "move":
                    MoveFile();
                    return true;
                case "make":
                    MakeFile();
                    return true;
                case "reads":
                    ReadFiles();
                    return true;
                case "delete":
                    DeleteFile();
                    return true;
                case "mask":
                    PrintMaskFiles();
                    return true;
                case "masks":
                    PrintAllMaskFiles();
                    return true;
                case "help":
                    PrintHelp();
                    return true;
                case "exit":
                    return false;
                default:
                    PrintRed("Неверный формат.");
                    return true;
            }
        }

        /// <summary>
        /// Запрашивает у пользователя маску в формате регулярных выражений C# и выводит на экран все файлы
        /// в текущей директории и поддиректориях, которые удовлетворяют заданной маске.
        /// </summary>
        public static void PrintAllMaskFiles()
        {
            try
            {
                PrintBlue("Введите маску, по которой необходимо вывести файлы во всех каталогах текущей директории");
                string mask = Console.ReadLine();
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);
                bool found = false;
                string separator = Path.DirectorySeparatorChar.ToString();
                foreach (var s in files)
                {
                    string[] tmp = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length >= 1)
                    {
                        Match result = Regex.Match(tmp[^1], mask);
                        if (result.Success)
                        {
                            PrintYellow(tmp[^1]);
                            found = true;
                        }
                    }
                }

                if (!found)
                {
                    PrintRed("Файлы с такой маской не найдены.");
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка поиска файлов по маске.");
            }
        }

        /// <summary>
        /// Запрашивает у пользователя маску в формате регулярных выражений C# и выводит на экран все файлы
        /// в текущей директории, которые удовлетворяют заданной маске.
        /// </summary>
        public static void PrintMaskFiles()
        {
            try
            {
                PrintBlue("Введите маску, по которой необходимо вывести файлы в текущей директории:");
                string mask = Console.ReadLine();
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
                bool found = false;
                string separator = Path.DirectorySeparatorChar.ToString();
                foreach (var s in files)
                {
                    string[] tmp = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length >= 1)
                    {
                        Match result = Regex.Match(tmp[^1], mask);
                        if (result.Success)
                        {
                            PrintYellow(tmp[^1]);
                            found = true;
                        }

                    }
                }

                if (!found)
                {
                    PrintRed("Файлы с такой маской не найдены.");
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка поиска файлов по маске.");
            }
        }

        /// <summary>
        /// Выводит на экран иструкции по работе с программой и инструкции по каждой команде.
        /// </summary>
        public static void PrintHelp()
        {
            PrintBlue($@"В программе всегда выведена текущая директория и знак >, {Environment.NewLine} 
означающий, что программа ждет ввод команды. Текущую
директорию можно менять с помощью команд.{Environment.NewLine} 
Если вы вводите абсолютный путь до файла, то необходимо вводить его включая имя файла. {Environment.NewLine}
Если ввести команду, которой нет в списке доступных, либо у нее неверный формат, то
программа сообщит об этом и предложит ввести команду заново. {Environment.NewLine}
Необходимо сначала вводить команду, затем следовать инструкциям программы. {Environment.NewLine}
Список доступных команд:");
            PrintGreen("drives");
            PrintBlue(@"Вывод всех доступных для использования дисков на устройстве,
показывает, сколько всего и сколько доступно памяти на каждом из дисков в мегабайтах.");
            PrintGreen("files");
            PrintBlue(@"Выводит всех файлов, находящихся в текущей директории.");
            PrintGreen("folders");
            PrintBlue(@"Вывод всех папок, находящихся в текущей директории.");
            PrintGreen("drive");
            PrintBlue(@"Смена диска. После ввода команды программа выведет список доступных дисков,
после чего нужно выбрать и ввести номер диска, на который необходимо перейти.");
            PrintGreen("abs");
            PrintBlue(@"Переход в директорию по абсолютному пути. После ввода команды программа предложит 
ввести абсолютный путь, куда нужно перейти. Допустимо вводить .., тогда
будет осуществлен переход в родительскую папку.");
            PrintGreen("cd");
            PrintBlue(@"Переход в директорию по относительному пути. После ввода команды программа предложит 
ввести название папки в текущей директории, в которую нужно перейти. Допустимо вводить .., тогда
будет осуществлен переход в родительскую папку. Допустимо вводить абсолютный путь.");
            PrintGreen("read");
            PrintBlue(@"Читает и выводит содержимое файла в заданной кодировке. После ввода команды
программа предложит ввести либо имя файла в текущей директории, либо абсолютный путь 
до файла. Далее необходимо выбрать кодировку, в которой нужно вывести файл.");
            PrintGreen("copy");
            PrintBlue(@"Копирует файл из одной директории в другую или в эту же. После ввода команды необходимо
ввести сначала то, что нужно скопировать (абсолютный путь до файла или его имя в текущей директории,
этот файл должен существовать),
затем то, куда нужно скопировать (абсолютный путь до файла вместе с новым именем или 
его новое имя в текущей директории). В директории, куда происходит копирование, не должно быть одноименного файла
с тем, который вы задали в качестве нового скопированного файла.");
            PrintGreen("move");
            PrintBlue(@"Перемещает файл из одной директории в другую или в эту же. После ввода команды необходимо
ввести сначала то, что нужно переместить (абсолютный путь до файла или его имя в текущей директории,
этот файл должен существовать),
затем то, куда нужно переместить (абсолютный путь до файла вместе с новым именем или 
его новое имя в текущей директории). В директории, куда происходит перемещение, не должно быть одноименного файла
с тем, который вы задали в качестве нового перемещенного файла.");
            PrintGreen("make");
            PrintBlue(@"Создает новый файл в указанной кодировке. После ввода команды
программа предложит ввести либо имя файла в текущей директории, либо абсолютный путь 
до файла. Далее необходимо выбрать кодировку, в которой нужно создать файл.
Файл создается в указанной кодировке с некоторым пробным текстом.");
            PrintGreen("reads");
            PrintBlue(@"Считывает указанные файлы в кодировке UTF-8 и выводит их конкатенацию. После ввода команды программа
предложит ввести количество считываемых файлов. Для каждого файла на отдельной строке программа предложит 
ввести либо имя файла в текущей директории, либо абсолютный путь до файла. Если введенного
файла не существует, программа сообщит об этом и пропустит его.");
            PrintGreen("delete");
            PrintBlue(@"Удаляет указанный файл. После ввода команды
программа предложит ввести либо имя файла в текущей директории, либо абсолютный путь 
до файла.");
            PrintGreen("mask");
            PrintBlue(@"Вывод всех файлов по заданной маске в текущей директории. После ввода команды программа
предлагает ввести пользователю маску, по которой будет осуществлен вывод файлов. Маска должна 
быть задана в формате регулярных выражений C#.)");
            PrintGreen("masks");
            PrintBlue(@"Вывод всех файлов по заданной маске в текущей директории и поддиректориях. После ввода команды программа
предлагает ввести пользователю маску, по которой будет осуществлен вывод файлов. Маска должна 
быть задана в формате регулярных выражений C#.");
            PrintGreen("help");
            PrintBlue(@"Выводит описание программы и инструкции по работе с ней.");
            PrintGreen("exit");
            PrintBlue(@"Завершает работу программы.");
        }

        /// <summary>
        /// Запрашивает и обрабатывает введеное пользователем число:
        /// количество файлов, которые должны быть считаны.
        /// </summary>
        /// <returns>Число: количество файлов, которые должны быть считаны.</returns>
        public static int GetCountFiles()
        {
            int countFiles = 0;
            while (true)
            {
                PrintBlue("Введите количество считываех файлов (от 1 до 10): ");
                if (!int.TryParse(Console.ReadLine(), out countFiles) || countFiles < 1 || countFiles > 10)
                {
                    PrintRed("Данные некорректны, повторите попытку.");
                    continue;
                }

                break;
            }

            return countFiles;
        }

        /// <summary>
        /// Считывает файл по указанному пути в кодировке UTF-8.
        /// </summary>
        /// <param name="path">Абсолютный путь или имя файла в текущей директории, который должен быть считан.</param>
        /// <returns>Возвращает массив строк, считанных с указанного файла.</returns>
        public static string[] GetFile(string path)
        {
            string[] file = { };
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                PrintRed("Файла не существует.");
                return file;
            }

            file = File.ReadAllLines(path, Encoding.UTF8);
            return file;
        }

        /// <summary>
        /// Метод отвечает за конкатенацию и вывод считанных файлов.
        /// Метод запрашивает у пользователя количество файлов, которые нужно считать,
        /// а также абсолютные пути или имена файлов в текущей директории.
        /// Далее метод выводит на экран конкатенацию указанных файлов.
        /// </summary>
        public static void ReadFiles()
        {
            try
            {
                int countFiles = GetCountFiles();
                PrintBlue(@"Введите имена или абсолютные пути до файлов, которые нужно считать,
на одной строке должен быть один файл.");

                List<string> readText = new List<string>();
                for (int i = 0; i < countFiles; ++i)
                {
                    Console.Write($"{i + 1} ");
                    string path = Console.ReadLine();
                    string[] lines = GetFile(path);
                    foreach (var line in lines)
                    {
                        readText.Add(line);
                    }
                }

                PrintGreen("Конкатенация заданных файлов: ");
                foreach (var line in readText)
                {
                    PrintYellow(line);
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка чтения файлов.");
                return;
            }
        }

        /// <summary>
        /// Метод отвечает за создание файла в указанной кодировке.
        /// Сначала запрашивается абсолютный путь либо имя нового файла в текущей директории
        /// и его кодировка, затем создается новый файл с этим именем в заданной кодировке,
        /// туда записываются несколько пробных строк.
        /// </summary>
        public static void MakeFile()
        {
            try
            {
                PrintBlue("Введите название файла, который будет создан:");
                string path = Console.ReadLine();

                if (string.IsNullOrEmpty(path) || File.Exists(path))
                {
                    PrintRed("Недопустимый формат или такой файл уже существует.");
                    return;
                }

                Encoding enc = GetEncodingName();

                if (!File.Exists(path))
                {
                    using (var sw = new StreamWriter(File.Open(path, FileMode.Create), enc))
                    {
                        sw.WriteLine($@"File encoding: {enc.WebName}. Lines for check: {Environment.NewLine}
world - english word
123 - digits
привет - russian word
!@#$%^& - special symbols
†‡ў§µ®©√╔ - strange symbols");
                    }
                    PrintGreen("Файл создан.");
                }
                else
                {
                    PrintRed("Такой файл уже существует.");
                    return;
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка создания файла.");
                return;
            }
        }

        /// <summary>
        /// Метод отвечает за удаление указанного файла.
        /// Сначала запрашивается абсолютный путь либо имя файла в текущей директории,
        /// затем этот файл удаляется.
        /// </summary>
        public static void DeleteFile()
        {
            try
            {
                PrintBlue("Введите имя файла, который должен быть удален, или его абсолютный путь:");
                string path = Console.ReadLine();
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    PrintRed("Такого файла нет.");
                    return;
                }
                else
                {
                    File.Delete(path);
                    PrintGreen("Файл удален.");
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка удаления файла.");
                return;
            }
        }

        /// Метод отвечает за перемещение указанного файла.
        /// Сначала запрашивается абсолютный путь либо имя файла в текущей директории,
        /// котороый должен быть перемещен, затем запрашивается абсолютный путь либо
        /// имя нового файла в текущей директории, для того, чтобы переместить
        /// старый файл по абсолютному пути либо просто переименовать старый файл
        /// в текущей директории.
        public static void MoveFile()

        {
            try
            {
                PrintBlue("Введите имя файла, который должен быть перемещен или его абсолютный путь:");
                string path1 = Console.ReadLine();
                PrintBlue(@"Введите имя, которое будет у файла после перемещения или его абсолютный путь.
Имя нового файла не должно совпадать с любым существующим в текущей директории файлом.");
                string path2 = Console.ReadLine();

                File.Move(path1, path2);
                PrintGreen("Файл перемещен.");
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка перемещения файла. Проверьте корректность введенных данных.");
                return;
            }
        }

        /// <summary>
        /// Метод отвечает за копирование указанного файла.
        /// Сначала запрашивается абсолютный путь либо имя файла в текущей директории,
        /// котороый должен быть скопирован, затем запрашивается абсолютный путь либо
        /// имя нового файла в текущей директории, для того, чтобы скопировать
        /// старый файл по абсолютному пути либо просто создать еще один такой же файл
        /// в текущей директории с другим именем.
        /// </summary>
        public static void CopyFile()

        {
            try
            {
                PrintBlue("Введите имя файла, который должен быть скопирован или его абсолютный путь:");
                string path1 = Console.ReadLine();
                PrintBlue(@"Введите имя, которое будет у файла после копирования или его абсолютный путь.
Имя нового файла не должно совпадать с любым из файлов в директории, куда этот файл будет скопирован.");
                string path2 = Console.ReadLine();

                File.Copy(path1, path2);
                PrintGreen("Файл скопирован.");
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка копирования файла. Проверьте корректность введенных данных.");
                return;
            }
        }

        /// <summary>
        /// Метод отвечает за получение от пользователя желаемой кодировки.
        /// </summary>
        /// <returns>Возвращает кодировку, которую пользователь выбрал.</returns>
        public static Encoding GetEncodingName()
        {
            PrintYellow("1 UTF-8");
            PrintYellow("2 UTF-16");
            PrintYellow("3 KOI8-U");
            PrintYellow("4 Windows-1251");
            int number = 0;
            while (true)
            {
                PrintBlue("Выберите и введите номер кодировки, в которой нужно прочитать файл:");

                if (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > 4)
                {
                    PrintRed("Некорректные данные, повторите попытку.");
                    continue;
                }

                break;
            }

            switch (number)
            {
                case 1:
                    return Encoding.UTF8;
                case 2:
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("utf-16");
                case 3:
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding(21866);
                case 4:
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding(1251);
                default:
                    return Encoding.Default;
            }
        }

        /// <summary>
        /// Метод отвечает за чтение файла в заданной кодировке и вывод его содержимого на экран.
        ///  Сначала запрашивается абсолютный путь либо имя файла в текущей директории,
        /// котороый должен быть считан, затем запрашивается кодировка, в которой
        /// этот файл должен быть прочитан. Далее на экран выводится содержимое
        /// файла в заданной кодировке.
        /// </summary>
        public static void ReadFile()
        {
            try
            {
                PrintBlue("Введите имя файла,который нужно прочитать, или абсолютный путь к нему:");
                string path = Console.ReadLine();
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    PrintRed("Файла не существует.");
                    return;
                }

                Encoding enc = GetEncodingName();
                string[] readText;

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    readText = File.ReadAllLines(path, enc);
                    PrintGreen("Содержимое файла:");
                    foreach (var line in readText)
                        PrintYellow(line);
                    Console.WriteLine();
                }
                else
                {
                    PrintRed("Файла не существует.");
                    return;
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка чтения файла.");
                return;
            }

        }

        /// <summary>
        /// Метод осуществляет переход в папку, которая находится в
        /// текущей директории и имя которой указал пользователь.
        /// Сначала заправшивается имя папки, затем осуществляется переход.
        /// </summary>
        public static void GoToRelativePath()
        {
            try
            {
                PrintBlue("Введите имя папки текущего каталога, в которую нужно перейти: ");
                string name = Console.ReadLine();

                if (!string.IsNullOrEmpty(name) && Directory.Exists(name))
                {
                    Directory.SetCurrentDirectory(name);
                }
                else
                {
                    PrintRed("Пути не существует.");
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка перехода по пути.");
                return;
            }
        }

        /// <summary>
        /// Метод осуществляет переход по абсолютному пути, который был
        /// указан пользователем.
        /// Сначала заправшивается абсолютный путь, затем осуществляется переход.
        /// </summary>
        public static void GoToAbsolutePath()
        {
            try
            {
                PrintBlue("Введите абсолютный путь:");
                string path = Console.ReadLine();

                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    Directory.SetCurrentDirectory(path);
                }
                else
                {
                    PrintRed("Пути не существует.");
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка перехода по заданному пути.");
                return;
            }
        }

        /// <summary>
        /// Метод осуществляет смену текущего диска на заданный пользователем.
        /// Сначала выводится список доступных диско, затем предлагается
        /// выбрать диск, куда будет осуществлен переход.
        /// </summary>
        public static void GoToDisk()
        {
            try
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                for (int i = 0; i < allDrives.Length; ++i)
                {
                    if (allDrives[i].IsReady == true)
                        PrintYellow($"{i + 1} Диск {allDrives[i].Name}");
                }

                int number;
                PrintBlue("Выберите и введите номер диска:");
                while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > allDrives.Length)
                {
                    PrintRed("Данные некоррекнты, повторите попытку.");
                    PrintBlue("Выберите и введите номер диска:");
                }
                Directory.SetCurrentDirectory(allDrives[number - 1].Name);
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка перехода на другой диск.");
                return;
            }
        }

        /// <summary>
        /// Метод осуществляет вывод на экран всех файлов, находящихся в текущей директории.
        /// </summary>
        public static void PrintAllFiles()
        {
            try
            {

                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
                if (files.Length == 0)
                    PrintRed("Файлов нет.");
                string separator = Path.DirectorySeparatorChar.ToString();
                foreach (var s in files)
                {
                    string[] tmp = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length >= 1)
                        PrintYellow(tmp[^1]);
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка чтения файлов.");
                return;
            }
        }

        /// <summary>
        /// Метод осуществляет вывод на экран всех папок, находящихся в текущей директории.
        /// </summary>
        public static void PrintFolders()
        {
            try
            {

                string[] directories = Directory.GetDirectories(Directory.GetCurrentDirectory());
                if (directories.Length == 0)
                    PrintRed("Директорий нет.");
                string separator = Path.DirectorySeparatorChar.ToString();
                foreach (var s in directories)
                {
                    string[] tmp = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length >= 1)
                        PrintYellow(tmp[^1]);
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка получения директорий.");
                return;
            }
        }

        /// <summary>
        /// Метод осуществляет вывод на экран всех доступных дисков на устройстве, их общий
        /// объем памяти, а также сколько доступно для использования.
        /// </summary>
        public static void PrintAllDrives()
        {
            try
            {

                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    PrintYellow($"Диск {d.Name}");
                    if (d.IsReady == true)
                    {
                        Console.WriteLine(
                            "  Доступно на диске:       {0, 15} mbytes",
                            ((long)d.AvailableFreeSpace) / 1024 / 1024);

                        Console.WriteLine(
                            "  Всего памяти:            {0, 15} mbytes ",
                            ((long)d.TotalSize) / 1024 / 1024);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintRed("Ошибка чтения дисков.");
                return;
            }
        }

        /// <summary>
        /// Метод выводит на экран переданную строку зеленым цветом.
        /// </summary>
        /// <param name="s">Строка, которую нужно вывести на экран.</param>
        public static void PrintGreen(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        /// <summary>
        /// Метод выводит на экран переданную строку синим цветом.
        /// </summary>
        /// <param name="s">Строка, которую нужно вывести на экран.</param>
        public static void PrintBlue(string s)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        /// <summary>
        /// Метод выводит на экран переданную строку красным цветом.
        /// </summary>
        /// <param name="s">Строка, которую нужно вывести на экран.</param>
        public static void PrintRed(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        /// <summary>
        /// Метод выводит на экран переданную строку желтым цветом.
        /// </summary>
        /// <param name="s">Строка, которую нужно вывести на экран.</param>
        public static void PrintYellow(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }
}

