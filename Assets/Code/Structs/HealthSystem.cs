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
            int targetIndex = random.Next(0, locSelfListShips.Count); // �������� ��������� �������

            DataShip targetShip = locSelfListShips[targetIndex];
            int damageToApply = Math.Min(targetShip.armorShip + targetShip.shieldShip, locTotalDamage); // ��������� ����

            // ������������ ���� ����� ������ � �����
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

            // ���������, ����� �� ������ ������� �� ������
            if (targetShip.armorShip <= 0)
            {
                locSelfListShips.RemoveAt(targetIndex);
            }

            locTotalDamage -= damageToApply; // ��������� ���������� �����������
        }
    }

    public void RegenerationShield(List<DataShip> locSelfShips)
    {
        DataShip locDataShip = new DataShip();
        for (int i = 0; i < locSelfShips.Count; i++)
        {
            locDataShip = locSelfShips[i];
            locDataShip.shieldShip += Math.Min(locSelfShips[i].maxShieldShip - locSelfShips[i].shieldShip, locSelfShips[i].regenShield); // ��������� ��� ������, 
            locSelfShips[i] = locDataShip;
        }
    }
}
