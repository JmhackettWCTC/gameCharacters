using NLog;
using System.Reflection;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

// deserialize mario json from file into List<Mario>
string marioFileName = "mario.json";
List<Mario> marios = new List<Mario>();
// check if file exists
if (File.Exists(marioFileName))
{
  marios = JsonSerializer.Deserialize<List<Mario>>(File.ReadAllText(marioFileName), jsonOptions) ?? new List<Mario>();
  logger.Info($"File deserialized {marioFileName}");
}

// deserialize Donkey Kong characters from dk.json
string dkFileName = "dk.json";
List<DonkeyKong> dks = new List<DonkeyKong>();
if (File.Exists(dkFileName))
{
  dks = JsonSerializer.Deserialize<List<DonkeyKong>>(File.ReadAllText(dkFileName), jsonOptions) ?? new List<DonkeyKong>();
  logger.Info($"File deserialized {dkFileName}");
}

// deserialize Street Fighter II characters from sf2.json
string sfFileName = "sf2.json";
List<StreetFighter> sfs = new List<StreetFighter>();
if (File.Exists(sfFileName))
{
  sfs = JsonSerializer.Deserialize<List<StreetFighter>>(File.ReadAllText(sfFileName), jsonOptions) ?? new List<StreetFighter>();
  logger.Info($"File deserialized {sfFileName}");
}

do
{
  // display choices to user
  Console.WriteLine("1) Display Mario Characters");
  Console.WriteLine("2) Add Mario Character");
  Console.WriteLine("3) Remove Mario Character");
  Console.WriteLine("4) Display Donkey Kong Characters");
  Console.WriteLine("5) Add Donkey Kong Character");
  Console.WriteLine("6) Remove Donkey Kong Character");
  Console.WriteLine("7) Display Street Fighter Characters");
  Console.WriteLine("8) Add Street Fighter Character");
  Console.WriteLine("9) Remove Street Fighter Character");
  Console.WriteLine("Enter to quit");

  // input selection
  string? choice = Console.ReadLine();
  logger.Info("User choice: {Choice}", choice);

  if (choice == "1")
  {
    // Display Mario Characters
    foreach(var c in marios)
    {
      Console.WriteLine(c.Display());
    }
  }
  else if (choice == "2")
  {
    // Add Mario Character
    Mario mario = new()
    {
      Id = marios.Count == 0 ? 1 : marios.Max(c => c.Id) + 1
    };
    InputCharacter(mario);
    marios.Add(mario);
    File.WriteAllText(marioFileName, JsonSerializer.Serialize(marios));
    logger.Info($"Mario added: {mario.Name}");
  }
  else if (choice == "3")
  {
    // Remove Mario Character
    Console.WriteLine("Enter the Id of the character to remove:");
    if (UInt64.TryParse(Console.ReadLine(), out UInt64 mid))
    {
      Mario? character = marios.FirstOrDefault(c => c.Id == mid);
      if (character == null)
      {
        logger.Error($"Mario Id {mid} not found");
      } else {
        marios.Remove(character);
        File.WriteAllText(marioFileName, JsonSerializer.Serialize(marios));
        logger.Info($"Mario Id {mid} removed");
      }
    } else {
      logger.Error("Invalid Id");
    }
  }
  else if (choice == "4")
  {
    // Display Donkey Kong Characters
    foreach(var c in dks)
    {
      Console.WriteLine(c.Display());
    }
  }
  else if (choice == "5")
  {
    // Add Donkey Kong Character
    DonkeyKong dk = new()
    {
      Id = dks.Count == 0 ? 1 : dks.Max(c => c.Id) + 1
    };
    InputCharacter(dk);
    dks.Add(dk);
    File.WriteAllText(dkFileName, JsonSerializer.Serialize(dks));
    logger.Info($"DonkeyKong added: {dk.Name}");
  }
  else if (choice == "6")
  {
    // Remove Donkey Kong Character
    Console.WriteLine("Enter the Id of the character to remove:");
    if (UInt64.TryParse(Console.ReadLine(), out UInt64 did))
    {
      DonkeyKong? character = dks.FirstOrDefault(c => c.Id == did);
      if (character == null)
      {
        logger.Error($"DonkeyKong Id {did} not found");
      } else {
        dks.Remove(character);
        File.WriteAllText(dkFileName, JsonSerializer.Serialize(dks));
        logger.Info($"DonkeyKong Id {did} removed");
      }
    } else {
      logger.Error("Invalid Id");
    }
  }
  else if (choice == "7")
  {
    // Display Street Fighter Characters
    foreach(var c in sfs)
    {
      Console.WriteLine(c.Display());
    }
  }
  else if (choice == "8")
  {
    // Add Street Fighter Character
    StreetFighter sf = new()
    {
      Id = sfs.Count == 0 ? 1 : sfs.Max(c => c.Id) + 1
    };
    InputCharacter(sf);
    sfs.Add(sf);
    File.WriteAllText(sfFileName, JsonSerializer.Serialize(sfs));
    logger.Info($"StreetFighter added: {sf.Name}");
  }
  else if (choice == "9")
  {
    // Remove Street Fighter Character
    Console.WriteLine("Enter the Id of the character to remove:");
    if (UInt64.TryParse(Console.ReadLine(), out UInt64 sid))
    {
      StreetFighter? character = sfs.FirstOrDefault(c => c.Id == sid);
      if (character == null)
      {
        logger.Error($"StreetFighter Id {sid} not found");
      } else {
        sfs.Remove(character);
        File.WriteAllText(sfFileName, JsonSerializer.Serialize(sfs));
        logger.Info($"StreetFighter Id {sid} removed");
      }
    } else {
      logger.Error("Invalid Id");
    }
  }
  else if (string.IsNullOrEmpty(choice)) {
    break;
  } else {
    logger.Info("Invalid choice");
  }
} while (true);

logger.Info("Program ended");

static void InputCharacter(Character character)
{
  Type type = character.GetType();
  PropertyInfo[] properties = type.GetProperties();
  var props = properties.Where(p => p.Name != "Id");
  foreach (PropertyInfo prop in props)
  {
    if (prop.PropertyType == typeof(string))
    {
      Console.WriteLine($"Enter {prop.Name}:");
      prop.SetValue(character, Console.ReadLine());
    } else if (prop.PropertyType == typeof(List<string>)) {
      List<string> list = new List<string>();
      do {
        Console.WriteLine($"Enter {prop.Name} or (enter) to quit:");
        string response = Console.ReadLine()!;
        if (string.IsNullOrEmpty(response)){
          break;
        }
        list.Add(response);
      } while (true);
      prop.SetValue(character, list);
    }
  }
}