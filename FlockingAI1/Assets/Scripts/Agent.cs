using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    private static float border = 100f;
    private static float sight = 75f;
    private static float space = 30f;
    private static float speed = 50f;

    private float boundary;

    public float dX;
    public float dY;

    public bool isZombie;
    public Vector2 position;
    public SpriteRenderer sprRenderer;

    public void Initialize(bool zombie, int boundary)
    {
        position = new Vector2(Random.Range(0, boundary), Random.Range(0, boundary));
        this.boundary = boundary;
        isZombie = zombie;
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(List<Agent> agents)
    {
        //Agents flock, zombie's hunt 
        if (!isZombie) Flock(agents, 2.5f, 0.01f, 1f);
        else Hunt(agents);
        CheckBounds();
        CheckSpeed();

        //transform.LookAt(position);

        position.x += dX;
        position.y += dY;

        transform.position = position;
    }

    private void Flock(List<Agent> agents, float scaleFactorS, float scaleFactorC, float scaleFactorA)
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        float mouseDistance = Distance(position, mousePosition);

        if (mouseDistance < sight)
        {
            // Evade
            dX += position.x - mousePosition.x;
            dY += position.y - mousePosition.y;
        }

        foreach (Agent a in agents)
        {
            float distance = Distance(position, a.position);
            if (a != this && !a.isZombie)
            {
                if (distance < space)
                {
                    // Separation
                    dX += (position.x - a.position.x) * scaleFactorS;
                    dY += (position.y - a.position.y) * scaleFactorS;
                }
                else if (distance < sight)
                {
                    // Cohesion
                    dX += (a.position.x - position.x) * scaleFactorC;
                    dY += (a.position.y - position.y) * scaleFactorC;
                }
                if (distance < sight)
                {
                    // Alignment
                    dX += a.dX * scaleFactorA;
                    dY += a.dY * scaleFactorA;
                }
            }
            if (a.isZombie && distance < sight)
            {
                // Evade
                dX += position.x - a.position.x;
                dY += position.y - a.position.y;
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
            dY += prey.position.y - position.y;
        }
    }

    private static float Distance(Vector2 p1, Vector2 p2)
    {
        float val = Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y, 2);
        return Mathf.Sqrt(val);
    }

    private void CheckBounds()
    {
        float val = boundary - border;
        if (position.x < border) dX += border - position.x;
        if (position.y < border) dY += border - position.y;
        if (position.x > val) dX += val - position.x;
        if (position.y > val) dY += val - position.y;
    }

    private void CheckSpeed()
    {
        float s;
        if (!isZombie) s = speed * Time.deltaTime;
        else s = speed / 6f * Time.deltaTime; //Zombie's are slower

        float val = Distance(Vector2.zero, new Vector2(dX, dY));
        if (val > s)
        {
            dX = dX * s / val;
            dY = dY * s / val;
        }
    }
}
