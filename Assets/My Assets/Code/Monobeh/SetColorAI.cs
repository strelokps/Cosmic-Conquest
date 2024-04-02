using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

public class SetColorAI : MonoBehaviour
{

    private GeneralConfig _generalConfig;
    [SerializeField] private Button _button;
    [SerializeField] private RawImage _ranbowChart;
    [SerializeField] private Image viewColor;
    [SerializeField] private GameObject _chaeckPressButton; //нужен для идентификации какая кнопка нажата
    private Vector2 mousePos = new Vector2();
    private RectTransform rect;
    private int width = 0;
    private int height = 0;
    private Texture2D _t2d;
    private string _getNumerFromEndNameGOAI;
    private int _parserStringNumerFromNameGOAI;

    private GetColorFromPixel _colorFromPixelAI;


    private void Start()
    {
        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");

        if (gameObject.name.Length > 0)
        {
            _getNumerFromEndNameGOAI = gameObject.name.Substring(gameObject.name.Length - 1);
            _parserStringNumerFromNameGOAI = int.Parse(_getNumerFromEndNameGOAI);
        }
        else
        {
        }
        _colorFromPixelAI = new GetColorFromPixel();
        SetColorInStart(_generalConfig.arrColor_SO[_parserStringNumerFromNameGOAI]);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) & _ranbowChart.IsActive() & _chaeckPressButton.activeInHierarchy)
        {
            _colorFromPixelAI.TakeColor(_ranbowChart, ref viewColor);
        }
    }


    public void SetColor()
    {
        var colorButton = _button.GetComponent<Button>().colors;
        colorButton.normalColor = viewColor.material.color;
        colorButton.selectedColor = viewColor.material.color;
        colorButton.highlightedColor = viewColor.material.color;
        colorButton.pressedColor = viewColor.material.color;
        _generalConfig.arrColor_SO[_parserStringNumerFromNameGOAI] = viewColor.material.color;
        _generalConfig.SetDirty();
        _button.colors = colorButton;

    }

    public void SetColorInStart(Color locColor)
    {
        var colorButton = _button.GetComponent<Button>().colors;
        colorButton.normalColor = locColor;
        colorButton.selectedColor = locColor;
        colorButton.highlightedColor = locColor;
        colorButton.pressedColor = locColor;
        _button.colors = colorButton;
        viewColor.material.color = locColor;
    }


    public void SetAIColor()
    {
        _generalConfig.arrColor_SO[_parserStringNumerFromNameGOAI] = viewColor.material.color;
        _generalConfig.SetDirty();
        SetColor();

    }





}
