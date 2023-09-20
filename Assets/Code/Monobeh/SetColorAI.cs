using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

public class SetColorAI : MonoBehaviour
{
    private GameObject[] _allChildrenParent;
    private GetColorFromPixel _colorFromPixelAI;
    [SerializeField] private AIBase _aiBase;
    public List<Transform> children = new List<Transform>();

    private void Start()
    {
        

        _colorFromPixelAI = new GetColorFromPixel();
        var children = transform.parent.GetComponentsInChildren<Transform>();
        //foreach (var child in children)
        //{
        //    Debug.Log($" //___ {child.name}");

        //}
        //foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        //{
        //    Debug.Log(g.name);
        //}

        GetAllChildren(transform.parent);


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

    public void GetAllActiveChildren()
    {
        var children = transform.parent.GetComponentsInChildren<Transform>();
    }

    List<Transform> GetAllChildren(Transform parent)
    {
      

        // Перебираем все дочерние объекты
        foreach (Transform child in parent)
        {
            // Добавляем текущего дочернего объекта
            children.Add(child);

            // Рекурсивно вызываем эту функцию для дочерних объектов текущего объекта
            children.AddRange(GetAllChildren(child));
        }

        foreach (var index in children)
        {
            if (index.gameObject.activeInHierarchy)
            {
                Debug.Log($"//___ {index.name} - {index.parent}");
            }
        }

        return children;
    }


}
