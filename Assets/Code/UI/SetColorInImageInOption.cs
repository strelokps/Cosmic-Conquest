//using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetColorInImageInOption : MonoBehaviour
{
    [SerializeField] private RawImage _ranbowChart;
    private Camera _camera;
    [SerializeField] private Collider _collider;
    private Texture2D _t2d;
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Image viewColor;
    private Material materialView;


    private Vector2 mousePos = new Vector2();
    private RectTransform rect;
    private int width = 0;
    private int height = 0;



    private void Start()
    {
        _camera = Camera.main;
        _t2d = (Texture2D)_ranbowChart.mainTexture;
        materialView = viewColor.material;

        var rawImage = _ranbowChart.GetComponent<RawImage>();
        rect = rawImage.GetComponent<RectTransform>();

        width = (int)rect.rect.width;
        height = (int)rect.rect.height;

        _t2d = rawImage.texture as Texture2D;

        var pixelData = _t2d.GetPixels();
        Debug.Log($"Total pixels {pixelData.Length}");

        var colorIndex = new List<Color>();
        var total = pixelData.Length;

        for (int i = 0; i < total; i++)
        {
            var color = pixelData[i];
            if (colorIndex.IndexOf(color) == -1)
            {
                colorIndex.Add(color);
            }
        }
        Debug.Log($"Index color {colorIndex.Count}");
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, Camera.main, out mousePos);
        mousePos.x = width - (width / 2 - mousePos.x);
        if (mousePos.x > width || mousePos.x < 0)
            mousePos.x = -1;

        mousePos.y = Mathf.Abs((height / 2 - mousePos.y) - height);
        if (mousePos.y > height || mousePos.y < 0)
            mousePos.y = -1;

        if (Input.GetMouseButton(0))
        {
            if (mousePos.x > -1 && mousePos.y > -1)
            {
                var color = _t2d.GetPixel((int)mousePos.x, (int)mousePos.y);
                materialView.color = color;
            }
        }
    }


    #region полурабочий вариант выбора цвета
/*    
[SerializeField] private Image _ranbowChart;
    private Texture2D _t2d;
    private int _ranbowChartCoord_X;
    private int _ranbowChartCoord_Y;

    public Image viewColor;
    private Material r;

    private void Start()
    {
        _t2d = (Texture2D)_ranbowChart.mainTexture;


        _ranbowChartCoord_X = _t2d.width;
        _ranbowChartCoord_Y = _t2d.height;
        r = viewColor.material;

    }

    private void Update()
    {// Получаем экранные координаты курсора мыши
        Vector3 mousePosition = Input.mousePosition;

        // Получаем позицию изображения в экранных координатах
        Vector3 imagePosition = Camera.main.WorldToScreenPoint(_ranbowChart.rectTransform.position);

        // Вычисляем позицию курсора относительно изображения
        Vector3 localPosition = mousePosition - imagePosition;

        // Получаем размеры прямоугольника изображения
        Rect imageRect = _ranbowChart.rectTransform.rect;

        // Ограничиваем позицию курсора в пределах прямоугольника изображения
        float clampedX = Mathf.Clamp(localPosition.x, 0, imageRect.width);
        float clampedY = Mathf.Clamp(localPosition.y, 0, imageRect.height);

        // Приводим позицию курсора относительно изображения к координатам текстуры
        float normalizedX = clampedX / imageRect.width;
        float normalizedY = clampedY / imageRect.height;

        // Получаем размеры текстуры
        int textureWidth = _t2d.width;
        int textureHeight = _t2d.height;

        // Вычисляем конечные координаты пикселя на текстуре
        int pixelX = Mathf.RoundToInt(normalizedX * textureWidth);
        int pixelY = Mathf.RoundToInt(normalizedY * textureHeight);

        // Получаем цвет пикселя на текстуре
        Color pixelColor = _t2d.GetPixel(pixelX, pixelY);
        r.color = pixelColor;
        Debug.Log($"normalizedX: {normalizedX} textureCoordinateX:  {clampedX}  textureWidth:  {textureWidth}  localPosition.x: {localPosition.x} _ranbowChart.rectTransform.rect.width:  {_ranbowChart.rectTransform.rect.width} pixelX:  {pixelX}");
    }
*/
#endregion
    
}

