using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaSpeakers : MonoBehaviour
{
    public AnimationCurve speakerCurve;

    float animationStartTime = 0f;
    float animationDuration = 0.25f;

    float intialScale;

    float miniBeatScale;
    float beatScale;

    private void Start()
    {
        intialScale = transform.localScale.x;
        beatScale = intialScale + 2.5f;
        miniBeatScale = intialScale + 1.25f;
    }

    public void DoAnimation()
    {
        animationStartTime = Time.time;
        StartCoroutine(AnimationCorutine());
        StartCoroutine(Minibeat());
    }

    IEnumerator AnimationCorutine()
    {
        while (Time.time < animationStartTime + animationDuration)
        {
            float currTimePercent = (Time.time - animationStartTime) / animationDuration;
            float scale = intialScale + speakerCurve.Evaluate(currTimePercent) * (beatScale - intialScale);
            transform.localScale = new Vector3(
                scale,
                transform.localScale.y,
                scale);
            yield return null;
        }
        yield break;
    }

    IEnumerator Minibeat()
    {
        yield return new WaitForSeconds(Managers.Turn.turnDuration / 2f);
        while (Time.time < animationStartTime + animationDuration)
        {
            float currTimePercent = (Time.time - animationStartTime) / animationDuration;
            float scale = intialScale + speakerCurve.Evaluate(currTimePercent) * (miniBeatScale - intialScale);
            transform.localScale = new Vector3(
                scale,
                transform.localScale.y,
                scale);
            yield return null;
        }
        yield break;
    }
}
