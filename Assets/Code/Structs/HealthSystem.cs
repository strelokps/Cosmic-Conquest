using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct HealthSystem
{
    private DataShip _dataShip;
    public void TakeDamage(ref List<DataShip> locSelfListShips, int locTotalDamage)
    {
        System.Random random = new System.Random();

        while (locTotalDamage > 0 && locSelfListShips.Count > 0)
        {
            int targetIndex = random.Next(0, locSelfListShips.Count); // Выбираем случайный корабль

            DataShip targetShip = locSelfListShips[targetIndex];
            int damageToApply = Math.Min(targetShip.armorShip + targetShip.shieldShip, locTotalDamage); // Вычисляем урон

            // Распределяем урон между броней и щитом
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

            // Проверяем, нужно ли убрать корабль из списка
            if (targetShip.armorShip <= 0)
            {
                locSelfListShips.RemoveAt(targetIndex);
            }

            locTotalDamage -= damageToApply; // Обновляем оставшееся повреждение
        }
    }

    public void RegenerationShield(List<DataShip> locSelfShips)
    {
        DataShip locDataShip = new DataShip();
        for (int i = 0; i < locSelfShips.Count; i++)
        {
            locDataShip = locSelfShips[i];
            locDataShip.shieldShip += Math.Min(locSelfShips[i].maxShieldShip - locSelfShips[i].shieldShip, locSelfShips[i].regenShield); // Вычисляем что меньше, 
            locSelfShips[i] = locDataShip;
        }
    }
}
