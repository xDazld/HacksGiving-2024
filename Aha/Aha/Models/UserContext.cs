using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Aha.Models
{
    public struct UserContext
    {
        public List<String> interests { get; set; }
        public int age { get; set; }
        public string language { get; set; }
    }

    public class UserContextManager
    {
        public readonly string FilePath;
        public UserContext User { get; set; }

        private static UserContextManager instance = new();

        private UserContextManager()
        {
            FilePath = Path.Combine(FileSystem.AppDataDirectory, "userContext.json");
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                this.User = JsonSerializer.Deserialize<UserContext>(json);
            }
        }

        public static UserContextManager getUserContext()
        {
            return instance;
        }

        public async Task SaveAsync()
        {
            var json = JsonSerializer.Serialize(User);
            await File.WriteAllTextAsync(FilePath, json);
        }

        public string getUserContextAsJson()
        {
            return JsonSerializer.Serialize(User);
        }
    }
}
