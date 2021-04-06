namespace JPEG.Utilities
{
    public interface IContainerSource<out TData>
    {
        int NumberOfTasks { get; }
        TData Get(int taskId);
    }
}