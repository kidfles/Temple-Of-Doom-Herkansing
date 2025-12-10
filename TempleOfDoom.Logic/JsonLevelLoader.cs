using System.IO;
using System.Text.Json;
using TempleOfDoom.Core;
using TempleOfDoom.Core.DTOs;

namespace TempleOfDoom.Logic
{
    public class JsonLevelLoader : ILevelLoader
    {
        public Level LoadLevel(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The level file was not found: {path}");
            }

            string jsonContent = File.ReadAllText(path);
            LevelDto levelDto = JsonSerializer.Deserialize<LevelDto>(jsonContent);

            // TODO: Convert LevelDto to object.
            // For now, return a new Level.
            return new Level();
        }
    }
}
