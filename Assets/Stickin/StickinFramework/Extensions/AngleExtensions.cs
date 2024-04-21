
namespace stickin
{
    public static class AngleExtensions
    {
        public static int NormalizeAngle360(int angle)
        {
            while (angle > 360)
                angle -= 360;

            while (angle < 0)
                angle += 360;

            return angle;
        }
    }
}