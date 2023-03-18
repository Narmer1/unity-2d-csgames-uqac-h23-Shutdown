using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountNbOfEnemies : MonoBehaviour
{
    //Ce script sert uniquement � compter le nombre d'ennemies pr�sents dans le jeu
    [SerializeField] private GameObject parentOfAllEnnemies;

    void Start()
    {
        int total = transform.childCount;
        GameState.nbEnnemis = total;
        Debug.Log("Total ennemies = " + total);
    }
}
