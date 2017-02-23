using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public Canvas quitMenu;
	public Canvas startMenu;
	public Button singleStart;
	public Button versusStart;
	public Button tutorialStart;
	public Button exit;

	// Use this for initialization
	void Start () {
		startMenu = startMenu.GetComponent<Canvas> ();
		quitMenu = quitMenu.GetComponent<Canvas> ();
		singleStart = singleStart.GetComponent<Button> ();
		versusStart = versusStart.GetComponent<Button> ();
		tutorialStart = tutorialStart.GetComponent<Button> ();
		exit = exit.GetComponent<Button> ();
		startMenu.enabled = true;
		quitMenu.enabled = false;
	}

	public void ExitPress() {
		quitMenu.enabled = true;
		startMenu.enabled = false;
	}

	public void NoPress() {
		quitMenu.enabled = false;
		startMenu.enabled = true;
	}
	
	public void StartSingle() {
		SceneManager.LoadScene ("SinglePlay");
	}

	public void StartMulti() {
		SceneManager.LoadScene (2);
	}

	public void StartTutorial() {
		SceneManager.LoadScene (3);
	}

	public void ExitGame() {
		Application.Quit ();
	}
}
