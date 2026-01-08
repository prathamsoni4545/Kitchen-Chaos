using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
   private bool isFisrtUpdate = true;

    private void Update()
    {
        if (isFisrtUpdate)
        {
            isFisrtUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
