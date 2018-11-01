using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingDead : MonoBehaviour {

    private float timer;
    private Swarm swarm;

    [SerializeField]
    public Material regularMaterial;

    [SerializeField]
    public Material zombieMaterial;

    [SerializeField]
    private GameObject agentPrefab;

    [SerializeField]
    private MeshFilter boundary;

    [SerializeField]
    private int swarmCount;

    public void Start()
    {
        swarm = new Swarm(swarmCount, boundary.gameObject, zombieMaterial, regularMaterial, agentPrefab);

        timer = 0;
    }

    public void Update()
    {
        swarm.MoveAgents();
    }
}
