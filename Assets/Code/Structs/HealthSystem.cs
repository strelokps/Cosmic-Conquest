using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public struct HealthSystem
{
    private DataShip _dataShip;
    public void TakeDamage(ref List<DataShip> locSelfListShips, List<DataShip> locEnemyDataShips) 
    {
        int locTotalDamage = 0;
        Random e;
        UnityEngine.Random.Range(1,5);
        for (int i = 0; i < locEnemyDataShips.Count; i++)
        {
            locTotalDamage +=
                UnityEngine.Random.Range(locEnemyDataShips[i].damageShipMin, locEnemyDataShips[i].damageShipMax);
        }
        System.Random random = new System.Random();

        while (locTotalDamage > 0 && locSelfListShips.Count > 0)
        {
            int targetIndex = random.Next(0, locSelfListShips.Count); // ¬ыбираем случайный корабль

            DataShip targetShip = locSelfListShips[targetIndex];
            int damageToApply = Math.Min(targetShip.armorShip + targetShip.shieldShip, locTotalDamage); // ¬ычисл€ем урон

            // –аспредел€ем урон между броней и щитом
            if (damageToApply <= targetShip.shieldShip)
            {
                targetShip.shieldShip -= damageToApply;
            }
            else
            {
                int remainingDamage = damageToApply - targetShip.shieldShip;
                targetShip.shieldShip = 0;
                targetShip.armorShip -= remainingDamage;
            }

            // ѕровер€ем, нужно ли убрать корабль из списка
            if (targetShip.armorShip <= 0)
            {
                locSelfListShips.RemoveAt(targetIndex);
            }

            locTotalDamage -= damageToApply; // ќбновл€ем оставшеес€ повреждение
        }
    }

    public void RegenerationShield(List<DataShip> locSelfShips)
    {
        DataShip locDataShip = new DataShip();
        for (int i = 0; i < locSelfShips.Count; i++)
        {
            if (locSelfShips[i].shieldShip < locSelfShips[i].maxShieldShip)
            {
                locDataShip = locSelfShips[i];

                locDataShip.shieldShip += Math.Min(locSelfShips[i].maxShieldShip - locSelfShips[i].shieldShip,
                    locSelfShips[i].regenShield); // ¬ычисл€ем что меньше, уроень регинераци или разница между max и текущем уровнем щита

                locSelfShips[i] = locDataShip;
            }
        }
    }
}
