using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm {
    public List<Agent> agents = new List<Agent>();

    public Swarm(int boundary, GameObject agentPrefab)
    {
        for (int i = 0; i < 15; i++)
        {
            Agent agent = GameObject.Instantiate(agentPrefab).GetComponent<Agent>();
            agent.Initialize((i > 12), boundary);
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
