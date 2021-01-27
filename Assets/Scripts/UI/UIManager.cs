using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public float timeInTransition;
    [SerializeField] private float timeMainMenuButton;
    [SerializeField] private float timePlayAgainButton;

    [SerializeField] private GameObject victoryModel;
    [SerializeField] private GameObject defeatModel;
    [SerializeField] private GameObject buttonMainMenu;
    [SerializeField] private GameObject buttonPlayAgain;

    void Awake()
    {
        Managers.ui = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        victoryModel.SetActive(false);
        defeatModel.SetActive(false);
        buttonMainMenu.SetActive(false);
        buttonPlayAgain.SetActive(false);
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

    private void EnableWinButtons(object info)
    {
        GameObject gameObject = (GameObject)info;

        buttonMainMenu.SetActive(true);
        buttonPlayAgain.SetActive(true);

        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            victoryModel.SetActive(true);
        }
        else
        {
            defeatModel.SetActive(true);
        }

        ChangeButtonAlpha(buttonMainMenu,  0);
        ChangeButtonAlpha(buttonPlayAgain, 0);
        StartCoroutine(EnableTransitionButon(buttonMainMenu, timeMainMenuButton));
        StartCoroutine(EnableTransitionButon(buttonPlayAgain, timePlayAgainButton));
    }

    IEnumerator EnableTransitionButon(GameObject button, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeButtonAlpha(button, 0);
        float timer = 0.0f;
        while (timer < timeInTransition)
        {
            timer += Time.deltaTime;
            float alpha = timer / timeInTransition;
            ChangeButtonAlpha(button, alpha);
            yield return null;
        }
    }

    public void ChangeButtonAlpha(GameObject button, float alpha)
    {
        var colors = button.GetComponent<Button>().colors;

        //Set alpha for all states of the buttons
        colors.normalColor = new Color(colors.normalColor.r, colors.normalColor.r, colors.normalColor.r, alpha);
        colors.highlightedColor = new Color(colors.highlightedColor.r, colors.highlightedColor.r, colors.highlightedColor.r, alpha);
        colors.pressedColor = new Color(colors.pressedColor.r, colors.pressedColor.r, colors.pressedColor.r, alpha);
        colors.selectedColor = new Color(colors.selectedColor.r, colors.selectedColor.r, colors.selectedColor.r, alpha);
        colors.disabledColor = new Color(colors.disabledColor.r, colors.disabledColor.r, colors.disabledColor.r, alpha);

        button.GetComponent<Button>().colors = colors;

        ChangeTextAlpha(button.transform.GetChild(0).GetComponent<Text>(), alpha);

    }

    public void ChangeTextAlpha(Text text, float alpha)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }


}