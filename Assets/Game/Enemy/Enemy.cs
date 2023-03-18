using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //On permet de set les points à parcourir depuis l'éditeur
    [SerializeField] private Transform[] _pointsToGo;
    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject player;

    private int _destPoint = 0;
    private float _waitTime;
    private bool _isPlayerAround = false;
    private Vector3 _lastKnowPosition;
    private float _timeLeftToSearch = 0f;

    void Start()
    {

        //On attends deux secondes avant que l'ennemi ne reparte
        _waitTime = 5f;

        //Puis on set le spot
        _destPoint = 0;

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
       if(collider.GetComponent<PlayerController>())
        {
             _isPlayerAround = true;
        }
           
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerController>())
        {
            _lastKnowPosition = player.transform.position;
            _timeLeftToSearch = 3f;
            _isPlayerAround = false;
            Debug.Log("Test pos y : " + (_lastKnowPosition.y / _lastKnowPosition.y));
            Debug.Log("Test pos x : " + (_lastKnowPosition.x / _lastKnowPosition.x));

        }

    }


    void Update()
    {
        //Si le player n'est pas détecté, on continue la routine
        if(!_isPlayerAround){ 

            //S'il reste du temps pour chercher le player, on doit le chercher
            if(_timeLeftToSearch > 0)
            {
                _lastKnowPosition = _lastKnowPosition + Vector3.one;
                transform.position = Vector3.MoveTowards(transform.position, _lastKnowPosition, speed * Time.deltaTime);
                _timeLeftToSearch -= Time.deltaTime;
            }

            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _pointsToGo[_destPoint].position, speed * Time.deltaTime);

                //Si on est très proche de la destination 
                if (Vector3.Distance(transform.position, _pointsToGo[_destPoint].position) < 0.2f)
                {
                    //Et qu'on a bien attendu le temps nécéssaire, on peut donc passer au point suivant
                    if (_waitTime <= 0f)
                    {
                        _destPoint = (_destPoint + 1) % _pointsToGo.Length;
                        _waitTime = 5f;
                    }
                    //Sinon, on doit encore attendre
                    else
                    {
                        _waitTime -= Time.deltaTime;
                    }
                }
            }
           
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        }

    }
}
