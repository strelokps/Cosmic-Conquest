using TMPro;
using UnityEngine;

namespace Assets.Code.UI
{
    public class MediatorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMoneyOnUI;
        void Start()
        {//���� GO c ������ ViewValMoneyText, �� ������ ���� ������ �������
            if (_textMoneyOnUI == null)
            {
                _textMoneyOnUI = new TextMeshProUGUI();
                _textMoneyOnUI = GameObject.FindGameObjectWithTag("DisplaySolarium").GetComponentInChildren<TextMeshProUGUI>();
            }

        }

        public void SetValMoney(string locStringValMoney)
        {
            _textMoneyOnUI.text = locStringValMoney;
        }
    }
}
