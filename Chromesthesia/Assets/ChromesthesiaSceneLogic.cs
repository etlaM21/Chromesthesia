using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChromesthesiaSceneLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
	public void exitChromesthesia(){
		Main.Instance.AudioSource.Stop();
		SceneManager.LoadScene("SelectScene");
	}
}
