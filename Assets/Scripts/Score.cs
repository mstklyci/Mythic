using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public bool team1Win, team2Win;

    void Awake()
    {
        team1Win = false;
        team2Win = false;
    }

}