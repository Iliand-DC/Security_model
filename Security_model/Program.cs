using System.ComponentModel.Design;

namespace ConsoleApplication1;
class Program
{
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

        Console.Write("Введите имя пользователя для входа: ");
        var userName = Console.ReadLine();
        var user = userManagment.GetUserByName(userName);

        Console.Write("Введите название файла, к которому хотите получить доступ: ");
        var fileName = Console.ReadLine();
        Console.WriteLine(user.GetAccessToFile(fileName));

        // Console.WriteLine(user.GetInfo());
        // Console.WriteLine(user.GetAccessToFile("Файл1"));
        // Console.WriteLine(user.GetAccessToFile("Файл2"));
        // Console.WriteLine(user.GetAccessToFile("Файл3"));

        // Console.WriteLine(user2.GetInfo());
        // Console.WriteLine(user2.GetAccessToFile("Файл1"));
        // Console.WriteLine(user2.GetAccessToFile("Файл2"));
        // Console.WriteLine(user2.GetAccessToFile("Файл3"));

        // user2.ChangeAccessLevel(user, "Конфинденциальный");
        
        // Console.WriteLine(user.GetInfo());
        // Console.WriteLine(user.GetAccessToFile("Файл1"));
        // Console.WriteLine(user.GetAccessToFile("Файл2"));
        // Console.WriteLine(user.GetAccessToFile("Файл3"));
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