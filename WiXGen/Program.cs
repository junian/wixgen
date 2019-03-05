using System;
using System.IO;
using System.Reflection;

namespace WiXGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 1)
                return;

            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Directory doesn't exist.");
                return;
            }

            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                Console.WriteLine(GenerateComponent(filename));
            }

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                Console.WriteLine(GenerateComponentRef(filename));
            }

        }

        static string GetFileId(string filename) => string.Join("", filename.Split('.', '-'));

        static string GenerateComponentRef(string filename) => $"<ComponentRef Id=\"{GetFileId(filename)}\" />";

        static string GenerateComponent(string filename)
        {
            var fileid = GetFileId(filename);
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WiXGen.WixComponentTemplate.xml";

            var xmlTemplate = "";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                xmlTemplate = reader.ReadToEnd();
            }

            return string.Format(xmlTemplate, fileid, filename);
        }
    }
}
