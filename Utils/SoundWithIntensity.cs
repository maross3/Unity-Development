public class SoundWithIntensity : MonoBehaviour
{
    public List<SoundWithIntensities> sounds;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public Transform player;
    public Transform goal;

    public float transitionDistance = 10f; 
    public float maxVolume = 1f;

    private void Start()
    {
        UpdateAudio(0);
    }
    
    private void StopAllAudio()
    {
        audioSource1.Stop();
        audioSource1.volume = 0;
        audioSource2.Stop();
        audioSource2.volume = 0;
        audioSource3.Stop();
        audioSource3.volume = 0;
    }
    
    private void StartAllAudio()
    {
        audioSource1.Play();
        audioSource2.Play();
        audioSource3.Play();
    }
    
    private void UpdateAudio(LevelNames index)
    {
        StopAllAudio();
        UpdateAudioClipIntensities((int) index);
        StartAllAudio();
    }

    private void UpdateAudioClipIntensities(int index)
    {
        audioSource1.clip = sounds[index].heatOne;
        audioSource2.clip = sounds[index].heatTwo;
        audioSource3.clip = sounds[index].heatThree;
    }
    private void Update()
    {
        var distanceToGoal = Vector3.Distance(goal.position, player.position);

        if (distanceToGoal - transitionDistance < -3)  
        {
            var transitionFactor = Mathf.InverseLerp(0f, transitionDistance, distanceToGoal);
            
            audioSource1.volume = maxVolume;
            audioSource2.volume = Mathf.Lerp(maxVolume, 0f, transitionFactor);
            audioSource3.volume = Mathf.Lerp(0f, maxVolume, transitionFactor);
        }
        else if (distanceToGoal - transitionDistance > 4 && distanceToGoal - transitionDistance < 6)
        {
            var transitionFactor = Mathf.InverseLerp(0f, transitionDistance, distanceToGoal);

            audioSource1.volume = maxVolume;
            audioSource2.volume = Mathf.Lerp(0f, maxVolume, transitionFactor);
            audioSource3.volume = 0;
        }
        else
        {
            audioSource1.volume = maxVolume;
            audioSource2.volume = 0;
            audioSource3.volume = 0; 
        }
    }
}
