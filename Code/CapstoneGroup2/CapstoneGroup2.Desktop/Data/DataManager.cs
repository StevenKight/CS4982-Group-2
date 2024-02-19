using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace CapstoneGroup2.Desktop.Data
{
    public static class DataManager
    {
        #region Methods

        public static void SaveVarBinaryAsPdf(byte[] varBinaryData, string outputPath)
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            var filePath = Path.Combine(localFolder.Path, "current.pdf");
            using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);

            fileStream.Write(varBinaryData, 0, varBinaryData.Length);
        }

        public static async Task<byte[]> FileToBinary(StorageFile storageFile)
        {
            try
            {
                if (storageFile != null)
                {
                    using var stream = await storageFile.OpenReadAsync();
                    using var reader = new BinaryReader(stream.AsStream());
                    return reader.ReadBytes((int)stream.Size);
                }

                Console.WriteLine("StorageFile is null.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting file to binary: " + ex.Message);
                return null;
            }
        }

        #endregion
    }
}