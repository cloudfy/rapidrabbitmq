// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

if (Directory.Exists(Directory.GetCurrentDirectory() + "/runtime"))
{
    Console.WriteLine("wwwroot exists");
}
else
{
    Console.WriteLine("wwwroot does not exist");
}