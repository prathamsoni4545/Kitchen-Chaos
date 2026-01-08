using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += (_, __) =>
        {
            Transform plate = Instantiate(plateVisualPrefab, counterTopPoint);

            float offsetY = 0.1f;
            plate.localPosition =
                new Vector3(0, offsetY * plateVisualGameObjectList.Count, 0);

            plateVisualGameObjectList.Add(plate.gameObject);
        };

        platesCounter.OnPlateRemoved += (_, __) =>
        {
            if (plateVisualGameObjectList.Count == 0) return;

            GameObject plate =
                plateVisualGameObjectList[^1];

            plateVisualGameObjectList.RemoveAt(
                plateVisualGameObjectList.Count - 1);

            Destroy(plate);
        };
    }
}
