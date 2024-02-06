using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

namespace BitshiftGames.Essentials.UI
{
    public class RunningNumberAnimator : MonoBehaviour
    {
        public TextMeshProUGUI label;

        [Header("Settings")]
        public int targetValue;
        public int initialValue;
        public string prefix;
        [Space(5)]
        public int stepSize;
        public int countUpDelay;


        public async void AnimateNumbers()
        {
            int reps = Mathf.CeilToInt(Mathf.Abs(targetValue - initialValue));
            int currentValue = initialValue;
            label.text = currentValue.ToString() + prefix;

            for (int i = 0; i < reps; i++)
            {
                currentValue = Mathf.Clamp(currentValue + (stepSize * targetValue > initialValue ? 1 : -1),
                    targetValue > initialValue ? initialValue : targetValue,
                    targetValue > initialValue ? targetValue : initialValue);

                label.text = currentValue.ToString() + prefix;
                await Task.Delay(countUpDelay);
            }
        }
    }
}
