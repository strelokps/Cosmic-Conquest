using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagerFieldAIInWindowOption 
{
    [SerializeField] private TextMeshProUGUI _textNameAI;
    [SerializeField] private Color _colorAI;
    [SerializeField] private int idAI;
    [SerializeField] private int[] _teramAI;

    private void Start()
    {
        var numAI = Resources.Load<GeneralConfig>("GeneralConfig_SO");
        _teramAI = new int[numAI.numberAI];
    }
}
