using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace stickin
{
    public enum HapticTypes
    {
        Selection,
        VeryLight,
        Light,
        Failure,
        Success,
        Warning
    }

    public class AndroidTaptic
    {

        public static long VeryLightDuration = 5;
        public static int VeryLightAmplitude = 5;

        public static long LightDuration = 20;
        public static long MediumDuration = 40;
        public static long HeavyDuration = 80;

        public static int LightAmplitude = 40;

        private static int _sdkVersion = -1;

        void Vib()
        {
#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        public static void Vibrate()
        {
            AndroidVibrate(MediumDuration);
        }

        public static void Haptic(HapticTypes type)
        {
            switch (type)
            {
                case HapticTypes.Selection:
                    AndroidVibrate(LightDuration, LightAmplitude);
                    break;

                case HapticTypes.VeryLight:
                    AndroidVibrate(VeryLightDuration, VeryLightAmplitude);
                    break;

                case HapticTypes.Light:
                    AndroidVibrate(LightDuration, LightAmplitude);
                    break;
            }
        }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    private static AndroidJavaObject AndroidVibrator =
 CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    private static AndroidJavaClass VibrationEffectClass;
    private static AndroidJavaObject VibrationEffect;
    private static int DefaultAmplitude;
#else
        private static AndroidJavaClass UnityPlayer;
        private static AndroidJavaObject CurrentActivity;
        private static AndroidJavaObject AndroidVibrator = null;
        private static AndroidJavaClass VibrationEffectClass = null;
        private static AndroidJavaObject VibrationEffect;
        private static int DefaultAmplitude;
#endif

        public static void AndroidVibrate(long milliseconds)
        {
            if (AndroidVibrator != null)
            {
                AndroidVibrator.Call("vibrate", milliseconds);
            }
        }

        public static void AndroidVibrate(long milliseconds, int amplitude)
        {
            if (AndroidVibrator != null)
            {
                if ((AndroidSDKVersion() < 26))
                {
                    AndroidVibrate(milliseconds);
                }
                else
                {
                    VibrationEffectClassInitialization();
                    VibrationEffect =
                        VibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot",
                            new object[] {milliseconds, amplitude});
                    AndroidVibrator.Call("vibrate", VibrationEffect);
                }
            }
        }

        public static void AndroidVibrate(long[] pattern, int repeat)
        {
            if (AndroidVibrator != null)
            {
                if ((AndroidSDKVersion() < 26))
                {
                    AndroidVibrator.Call("vibrate", pattern, repeat);
                }
                else
                {
                    VibrationEffectClassInitialization();
                    VibrationEffect =
                        VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform",
                            new object[] {pattern, repeat});
                    AndroidVibrator.Call("vibrate", VibrationEffect);
                }
            }
        }

        public static void AndroidVibrate(long[] pattern, int[] amplitudes, int repeat)
        {
            if (AndroidVibrator != null)
            {
                if ((AndroidSDKVersion() < 26))
                {
                    AndroidVibrator.Call("vibrate", pattern, repeat);
                }
                else
                {
                    VibrationEffectClassInitialization();
                    VibrationEffect = VibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform",
                        new object[] {pattern, amplitudes, repeat});
                    AndroidVibrator.Call("vibrate", VibrationEffect);
                }
            }
        }

        public static void AndroidCancelVibrations()
        {
            if (AndroidVibrator != null)
                AndroidVibrator.Call("cancel");
        }

        private static void VibrationEffectClassInitialization()
        {
            if (VibrationEffectClass == null)
                VibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
        }

        public static int AndroidSDKVersion()
        {
            if (_sdkVersion == -1 &&
                SystemInfo.operatingSystem.Contains("_"))
            {
                int apiLevel =
                    int.Parse(SystemInfo.operatingSystem.Substring(SystemInfo.operatingSystem.IndexOf("-") + 1, 3));
                _sdkVersion = apiLevel;
                return apiLevel;
            }

            return _sdkVersion;
        }
    }
}