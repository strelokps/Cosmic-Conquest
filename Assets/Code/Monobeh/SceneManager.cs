using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


//TODO ������� ��� � ��������� ����� ���������� ��������� ���������� AI �� SO � ���������� �������� �������� ������ �� _aiSceneData.

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SceneParametrsSO _sceneParametrsSO;
    [SerializeField] private List<SceneMembersData> _listSceneMembersData ; 
    [SerializeField] private List<Transform> _allMembersParentTransforms; //������ ������� ��� ������� �� �����
    private GeneralConfig _generalConfig;

    private VersionBuildSO _versionBuild_SO;

    private int _numAI;

    [SerializeField] private TMP_Text _textVersion; // ���� ��������� ����� ��� ����������� ������������


    private void Start()
    {
        //������������
        #region ������������ VersionBuild
        VersionBuild();
        #endregion

        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");
        
        _sceneParametrsSO.TestScene(); //�������� ��������� �� AI �� SO
        _listSceneMembersData = new List<SceneMembersData>();
        _numAI = _sceneParametrsSO.prop_numAI;
        _listSceneMembersData = _sceneParametrsSO.prop_ListAiSceneData;
        
        CheckID(_listSceneMembersData, _allMembersParentTransforms);
    }

    //��������� id �� SO � ������������ � id �� ��������, ����� ����������� ������������, ������������ ������ � ����������� � ������ �������
    private void CheckID(List<SceneMembersData> locSceneMembersDatas, List<Transform> locListTransforms)
    {
        foreach (var indexMembers in locSceneMembersDatas)
        {
            foreach (var indexTransform in locListTransforms)
            {
                if (indexTransform.GetComponent<ParentManager>())
                {
                    var tr = indexTransform.GetComponent<ParentManager>();
                    
                    // AI
                    if (indexMembers.membersID == tr.prop_id)
                    {
                        tr.SetMembersSceneData(indexMembers);
                        //tr.Show();
                    }
                    
                    //player 

                    //if (tr.prop_id == _generalConfig.playerID)
                    //{
                    //    tr.SetMembersSceneData(_generalConfig.SetPlayerData());
                    //    //tr.Show();

                    //}

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
