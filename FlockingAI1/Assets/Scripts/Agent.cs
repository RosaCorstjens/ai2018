﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    private static float sight = 100f;
    private static float space = 50f;
    private static float movementSpeed = 75f;
    private static float rotateSpeed = 3f;

    private BoxCollider2D boundary;

    public float dX;
    public float dY;

    public bool isZombie;
    public Vector2 position;
    public SpriteRenderer sprRenderer;

    public void Initialize(bool zombie, Sprite zombieSprite, Sprite regularSprite, BoxCollider2D boundary)
    {
        position = new Vector2(Random.Range(boundary.bounds.min.x, boundary.bounds.max.x), Random.Range(boundary.bounds.min.y, boundary.bounds.max.y));

        this.boundary = boundary;

        isZombie = zombie;
        
        sprRenderer = GetComponent<SpriteRenderer>();

        if (isZombie)
            sprRenderer.sprite = zombieSprite;
        else
            sprRenderer.sprite = regularSprite;
    }

    public void Move(List<Agent> agents)
    {
        //Agents flock, zombie's hunt 
        if (!isZombie) Flock(agents, 3.5f, 0.01f, 1f);
        else Hunt(agents);
        CheckBounds();
        CheckSpeed();

        position.x += dX;
        position.y += dY;

        Vector2 direction = (Vector3)position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        transform.position = position;
    }

    private void Flock(List<Agent> agents, float scaleFactorS, float scaleFactorC, float scaleFactorA)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        // TODO: improve boundary check
        if (position.x < boundary.bounds.min.x)
            dX += boundary.bounds.min.x - position.x;
        if (position.y < boundary.bounds.min.y)
            dY += boundary.bounds.min.y - position.y;

        if (position.x > boundary.bounds.max.x)
            dX += boundary.bounds.max.x - position.x;
        if (position.y > boundary.bounds.max.y)
            dY += boundary.bounds.max.y - position.y;
    }

    private void CheckSpeed()
    {
        float s;
        if (!isZombie) s = movementSpeed * Time.deltaTime;
        else s = movementSpeed / 3f * Time.deltaTime; //Zombie's are slower

        float val = Distance(Vector2.zero, new Vector2(dX, dY));
        if (val > s)
        {
            dX = dX * s / val;
            dY = dY * s / val;
        }
    }
}
