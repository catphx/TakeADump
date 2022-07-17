using System.IO.Compression;
using System.Text;

namespace TakeADump.Utilities
{
    internal static class Extension
    {

        /// <summary>
        /// Converts the input, in our case the USER:PASS to a base64 format 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>

        public static string ToBase64(this string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

        /// <summary>
        /// Takes the stream from the response, and dearchives it, until we reach a readable format to output!
        /// </summary>
        /// <param name="streamByte"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public static async Task ConvertAndOutputAsync(byte[] streamByte)
        {
            try
            {
                var dateTime = $"{DateTime.Now.ToLongDateString()} - {DateTime.Now.ToLongTimeString().Replace(":", ".")}";

                using var zip = new ZipArchive(new MemoryStream(streamByte), ZipArchiveMode.Read);

                foreach (var entry in zip.Entries)
                {
                    var subCategory = Path.GetDirectoryName(entry.FullName)?.Replace("\\", " - ");
                    var category = subCategory == string.Empty ? "Remote" : $"Remote - {subCategory}";

                    using var stream = entry.Open();

                    using var tr = new StreamReader(stream);
                    var text = await tr.ReadToEndAsync();

                    _ = Directory.CreateDirectory($"{category} - {dateTime}");

                    await File.AppendAllTextAsync($"{category} - {dateTime}\\{(entry.FullName.Contains('\\') ? entry.FullName.Split("\\")[1] : entry.FullName)}", text);
                }
            }
            catch
            {
                throw new Exception("Issue while decompressing and outputing the configs! Make sure to open an issue on Github!");
            }
        }

    }
}
