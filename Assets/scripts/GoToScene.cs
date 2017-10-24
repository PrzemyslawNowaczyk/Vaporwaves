using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour {

    public SceneField SceneToGo;

	void Start () {
        SceneManager.LoadScene(SceneToGo);
	}

}
