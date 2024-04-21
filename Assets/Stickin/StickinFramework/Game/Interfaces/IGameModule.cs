namespace stickin
{
    public interface IGameModule
    {
        void Stop();
        void Pause();
        void Resume();
        void Destroy();
    }
}