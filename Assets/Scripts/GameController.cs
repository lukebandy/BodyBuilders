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
    public Transform gameBackground;
    public GameObject outroScene;

    public float gameTimeremaining;
    public static int gameScore;
    public Transform bodypartFolder;
    private float gameTimeLength = 90.0f;

    public Transform outroBodies;

    public GameObject claw;
    public GameObject backgroundEarth;
    public Hooks hooks;
    public Spawner spawner;
    Belt[] belts;
    CogRotation[] cogs;

    public AudioSource audioSourceSoundtrack;
    public AudioSource audioSourceIntro;

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
            audioSourceIntro.volume = 1.0f;
        }

        // Scale background plane to 
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Screen.width / Screen.height;
        gameBackground.localScale = new Vector3((width / 10.0f)/1.8f, (height / 10.0f), 1);
        outroScene.transform.localScale = new Vector3(width / 10.0f, 1, height / 10.0f);

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
                hooks.spawnWait = 3.0f + (17.0f * (gameTimeremaining / gameTimeLength));
                hooks.speed = 2.2f - (1.9f * (gameTimeremaining / gameTimeLength));
                spawner.spawnWait = 0.7f + (0.8f * (gameTimeremaining / gameTimeLength));
                foreach (Belt belt in belts)
                    belt.speed = 7.0f - (gameTimeremaining / 20.0f);
                foreach (CogRotation cogRotation in cogs)
                    cogRotation.rotateSpeed = 600.0f - (300.0f * (gameTimeremaining / gameTimeLength));
                float earthSize = (1 / ((gameTimeremaining / gameTimeLength) + 0.9f)) - 0.5f;
                backgroundEarth.gameObject.transform.localScale = new Vector3(earthSize, earthSize, earthSize);

                if (!gameAccessible) 
                    audioSourceSoundtrack.pitch = 1.5f - Mathf.Clamp((2f * (gameTimeremaining / gameTimeLength)), 0.0f, 0.5f);

                uiScreenGameDetails.text = "Arriving at Eath in: " +
                    Mathf.RoundToInt(gameTimeremaining).ToString() + " seconds";//\nScore: " + gameScore.ToString();
            }
            // When game has finished
            else {
                // Is the outro image completely showing
                if (outroScene.GetComponent<MeshRenderer>().enabled) {
                    // Reset game
                    claw.SetActive(false);
                    spawner.gameObject.SetActive(false);
                    foreach (Transform template in hooks.transform)
                        Destroy(template.gameObject);
                    foreach (Transform bodypart in bodypartFolder)
                        Destroy(bodypart.gameObject);
                    hooks.gameObject.SetActive(false);
                    audioSourceSoundtrack.pitch = 1.0f;
                    backgroundEarth.gameObject.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
                }
                // Reset game UI when it's animation is done
                if (!outroScene.GetComponent<Animation>().enabled) {
                    uiScreenTitle.gameObject.SetActive(true);
                    outroScene.gameObject.SetActive(false);
                    uiScreenGame.gameObject.SetActive(false);
                }
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

        gameTimeremaining = gameTimeLength;

        outroScene.SetActive(true);
        outroScene.GetComponent<Animation>().enabled = true;

        foreach (Transform placeholder in outroBodies) {
            foreach (Transform child in placeholder)
                Destroy(child.gameObject);
        }
    }

    public void SetAccessability(bool value) {
        gameAccessible = value;
    }

    public void UIQuit() {
        Application.Quit();
    }
}
 