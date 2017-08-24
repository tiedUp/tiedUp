using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace TiedUp.Core
{
    public class TiedUp
    {
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_TIEDUP_DIFFERENT_TIEDUP_SPEC = 1;
        public const int ERROR_OUT_OF_RANGE = 2;

        // Size of data to read/write from stream
        private const UInt64 sizeOfCluster = 64L; // 64 bits

        private TiedUpSpec tiedUpSpec;
        public TiedUp(TiedUpSpec tiedUpSpec)
        {
            this.tiedUpSpec = tiedUpSpec;
        }

        /// <summary>
        /// Mark a index as busy.
        /// </summary>
        /// <param name="index">Index inside of range specified by TiedUpSpec</param>
        /// <returns>
        /// 000 - ERROR_SUCCESS
        /// 001 - ERROR_TIEDUP_DIFFERENT_TIEDUP_SPEC
        /// 002 - ERROR_OUT_OF_RANGE
        /// </returns>
        public int Mark(UInt64 index)
        {
            // "/home/roosevelt/.local/share/IsolatedStorage/udzqkwy2.0zv/fassb4av.hu1/StrongName.ncwojqheyhc0dhynwiffp31oq4puy42y/AssemFiles/"
            UInt64 cluster = index / sizeOfCluster;
            UInt64 slot = index % sizeOfCluster;

            UInt64 bit = (UInt64)Math.Pow(2, slot);
            bit <<= (UInt16)(sizeOfCluster - bit);

            using (IsolatedStorageFile sharedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                IsolatedStorageFileStream marksFile = OpenMarksFile(sharedStorage);
                BinaryReader binaryReader = new BinaryReader(marksFile);

                UInt64 current = ReadCluster(binaryReader, cluster);

                current |= bit;

                UpdateCluster(marksFile, cluster, current);

                marksFile.Close();

                sharedStorage.Close();
            }

            return 0;
        }

        public int Marked(UInt64 index, ref bool result)
        {
            UInt64 cluster = index / sizeOfCluster;
            UInt64 slot = index % sizeOfCluster;

            UInt64 bit = (UInt64)Math.Pow(2, slot);
            bit <<= (UInt16)(sizeOfCluster - bit);

            using (IsolatedStorageFile sharedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                IsolatedStorageFileStream marksFile = OpenMarksFile(sharedStorage);
                BinaryReader binaryReader = new BinaryReader(marksFile);

                UInt64 current = ReadCluster(binaryReader, cluster);

                marksFile.Close();
                sharedStorage.Close();

                result = (current & bit) != 0;
            }
            return 0;
        }
        private static void UpdateCluster(IsolatedStorageFileStream file, UInt64 cluster, UInt64 current)
        {
            BinaryWriter binaryWriter = new BinaryWriter(file);

            binaryWriter.BaseStream.Seek((long)cluster, SeekOrigin.Begin);
            binaryWriter.Write(current);
            binaryWriter.Flush();
        }
        private static UInt64 ReadCluster(BinaryReader reader, UInt64 cluster)
        {
            reader.BaseStream.Seek((long)cluster, SeekOrigin.Begin);

            UInt64 current = reader.ReadUInt64();
            return current;
        }
        private IsolatedStorageFileStream OpenMarksFile(IsolatedStorageFile sharedStorage)
        {
            string path = $"tiedUp/{tiedUpSpec.Id}";
            const string fileName = "tiedMarks.dat";
            string fullFileName = $"{path}/{fileName}";

            sharedStorage.CreateDirectory(path);

            bool needSetMaxLength = !sharedStorage.FileExists(fullFileName);

            IsolatedStorageFileStream file = new IsolatedStorageFileStream(
                fullFileName,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.Read,
                1,
                sharedStorage);

            if (needSetMaxLength)
            {
                file.SetLength((tiedUpSpec.End - tiedUpSpec.Start) + 1);
            }
                
            return file;
        }
    }
}
