using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    private static float sight = 50f;
    private static float space = 20f;
    private static float movementSpeed = 50f;
    private static float rotateSpeed = 10f;
    private static float distToBoundary = 10f;

    private Vector3 boundary;
    private Vector3 mySize;

    public float dX;
    public float dZ;

    public bool isZombie;
    public Vector3 position;
    public MeshRenderer meshRenderer;

    private Material zombieMaterial;

    public void Initialize(bool zombie, Material zombieMaterial, Material regularMaterial, GameObject boundary)
    {
        this.boundary = boundary.GetComponent<BoxCollider>().bounds.size;

        position = RandomPointOnPlane(boundary.gameObject);
        transform.position = position;

        isZombie = zombie;
        
        meshRenderer = GetComponent<MeshRenderer>();

        this.zombieMaterial = zombieMaterial;

        if (isZombie)
            meshRenderer.material = zombieMaterial;
        else
            meshRenderer.material = regularMaterial;

        Bounds myBounds = GetComponent<MeshFilter>().mesh.bounds;
        mySize = new Vector3(myBounds.size.x, myBounds.size.y, myBounds.size.z);
    }

    public void Move(List<Agent> agents)
    {
        //Agents flock, zombie's hunt 
        if (!isZombie) Flock(agents, 2.5f, 0.01f, 1f);
        else Hunt(agents);
        CheckBounds();
        CheckSpeed();

        position.x += dX;
        position.z += dZ;

        Vector3 direction = position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        transform.position = position;
    }

    private void Flock(List<Agent> agents, float scaleFactorS, float scaleFactorC, float scaleFactorA)
    {
        // TODO
        /*Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseDistance = Distance(position, mousePosition);

        if (mouseDistance < sight)
        {
            // Evade
            dX += position.x - mousePosition.x;
            dZ += position.z - mousePosition.y;
        }*/

        foreach (Agent a in agents)
        {
            float distance = Distance(position, a.position);
            if (a != this && !a.isZombie)
            {
                if (distance < space)
                {
                    // Separation
                    dX += (position.x - a.position.x) * scaleFactorS;
                    dZ += (position.z - a.position.z) * scaleFactorS;
                }
                else if (distance < sight)
                {
                    // Cohesion
                    dX += (a.position.x - position.x) * scaleFactorC;
                    dZ += (a.position.z - position.z) * scaleFactorC;
                }
                if (distance < sight)
                {
                    // Alignment
                    dX += a.dX * scaleFactorA;
                    dZ += a.dZ * scaleFactorA;
                }
            }
            if (a.isZombie && distance < sight)
            {
                // Evade
                dX += position.x - a.position.x;
                dZ += position.z - a.position.z;
            }
        }
    }

    private void Hunt(List<Agent> agents)
    {
        float range = float.MaxValue;
        Agent prey = null;
        foreach (Agent a in agents)
        {
            if (!a.isZombie)
            {
                float distance = Distance(position, a.position);
                if (distance < sight && distance < range)
                {
                    range = distance;
                    prey = a;
                }
            }
        }
        if (prey != null)
        {
            // Move towards prey.
            dX += prey.position.x - position.x;
            dZ += prey.position.z - position.z;
        }
    }

    private static float Distance(Vector3 p1, Vector3 p2)
    {
        return (p2 - p1).magnitude;
    }

    private void CheckBounds()
    {
        // TODO: improve boundary check
        if (position.x < -boundary.x / 2 + distToBoundary)
            dX += -boundary.x / 2 + distToBoundary - position.x;
        if (position.z < -boundary.z / 2 + distToBoundary)
            dZ += -boundary.z / 2 + distToBoundary - position.z;

        if (position.x > boundary.x / 2 - distToBoundary)
            dX += boundary.x / 2 - distToBoundary - position.x;
        if (position.z > boundary.z / 2 - distToBoundary)
            dZ += boundary.z / 2 - distToBoundary - position.z;
    }

    private void CheckSpeed()
    {
        float s;
        if (!isZombie) s = movementSpeed * Time.deltaTime;
        else s = movementSpeed / 3f * Time.deltaTime; //Zombie's are slower

        float val = Distance(Vector2.zero, new Vector2(dX, dZ));
        if (val > s)
        {
            dX = dX * s / val;
            dZ = dZ * s / val;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        Agent otherAgent = other.gameObject.GetComponent<Agent>();

        if(otherAgent != null)
        {
            // if im not a zombie and the other is, become a zombie
            if (otherAgent.isZombie && !this.isZombie)
                BecomeZombie();
        }
    }

    private void BecomeZombie()
    {
        isZombie = true;
        meshRenderer.material = zombieMaterial;
    }

    private Vector3 RandomPointOnPlane(GameObject plane)
    {
        float randomX = Random.Range(-boundary.x / 2f + distToBoundary, boundary.x / 2f - distToBoundary);
        float randomZ = Random.Range(-boundary.z / 2f + distToBoundary, boundary.z / 2f - distToBoundary);

        return new Vector3(randomX, 0, randomZ);
    }
}
