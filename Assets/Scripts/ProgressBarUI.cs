using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        barImage.fillAmount = 0f;
        gameObject.SetActive(false); // 🔥 hidden at start

        hasProgress.OnProgressChanged += (_, e) =>
        {
            gameObject.SetActive(e.showProgress);
            barImage.fillAmount = e.progressNormalized;
        };
    }
}
