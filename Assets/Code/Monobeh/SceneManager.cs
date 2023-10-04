using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SceneParametrsSO _sceneParametrsSO;
    private int _numAI;
    [SerializeField] private List<AISceneData> _aiSceneData ;
    [SerializeField] private VersionBuildSO _versionBuild_SO;


    private void Start()
    {
        //версионность
        _versionBuild_SO = Resources.Load<VersionBuildSO>("versionBuild_SO");
        _versionBuild_SO.Increase();
        _versionBuild_SO.SetDirty();

        _sceneParametrsSO.TestScene();
        _aiSceneData = new List<AISceneData>();
        _numAI = _sceneParametrsSO.prop_numAI;
        _aiSceneData = _sceneParametrsSO.prop_ListAiSceneData;
        

        for (int i = 0; i < _aiSceneData.Count; i++)
        {
            Debug.Log($"Имена {_aiSceneData[i].nameAI}");
        }

        Debug.Log($"?!");
    }

}
