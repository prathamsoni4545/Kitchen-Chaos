using UnityEditor.Search;
using UnityEngine;

public class SelectCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;



    private void Start()
    {
        Player.Instance.OnSelectCounterChanged += Player_OnSelectCounterChanged;
    }

    private void Player_OnSelectCounterChanged(object sender, Player.OnSelectCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
     private void Show()
    {
        foreach (GameObject visualGameObect in visualGameObjectArray)
        {
            visualGameObect.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObect in visualGameObjectArray)
        {
             visualGameObect.SetActive(false);
        }
    }
}
