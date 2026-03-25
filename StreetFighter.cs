using System.Collections.Generic;

public class StreetFighter : Character
{
  public List<string> Moves { get; set; } = new List<string>();

  public override string Display()
  {
    return $"Id: {Id}\nName: {Name}\nDescription: {Description}\nMoves: {string.Join(", ", Moves)}\n";
  }
}
