using System;
using UnityEngine;

#if ST_ADS
using GoogleMobileAds.Api;
#endif

namespace stickin
{
	public class AdmobRewardWrapper
	{
#if ST_ADS
		private readonly string _id;
		private RewardedAd _rewardedAd;
		private Action _callbackVideoRewardComplete;
		private Action _callbackVideoRewardFail;
		private bool _isAdClosed;
		private bool _isAdEarned;

		public event Action<bool> OnRefresh;

		public AdmobRewardWrapper(string id)
		{
			_id = id;

			Request();
		}

		public bool IsLoad()
		{
			return _rewardedAd != null && _rewardedAd.CanShowAd();
		}

		public bool Request()
		{
			// Clean up the old ad before loading a new one.
			if (_rewardedAd != null)
			{
				_rewardedAd.Destroy();
				_rewardedAd = null;
			}

			Debug.Log("Loading the rewarded ad.");

			// create our request used to load the ad.
			var adRequest = new AdRequest();
			adRequest.Keywords.Add("unity-admob-sample");

			// send the request to load the ad.
			RewardedAd.Load(_id, adRequest,
				(RewardedAd ad, LoadAdError error) =>
				{
					// if error is not null, the load request failed.
					if (error != null || ad == null)
					{
						Debug.LogError($"Rewarded ad failed to load an ad with error : {error}");
						Refresh(false);
						return;
					}

					Debug.Log($"Rewarded ad loaded with response : {ad.GetResponseInfo()}");

					_rewardedAd = ad;
					Refresh(true);
				});

			return true;
		}

		public void Show(Action callbackComplete, Action callbackFail)
		{
			var result = false;

			_callbackVideoRewardComplete = callbackComplete;
			_callbackVideoRewardFail = callbackFail;

			if (_rewardedAd.CanShowAd())
			{
				Debug.Log("AdmobRewardWrapper.Show: OK");
				_rewardedAd.Show(RewardOnUserEarnedReward);
			}
			else
			{
				Debug.LogError("AdmobRewardWrapper.Show: not loaded");
				CallVideoRewardedCallbacks(false);
			}
		}

		public void Hide()
		{
		}

		private void Clear()
		{
			// if (_rewardedAd != null)
			// {
			// 	_rewardedAd.OnAdLoaded -= RewardOnAdLoaded;
			// 	_rewardedAd.OnAdFailedToLoad -= RewardOnAdFailedToLoad;
			// 	_rewardedAd.OnAdClosed -= RewardOnAdClosed;
			// 	_rewardedAd.OnAdFailedToShow -= RewardOnAdFailedToShow;
			// 	_rewardedAd.OnUserEarnedReward -= RewardOnUserEarnedReward;
			// 	_rewardedAd.OnAdOpening -= RewardOnAdOpening;
			// 	
			// 	_rewardedAd = null;
			// }
		}

		private void CallVideoRewardedCallbacks(bool success)
		{
			(success ? _callbackVideoRewardComplete : _callbackVideoRewardFail)?.Invoke();
			_callbackVideoRewardComplete = null;
			_callbackVideoRewardFail = null;
		}

		#region Callbacks

		private void RewardOnAdFailedToShow(object sender, AdErrorEventArgs args)
		{
			Debug.LogError($"AdmobRewardWrapper.RewardOnAdFailedToShow: with error = {args.Message}");

			Refresh(false);
		}

		private void RewardOnAdOpening(object sender, EventArgs args)
		{
			Debug.Log("AdmobRewardWrapper.RewardOnAdOpening");

			Refresh(false);
		}

		private void RewardOnUserEarnedReward(Reward reward)
		{
			Debug.Log("AdmobRewardWrapper.RewardOnUserEarnedReward");

			CallVideoRewardedCallbacks(true);
		}

		private void RewardOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			Debug.LogError($"AdmobRewardWrapper.RewardOnAdFailedToLoad: with message: {args.LoadAdError.GetMessage()}");
			Refresh(false);
		}

		private void RewardOnAdClosed(object sender, EventArgs args)
		{
			Debug.Log($"AdmobRewardWrapper.RewardOnAdClosed sender = {sender}");

			Refresh(false);
			// Request();
		}

		private void RewardOnAdLoaded(object sender, EventArgs args)
		{
			Debug.Log("AdmobRewardWrapper.RewardOnAdLoaded");
			Refresh(true);
		}

		private void Refresh(bool available)
		{
			OnRefresh?.Invoke(available);
		}

		#endregion

		public void Destroy()
		{
		}
#else
	public AdmobRewardWrapper(string id) { }
	public bool IsLoad() => false;
	public void Show(Action callbackComplete, Action callbackFail) { }
	public void Request() { }
#endif
	}
}