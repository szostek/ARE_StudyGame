using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public GameObject boxPrefab;
    public int citySize = 100;
    public float boxSpacing = 1.5f;
    public Vector2 scaleRange = new Vector2(0.5f, 3f);
    public float heightRange = 10f;
    public Vector2 positionRange = new Vector2(-50f, 50f);

    private void Start()
    {
        GenerateCity();
    }

    private void GenerateCity()
    {
        for (int i = 0; i < citySize; i++)
        {
            // Instantiate a box
            GameObject newBox = Instantiate(boxPrefab, RandomPosition(), Quaternion.identity);

            // Set the box as a child of the CityGenerator object
            newBox.transform.SetParent(transform);

            // Scale the box randomly
            float randomScale = Random.Range(scaleRange.x, scaleRange.y);
            float randomHeight = Random.Range(0, heightRange);
            newBox.transform.localScale = new Vector3(randomScale, randomHeight, randomScale);

            // Space the box from its neighbors
            newBox.transform.position += new Vector3(boxSpacing, 0, boxSpacing) * randomScale;
        }
    }

    private Vector3 RandomPosition()
    {
        float x = Random.Range(positionRange.x, positionRange.y);
        float z = Random.Range(positionRange.x, positionRange.y);

        return new Vector3(x, 0, z);
    }
}
