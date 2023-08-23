//using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Drawing;
using Color = UnityEngine.Color;

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

        width =  (int)rect.rect.width;
        height = (int)rect.rect.height;

        _t2d = rawImage.texture as Texture2D;
        var pixelData = _t2d.GetPixels();

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
        materialView.color = _t2d.GetPixel(0, 0); ;
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, Camera.main, out mousePos);
        mousePos.x = width - (width / 2 - mousePos.x );
        if (mousePos.x > width || mousePos.x < 0)
            mousePos.x = -1;

        mousePos.y = Mathf.Abs((height / 2 - mousePos.y) - height);
        if (mousePos.y > height || mousePos.y < 0)
            mousePos.y = -1;

        if (Input.GetMouseButton(0))
        {
            if (mousePos.x > -1 && mousePos.y > -1)
            {
                var color = _t2d.GetPixel((int)(mousePos.x * (_t2d.width / rect.rect.width)), (int)mousePos.y);
                materialView.color = color;
            }
        }
    }



    
}

