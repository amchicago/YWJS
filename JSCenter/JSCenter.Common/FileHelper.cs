﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCenter.Common
{
    public class FileHelper
    {
        public static void ByteToFile(byte[] bytes, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        public static byte[] FileToByte(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return bytes;
        }
    }
}
