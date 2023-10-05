using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


//TODO Описать тут в комментах схема реализации подгрузки параметров AI из SO и дальнейшую передачу парентам ссылки на _aiSceneData.

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SceneParametrsSO _sceneParametrsSO;
    [SerializeField] private List<SceneMembersData> _listSceneMembersData ;
    [SerializeField] private List<Transform> _allMembersParentTransforms;
    private GeneralConfig _generalConfig;

    private VersionBuildSO _versionBuild_SO;

    private int _numAI;

    [SerializeField] private TMP_Text _textVersion;


    private void Start()
    {
        //версионность
        #region Версионность
        VersionBuild();
        #endregion

        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");
        
        _sceneParametrsSO.TestScene(); //получили параметры по AI из SO
        _listSceneMembersData = new List<SceneMembersData>();
        _numAI = _sceneParametrsSO.prop_numAI;
        _listSceneMembersData = _sceneParametrsSO.prop_ListAiSceneData;
        

        for (int i = 0; i < _listSceneMembersData.Count; i++)
        {
            if (_listSceneMembersData[i].neutral.Count != 0)
            {
                Debug.Log($"Имена {_listSceneMembersData[i].neutral[0].nameAI}");
            }
        }

        CheckID(_listSceneMembersData, _allMembersParentTransforms);
    }

    private void CheckID(List<SceneMembersData> locSceneMembersDatas, List<Transform> locListTransforms)
    {
        ParentManager pr = GetComponent<ParentManager>();
        foreach (var indexMembers in locSceneMembersDatas)
        {
            foreach (var indexTransform in locListTransforms)
            {
                if (indexTransform.GetComponent<ParentManager>())
                {
                    var tr = indexTransform.GetComponent<ParentManager>();
                    
                    if (indexMembers.membersID == tr.prop_id)
                    {
                        tr.SetListAISceneData(indexMembers);
                        tr.Show();
                    }
                }
                else
                {
                    Debug.Log("No have component <ParentManager>");
                }

            }
        }
    }

    private void VersionBuild()
    {
        _versionBuild_SO = Resources.Load<VersionBuildSO>("versionBuild_SO");
        _versionBuild_SO.Increase();
        _versionBuild_SO.ShowBuild(_textVersion);
        _versionBuild_SO.SetDirty();
    }

}
