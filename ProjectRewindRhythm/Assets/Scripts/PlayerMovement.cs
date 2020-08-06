﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    public int jumpIndex;
    [Header("Lose & Win Stuff")]
    public GameObject gameOver1;
    public GameObject gameOver2;
    public GameObject gameOver3;
    public GameObject gameOver4;
    public GameObject win1;
    public GameObject win2;
    public GameObject win3;
    public GameObject win4;
    public GameObject win5;
    public GameObject win6;
    public GameObject win7;
    public GameObject win8;
    public GameObject win9;
    public GameObject win10;
    public GameObject comboTitle; 
    [Header("Strikes")]
    public GameObject strike1;
    public GameObject strike2;
    public GameObject strike3;
    [Header("Jump Locations")]
    public Vector3[] jumpPoints;
    [Header("Jump Forces")]
    public int[] jumpForces; 
    [Header("Speed Changes")]
    public float[] speedChanges;
    //[Header("Bool CAtch")]
    //public bool[] boolCatch;
    [Header("Other Variables")]
    public Text comboText;
    private AudioSource source;
    private GameObject otherSource; 
    public AudioClip landSFX;
    public AudioClip strikeSFX;
    public AudioClip loseSFX;
    public AudioClip comboSFX; 
    public float curSpeed;
    private bool tripped;
    private bool pressedButton;
    public float generosity;
    public int strikes;
    public int strikesAllowed;
    private bool allowedToJump;
    public bool gameStarted;
    public float startDelay;
    //public float veloc;
    public float landingTime;
    public bool allowedToCheck;
    public bool allowedToChangeCheck;
    private bool justJumped;
    private bool onlyLogOnce;
    public int combo;
    public int comboHeal;
    private bool playedComboSFX;
    public int highestCombo; 
    public int score;
    private bool gameWon;
    public int totalJumpsMade;
    public int amountToIncreaseRegularly; 

    void Start()
    {
        onlyLogOnce = false; 
        gameStarted = false;
        strike1.SetActive(false);
        strike2.SetActive(false);
        strike3.SetActive(false);
        gameOver1.SetActive(false);
        gameOver2.SetActive(false);
        gameOver3.SetActive(false);
        gameOver4.SetActive(false);
        win1.SetActive(false);
        win2.SetActive(false);
        win3.SetActive(false);
        win4.SetActive(false);
        win5.SetActive(false);
        win6.SetActive(false);
        win7.SetActive(false);
        win8.SetActive(false);
        win9.SetActive(false);
        win10.SetActive(false);
        playedComboSFX = false; 
        source = GetComponent<AudioSource>();
        otherSource = GameObject.Find("AudioController"); 
        playerRb = GetComponent<Rigidbody>();
        jumpIndex = 0;
        amountToIncreaseRegularly = 10; 
        pressedButton = false;
        allowedToJump = true;
        curSpeed = speedChanges[0];
        Invoke("StartLevel", startDelay);
        InvokeRepeating("RaiseScoreGradually", startDelay, 0.25f);
    }

    void Update()
    {
        comboText.text = "" + combo;

        if (combo > highestCombo)
        {
            highestCombo = combo; 
        }

        if (combo < 10)
        {
            comboText.color = Color.white; 
            comboText.fontSize = 85; 
        }
        else if (combo >= 10 && combo < 20)
        {
            comboText.color = new Color(0.50f, 1, 0.50f, 1);
            comboText.fontSize = 100;
        }
        else if (combo >= 20 && combo < 30)
        {
            comboText.color = new Color(0.25f, 1, 0.25f, 1);
            comboText.fontSize = 115;
        }
        else if (combo >= 30 && combo < 35)
        {
            comboText.color = new Color(0, 1, 0, 1);
            comboText.fontSize = 130;
        }
        else if (combo == 34)
        {
            comboText.color = new Color(1, 1, 0, 1);
            comboText.fontSize = 130;
        }

        if (!playedComboSFX && (combo == 10 || combo == 20 || combo == 30)) 
        {
            source.PlayOneShot(comboSFX, 2);
            playedComboSFX = true; 
        }

        if (combo == 11 || combo == 21 || combo == 31)
        {
            playedComboSFX = false; 
        }

        if ( strikes > 0 && comboHeal == 10)
        {
            strikes--;
            comboHeal = 0;
        }

        if (playerRb.velocity.y < -2 && allowedToChangeCheck && strikes < 3)
        {
            allowedToCheck = true;
            allowedToChangeCheck = false; 
        }
        
        if (playerRb.velocity.y == 0)
        {
            curSpeed = speedChanges[jumpIndex];
        }

        if (strikes == 0)
        {
            strike1.SetActive(false);
            strike2.SetActive(false);
            strike3.SetActive(false);
        }

        if (strikes == 1)
        {
            strike1.SetActive(true);
            strike2.SetActive(false);
            strike3.SetActive(false);
        }

        if (strikes == 2)
        {
            strike1.SetActive(true);
            strike2.SetActive(true);
            strike3.SetActive(false);
        }

        if (strikes == 3)
        {
            strike1.SetActive(true);
            strike2.SetActive(true);
            strike3.SetActive(true);
            Destroy(otherSource); 
            source.PlayOneShot(loseSFX, 0.2f);
            gameOver1.SetActive(true);
            gameOver2.SetActive(true);
            gameOver3.SetActive(true);
            gameOver4.SetActive(true);
        }

        if (gameStarted)
        {
            if (strikes >= strikesAllowed && !onlyLogOnce)
            {
                tripped = true;
                Debug.Log("<color=red>3 strikes, you're out!</color>");
                onlyLogOnce = true; 
            }

            if (!tripped)
            {
                transform.Translate(Vector3.left * Time.deltaTime * curSpeed);
            }

            if (jumpIndex < jumpPoints.Length && transform.position.x < jumpPoints[jumpIndex].x + 0.1f && strikes < 3 && !justJumped && transform.position.y <= jumpPoints[jumpIndex].y)
            {
                Debug.Log("<color=purple>Doing jump </color>" + jumpIndex + "<color=purple> because player is at </color>" + transform.position + "<color=purple> with </color>" + jumpForces[jumpIndex] + "<color=purple> force, running at </color>" + speedChanges[jumpIndex + 1] + "<color=purple> speed </color>");
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                playerRb.AddForce(Vector3.up * jumpForces[jumpIndex]);
                allowedToJump = false;
                justJumped = true; 
            }

            if (allowedToCheck)
            {
                if (jumpIndex <= jumpPoints.Length && transform.position.y <= jumpPoints[jumpIndex + 1].y + generosity && transform.position.y > jumpPoints[jumpIndex + 1].y  && justJumped && Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("<color=green>Spacebar pressed! Can sucessfully land.</color>");
                    source.PlayOneShot(landSFX, 3); 
                    Debug.Log("<color=magenta> incrimenting jumpIndex from " + jumpIndex + " to " + (jumpIndex + 1) + "</color>");
                    jumpIndex++;
                    allowedToJump = true;
                    allowedToCheck = false;
                    allowedToChangeCheck = false;
                    justJumped = false;
                    combo++;
                    comboHeal++;
                    totalJumpsMade++; 
                    score += 1000; 
                    Invoke("DelayedAllowToChangeCheck", landingTime);
                }
                else if (transform.position.y <= jumpPoints[jumpIndex + 1].y && justJumped)
                {
                    Debug.Log("<color=orange>Spacebar not pressed. Player gets a strike.</color>");
                    source.PlayOneShot(strikeSFX, 3);
                    strikes++;
                    Debug.Log("<color=magenta> incrimenting jumpIndex from " + jumpIndex + " to " + (jumpIndex + 1) + "</color>");
                    jumpIndex++;
                    allowedToJump = true;
                    allowedToCheck = false;
                    allowedToChangeCheck = false;
                    justJumped = false;
                    combo = 0;
                    comboHeal = 0;
                    score -= 1000;
                    Invoke("DelayedAllowToChangeCheck", landingTime); 
                }
            }
        }

        if (transform.position.x < -226)
        {
            gameWon = true; 
        }

        if (gameWon)
        {
            amountToIncreaseRegularly = 0;
            if (transform.position.x > -233)
            {
                transform.Translate(Vector3.left * Time.deltaTime * 8);
            }
            else
            {
                win1.SetActive(true);
                win2.SetActive(true);
                win3.SetActive(true);
                win4.SetActive(true);
                win5.SetActive(true);
                win6.SetActive(true);
                win6.GetComponent<Text>().text = "" + highestCombo;
                win7.SetActive(true);
                win8.SetActive(true);
                win8.GetComponent<Text>().text = "" + totalJumpsMade + "/34";
                win9.SetActive(true);
                win10.SetActive(true);
                win10.GetComponent<Text>().text = "" + score;

                strike1.SetActive(false);
                strike2.SetActive(false);
                strike3.SetActive(false);
                comboTitle.SetActive(false);
                comboText.gameObject.SetActive(false);
            }

            GameObject.Find("Main Camera").GetComponent<FollowPlayer>().following = false; 
        }
    }  

    void RaiseScoreGradually()
    {
        score += amountToIncreaseRegularly; 
    }

    void DelayedChangeVals()
    {
        curSpeed = speedChanges[jumpIndex];
    }

    void StartLevel()
    {
        gameStarted = true;
    }

    void DelayedAllowToChangeCheck()
    {
        allowedToChangeCheck = true; 
    }
}
