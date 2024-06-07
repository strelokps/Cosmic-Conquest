using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Random = System.Random;

public struct HealthSystem
{
    private DataShip _dataShip;
    // locEnemyDataShips - входящий демаг от вражеского флота
    public void TakeDamage(FleetManager _selfFleetManager, List<DataShip> locEnemyDataShips)
    {
        List <DataShip> locSelfListShips = _selfFleetManager.GetListDataFleet();
        float locTotalDamage = 0;
        float increasedDamage = 1;
        System.Random random = new System.Random();

        int countBreak = 0;
        for (int i = 0; i < locEnemyDataShips.Count; i++)
        {
            locTotalDamage = locEnemyDataShips[i].damageShip;


            //Debug.Log($"Общий урон {locTotalDamage}");

            //Атакующей флот стреляет. Флот по которому попали обрабатывает попадание. 
            while (locTotalDamage > 0 && locSelfListShips.Count > 0)
            {
                countBreak++;
                if (countBreak > 10000)
                {
                    Debug.LogError("The cycle in HealthSystem has gone to infinity");
                    break;
                }

                int targetIndex = random.Next(0, locSelfListShips.Count); // Выбираем случайный корабль для нанемения урона
                
                if (targetIndex == locSelfListShips.Count)
                {
                    Debug.LogError("в TakeDamage, targetIndex == locSelfListShips.Count должен быть выход за диапазон locSelfListShips[targetIndex] ?!!");
                }

                DataShip targetShip = locSelfListShips[targetIndex];

                if (locEnemyDataShips[i].typeShipIncreasedDamage == targetShip.typeShip)
                    increasedDamage *= locEnemyDataShips[i].increasedDamage;
                else
                {
                    increasedDamage = 1f;
                }

                //Debug.Log($"Есть ли увеличенный урон? {increasedDamage != 1f}");

                float damageToApply =
                    Math.Min(targetShip.armorShip + targetShip.shieldShip, ( locTotalDamage * increasedDamage)); // Вычисляем урон

                //Debug.Log($" До <color=red> shield {targetShip.shieldShip}  armor {targetShip.armorShip} </color>");
                
                // Распределяем урон между броней и щитом
                if (damageToApply <= targetShip.shieldShip)
                {
                    targetShip.shieldShip -= damageToApply;
                    locSelfListShips[targetIndex] = targetShip;     //копируем корабль обратно в список
                }
                else
                {
                    float remainingDamage = damageToApply - targetShip.shieldShip;
                    targetShip.shieldShip = 0;
                    targetShip.armorShip -= remainingDamage;

                    locSelfListShips[targetIndex] = targetShip;     //копируем корабль обратно в список

                }

                // Проверяем, нужно ли убрать корабль из списка
                if (targetShip.armorShip <= 0)
                {
                    locSelfListShips.RemoveAt(targetIndex);
                }
                //Debug.Log($"<color=green> После shield {targetShip.shieldShip}  armor {targetShip.armorShip} </color>");

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

    public void SetMaxArmorAndShield(List<DataShip> locSelfShips)
    {
        DataShip locDataShip = new DataShip();
        for (int i = 0; i < locSelfShips.Count; i++)
        {
            if (locSelfShips[i].shieldShip < locSelfShips[i].maxShieldShip)
            {
                locDataShip = locSelfShips[i];

                locDataShip.shieldShip = locDataShip.maxShieldShip; 

                locSelfShips[i] = locDataShip;
            }

            if (locSelfShips[i].armorShip < locSelfShips[i].maxArmorShip)
            {
                locDataShip = locSelfShips[i];

                locDataShip.armorShip = locDataShip.maxArmorShip; 

                locSelfShips[i] = locDataShip;
            }
        }
    }

    public void TakeDamage(float locTakeDamage)
    {
        Debug.Log($"<color=puprple> Take damage {locTakeDamage} </color>");
    }
}
