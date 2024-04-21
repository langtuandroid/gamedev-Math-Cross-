namespace stickin
{
    public static class FloatExtensions
    {
        public static int ToMilliseconds(this float seconds)
        {
            return (int) (seconds * 1000);
        }
    }
}