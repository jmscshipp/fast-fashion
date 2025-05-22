// thanks Brackeys!
// https://www.youtube.com/watch?app=desktop&v=bG0uEXV6aHQ&ab_channel=Brackeys
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    [SerializeField]
    private int width = 16;
    [SerializeField]
    private int height = 16;
    [SerializeField]
    private float scale = 1.0f;
    [SerializeField]
    private float offsetY = 0f;
    [SerializeField]
    private float offsetX = 0f;

    [SerializeField]
    private float cutoffValue = 0f;
    private float verticalScrollSpeed = 1f;

    // boundary limits + spawn locations for new persons`
    private float bottomYSpawnValue = 0f;
    private float topYSpawnValue = 9f;
    private float leftXSpawnValue = 0f;
    private float rightXSpawnValue = 9f;


    [SerializeField]
    private GameObject personPrefab;
    private List<Person> people = new List<Person>();

    [SerializeField]
    private float lowerBoundary = 0f;

    [SerializeField]
    private Renderer texRenderer;
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                people.Add(CreateNewPerson(i, j));
            }
        }
    }

    void Update()
    {
        // live noise representation
        texRenderer.material.mainTexture = GenerateTexture();

        if (Input.GetKey(KeyCode.RightArrow))
        {
            offsetX += 10f * Time.deltaTime;
            MovePeople(new Vector3(-10f * Time.deltaTime, 0f, 0f));

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            offsetX -= 10f * Time.deltaTime;
            MovePeople(new Vector3(10f * Time.deltaTime, 0f, 0f));

        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            offsetY += 10f * Time.deltaTime;
            MovePeople(new Vector3(0f, -10f * Time.deltaTime, 0f));

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            offsetY -= 10f * Time.deltaTime;
            MovePeople(new Vector3(0f, 10f * Time.deltaTime, 0f));
        }
    }

    private void MovePeople(Vector3 move)
    {
        List<Person> peopleToAdd = new List<Person>();

        foreach (Person person in people)
        {
            Vector3 newPersonLocation = person.Move(move);
            if (newPersonLocation.x > rightXSpawnValue)
            {
                peopleToAdd.Add(CreateNewPerson(leftXSpawnValue, person.transform.position.y));
                person.Kill();
            }
            else if (newPersonLocation.x < leftXSpawnValue)
            {
                peopleToAdd.Add(CreateNewPerson(rightXSpawnValue, person.transform.position.y));
                person.Kill();
            }
            else if (newPersonLocation.y > topYSpawnValue)
            {
                peopleToAdd.Add(CreateNewPerson(person.transform.position.x, bottomYSpawnValue));
                person.Kill();
            }
            else if (newPersonLocation.y < bottomYSpawnValue)
            {
                peopleToAdd.Add(CreateNewPerson(person.transform.position.x, topYSpawnValue));
                person.Kill();
            }
        }

        // update people list based on additions and subtractions from movement
        people.RemoveAll(x => x.markedForRemoval);
        people.AddRange(peopleToAdd);
    }

    private Person CreateNewPerson(float x, float y)
    {
        Person newPerson = Instantiate(personPrefab, new Vector3(x, y, 0f),
    Quaternion.identity).GetComponent<Person>();
        newPerson.ChangeMatColor(CalculateColor(x * 10f, y * 10f));
        return newPerson;
    }

    private Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    private Color CalculateColor (float x, float y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.Round(Mathf.PerlinNoise(xCoord, yCoord) + cutoffValue);
        return new Color(sample, sample, sample);
    }
}
