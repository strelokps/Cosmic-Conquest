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
        float locTotalDamage = 0;
        float increasedDamage = 0;
        System.Random random = new System.Random();

        int countBreak = 0;
        for (int i = 0; i < locEnemyDataShips.Count; i++)
        {
            locTotalDamage =
                UnityEngine.Random.Range(locEnemyDataShips[i].damageShipMin, locEnemyDataShips[i].damageShipMax);

            Debug.Log($"Общий урон {locTotalDamage}");


            while (locTotalDamage > 0 && locSelfListShips.Count > 0)
            {
                countBreak++;
                if (countBreak > 10000)
                    break;
                int targetIndex = random.Next(0, locSelfListShips.Count); // Выбираем случайный корабль

                DataShip targetShip = locSelfListShips[targetIndex];

                if (locEnemyDataShips[i].typeShipIncreasedDamage == targetShip.typeShip)
                    increasedDamage *= locEnemyDataShips[i].increasedDamage;
                else
                {
                    increasedDamage = 1f;
                }

                Debug.Log($"Есть ли увеличенный урон? {increasedDamage == 1f}");

                float damageToApply =
                    Math.Min(targetShip.armorShip + targetShip.shieldShip, (locTotalDamage * increasedDamage)); // Вычисляем урон
                Debug.Log($" До <color=red> shield {targetShip.shieldShip}  armor {targetShip.armorShip} </color>");

                // Распределяем урон между броней и щитом
                if (damageToApply <= targetShip.shieldShip)
                {
                    targetShip.shieldShip -= damageToApply;
                    locSelfListShips[targetIndex] = targetShip;
                }
                else
                {
                    float remainingDamage = damageToApply - targetShip.shieldShip;
                    targetShip.shieldShip = 0;
                    targetShip.armorShip -= remainingDamage;

                    locSelfListShips[targetIndex] = targetShip;

                }

                // Проверяем, нужно ли убрать корабль из списка
                if (targetShip.armorShip <= 0)
                {
                    locSelfListShips.RemoveAt(targetIndex);
                }
                Debug.Log($"<color=green> После shield {targetShip.shieldShip}  armor {targetShip.armorShip} </color>");

                locTotalDamage -= damageToApply; // Обновляем оставшееся повреждение
            }
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
                    locSelfShips[i].regenShield); // Вычисляем что меньше, уроень регинераци или разница между max и текущем уровнем щита

                locSelfShips[i] = locDataShip;
            }
        }
    }
}
