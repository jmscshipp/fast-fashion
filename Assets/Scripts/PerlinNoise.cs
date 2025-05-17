// thanks Brackeys!
// https://www.youtube.com/watch?app=desktop&v=bG0uEXV6aHQ&ab_channel=Brackeys
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
    private float cutoffValue = 0f;
    private float verticalScrollSpeed = 1f;

    [SerializeField]
    private GameObject personPrefab;
    private Person[ , ] people;

    [SerializeField]
    private float lowerBoundary = 0f;
    private void Start()
    {
        people = new Person[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CreateNewPerson(i, j);
            }
        }
    }

    void Update()
    {
        // show live noise representation:
        //for (int i = 0; i < width; i++)
        //{
        //    for (int j = 0; j < height; j++)
        //    {
        //        people[i, j].ChangeMatColor(CalculateColor(i, j));
        //    }
        //}

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                 if (people[i, j].Walk(verticalScrollSpeed) < lowerBoundary)
                {
                    Debug.Log("killing " + i + "," + j);
                    people[i, j].Kill();
                    for (int k = j; k < height - 1; k++)
                    {
                        Debug.Log("switching " + i + "," + k + " and " + i + "," + (k + 1));

                        people[i, k] = people[i, k + 1];
                    }
                    CreateNewPerson(i, height - 1);
                }
            }
        }

        //offsetY += verticalScrollSpeed * Time.deltaTime;
    }

    private void CreateNewPerson(int x, int y)
    {
        Person newPerson = Instantiate(personPrefab, new Vector3(x, y, 0f),
    Quaternion.identity).GetComponent<Person>();
        newPerson.ChangeMatColor(CalculateColor(x, y));
        people[x, y] = newPerson;
    }

    private Color CalculateColor (int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.Round(Mathf.PerlinNoise(xCoord, yCoord) + cutoffValue);
        return new Color(sample, sample, sample);
    }
}
