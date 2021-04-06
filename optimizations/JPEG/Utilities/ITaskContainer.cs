namespace JPEG.Utilities
{
    public interface ITaskContainer<out T>
    {
        T WaitRunAll();
    }
}