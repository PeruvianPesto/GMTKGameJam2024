using UnityEngine;
using UnityEngine.SceneManagement;

public class Parallax : MonoBehaviour
{
    public GameObject cam;  // Reference to the player object
    private string sceneName;
    public float oceanSpeed = 0.05f;
    public float dunespeed = 0.1f;
    public float cloudspeed = 0.15f;

    private Transform cloudTransform;
    private Transform cloudTransform2;
    private Transform oceanTransform;
    private Transform oceanTransform2;
    private Transform duneTransform;
    private Transform duneTransform2;
    private float length;
    private float startPosOcean, startPosOcean2;
    private float startPosDune, startPosDune2;
    private float startPosCloud, startPosCloud2;
    public float camx;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        oceanTransform = GameObject.Find("parallax_ocean").GetComponent<SpriteRenderer>().transform;
        oceanTransform2 = GameObject.Find("parallax_ocean2").GetComponent<SpriteRenderer>().transform;
        duneTransform = GameObject.Find("parallax_dunes").GetComponent<SpriteRenderer>().transform;
        duneTransform2 = GameObject.Find("parallax_dunes2").GetComponent<SpriteRenderer>().transform;
        cloudTransform = GameObject.Find("parallax_clouds").GetComponent<SpriteRenderer>().transform;
        cloudTransform2 = GameObject.Find("parallax_clouds2").GetComponent<SpriteRenderer>().transform;

        startPosOcean = oceanTransform.position.x;
        startPosOcean2 = oceanTransform2.position.x;
        startPosDune = duneTransform.position.x;
        startPosDune2 = duneTransform2.position.x;
        startPosCloud = cloudTransform.position.x;
        startPosCloud2 = cloudTransform2.position.x;
        length = oceanTransform.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Calculate the relative player movement from the initial position
        float camMove = cam.transform.position.x - initialCamPos;
        
        // Calculate the distance each layer should move
        float oceandist = camMove * -oceanSpeed;
        float dunedist = camMove * -dunespeed;
        float clouddist = camMove * -cloudspeed;

        // Update the positions for the ocean layer
        oceanTransform.position = new Vector3(startPosOcean + oceandist, oceanTransform.position.y, oceanTransform.position.z);
        oceanTransform2.position = new Vector3(startPosOcean2 + oceandist, oceanTransform2.position.y, oceanTransform2.position.z);

        // Update the positions for the dune layer
        duneTransform.position = new Vector3(startPosDune + dunedist, duneTransform.position.y, duneTransform.position.z);
        duneTransform2.position = new Vector3(startPosDune2 + dunedist, duneTransform2.position.y, duneTransform2.position.z);

        // Update the positions for the cloud layer
        cloudTransform.position = new Vector3(startPosCloud + clouddist, cloudTransform.position.y, cloudTransform.position.z);
        cloudTransform2.position = new Vector3(startPosCloud2 + clouddist, cloudTransform2.position.y, cloudTransform2.position.z);

        camx = cam.transform.position.x;

        // Check if the cam has moved past the length of the sprite and reset positions for each layer
        if (cam.transform.position.x > oceanTransform.position.x + length)
        {
            startPosOcean += length;
            startPosOcean2 += length;
        }
        else if (cam.transform.position.x < oceanTransform.position.x - length)
        {
            startPosOcean -= length;
            startPosOcean2 -= length;
        }

        if (cam.transform.position.x > duneTransform.position.x + length)
        {
            startPosDune += length;
            startPosDune2 += length;
        }
        else if (cam.transform.position.x < duneTransform.position.x - length)
        {
            startPosDune -= length;
            startPosDune2 -= length;
        }

        if (cam.transform.position.x > cloudTransform.position.x + length)
        {
            startPosCloud += length;
            startPosCloud2 += length;
        }
        else if (cam.transform.position.x < cloudTransform.position.x - length)
        {
            startPosCloud -= length;
            startPosCloud2 -= length;
        }
    }
}