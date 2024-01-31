using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


//TODO Описать тут в комментах схема реализации подгрузки параметров AI из SO и дальнейшую передачу парентам ссылки на _aiSceneData.
public class SceneManager : MonoBehaviour
{
    [SerializeField] private SceneParametrsSO _sceneParametrsSO;
    [SerializeField] private List<SceneMembersData> _listSceneMembersData ; 
    [SerializeField] private List<Transform> _allMembersParentTransforms; //руками линкуем все паренты из сцены
    private GeneralConfig _generalConfig;


    private int _numAI;



    private void Start()
    {

        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");
        
        _sceneParametrsSO.TestScene(); //получили параметры по AI из SO
        _listSceneMembersData = new List<SceneMembersData>();
        _numAI = _sceneParametrsSO.prop_numAI;
        _listSceneMembersData = _sceneParametrsSO.prop_ListAiSceneData;
        
        CheckID(_listSceneMembersData, _allMembersParentTransforms);
    }

    //Проверяем id из SO и сопоставляем с id из парентов, после нахохждения соответствия, перекидываем ссылку с параметрами в скрипт парента
    private void CheckID(List<SceneMembersData> locSceneMembersDatas, List<Transform> locListTransforms)
    {
        //все участники сцены _allMembersParentTransforms
        foreach (var indexMembers in locSceneMembersDatas)
        {
            //все parent объекты сцены, который руками подгрузили в 
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
