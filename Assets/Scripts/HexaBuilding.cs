using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaBuilding : MonoBehaviour
{
    public AnimationCurve moveCurve;
    float initialHeight;
    float targetHeight;

    float interploationDuration;
    float startInterpolationTime;

    const float minHeight = 1f;
    const float maxHeight = 3.75f;
    const float minHeightChange = 0.75f;

    const float minInterpolationDuration = 1.7021276595744680851063829787234f;
    const float maxInterpolationDuration = 1.7021276595744680851063829787234f;

    void Start()
    {
        initialHeight = Random.Range(minHeight, maxHeight);
        interploationDuration = Random.Range(minInterpolationDuration, maxInterpolationDuration);

        startInterpolationTime = Time.time;

        transform.localScale = new Vector3(
            transform.localScale.x,
            initialHeight,
            transform.localScale.z);
    }

    private void Update()
    {
        float yScale = initialHeight + moveCurve.Evaluate((Time.time - startInterpolationTime) / interploationDuration) * (targetHeight - initialHeight);
        transform.localScale = new Vector3(
            transform.localScale.x,
            yScale,
            transform.localScale.z);

        if (Time.time > startInterpolationTime + interploationDuration)
        {
            startInterpolationTime = startInterpolationTime + interploationDuration;
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