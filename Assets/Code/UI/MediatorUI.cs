using TMPro;
using UnityEngine;

namespace Assets.Code.UI
{
    public class MediatorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMoneyOnUI;
        // Start is called before the first frame update
        void Start()
        {//»щем GO c именем ViewValMoneyText, на случай если ссылка слетела
            if (_textMoneyOnUI == null)
            {
                _textMoneyOnUI = new TextMeshProUGUI();
                _textMoneyOnUI = GameObject.Find("ViewValMoneyText").GetComponentInChildren<TextMeshProUGUI>();
            }

        }

        public void SetValMoney(string locStringValMoney)
        {
            _textMoneyOnUI.text = locStringValMoney;
        }
    }
}
