using System.Collections.Generic;
using UnityEngine;

#if ST_ADS
using GoogleMobileAds.Api;
#endif

namespace stickin
{
    public static class AdmobConfiguration
    {
#if ST_ADS
        public static void Init(bool isTest, List<string> testDevices)
        {
            MobileAds.SetiOSAppPauseOnBackground(true);

            if (isTest)
            {
#if UNITY_ANDROID
			var testDevice = SystemInfo.deviceUniqueIdentifier.ToUpper();
#else
                var testDevice = Md5Sum(UnityEngine.iOS.Device.advertisingIdentifier);
#endif

                Debug.Log($"AdmobConfiguration: test device = {testDevice}");

                List<string> deviceIds = new List<string>();
                deviceIds.Add(testDevice);

                foreach (var device in testDevices)
                    deviceIds.Add(device);

                RequestConfiguration requestConfiguration = new RequestConfiguration
                        .Builder()
                    .SetTestDeviceIds(deviceIds)
                    .build();
                MobileAds.SetRequestConfiguration(requestConfiguration);
            }
        }

        private static string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            System.Security.Cryptography.MD5CryptoServiceProvider md5 =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }
#else
    public static void Init(bool isTest, List<string> testDevices) {}
#endif
    }
}