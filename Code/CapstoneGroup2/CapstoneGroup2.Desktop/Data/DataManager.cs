using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage;

namespace CapstoneGroup2.Desktop.Data
{
    public static class DataManager
    {

        public static void SaveVarBinaryAsPdf(byte[] varBinaryData, string outputPath)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            var filePath = Path.Combine(localFolder.Path, "current.pdf");
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                
                fileStream.Write(varBinaryData, 0, varBinaryData.Length);
            }
        }

        public static async Task<byte[]> FileToBinary(StorageFile storageFile)
        {
            try
            {
                if (storageFile != null)
                {
                    using (var stream = await storageFile.OpenReadAsync())
                    {
                        using (var reader = new BinaryReader(stream.AsStream()))
                        {
                            return reader.ReadBytes((int)stream.Size);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("StorageFile is null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting file to binary: " + ex.Message);
                return null;
            }
        }
    }
}
