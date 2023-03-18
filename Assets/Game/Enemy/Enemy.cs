using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //On permet de set les points à parcourir depuis l'éditeur
    [SerializeField] private Transform[] _pointsToGo;
    [SerializeField] private float speed = 2f;

    private int _destPoint = 0;
    private float _waitTime;

    void Awake()
    {
    }


    // Start is called before the first frame update
    void Start()
    {

        //On attends deux secondes avant que l'ennemi ne reparte
        _waitTime = 5f;

        //Puis on set le spot
        _destPoint = 0;
    }



    // Update is called once per frame
    void Update()
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
