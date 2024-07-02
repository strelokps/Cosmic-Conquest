using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Random = System.Random;

public class HealthSystem : MonoBehaviour 
{
    private List<DataShip> _selfShips = new List<DataShip>();
    private ShipManager _shipManager = new ShipManager();

    public void InitHealthSystem(ShipManager locShipManager, List<DataShip> locSelfShips)
    {
        _shipManager = GetComponent<ShipManager>();
        _selfShips = locSelfShips;
    }

    // locEnemyDataShips - входящий демаг от вражеского флота
    public void TakeDamage(List<DataShip> enemyFleet)
    {
        _selfShips = GetComponent<ShipManager>().GetShipsList();
        
        float increasedDamage = 1;

        DataShip tempShip = new DataShip();


        for (int i = 0; i < enemyFleet.Count; i++)
        {

            float remainingDamage = enemyFleet[i].damageShip;

            for (int j = 0; j < _selfShips.Count; j++)
            {
                if (remainingDamage <= 0)
                    break;

                if (_selfShips[j].typeShipIncreasedDamage == enemyFleet[i].typeShip)
                {
                    print($"damage: {remainingDamage}  {_selfShips[j].typeShipIncreasedDamage} = {enemyFleet[i].typeShip}");

                    remainingDamage *= enemyFleet[i].increasedDamage;
                }


                // Сначала наносим урон щиту
                if (_selfShips[j].shieldShip > 0)
                {
                    float shieldDamage = Mathf.Min(_selfShips[j].shieldShip, remainingDamage);
                    
                    tempShip = _selfShips[j];
                    tempShip.shieldShip -= shieldDamage;
                    _selfShips[j] = tempShip;
                    
                    remainingDamage -= shieldDamage;
                }

                // Если урон остался, наносим его по броне
                if (remainingDamage > 0 && _selfShips[j].armorShip > 0)
                {
                    float armorDamage = Mathf.Min(_selfShips[j].armorShip, remainingDamage);
                    print($"<color=yellow> Есть пробитие 0 {_selfShips[j].armorShip}  {_selfShips[j].typeShip}</color>");

                    tempShip = _selfShips[j];
                    tempShip.armorShip -= armorDamage;
                    _selfShips[j] = tempShip;

                    print($"<color=yellow> Есть пробитие 1 {_selfShips[j].armorShip}  {_selfShips[j].typeShip}</color>");

                    if (_selfShips[j].armorShip <= 0)
                    {
                        _selfShips.RemoveAt(j);

                        if (_selfShips.Count <= 0)
                        {
                            _shipManager.OnOffShipGO(false);
                        }
                    }

                    remainingDamage -= armorDamage;
                }
            }
            _shipManager.DisplayArmorAndShield();

        }
    }
    

    public void RegenerationShield( List<DataShip> locSelfShips)
    {


        DataShip locDataShip = new DataShip();
        for (int i = 0; i < locSelfShips.Count; i++)
        {
            if (locSelfShips[i].shieldShip < locSelfShips[i].maxShieldShip)
            {
                if (locSelfShips[i].shieldShip < 0)
                {
                    locDataShip = locSelfShips[i];
                    locDataShip.shieldShip = 0;
                    locSelfShips[i] = locDataShip;
                }

                if (locSelfShips[i].armorShip < 0)
                {
                    locDataShip = locSelfShips[i];
                    locDataShip.armorShip = 0;
                    locSelfShips[i] = locDataShip;
                }

                locDataShip = locSelfShips[i];

                locDataShip.shieldShip += Math.Min(locSelfShips[i].maxShieldShip - locSelfShips[i].shieldShip,
                    locSelfShips[i].regenShield); // Вычисляем что меньше, уровень регенираци или разница между max и текущем уровнем щита

                locSelfShips[i] = locDataShip;
            }
        }
    }



    public void CalcArmorAndShield(ref float locShield, ref float locArmor, List<DataShip> locSelfShips)
    {
        locShield = 0;
        locArmor = 0;

        DataShip locDataShip = new DataShip();

        for (int i = 0; i < locSelfShips.Count; i++)
        {
            if (locSelfShips[i].shieldShip < 0) //делаем проверку, что бы значение не уцходило ниже 0
            {
                locDataShip = locSelfShips[i];
                locDataShip.shieldShip = 0;
                locSelfShips[i] = locDataShip;
            }

            if (locSelfShips[i].armorShip < 0) //делаем проверку, что бы значение не уцходило ниже 0
            {
                locDataShip = locSelfShips[i];
                locDataShip.armorShip = 0;
                locSelfShips[i] = locDataShip;
            }


            locShield += locSelfShips[i].shieldShip;
            locArmor += locSelfShips[i].armorShip;
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

    
}
