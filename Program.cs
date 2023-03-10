using StbImageSharp;
using StbImageWriteSharp;

Directory.CreateDirectory("in");
Directory.CreateDirectory("out");

var path = Path.Combine(Environment.CurrentDirectory, "in");
var writer = new ImageWriter();

foreach (var entry in Directory.GetFiles(path))
{
    var filename = entry.ToLower();
    var extension = Path.GetExtension(filename);

    if (extension.Contains("bmp"))
    {
        try
        {
            var buffer = File.ReadAllBytes(filename);
            var image = ImageResult.FromMemory(buffer, StbImageSharp.ColorComponents.RedGreenBlueAlpha);

            var pixels = image.Data;
            var offset = 0;

            for (int i = 0; i < image.Width * image.Height; i++)
            {
                if (pixels[offset] == 0 && pixels[offset + 1] == 0 && pixels[offset + 2] == 0 && pixels[offset + 3] == 255)
                {
                    pixels[offset + 3] = 0;
                }

                offset += 4;
            }

            filename = Path.Combine(Environment.CurrentDirectory, "out", Path.GetFileName(filename).Replace(".bmp", ".png"));
            using var stream = File.Open(filename, FileMode.OpenOrCreate);
            writer.WritePng(image.Data, image.Width, image.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream);

            Console.WriteLine(filename);
        }   
        catch
        {
            Console.WriteLine($"ERROR: {filename}");
        }
    } 
}