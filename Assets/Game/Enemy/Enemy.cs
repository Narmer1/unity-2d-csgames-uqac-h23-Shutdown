using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //On permet de set les points à parcourir depuis l'éditeur
    [SerializeField] private Transform[] _pointsToGo;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedDetection = 2f;
    [SerializeField] private GameObject player;

    private int _destPoint = 0;
    private float _waitTime;
    private bool _isPlayerAround = false;
    private Vector3 _lastKnowPosition;
    private float _timeLeftToSearch = 0f;
    private Vector3 _lastDirectionOfPlayer = Vector3.zero;

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
            //Set des variables
            _lastDirectionOfPlayer = Vector3.zero;
            _lastKnowPosition = player.transform.position;

            //On pense à reset le compteur et la variable qui regarde où va le joueur
            _timeLeftToSearch = 5f;
            _isPlayerAround = false;

        }

    }


    void Update()
    {
        //Si le player n'est pas détecté, on continue la routine
        if(!_isPlayerAround){ 

            //S'il reste du temps pour chercher le player, on doit le chercher
            if(_timeLeftToSearch > 0)
            {
                //On dirige l'IA dans la direction générale du player, mais s'il ne se fait pas repérer, l'IA abandonne au bout de 5sec
                _lastDirectionOfPlayer = player.transform.position - _lastKnowPosition;

                //Ensuite on la fait avancer
                _lastKnowPosition = _lastKnowPosition + _lastDirectionOfPlayer;
                transform.position = Vector3.MoveTowards(transform.position, _lastKnowPosition, speed * Time.deltaTime);

                //Et on décrémente le compteur
                _timeLeftToSearch -= Time.deltaTime;
            }

            else
            {
                //On fait avancer l'IA jusqu'à sa destination (le point de sa routine)
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
            //On considère que l'IA va plus vite quand elle repere le joueur !
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speedDetection * Time.deltaTime);

        }

    }
}
