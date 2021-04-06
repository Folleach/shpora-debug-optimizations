using System;
using System.Threading.Tasks;

namespace JPEG.Utilities
{
    public class ActionTaskContainer<TData> : ITaskContainer<object>
    {
        private readonly Task[] tasks;

        public ActionTaskContainer(IContainerSource<TData> dataSource, Action<int, Func<TData>> task)
        {
            tasks = new Task[dataSource.NumberOfTasks];
            for (var i = 0; i < tasks.Length; i++)
            {
                var taskId = i;
                tasks[i] = new Task(() => task(taskId, () => dataSource.Get(taskId)));
            }
        }

        public object WaitRunAll()
        {
            foreach (var task in tasks)
                task.Start();
            Task.WaitAll(tasks);
            return null;
        }
    }
}