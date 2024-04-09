using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
   private TestPlanet _planet = new TestPlanet();

   private int y = 3;

   private void Awake()
   {
       _planet.Initialize(y);
       _planet.eChangeOwenerPlanet += ChangeTarget;
       _planet.eInitChange += ChangeTarget2;
   }

   private void ChangeTarget()
   {
       print($"<color=green>Изменение цели</color>");
   }
   
   private void ChangeTarget2( int x)
   {
       print($"<color=yellow>Изменение int {x}</color>");
   }

   private void OnDestroy()
   {
       _planet.eChangeOwenerPlanet -= ChangeTarget;
       _planet.eInitChange -= ChangeTarget2;
       print($"off");
    }
}
