using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public Canvas uiScreenIntro;
    public Canvas uiScreenTitle;
    public Canvas uiScreenGame;
    public TextMeshProUGUI uiScreenGameDetails;
    public TextMeshProUGUI uiScreenTitleHighscores;
    public Image uiScreenOutro;
    public Transform gameBackground;

    public float gameTimeremaining;
    public static int gameScore;
    public Transform bodypartFolder;

    public GameObject claw;
    public Hooks hooks;
    public Spawner spawner;
    Belt[] belts;
    CogRotation[] cogs;

    public AudioSource audioSourceSoundtrack;

    public bool gameAccessible;
    private int[] gameScoreRecords;

    // Start is called before the first frame update
    void Start() {
        belts = FindObjectsOfType<Belt>();
        cogs = FindObjectsOfType<CogRotation>();
        gameScoreRecords = new int[3];
    }

    // Update is called once per frame
    void Update() {
        // Intro skip
        if (Input.GetKeyDown("space")) {
            GetComponent<Animation>().Stop();
            uiScreenIntro.gameObject.SetActive(false);
        }

        // Scale background plane to 
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Screen.width / Screen.height;
        gameBackground.localScale = new Vector3(width / 10.0f, 1, height / 10.0f);

        // Default cog speed
        foreach (CogRotation cogRotation in cogs)
            cogRotation.rotateSpeed = 20.0f;

        // When main menu is showing
        if (uiScreenTitle.gameObject.activeInHierarchy) {
            string highscores = "1) " + gameScoreRecords[0].ToString() + "\n2) " +
                gameScoreRecords[1].ToString() + "\n3) " + gameScoreRecords[2].ToString();
            uiScreenTitleHighscores.text = highscores;
            Cursor.visible = true;
        }

        // When game UI is showing
        if (uiScreenGame.gameObject.activeInHierarchy) {
            Cursor.visible = false;
            gameTimeremaining -= Time.deltaTime;
            // CORE GAME LOOP
            if (gameTimeremaining > 0) {
                hooks.spawnWait = 3.0f + (17.0f * (gameTimeremaining / 120.0f));
                hooks.speed = 2.5f - (1.9f * (gameTimeremaining / 120.0f));
                spawner.spawnWait = 1.0f + (1.0f * (gameTimeremaining / 120.0f));
                foreach (Belt belt in belts)
                    belt.speed = 7.0f - (gameTimeremaining / 20.0f);
                foreach (CogRotation cogRotation in cogs)
                    cogRotation.rotateSpeed = 600.0f - (300.0f * (gameTimeremaining / 120.0f));

                if (!gameAccessible) 
                    audioSourceSoundtrack.pitch = 1.5f - Mathf.Clamp((2f * (gameTimeremaining / 120.0f)), 0.0f, 0.5f);

                uiScreenGameDetails.text = "ETA until Earth: " +
                    Mathf.RoundToInt(gameTimeremaining).ToString() + " seconds";//\nScore: " + gameScore.ToString();
            }
            // When game has finished
            else {
                // Is the outro image completely showing
                if (uiScreenOutro.color.a == 1) {
                    // Reset game
                    claw.SetActive(false);
                    spawner.gameObject.SetActive(false);
                    foreach (Transform template in hooks.transform)
                        Destroy(template.gameObject);
                    foreach (Transform bodypart in bodypartFolder)
                        Destroy(bodypart.gameObject);
                    hooks.gameObject.SetActive(false);
                    audioSourceSoundtrack.pitch = 1.0f;
                    uiScreenTitle.gameObject.SetActive(true);
                }
                // Reset game UI when it's animation is done
                if (!uiScreenGame.GetComponent<Animation>().isPlaying)
                    uiScreenGame.gameObject.SetActive(false);
            }
        }
    }

    public void UIStart() {
        uiScreenTitle.gameObject.SetActive(false);
        uiScreenGame.gameObject.SetActive(true);

        claw.GetComponent<Claw>().Move();
        claw.SetActive(true);
        spawner.gameObject.SetActive(true);
        hooks.gameObject.SetActive(true);

        gameTimeremaining = 120.0f;
    }

    public void SetAccessability(bool value) {
        gameAccessible = value;
    }
}
 