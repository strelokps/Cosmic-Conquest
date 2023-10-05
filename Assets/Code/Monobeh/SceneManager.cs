using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SceneParametrsSO _sceneParametrsSO;
    [SerializeField] private List<AISceneData> _aiSceneData ;
    [SerializeField] private VersionBuildSO _versionBuild_SO;
    [SerializeField] private List<Transform> _allAIParentTransforms;
    [SerializeField] private Transform _playerParentTransform;
    
    private int _numAI;


    private void Start()
    {
        //версионность
        #region Версионность
        _versionBuild_SO = Resources.Load<VersionBuildSO>("versionBuild_SO");
        _versionBuild_SO.Increase();
        _versionBuild_SO.SetDirty();
        #endregion

        _sceneParametrsSO.TestScene();
        _aiSceneData = new List<AISceneData>();
        _numAI = _sceneParametrsSO.prop_numAI;
        _aiSceneData = _sceneParametrsSO.prop_ListAiSceneData;
        

        for (int i = 0; i < _aiSceneData.Count; i++)
        {
            if (_aiSceneData[i].neutral != null)
            Debug.Log($"Имена {_aiSceneData[i].neutral[0].nameAI}");
        }

    }

}
