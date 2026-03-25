using System.Collections.Generic;

public class DonkeyKong : Character
{
  public string? Species { get; set; }

  public override string Display()
  {
    return $"Id: {Id}\nName: {Name}\nSpecies: {Species}\nDescription: {Description}\n";
  }
}
