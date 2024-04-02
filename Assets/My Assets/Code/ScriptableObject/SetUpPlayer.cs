using UnityEngine;

namespace Assets.Code.ScriptableObject
{
    [CreateAssetMenu(fileName = "SetUpPlayerSO", menuName = "CosmicCon/Config/SetUpPlayer", order = 51)]
    public class SetUpPlayer : UnityEngine.ScriptableObject
    {
        public string playerName;
        public Color playerColor;
        public bool havePlayerTeam;
        public int howManyAIinTeam;

    }
}
