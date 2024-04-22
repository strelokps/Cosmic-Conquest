using UnityEngine;

namespace Assets.Code.Monobeh
{
    public class PlanetMonobeh : MonoBehaviour
    {
        public bool testFlag;

        public Transform GeTransform()
        {
            return transform.GetComponent<GameObject>().transform;
        }

    }
}
