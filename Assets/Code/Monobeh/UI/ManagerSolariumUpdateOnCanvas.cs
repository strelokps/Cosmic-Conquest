using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class ManagerSolariumUpdateOnCanvas : MonoBehaviour
{
    
    [SerializeField] private TMP_Text _locTextSolarium;


    private void Awake()
    {
        if (_locTextSolarium == null)
        {
            var goText = GameObject.FindGameObjectWithTag("DisplaySolarium");
            _locTextSolarium = goText.GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
        EventBus.OnSolariumUpdate += UpdateSolarium;
    }

    private void OnDisable()
    {
        EventBus.OnSolariumUpdate -= UpdateSolarium;
    }

    private void UpdateSolarium(int locSolarium)
    {
        _locTextSolarium.text = locSolarium.ToString();
    }
}
