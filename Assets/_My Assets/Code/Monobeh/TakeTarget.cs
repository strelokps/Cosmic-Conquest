using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTarget : MonoBehaviour
{

    public Vector3 TakeTargetForAttackingFleet(ref bool flagDisableFire,  GameObject locGOTarget)
    {
        Vector3 pointToHit = new Vector3();
        flagDisableFire = false;

        if (locGOTarget)
        {
            List<Transform> listPointToHit = locGOTarget.GetComponent<FleetManager>().TakePointForTarget(); //hash

            if (listPointToHit.Count > 0)
            {
                pointToHit = GetRandomPointToHit(listPointToHit);
                flagDisableFire = true;

            }
            else
            {
               print("<color=red>Cap, dont't see target -> listPointToHit.Count <= 0</color>");
                flagDisableFire = false;
            }
        }

        return pointToHit;
    }

    public Vector3 TakeTargetForDefenderFleet(ref bool flagDisableFire, List<Transform> locListTransformsForTargetForDefenderFleet)
    {
        Vector3 pointToHit = new Vector3();
        if (locListTransformsForTargetForDefenderFleet.Count > 0)
        {
            pointToHit = GetRandomPointToHit(locListTransformsForTargetForDefenderFleet);
        }
        else
        {
            Debug.LogError("Cap, dont't see target -> listPointToHit.Count <= 0");
            flagDisableFire = false;
        }
        return pointToHit;
    }

    private Vector3 GetRandomPointToHit(List<Transform> valuesPointToHit)
    {
        int index = Random.Range(0, valuesPointToHit.Count); // Take random index
        return valuesPointToHit[index].position;
    }


}
