using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaBuilding : MonoBehaviour
{
    public AnimationCurve moveCurve;
    float initialHeight;
    float targetHeight;

    float interploationDuration;

    const float minHeight = 1f;
    const float maxHeight = 3.75f;
    //const float minHeightChange = 0.75f;

    const float minInterpolationDuration = 0.92307692307692307692307692307692F;
    const float maxInterpolationDuration = 0.92307692307692307692307692307692F;

    void Start()
    {
        initialHeight = Random.Range(minHeight, maxHeight);
        interploationDuration = Random.Range(minInterpolationDuration, maxInterpolationDuration);

        transform.localScale = new Vector3(
            transform.localScale.x,
            initialHeight,
            transform.localScale.z);
    }

    private void Update()
    {
        float yScale = initialHeight + moveCurve.Evaluate((Time.time - Managers.Turn.startTurnTime) / interploationDuration) * (targetHeight - initialHeight);
        transform.localScale = new Vector3(
            transform.localScale.x,
            yScale,
            transform.localScale.z);

        //Less than ideal
        if (Mathf.Approximately(Managers.Turn.startTurnTime, Time.time))
        {
            initialHeight = targetHeight;

            if (targetHeight == maxHeight)
            {
                targetHeight = minHeight;
            }
            else
            {
                targetHeight = (Random.Range(0, 2) == 0 ? minHeight : maxHeight);
            }
            interploationDuration = Random.Range(minInterpolationDuration, maxInterpolationDuration);
        }
    }
}