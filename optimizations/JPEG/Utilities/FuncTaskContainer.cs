using System;
using System.Threading.Tasks;

namespace JPEG.Utilities
{
    public class FuncTaskContainer<TResult> : ITaskContainer<TResult[]>
        where TResult : new()
    {
        private readonly Task[] tasks;
        private readonly TResult[] resultData;

        public FuncTaskContainer(int numberOfTasks, Action<int, TResult> task)
        {
            tasks = new Task[numberOfTasks];
            resultData = new TResult[numberOfTasks];
            for (var i = 0; i < tasks.Length; i++)
            {
                var taskId = i;
                var data = resultData[taskId] = new TResult();
                tasks[i] = new Task(() => task(taskId, data));
            }
        }

        public TResult[] WaitRunAll()
        {
            foreach (var task in tasks)
                task.Start();
            Task.WaitAll(tasks);
            return resultData;
        }
    }
}