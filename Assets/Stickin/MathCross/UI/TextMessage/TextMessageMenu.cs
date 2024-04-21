using System.Collections;
using stickin.menus;
using UnityEngine;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class TextMessageMenu : BaseMenu
    {
        public const string AdsNotReady = "Ads not ready";
            
        [SerializeField] private Text _text;

        public static void ShowWithText(string text)
        {
            // Ads not ready
            MenusService.Show<TextMessageMenu>(new Hashtable { ["text"] = text });
        }
        
        public override void SetData(Hashtable data = null)
        {
            base.SetData(data);

            if (data != null && data.ContainsKey("text"))
                _text.text = (string) data["text"];
        }

        protected override void ShowComplete()
        {
            base.ShowComplete();
            
            Hide();
        }
    }
}