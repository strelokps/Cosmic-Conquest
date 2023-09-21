using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SetPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField _setPlayerName;
    private GeneralConfig _generalConfig;

    private void Start()
    {
        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");
        _setPlayerName = gameObject.GetComponent<TMP_InputField>();
        if (_generalConfig.playerName.Length <= 0)
        {
            Debug.LogError("ƒлина имени игрока ЌќЋ№!!! ");
        }
        else
        {
            _setPlayerName.text = _generalConfig.playerName;
        }
    }
    //метод нужен дл€ событи€ InputField, которое обрабатывает ввод имени в строку
    public void SavePlayerName()
    {
        _generalConfig.playerName = _setPlayerName.text;
        Debug.Log($"Text player name {_setPlayerName.text}");
        _generalConfig.SetDirty();


    }
}
