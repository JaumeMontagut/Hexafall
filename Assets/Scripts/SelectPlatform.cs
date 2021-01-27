using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MyEvents;

public class SelectPlatform : MonoBehaviour
{
    private PlayerVars playerVars;
    private PlayerMove playerMove;
    private PhotonView photonView;
    
    private GameObject selectedPlatform;
    Color standardColor = Color.blue;//TODO: Get from the preafab before OnEnable is called (start doesn't work)
    Color selectedColor = Color.yellow;

    public GameObject SelectedPlatform
    {
        set {
            if (selectedPlatform != null)
            {
                selectedPlatform.GetComponent<Renderer>().material.SetColor("_EmissionColor", standardColor);
            }
            selectedPlatform = value;
            selectedPlatform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            selectedPlatform.GetComponent<Renderer>().material.SetColor("_EmissionColor", selectedColor);
        }
        get
        {
            return selectedPlatform;
        }
    }

    void Start()
    {
        playerVars = GetComponent<PlayerVars>();
        playerMove = GetComponent<PlayerMove>();
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 100f, LayerMask.GetMask("Platform")))
            {
                List<HexagonalTile> neighbors = playerVars.currentPlatform.GetNeighbors();
                foreach (HexagonalTile neighbor in neighbors)
                {
                    if (neighbor.gameObject == hitInfo.transform.gameObject)
                    {
                        SelectedPlatform = hitInfo.transform.gameObject;
                        break;
                    }
                }
            }
        }
    }

    private void SelectStartingPlatform(object info)
    {
        List<HexagonalTile> neighbours = Managers.Tiles.start.GetNeighbors();
        SelectedPlatform = neighbours[Random.Range(0, neighbours.Count)].gameObject;
    }

    private void OnEnable()
    {
        EventManager.StartListening(MyEventType.StateWaitingEnter, SelectStartingPlatform);
    }

    private void OnDisable()
    {
        EventManager.StopListening(MyEventType.StateWaitingEnter, SelectStartingPlatform);
    }
}
