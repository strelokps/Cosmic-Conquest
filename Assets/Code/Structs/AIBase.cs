using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

public class AIBase : MonoBehaviour
{
    [Header("SO AI"), SerializeField] private SetUpAI _setupAI;

    [Header("Self AI settings")]
    public string nameAI;
    public int idAI;
    public Color colorAI;
    public int lvlTech;

    [Header("Team")]
    public List<AIBase> friends;
    public List<AIBase> enemy;
    public List<AIBase> neutral;

    [Header("For Color")]
    [SerializeField] private Button _button;
    [SerializeField] private RawImage _ranbowChart;
    [SerializeField] private Image viewColor;
    private RectTransform rect;
    private int width = 0;
    private int height = 0;
    private Texture2D _t2d;

    private GetColorFromPixel _colorFromPixelAI;

    private void Start()
    {
        if (_setupAI == null)
            Debug.Log("No have load SO SetUpAI");

        _colorFromPixelAI = new GetColorFromPixel();
        
        var rawImage = _ranbowChart.GetComponent<RawImage>();
        rect = rawImage.GetComponent<RectTransform>();
        width = (int)rect.rect.width;
        height = (int)rect.rect.height;
        _t2d = rawImage.texture as Texture2D;
        var pixelData = _t2d.GetPixels();

    }

    public void SetColor(int locValue)
    {
        lvlTech = locValue;
        
    }


    public void SetColorInStart(Color locColor)
    {
        var colorButton = _button.GetComponent<Button>().colors;
        ;
        colorButton.normalColor = locColor;
        colorButton.selectedColor = locColor;
        colorButton.highlightedColor = locColor;
        colorButton.pressedColor = locColor;
        _button.colors = colorButton;
        viewColor.material.color = locColor;
    }
    public void SetColor()
    {
        var colorButton = _button.GetComponent<Button>().colors;
        ;
        colorButton.normalColor = viewColor.material.color;
        colorButton.selectedColor = viewColor.material.color;
        colorButton.highlightedColor = viewColor.material.color;
        colorButton.pressedColor = viewColor.material.color;

        _button.colors = colorButton;

    }

    public void SetAIColor()
    {
        _setupAI.colorAI_SO = viewColor.material.color;
        SetColor();
    }

    public void StartGetColorFromPixel()
    {
        _colorFromPixelAI.TakeColor(rect, height, width, _t2d, ref viewColor);
        _setupAI.colorAI_SO = viewColor.material.color;
    }
}
