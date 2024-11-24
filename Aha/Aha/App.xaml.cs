using Aha.Models;
using Aha.Views.InitialContextQuiz;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Aha
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (File.Exists(UserContextManager.getUserContext().FilePath))
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new QInitial();
            }
        }

        protected override void OnStart()
        {
            CopyFileIfNotExistsAsync("LocationContexts.csv");
        }

        public static async Task CopyFileIfNotExistsAsync(string fileName)
        {
            var destinationPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            // Check if the file already exists
            if (File.Exists(destinationPath))
            {
                return;
            }

            // Get the stream from the embedded resource
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(destinationPath);

            // Copy the file
            await writer.WriteAsync(await reader.ReadToEndAsync());
        }
    }
}

