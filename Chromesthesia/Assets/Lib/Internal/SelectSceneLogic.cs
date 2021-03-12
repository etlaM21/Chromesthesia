using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectSceneLogic : MonoBehaviour
{
    public AudioClip[] songs;

    public string[] titles = new string[3];

    public string[] artists = new string[3];

    public GameObject buttonPrefab;

    public GameObject panelForButtons;

    public List<GameObject> buttons;

    public Vector2 canvasSize;

    // Start is called before the first frame update
    void Start()
    {
        int buttonIndex = 0;
        canvasSize = panelForButtons.GetComponent<RectTransform>().sizeDelta;
        // Create Buttons from Song Array
        foreach (AudioClip track in songs)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.transform.SetParent(panelForButtons.transform);//Setting button parent
            float yPos = canvasSize.y/2 - 8.5f * (buttonIndex) - 5.5f;
            button.GetComponent<RectTransform>().localPosition = new Vector3(0, yPos, 0);
            button.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            button.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 1);
            
            // localRotation.eulerAngles = new Vector3(0, 0, 0);
            button.GetComponent<Button>().onClick.AddListener(() => setSongThenStart(track));//Setting what button does when clicked
            //Next line assumes button has child with text as first gameobject like button created from GameObject->UI->Button
            button.transform.GetChild(0).GetComponent<Text>().text = titles[buttonIndex];//Changing text
            button.transform.GetChild(1).GetComponent<Text>().text = artists[buttonIndex];//Changing text
            buttons.Add(button);
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
        Main.Instance.songFinished = false;
        SceneManager.LoadScene("Chromesthesia");
    }

    public void startChromesthesia(){
        SceneManager.LoadScene("Chromesthesia");
        // Main.Instance.InitializeChromesthesia();
    }

    public void moveCanvasDown(float amount){
        Debug.Log(buttons[0].GetComponent<RectTransform>().localPosition.y);
        Debug.Log(canvasSize.y/2 - 5.5f);
        if(buttons[0].GetComponent<RectTransform>().localPosition.y > (canvasSize.y/2 - 5.5f)){
            foreach(GameObject button in buttons) {
                button.GetComponent<RectTransform>().localPosition -= new Vector3(0, amount, 0);
            }
        }
    }

    public void moveCanvasUp(float amount){
        if(buttons[buttons.Count-1].GetComponent<RectTransform>().localPosition.y < (canvasSize.y/2 - 5.5f)){
            foreach(GameObject button in buttons) {
                button.GetComponent<RectTransform>().localPosition += new Vector3(0, amount, 0);
            }
        }
    }
}
