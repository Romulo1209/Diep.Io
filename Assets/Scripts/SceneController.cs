using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    public TMP_Text ScoreText;
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void SetScore(int score)
    {
        ScoreText.text = "GAME OVER \nYour Score is " + score + "\nTAP TO RESTART!";
    }
}
