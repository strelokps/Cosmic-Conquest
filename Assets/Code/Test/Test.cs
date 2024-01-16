using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Test
{
    //public Color color;
    //public bool testFlag;
    //private GeneralConfig _generalConfig;
    //private GeneralConfigDefaultSettings _generalConfigDefaultSettings;

    //private void Start()
    //{
    //    _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");
    //    _generalConfigDefaultSettings = Resources.Load<GeneralConfigDefaultSettings>("GeneralConfig_DefaultSettings_SO");
    //    _generalConfigDefaultSettings.numberAI = _generalConfig.numberAI;
    //}
    /*
    private void StartedAttack()
    {

        _targetToMove = _distParametrPlanetMono.SelfTransform.position;

        if (!CheckDistPlanetIsEnemy())
        {
            _distParametrPlanetMono.AddToListAttackerFleet(gameObject);
        }
        else
        {
            _distParametrPlanetMono.AddToListIncomeFriendlyFleet(gameObject);
        }

        _stateFleet = FleetStateStruct.enumFleetState.OrbitCallDefendefFleet;
    }


     //если у планеты есть флот защиты то будем атаковать его
            if (_distParametrPlanetMono.GetCountDefenceFleet() > 0)
            {
                //TODO атака уже существующего флота
                print($"Attack def fleet on orbit!!!!!");
                _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsDefenceFleet;
                _targetTransform = _distParametrPlanetMono.GetTransformDefenceFleet();
               
            }
    //TODO движение в сторону дружественного защитного флота на орбите
     private void MovingTowardsToDefenceFleet()
    {
        Vector3 targetRotation = _targetTransform.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRotation), 4f * Time.deltaTime);
        float angle = Quaternion.Angle(transform.rotation, _targetTransform.rotation);
        print($"”гол {angle}");

    }


    */
}
