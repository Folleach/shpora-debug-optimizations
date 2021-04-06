using System.Collections.Generic;
using System.IO;

namespace JPEG.Utilities
{
    public class QuantizedSource : IContainerSource<byte[]>
    {
        private readonly MemoryStream stream;
        private readonly int dctSize;
        private readonly Queue<byte[]>[] readBuffer;
        private readonly object dataLocker = new object();
        private int currentPoint = 0;

        public QuantizedSource(int numberOfTasks, MemoryStream stream, int dctSize)
        {
            this.stream = stream;
            this.dctSize = dctSize;
            NumberOfTasks = numberOfTasks;
            readBuffer = new Queue<byte[]>[NumberOfTasks];
            for (var i = 0; i < readBuffer.Length; i++)
                readBuffer[i] = new Queue<byte[]>();
        }

        public int NumberOfTasks { get; }

        public byte[] Get(int taskId)
        {
            lock (dataLocker)
            {
                var current = readBuffer[taskId];
                if (current.Count > 0)
                    return current.Dequeue();
                while (currentPoint != taskId)
                {
                    ReadToPoint(currentPoint, 3);
                    currentPoint = (currentPoint + 1) % NumberOfTasks;
                }

                ReadToPoint(currentPoint, 3);
                currentPoint = (currentPoint + 1) % NumberOfTasks;
                return current.Dequeue();
            }
        }

        private void ReadToPoint(int point, int times)
        {
            for (var i = 0; i < times; i++)
            {
                var temporary = new byte[dctSize * dctSize];
                stream.Read(temporary, 0, temporary.Length);
                readBuffer[point].Enqueue(temporary);
            }
        }
    }
}