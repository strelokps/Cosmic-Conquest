//using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Drawing;
using Color = UnityEngine.Color;
using Assets.Code.ScriptableObject;

public class SetColorInImageInOption : MonoBehaviour
{

    private GeneralConfig _generalConfig;
    [SerializeField] private Button _button;
    [SerializeField] private RawImage _ranbowChart;
    private Texture2D _t2d;
    [SerializeField] private Image viewColor;
    private Material materialView;
    private Vector2 mousePos = new Vector2();
    private RectTransform rect;
    private int width = 0;
    private int height = 0;


    private void Start()
    {

        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");

        _t2d = (Texture2D)_ranbowChart.mainTexture;
        materialView = viewColor.material; //линкуем материал

        var rawImage = _ranbowChart.GetComponent<RawImage>();
        rect = rawImage.GetComponent<RectTransform>();

        width =  (int)rect.rect.width;
        height = (int)rect.rect.height;

        _t2d = rawImage.texture as Texture2D;
        var pixelData = _t2d.GetPixels();

        var colorIndex = new List<Color>();
        var total = pixelData.Length;

        SetDefaultSetPlayerColor();
        SetColorInStart(_generalConfig.colorPlayer);
    }

    private void Update()
    {
        TakeColor();
    }

    private void TakeColor()
    {

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, Camera.main, out mousePos);
        mousePos.x = width - (width / 2 - mousePos.x);
        if (mousePos.x > width || mousePos.x < 0)
            mousePos.x = -1;

        mousePos.y = -((height / 2 - mousePos.y) - height);
        if (mousePos.y > height || mousePos.y < 0)
            mousePos.y = -1;

        if (Input.GetMouseButton(0))
        {
            if (mousePos.x > -1 && mousePos.y > -1)
            {
                var color = _t2d.GetPixel((int)(mousePos.x * (_t2d.width / rect.rect.width)), (int)(mousePos.y * (_t2d.height / rect.rect.height)));
                viewColor.material.color = color;
            }
        }
      

    }

    public void SetColor()
    {
        var colorButton = _button.GetComponent<Button>().colors;
        ;
        colorButton.normalColor = viewColor.material.color;
        _button.colors = colorButton;
        
    }

    public void SetColorInStart(Color locColor)
    {
        var colorButton = _button.GetComponent<Button>().colors;
        ;
        colorButton.normalColor = locColor;
        _button.colors = colorButton;
        viewColor.material.color = locColor;
    }


    public void SetPlayerColor()
    {
        SetColor();

        _generalConfig.colorPlayer = viewColor.material.color;
    }

    private void SetDefaultSetPlayerColor()
    {
        if (_generalConfig.colorPlayer == null | _generalConfig.colorPlayer == Color.black)
        {
            _generalConfig.colorPlayer = Color.blue; ;
        }
    }

}

