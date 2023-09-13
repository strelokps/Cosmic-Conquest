using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

public class SetColorAI : MonoBehaviour
{
    private GetColorFromPixel _colorFromPixelAI;
    [SerializeField] private AIBase _aiBase;

    private void Start()
    {

        _colorFromPixelAI = new GetColorFromPixel();

    }

    public Color ReturnColorFromPixel(GetColorFromPixel _colorFromPixel)
    {
        return Color.blue;
    }

    private void Update()
    {
        ReturnColorFromPixel(_colorFromPixelAI);
        _aiBase.lvlTech++;
    }

}
