using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if ST_PURCHASING
using stickin;
using UnityEngine.Purchasing;
#endif

namespace stickin.menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonRestorePurchases : MonoBehaviour
    {
        [SerializeField] private UnityEvent _restoreCompleteEvent;
        
        private Button _btn;

        private void Start()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);
            
// #if ST_PURCHASING
//                 ServicePurchases.Instance.OnUpdateProducts += OnUpdateProducts;
//                 OnUpdateProducts(ServicePurchases.Instance.Products);
// #endif
        }

        private void OnClick()
        {
#if ST_PURCHASING
            ServicePurchases.Instance.Restore(OnRestoreComplete, OnRestoreFail);
#endif
        }

        private void OnRestoreComplete()
        {
            _restoreCompleteEvent?.Invoke();
        }

        private void OnRestoreFail()
        {
            
        }
    }
}