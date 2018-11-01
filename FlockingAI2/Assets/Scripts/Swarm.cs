using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm {
    public List<Agent> agents = new List<Agent>();

    public Swarm(int swarmCount, GameObject boundary, Material zombieMaterial, Material regularMaterial, GameObject agentPrefab)
    {
        for (int i = 0; i < swarmCount; i++)
        {
            Agent agent = GameObject.Instantiate(agentPrefab).GetComponent<Agent>();
            agent.Initialize((i > swarmCount * 0.9f), zombieMaterial, regularMaterial, boundary);
            agents.Add(agent);
        }
    }

    public void MoveAgents()
    {
        foreach (Agent a in agents)
        {
            a.Move(agents);
        }
    }
}
