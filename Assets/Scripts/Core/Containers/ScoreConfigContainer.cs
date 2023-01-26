using Core.Common;
using UnityEngine;

namespace Containers
{
    [CreateAssetMenu(menuName = "MTrivia/Containers/ScoreConfigs", fileName = nameof(ScoreConfigContainer))]
    public class ScoreConfigContainer : SingletonScriptableObject<ScoreConfigContainer>
    {
        [Header("Score Settings")]
        public int CorrectAnswerScore = 10;
        public int WrongAnswerPenalty = 5;
        public int TimeoutPenalty = 3;
    }
}