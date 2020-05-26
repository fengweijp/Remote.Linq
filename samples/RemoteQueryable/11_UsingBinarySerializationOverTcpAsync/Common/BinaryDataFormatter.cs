﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Common
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    public static class BinaryDataFormatter
    {
        public static async Task WriteAsync(this Stream stream, object obj)
        {
            byte[] data;
            using (MemoryStream dataStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(dataStream, obj);
                dataStream.Position = 0;
                data = dataStream.ToArray();
            }

            long size = data.LongLength;
            byte[] sizeData = BitConverter.GetBytes(size);

            await stream.WriteAsync(sizeData, 0, sizeData.Length).ConfigureAwait(false);
            await stream.WriteAsync(new[] { obj is Exception ? (byte)1 : (byte)0 }, 0, 1).ConfigureAwait(false);
            await stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
        }

        public static async Task<T> ReadAsync<T>(this Stream stream)
        {
            byte[] bytes = new byte[256];

            await stream.ReadAsync(bytes, 0, 8).ConfigureAwait(false);
            long size = BitConverter.ToInt64(bytes, 0);

            byte[] exceptionFlag = new byte[1];
            int i = await stream.ReadAsync(exceptionFlag, 0, 1).ConfigureAwait(false);
            if (i != 1)
            {
                throw new IOException("Unable to read expected error indication flag.");
            }

            object obj;
            using (MemoryStream dataStream = new MemoryStream())
            {
                int count = 0;
                do
                {
                    int length = size - count < bytes.Length
                        ? (int)(size - count)
                        : bytes.Length;

                    i = await stream.ReadAsync(bytes, 0, length).ConfigureAwait(false);
                    count += i;

                    dataStream.Write(bytes, 0, i);
                }
                while (count < size);

                dataStream.Position = 0;

                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(dataStream);
            }

            if (exceptionFlag[0] != 0)
            {
                Exception exception = (Exception)obj;
                throw exception;
            }

            return (T)obj;
        }
    }
}
