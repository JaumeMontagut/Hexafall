using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] adjacentPlatforms;
    public bool isPath = false;
    public bool isStart = false;
    public bool isEnd = false;

    [Range(0.0f, 1.0f)] [SerializeField] private float alphaReducedValue = 0.0f;                //Value which will determinate how much transparent will be the platform when the player falls.
    [SerializeField] private float waitToStartFadingAlpha = 0.0f;                               //Time to wait until the platform start to restore the alpha.
    [SerializeField] private float totalTimeFadingAlpha = 0.0f;                                 //How long is it going to be fading after the wait.
    private float timerToRestoreAlpha = 0.0f;                                                   //The timer since the restore of the alpha started (included the initial wait).
    [HideInInspector] public bool fadingAlpha
    {
        get; private set;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingAlpha)
            RestoreAlphaWithFade();
    }

    public void ReduceAlpha()
    {
        //Reduce the alpha of the platform to the value determinated 
        ChangeAlpha(alphaReducedValue);
    }

    private void ChangeAlpha(float alpha)
    {
        ///Change the alpha of the material of the game object

        //The alpha varies between 0 and 1.
        if (alpha > 1.0f)
            alpha = 1.0f;

        else if (alpha < 0.0f)
            alpha = 0.0f;

        //change alpha
        Color currentColor = gameObject.GetComponent<Renderer>().material.color;
        currentColor.a = alpha;
        gameObject.GetComponent<Renderer>().material.color = currentColor;

    }
    public void StartRestoringAlphaWithFade()
    {
        //This will provoke to start to enter in the RestoreAlphaWithFade and it will end when the alpha is restored automatically.
        fadingAlpha = true;
    }
    private void RestoreAlphaWithFade()
    {
        /// Restore the alpha of the platform with a fade. It might have a wait before the platform start fading.

        //Here we are interpreting that the minimum value posbile it's when the alpha is reduced and the maximum when it's opaque (0.0f it's transparent and 1.0f it's opaque)
        float minAlphaValue = alphaReducedValue;
        float maxAlphaValue = 1.0f;

        timerToRestoreAlpha += Time.deltaTime;

        //Start when the wait it's over.
        if(timerToRestoreAlpha >= waitToStartFadingAlpha)
        {
            float AfterStartFadingTimer = timerToRestoreAlpha - waitToStartFadingAlpha;             //Timer after the wait it's over.
            float fadingRatio = AfterStartFadingTimer * maxAlphaValue / totalTimeFadingAlpha;       //Ratio of the restore process. 0.0f -> minimum value and 1.0f -> max value for ratio.

            if (fadingRatio >= 1.0f)
            {
                //The fade is finished. change the bool to not enter in this function more and reset the timer for the next time.
                fadingAlpha = false;
                timerToRestoreAlpha = 0.0f;
                return;
            }

            float differenceAlpha = maxAlphaValue - minAlphaValue;                                  //The value of how much the alpha is going to increase. Differrence between the maximum value minus the minimum value.
            float incrementAlpha = fadingRatio * differenceAlpha;                                   //The value of how much has to increase the alpha from the minimum value.

            float currentAlpha = minAlphaValue + incrementAlpha;                                    
            ChangeAlpha(currentAlpha);

        }

    }

    private void OnDrawGizmos()
    {
        if (isPath)
        {
            Gizmos.color = Color.yellow;
            Vector3 gizmosPos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            Gizmos.DrawSphere(gizmosPos, 1);
        }
    }
}
