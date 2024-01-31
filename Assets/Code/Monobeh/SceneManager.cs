using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
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


    private int _numAI;



    private void Start()
    {

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
        //��� ��������� ����� _allMembersParentTransforms
        foreach (var indexMembers in locSceneMembersDatas)
        {
            //��� parent ������� �����, ������� ������ ���������� � 
            foreach (var indexTransform in locListTransforms)
            {
                if (indexTransform.GetComponent<ParentManager>())
                {
                    var tr = indexTransform.GetComponent<ParentManager>();

                    for (int i = 0; i < indexMembers.enemy.Count; i++)
                    {
                        if (indexMembers.enemy[i].membersID == tr.prop_id)
                        {
                            SceneMembersData locMB = indexMembers.enemy[i];
                            locMB.parentTransform = indexTransform;
                            indexMembers.enemy[i] = locMB;
                            indexTransform.name = locMB.nameMembers;
                        }
                    }
                    // AI + Player
                    if (indexMembers.membersID == tr.prop_id)
                    {
                        tr.SetMembersSceneData(indexMembers);
                        if (!indexMembers.flagPlayer & !indexMembers.flagNeutral)
                        {
                            indexTransform.AddComponent<AI_logic>();
                        }
                        else
                        {
                            indexTransform.tag = "Player";
                        }

                    }

                }
                else
                {
                    Debug.Log("No have component <ParentManager>");
                }

            }
        }
    }
  
    
}
