// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
 * rapidrabbitmq --prepare,-r
 *               --cleanup,-c
 *               --run, -r
*/

if (Directory.Exists(Directory.GetCurrentDirectory() + "/runtime"))
{
    Console.WriteLine("wwwroot exists");
}
else
{
    Console.WriteLine("wwwroot does not exist");
    HttpClient httpClient = new ();
    var response = await httpClient.GetAsync(@"https://raw.githubusercontent.com/cloudfy/rapidrabbitmq/main/dep/erl.bin");
    response.EnsureSuccessStatusCode();

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Downloading dependency Erlang...");
        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/runtime");
        using (var fileStream = new FileStream(Directory.GetCurrentDirectory() + "/runtime/erl.zip", FileMode.Create))
        {
            await response.Content.CopyToAsync(fileStream);
            fileStream.Position = 0;
            System.IO.Compression.ZipFile.ExtractToDirectory(fileStream, Directory.GetCurrentDirectory() + "/runtime");
        }
        File.Delete(Directory.GetCurrentDirectory() + "/runtime/erl.zip");

        // change ini
        string iniSource = Directory.GetCurrentDirectory() + "/runtime/bin/erl.ini.src";
        string iniDestination = Directory.GetCurrentDirectory() + "/runtime/bin/erl.ini";
        var iniContent = await File.ReadAllTextAsync(iniSource);
        iniContent = iniContent.Replace("{{path}}", (Directory.GetCurrentDirectory() + "\\runtime").Replace("\\", "\\\\"));
        if (File.Exists(iniDestination)) File.Delete(iniDestination);
        await File.WriteAllTextAsync(iniDestination, iniContent);
    }
    else
    {
        Console.WriteLine("Failed to download erl.bin");
    }
}