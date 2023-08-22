using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetColorInImageInOption : MonoBehaviour
{
    public UnityEvent<Color> ColorPickerEvent;

    private RectTransform _cursor;
    [SerializeField] private Texture2D _colorChart;

    public void PickerColor(BaseEventData locData)
    {
        PointerEventData pointer = locData as PointerEventData;

        _cursor.position = pointer.position;
        Color pickedColor = _colorChart.GetPixel(
            (int)(_cursor.localPosition.x *
                  (_colorChart.width / transform.GetChild(0).GetComponent<RectTransform>().rect.width)),
            (int)(_cursor.localPosition.y *
                  (_colorChart.height / transform.GetChild(0).GetComponent<RectTransform>().rect.height)));
        Debug.Log($"Цвет: {pickedColor}");

    }
}
