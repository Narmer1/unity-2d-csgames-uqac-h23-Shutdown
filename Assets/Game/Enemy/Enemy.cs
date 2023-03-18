using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //On permet de set certaines des variables depuis l'éditeur
    [SerializeField] private Transform[] _pointsToGo;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedDetection = 2f;
    [SerializeField] private GameObject player;

    private int _destPoint = 0;
    private float _waitTime;
    private Vector3 _lastKnowPosition;
    private Vector3 _lastDirectionOfPlayer = Vector3.zero;

    private bool hasCollidedWithSomething = false;

    //private bool _isPlayerAround = false;
    //private float _timeLeftToSearch = 0f;

    void Start()
    {
        //On set à cinq secondes le temps avant que l'ennemi ne reparte de chaque points de patrouille
        _waitTime = 5f;

        //Puis on set le point de patrouille
        _destPoint = 0;

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //Si l'ennemi rencontre quelque chose, et que ce quelque chose est le player, on passe en mode détection
       if(collider.GetComponent<PlayerController>())
        {
            GameState.isPlayerSpotted = true;
            Debug.Log("Direct° : " + (player.transform.position - transform.position));
        }
           
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        //Si le player n'est plus dans la zone de détection
        if (collider.GetComponent<PlayerController>())
        {
            //Set des variables
            _lastDirectionOfPlayer = Vector3.zero;
            _lastKnowPosition = player.transform.position;

            //On pense à reset le compteur et la variable qui regarde où va le joueur
            GameState.timerOfSearch = 5f * GameState.nbEnnemis;
            GameState.isPlayerSpotted = false;

        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //Si ce n'est pas le player, on set le bool de collision
        if (!collision.gameObject.GetComponent<PlayerController>())
        {
            hasCollidedWithSomething = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //Si ce n'est pas le player, on set le bool de collision
        if (!collision.gameObject.GetComponent<PlayerController>())
        {
            hasCollidedWithSomething = false;
        }
    }


    void Update()
    {
        //Si le player n'est pas détecté, on continue la routine
        if(!GameState.isPlayerSpotted)
        { 
            //S'il reste du temps pour chercher le player, on doit le chercher
            if(GameState.timerOfSearch > 0)
            {
                //On dirige l'IA dans la direction générale du player, mais s'il ne se fait pas repérer, l'IA abandonne au bout de 5sec
                _lastDirectionOfPlayer = player.transform.position - _lastKnowPosition;

                //Ensuite on la fait avancer
                _lastKnowPosition = _lastKnowPosition + _lastDirectionOfPlayer;
                transform.position = Vector3.MoveTowards(transform.position, _lastKnowPosition, speed * Time.deltaTime);

                //Et on décrémente le compteur
                GameState.timerOfSearch -= Time.deltaTime;
            }


            //Si le temps est écoulé, la routine reprend
            else
            {
                //On fait avancer l'IA jusqu'à sa destination (le point de sa routine)
                transform.position = Vector3.MoveTowards(transform.position, _pointsToGo[_destPoint].position, speed * Time.deltaTime);

                //Si on est très proche de la destination 
                if (Vector3.Distance(transform.position, _pointsToGo[_destPoint].position) <= 0.1f)
                {
                    //Et qu'on a bien attendu le temps nécéssaire, on peut donc passer au point suivant
                    if (_waitTime <= 0f)
                    {
                        _destPoint = (_destPoint + 1) % _pointsToGo.Length;
                        _waitTime = 5f;
                    }
                    //Sinon, on doit encore attendre avant de changer de destination
                    else
                    {
                        _waitTime -= Time.deltaTime;
                    }
                }
            }
           
        }


        else
        {
            if(hasCollidedWithSomething)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position + Vector3.back, speedDetection * Time.deltaTime);
            }
            else
            {
                //On considère que l'IA va plus vite quand elle repere le joueur !
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speedDetection * Time.deltaTime);
            }
        }

    }
}
