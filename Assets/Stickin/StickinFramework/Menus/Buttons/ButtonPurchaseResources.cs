using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if ST_PURCHASING
using stickin;
using UnityEngine.Purchasing;
#endif

namespace stickin.menus
{
    public enum ButtonPurchaseResourcesType
    {
        Purchase,
        Reward,
        Free
    }
    
    [RequireComponent(typeof(Button))]
    public class ButtonPurchaseResources : MonoBehaviour
    {
        [SerializeField] private string _purchaseName;
        
        [Header("Configs")] 
        [SerializeField] private RewardResourcesConfig _rewardResourcesConfig;
        [SerializeField] private ButtonPurchaseResourcesType _type;

        [Header("Views")]
        [SerializeField] private Text _priceText;

        [Header("Events")] 
        [SerializeField] private UnityEvent _startPurchaseEvent;
        [SerializeField] private UnityEvent _endPurchaseEvent;
        [SerializeField] private UnityEvent _failPurchaseEvent;

        [InjectField] private AdsService _adsService;

        private Button _btn;

        private void Start()
        {
            InjectService.BindFields(this);
            
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);

            if (_type == ButtonPurchaseResourcesType.Purchase)
            {
#if ST_PURCHASING
                ServicePurchases.Instance.OnUpdateProducts += OnUpdateProducts;
                OnUpdateProducts(ServicePurchases.Instance.Products);
#endif
            }
            else if (_type == ButtonPurchaseResourcesType.Reward)
            {
                if (_priceText != null &&
                    _rewardResourcesConfig != null &&
                    _rewardResourcesConfig.Resources != null &&
                    _rewardResourcesConfig.Resources.Length > 0)
                {
                    _priceText.text = $"{_rewardResourcesConfig.Resources[0].Value}";
                }
            }
        }

        private void OnClick()
        {
            if (_type == ButtonPurchaseResourcesType.Purchase)
            {
                // _startPurchaseEvent?.Invoke();
#if ST_PURCHASING
                ServicePurchases.Instance.BuyPurchase(_purchaseName, OnBuyComplete, OnBuyFail);
#endif
            }
            else if (_type == ButtonPurchaseResourcesType.Free)
            {
                OnBuyComplete();
                _btn.interactable = false;
            }
            else if (_type == ButtonPurchaseResourcesType.Reward)
            {
                if (_adsService.IsRewardAvailable())
                {
                    _adsService.ShowReward(() =>
                    {
                        OnBuyComplete();
                        _btn.interactable = false;
                    }, () => { _failPurchaseEvent?.Invoke(); });
                }
                else
                    _failPurchaseEvent?.Invoke();
            }
        }
        
#if ST_PURCHASING
        private void OnUpdateProducts(Product[] products)
        {
            var bundleId = ServicePurchases.Instance.GetPurchaseId(_purchaseName);
            Debug.Log($"ButtonPurchaseResources.OnUpdateProducts: {_purchaseName}    bundleId = {bundleId}");

            var isFind = false;
            if (products != null)
            {
                foreach (var product in products)
                {
                    if (bundleId == product.definition.id)
                    {
                        // Debug.LogError($"ButtonPurchaseResources: _priceText = {_priceText}    price = {product.metadata.localizedPriceString}");
                        if (_priceText != null)
                            _priceText.text = product.metadata.localizedPriceString;

                        // if (_titleText != null) {
                        //     _titleText.text = product.metadata.localizedDescription;
                        // }

                        Debug.Log($"ButtonPurchaseResources.OnUpdateProducts OK: {_purchaseName}");
                        isFind = true;
                        break;
                    }
                }

                if (!isFind)
                {
                    Debug.LogError(
                        $"ButtonPurchaseResources: Not find bundleId = {bundleId} products = {products}    products.count = {products?.Length}");
                    foreach (var product in products)
                    {
                        Debug.LogError(
                            $"Not find: transactionID = {product.transactionID}     " +
                            $"definition.id = {product.definition.id}     " +
                            $"definition.storeSpecificId = {product.definition.storeSpecificId}");
                    }
                }
            }
            else
            {
                Debug.LogError("ButtonPurchaseResources.OnUpdateProducts: products is null");
            }
        }
#endif
        
        private void OnBuyComplete()
        {
            Debug.Log($"ButtonPurchaseResources.OnBuyComplete: {_purchaseName}");
            _rewardResourcesConfig.Collected(transform);
        }
        
        private void OnBuyFail()
        {
            Debug.LogError($"ButtonPurchaseResources.OnBuyFail: {_purchaseName}");
            _failPurchaseEvent?.Invoke();
        }
    }
}