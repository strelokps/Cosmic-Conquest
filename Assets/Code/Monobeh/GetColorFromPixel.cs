using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetColorFromPixel : MonoBehaviour
{
    private Vector2 mousePos = new Vector2();


    public void TakeColor(RectTransform rect, int height, int width, Texture2D _t2d, ref Image viewColor)
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

}
