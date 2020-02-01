﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public Canvas uiScreenTitle;
    public Canvas uiScreenGame;
    public TextMeshProUGUI uiScreenGameDetails;
    public TextMeshProUGUI uiScreenTitleHighscores;

    public float gameTimeremaining;
    public static int gameScore;

    public GameObject claw;
    public Hooks hooks;
    public Spawner spawner;
    Belt[] belts;

    AudioSource audioSource;
    bool gameAccessible;
    private int[] gameScoreRecords;

    // Start is called before the first frame update
    void Start() {
        belts = FindObjectsOfType<Belt>();
        gameScoreRecords = new int[3];
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (uiScreenTitle.gameObject.activeInHierarchy) {
            string highscores = "1) " + gameScoreRecords[0].ToString() + "\n2) " +
                gameScoreRecords[1].ToString() + "\n3) " + gameScoreRecords[2].ToString();
            uiScreenTitleHighscores.text = highscores;
            Cursor.visible = true;
        }
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

                if (!gameAccessible) 
                    audioSource.pitch = 1.5f - Mathf.Clamp((2f * (gameTimeremaining / 120.0f)), 0.0f, 0.5f);

                uiScreenGameDetails.text = "ETA until Earth: " + 
                    Mathf.RoundToInt(gameTimeremaining).ToString() + " seconds\nScore: " + gameScore.ToString();
            }
            else {
                // Change high scores
                if (gameScore > gameScoreRecords[0]) {
                    gameScoreRecords[2] = gameScoreRecords[1];
                    gameScoreRecords[1] = gameScoreRecords[0];
                    gameScoreRecords[0] = gameScore;
                }
                else if (gameScore > gameScoreRecords[1]) {
                    gameScoreRecords[2] = gameScoreRecords[1];
                    gameScoreRecords[1] = gameScore;
                }
                else if (gameScore > gameScoreRecords[2]) {
                    gameScoreRecords[2] = gameScore;
                }

                // Change UI
                uiScreenTitle.gameObject.SetActive(true);
                uiScreenGame.gameObject.SetActive(false);

                // Reset game
                claw.SetActive(false);
                spawner.gameObject.SetActive(false);
                foreach (Transform template in hooks.transform )
                    Destroy(template.gameObject);
                audioSource.pitch = 1.0f;
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
 