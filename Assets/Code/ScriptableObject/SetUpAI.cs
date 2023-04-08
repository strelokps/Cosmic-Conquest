using UnityEngine;

namespace Assets.Code.ScriptableObject
{
    [CreateAssetMenu(fileName = "SetUpAISO", menuName = "CosmicCon/Config/SetUpAI", order = 51)]
    public class SetUpAI : UnityEngine.ScriptableObject
    {
        //через СО устанавливаем кол-во компьютерных игроков
        public int numberAI;
        public Transform parentTransform;
        public Color colorAI;
        public string nameAI;
        public int difficultyAI;
        public Color[] arrColor = {Color.red, Color.blue, Color.green, Color.yellow, Color.cyan};
    }
}
