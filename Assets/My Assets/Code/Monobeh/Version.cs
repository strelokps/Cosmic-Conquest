using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEngine;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class Version : MonoBehaviour
{
    private VersionBuildSO _versionBuild_SO;
    [Header("������������")]
    [SerializeField] private TMP_Text _textVersion; // ���� ��������� ����� ��� ����������� ������������



    private void Start()
    {
        //������������

        #region ������������ VersionBuild

        VersionBuild();

        #endregion
    }


    private void VersionBuild()
    {
#if UNITY_EDITOR
      
        if (UnityEditor.EditorApplication.isPlaying) return;

        _versionBuild_SO = Resources.Load<VersionBuildSO>("versionBuild_SO");
        _versionBuild_SO.Increase();
        _versionBuild_SO.ShowBuild(_textVersion);
        _versionBuild_SO.SetDirty();
#endif

    }
}
