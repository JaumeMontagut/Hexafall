using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float timeInTransition;
    [SerializeField] private float timeMainMenuButton;
    [SerializeField] private float timePlayAgainButton;

    private GameObject CanvasResult;
    void Awake()
    {
        Managers.ui = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CanvasResult = GameObject.Find("CanvasResult");
        CanvasResult.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        EventManager.StartListening(MyEventType.PlayerReachGoal, EnableWinButtons);
    }

    private void OnDisable()
    {
        EventManager.StopListening(MyEventType.PlayerReachGoal, EnableWinButtons);
    }

    private void EnableWinButtons(dynamic info)
    {
        CanvasResult.SetActive(true);


        GameObject ButtonMainMenu = GameObject.Find("ButtonMainMenu");
        ChangeButtonColor(ButtonMainMenu, new Color(ButtonMainMenu.GetComponent<Button>().colors.normalColor.r, ButtonMainMenu.GetComponent<Button>().colors.normalColor.r, ButtonMainMenu.GetComponent<Button>().colors.normalColor.r, 0));
        
        GameObject ButtonPlayAgain = GameObject.Find("ButtonPlayAgain");
        ChangeButtonColor(ButtonPlayAgain, new Color(ButtonPlayAgain.GetComponent<Button>().colors.normalColor.r, ButtonPlayAgain.GetComponent<Button>().colors.normalColor.r, ButtonPlayAgain.GetComponent<Button>().colors.normalColor.r, 0));


        StartCoroutine(EnableTransitionButon(ButtonMainMenu, timeMainMenuButton));
        StartCoroutine(EnableTransitionButon(ButtonPlayAgain, timePlayAgainButton));


    }

    IEnumerator EnableTransitionButon(GameObject button, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeButtonColor( button, new Color(button.GetComponent<Button>().colors.normalColor.r, button.GetComponent<Button>().colors.normalColor.r, button.GetComponent<Button>().colors.normalColor.r, 0));
        float timer = 0.0f;
        while (timer < timeInTransition)
        {
            timer += Time.deltaTime;
            float alpha = timer / timeInTransition;
            ChangeButtonColor(button, new Color(button.GetComponent<Button>().colors.normalColor.r, button.GetComponent<Button>().colors.normalColor.r, button.GetComponent<Button>().colors.normalColor.r, alpha));
            yield return null;

        }
    }

    public void ChangeButtonColor(GameObject button, Color color)
    {
        var colors = button.GetComponent<Button>().colors;
        colors.normalColor = color;
        button.GetComponent<Button>().colors = colors;

        GameObject text = button.transform.GetChild(0).gameObject;
        ChangeTextColor(text, new Color(text.GetComponent<Text>().color.r, text.GetComponent<Text>().color.g, text.GetComponent<Text>().color.b, color.a));
        
    }

    public void ChangeTextColor(GameObject text, Color color)
    {
        text.GetComponent<Text>().color = color;
    }
}
