using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingDead : MonoBehaviour {

    private float timer;
    private Swarm swarm;

    [SerializeField]
    private Sprite regularSprite;

    [SerializeField]
    private Sprite zombieSprite;

    [SerializeField]
    private GameObject agentPrefab;

    public void Start()
    {
        int boundary = 640;

        swarm = new Swarm(boundary, agentPrefab);
        timer = 0;
    }

    public void Update()
    {
        swarm.MoveAgents();
    }
}
