using System;
using System.Globalization;
using UnityEngine;

namespace stickin.mathcross
{
    public class SpinController
    {
        #region Singleton

        public static SpinController Instance => _instance ?? (_instance = new SpinController());

        private static SpinController _instance;

        private SpinController()
        {
            Init();
        }

        #endregion

        private const string SPIN_DATE_KEY = "SpinDateKey";
        private const string SHORT_DATE_FORMAT = "dd.MM.yyyy";
        public const int MAX_COUNT_SPINS = 1;

        [InjectField] private ResourcesService _resourcesService;
        
        private int _countAvailableSpins;

        public int CountAvailableSpins => _countAvailableSpins;
        public bool IsFreeSpin => CountAvailableSpins == MAX_COUNT_SPINS;

        public event Action OnRefresh;

        private void Init()
        {
            InjectService.BindFields(this);
            
            var dateNowStr = DateTime.Now.ToString(SHORT_DATE_FORMAT, DateTimeFormatInfo.CurrentInfo);
            var dateYesterdayStr = DateTime.Now.AddDays(-1).ToString(SHORT_DATE_FORMAT, DateTimeFormatInfo.CurrentInfo);
            var lastDateStr = PlayerPrefs.GetString(SPIN_DATE_KEY, dateYesterdayStr);

            var subDate = DateTime.Now - DateTime.ParseExact(lastDateStr, SHORT_DATE_FORMAT, DateTimeFormatInfo.CurrentInfo);

            if (subDate.Days == 0)
            {
                _countAvailableSpins = _resourcesService.GetResourceValueInt("spin");
            }
            else
            {
                _countAvailableSpins = MAX_COUNT_SPINS;
                _resourcesService.SetResource("spin", _countAvailableSpins);
                
                PlayerPrefs.SetString(SPIN_DATE_KEY, dateNowStr);
            }
            
            OnRefresh?.Invoke();
        }

        public void DecreaseCountAvailableSpins()
        {
            _countAvailableSpins--;
            _resourcesService.SetResource("spin", _countAvailableSpins);

            OnRefresh?.Invoke();
        }

        public void Refresh()
        {
        }
    }
}