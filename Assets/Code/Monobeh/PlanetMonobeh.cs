using UnityEngine;

namespace Assets.Code.Monobeh
{
    public class PlanetMonobeh : MonoBehaviour
    {

        public Transform GeTransform()
        {
            return transform.GetComponent<GameObject>().transform;
        }

    }
}
