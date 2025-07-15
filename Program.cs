using System.Collections.Generic;
using System;
using System.IO;
using System.Text.Json;

//template for data
class NoteData
{
    public int type { get; set; } = 0;
    public int beatDirection { get; set; } = 0;
    public double speed { get; set; } = 3.0;
    public int start { get; set; }
    public double end { get; set; } = -1.0;

}
class NoteMap
{
    public double speed { get; set; } = 5.0;
    public double offsetMs { get; set; } = 0.0;
    public List<NoteData> beats { get; set; } = new();
}




    class Program
    {
    // direction will be random
        static readonly int[] Directions = { 1,2,3,4 };
        static readonly Random Rand = new();

        static void Main()
        {

        Console.WriteLine("Enter .osu file path");
            string osuPath = Console.ReadLine().Trim().Trim('"');
        Console.WriteLine("Enter folder for new created json file");
            string jsonPath = Console.ReadLine().Trim().Trim('"');

            if (!File.Exists(osuPath))
            {
                Console.WriteLine($"File not found: {osuPath}");
                return;
            }

            List<NoteData> notes = new();
            bool readingHitObjects = false;

            foreach (var line in File.ReadLines(osuPath))
            {
                if (line.Trim() == "[HitObjects]")
                {
                    readingHitObjects = true;
                    continue;
                }

                if (readingHitObjects)
                {
                    if (string.IsNullOrWhiteSpace(line)) break;

                    var parts = line.Split(',');
                    if (parts.Length < 3) continue;

                    if (!int.TryParse(parts[2], out int time)) continue;

                    notes.Add(new NoteData
                    {
                        type = 0,
                        beatDirection = Directions[Rand.Next(3)],
                        speed = 3.0,
                        start = time,
                        end = -1.0 ,                      
                    });
                }
            }
        NoteMap map = new NoteMap()
        {
            speed = 5.0,
            offsetMs = 0.0,
            beats = notes
        };
            string json = JsonSerializer.Serialize(map, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, json);
            Console.WriteLine($"Converted {notes.Count} notes to {jsonPath}");
        }
    }



