using System.ComponentModel.Design;

namespace ConsoleApplication1;
class Program
{
    public static void options()
    {
        Console.WriteLine("\n1. Добавить пользователя");
        Console.WriteLine("2. Добавить файл");
        Console.WriteLine("3. Выбрать пользователя");
        Console.WriteLine("4. Получить доступ к файлам");
        Console.WriteLine("5. Изменить права пользователей");
        Console.WriteLine("6. Показать список пользователей");
        Console.WriteLine("7. Показать список файлов");
        Console.WriteLine("8. Выход");
        Console.Write("Введите номер действия: ");
    }
    public static void menu()
    {
        var files = new List<File>() {
            new File("Файл1", "Общий"),
            new File("Файл2", "Конфинденциальный"),
            new File("Файл3", "Секретный")
        };

        FileManager fileManager = new FileManager(files);
        Console.WriteLine(fileManager.GetInfoAboutAllFiles());

        var users = new List<User>() {
            new User("Джон", "Общий", fileManager),
            new User("Бобик", "Секретный", fileManager)
        };

        UserManagment userManagment = new UserManagment(users);
        Console.WriteLine(userManagment.GetInfoAboutAllUsers());

        User currentUser = null;
        string choise = "Хихи";
        while (choise != "8")
        {
            options();
            choise = Console.ReadLine();
            switch(choise)
            {
                case "1":
                    Console.Write("Введите имя пользователя: ");
                    var userName = Console.ReadLine();
                    Console.Write("Введите уровень доступа пользователя: ");
                    var userAccess = Console.ReadLine();
                    User user = new User(userName, userAccess, fileManager);
                    userManagment.AddUser(user);
                    Console.WriteLine("Успех\n");
                    break;
                case "2":
                    Console.Write("Введите имя пользователя: ");
                    var fileName = Console.ReadLine();
                    Console.Write("Введите уровень доступа пользователя: ");
                    var fileAccess = Console.ReadLine();
                    File file = new File(fileName, fileAccess);
                    fileManager.AddFile(file);
                    Console.WriteLine("Успех\n");
                    break;
                case "3":
                    Console.Write("Введите имя пользователя: ");
                    var chooseUser = Console.ReadLine();
                    currentUser = userManagment.GetUserByName(chooseUser);  
                    Console.WriteLine($"Успешная смена пользователя, {currentUser.GetName()}\n"); 
                    break;
                case "4":
                    if (currentUser != null)
                    {
                        Console.Write("Введите название файла: ");
                        fileName = Console.ReadLine();
                        Console.WriteLine(currentUser.GetAccessToFile(fileName));
                    }
                    else Console.WriteLine("Пользователь не выбран\n");
                    break;
                case "5":
                    if (currentUser != null)
                    {
                        Console.Write("Введите имя пользователя, которому хотите изменить права: ");
                        chooseUser = Console.ReadLine();
                        user = userManagment.GetUserByName(chooseUser);
                        Console.Write("Введите уровень допуска, на который хотите сменить права пользователю: ");
                        var newLevel = Console.ReadLine();
                        currentUser.ChangeAccessLevel(user, newLevel);
                    }
                    break;
                case "6":
                    Console.WriteLine(userManagment.GetInfoAboutAllUsers());
                    break;
                case "7":
                    Console.WriteLine(fileManager.GetInfoAboutAllFiles());
                    break;
                case "8":
                    break;
                default: 
                    Console.WriteLine("Что-то пошло не так, попробуйте снова\n");
                    break;
            }
        }
    }
    static void Main(string[] args)
    {
        menu();
    }
}
enum AccessLevel
{
    General = 0,
    Confidential = 1,
    Secret = 2
}

class File
{
    private string name;
    private AccessLevel accessLevel;
    public File(string _name, string _accessLevel)
    {
        name = _name;
        if (_accessLevel == "Общий")
            accessLevel = AccessLevel.General;
        else if (_accessLevel == "Конфинденциальный")
            accessLevel = AccessLevel.Confidential;
        else if (_accessLevel == "Секретный")
            accessLevel = AccessLevel.Secret;
    }
    public string GetName() { return name; }
    public AccessLevel GetAccessLevel() { return accessLevel; }
    public string GetInfo() { return $"Имя файла: {GetName()}\nУровень доступа: {GetAccessLevel()}\n\n"; }
}

class FileManager
{
    private List<File> files;
    public FileManager(List<File> _files){ files = _files; }
    public FileManager(File file) { files = new List<File>() {file}; }

    public File? FindFileByName(string fileName){
        foreach (var item in files)
            if (item.GetName() == fileName)
                return item;
        return null;
    }
    public void AddFile(File file) { files.Add(file); }
    
    public string GetInfoAboutAllFiles()
    {
        string result = "";
        foreach (var item in files)
            result += item.GetInfo();
        return result;
    }
}

class User
{
    private FileManager fileManager;
    private AccessLevel accessLevel;
    private string name;
    public User(string _name, string _accessLevel, FileManager _fileManager)
    {
        name = _name;
        if (_accessLevel == "Общий")
            accessLevel = AccessLevel.General;
        else if (_accessLevel == "Конфинденциальный")
            accessLevel = AccessLevel.Confidential;
        else if (_accessLevel == "Секретный")
            accessLevel = AccessLevel.Secret;
        fileManager = _fileManager;
    }
    public string GetName() { return name; }
    public AccessLevel GetAccessLevel() { return accessLevel; }
    public void SetAccessLevel(AccessLevel newLevel) { accessLevel = newLevel; }
    public string GetInfo() { return $"Имя пользователя: {GetName()}\nУровень доступа: {GetAccessLevel()}\n\n"; }
    public string GetAccessToFile(string fileName)
    {
        var file = fileManager.FindFileByName(fileName);
        if (file!=null)
        {
            switch (GetAccessLevel())
            {
                case AccessLevel.General:
                    if (file.GetAccessLevel() == AccessLevel.Secret)
                        return "Доступ ограничен\n";
                    else if (file.GetAccessLevel() == AccessLevel.Confidential)
                        return "Доступ только для чтения\n";
                    else return "Доступ на запись и чтение\n";
                case AccessLevel.Confidential:
                    if((int)file.GetAccessLevel() < 2)
                        return "Доступ на запись и чтение\n";
                    else return "Доступ ограничен\n";
                case AccessLevel.Secret:
                    return "Доступ на запись и чтение\n";
                default: return "У пользователя не задан уровень допуска\n";
            }
        }
        else return "Файла не существует\n";
    }
    public void ChangeAccessLevel(User user, string _newLevel)
    {
        AccessLevel newLevel;
        switch(_newLevel)
        {
            case "Общий":
                newLevel = AccessLevel.General;
                break;
            case "Конфинденциальный":
                newLevel = AccessLevel.Confidential;
                break;
            case "Секретный":
                newLevel = AccessLevel.Secret;
                break;
            default: 
                newLevel = AccessLevel.General;
                break;
        }
        if ((int)GetAccessLevel() > (int)user.GetAccessLevel())
        {
            user.SetAccessLevel(newLevel);
        }
    }
}

class UserManagment
{
    private List<User> users;
    public UserManagment() { users = new List<User>(); }
    public UserManagment(List<User> _users) { users = _users; }
    public User? GetUserByName(string userName)
    {
        foreach (var user in users)
            if (user.GetName() == userName)
                return user;
        return null;
    }
    public void AddUser(User user) { users.Add(user); }
    public string GetInfoAboutAllUsers() 
    {
        string result = "";
        foreach (var user in users)
            result += user.GetInfo();
        return result;
    }
}