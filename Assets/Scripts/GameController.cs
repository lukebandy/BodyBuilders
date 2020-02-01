using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public Canvas uiScreenTitle;
    public Canvas uiScreenGame;
    public TextMeshProUGUI uiScreenGameDetails;

    public float gameTimeremaining;
    public static int gameScore;

    public GameObject claw;
    public Hooks hooks;
    public Spawner spawner;
    Belt[] belts;

    // Start is called before the first frame update
    void Start() {
        belts = FindObjectsOfType<Belt>();
    }

    // Update is called once per frame
    void Update() {
        if (uiScreenGame.gameObject.activeInHierarchy) {
            gameTimeremaining -= Time.deltaTime;
            // CORE GAME LOOP
            if (gameTimeremaining > 0) {
                hooks.spawnWait = 3.0f + (17.0f * (gameTimeremaining / 120.0f));
                hooks.speed = 2.5f - (1.9f * (gameTimeremaining / 120.0f));
                spawner.spawnWait = 1.0f + (1.0f * (gameTimeremaining / 120.0f));
                foreach (Belt belt in belts)
                    belt.speed = 7.0f - (gameTimeremaining / 20.0f);

                uiScreenGameDetails.text = "ETA until Earth: " + 
                    Mathf.RoundToInt(gameTimeremaining).ToString() + " seconds\nScore: " + gameScore.ToString();
            }
            else {
                uiScreenTitle.gameObject.SetActive(true);
                uiScreenGame.gameObject.SetActive(false);

                claw.SetActive(false);
                spawner.gameObject.SetActive(false);
            }
        }
    }

    public void UIStart() {
        uiScreenTitle.gameObject.SetActive(false);
        uiScreenGame.gameObject.SetActive(true);

        claw.SetActive(true);
        spawner.gameObject.SetActive(true);
        hooks.gameObject.SetActive(true);

        gameTimeremaining = 120.0f;
    }
}
 