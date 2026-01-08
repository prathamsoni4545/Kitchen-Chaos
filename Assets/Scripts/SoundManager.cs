using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundVolume";
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    public float Volume  = 1f;
    private void Awake()
    {
        Instance = this;

        Volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME,1f);
    }


    private void Start()
    {

        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trashSounds, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDropSounds, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickupSound, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e  )
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.choppingSounds, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e   )
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccessSounds, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFailSounds , deliveryCounter.transform.position); 
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 2f)

    {
        PlaySound( audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);

    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 2f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * Volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstepSounds, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.footstepSounds, Vector3.zero);
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warningSounds, position);
    }

    public void ChangeVolume()
    {
        Volume += .1f;
        if (Volume > 1f)
        {
            Volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, Volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return Volume;
    }

}
