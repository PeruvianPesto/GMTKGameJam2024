using UnityEngine;
using UnityEngine.SceneManagement;
//Comment to save scene
//^^
//^^^
public class ZoneLoader : MonoBehaviour
{
    // Name of scene 
    public string sceneToLoad;

    // This function is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other)
    { 
        Debug.Log("Zone Loader hit!"); 
        if (other.CompareTag("Player") || other.CompareTag("BigPlayer") || other.CompareTag("SmallPlayer"))
        { 
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
