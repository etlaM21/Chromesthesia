using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectSceneLogic : MonoBehaviour
{
    public AudioClip song01;
    public AudioClip[] songs;

    public GameObject buttonPrefab;

    public GameObject panelForButtons;

    // Start is called before the first frame update
    void Start()
    {
        int buttonIndex = 0;
        Vector2 canvasSize = panelForButtons.GetComponent<RectTransform>().sizeDelta;
        Debug.Log(canvasSize);
        // Create Buttons from Song Array
        foreach (AudioClip track in songs)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.SetParent(panelForButtons.transform);//Setting button parent
            float yPos = canvasSize.y/2 - 50 * (buttonIndex + 1);
            button.GetComponent<RectTransform>().localPosition = new Vector3(0, yPos, 0);
            button.GetComponent<Button>().onClick.AddListener(() => setSongThenStart(track));//Setting what button does when clicked
            //Next line assumes button has child with text as first gameobject like button created from GameObject->UI->Button
            button.transform.GetChild(0).GetComponent<Text>().text = "" + track;//Changing text
            buttonIndex++;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void setSongThenStart(AudioClip songToSet)
    {
        Main.Instance.AudioSource.clip = songToSet;
        SceneManager.LoadScene("Chromesthesia");
    }

    public void startChromesthesia(){
        SceneManager.LoadScene("Chromesthesia");
        // Main.Instance.InitializeChromesthesia();
    }
}
