using UnityEngine;

public class AudioSourceDestroyer : MonoBehaviour
{
    public bool autoDestroy = true;
    public bool destroySelfOnCompletion = true;
    public bool destroyObjectOnCompletion = false;

    [Header("")]
    public AudioSource source;
    
    private bool destructionQueued = false;

    public void QueueDestruction()
    {
        destructionQueued = true;
    }

    private void Update()
    {
        if (source == null)
        {
            Debug.Log($"{this} source = null");

            return;
        }

        if (autoDestroy)
            destructionQueued = true;

        if (destructionQueued && !source.isPlaying)
            DestroySource();
            
    }

    private void DestroySource()
    {
        DestroyImmediate(source);

        if (destroyObjectOnCompletion)
            Destroy(gameObject);

        if (destroySelfOnCompletion)
            DestroyImmediate(this);
    }
}
