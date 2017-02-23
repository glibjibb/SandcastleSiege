using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	public float cameraSpeed;
	public Canvas quitMenu;
	public Text hp;
	public Text troopHP;
	public GameObject adjustMenu;
	public GameObject troopView;
	public GameObject buildTypes;
	public string playerTag;
	private bool isBattle;
	private string groundTag;
	private int tool;
	private GameObject selected;
	private bool dragging;
	private GameObject buildPrefab;
	private Color defaultColor;
	private bool troopSend;

	// Use this for initialization
	void Start () {
		quitMenu.enabled = false;
		isBattle = false;
		troopSend = false;
		buildTypes.SetActive (false);
		dragging = false;
		tool = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//CAMERA MOVEMENT
		float moveZ = Input.GetAxisRaw ("Vertical");
		float moveX = Input.GetAxisRaw ("Horizontal");
		float zoom, rotate;
		if (Input.GetKey (KeyCode.LeftShift)) {
			moveX = 0;
			zoom = Input.GetAxisRaw ("Vertical");
			if (zoom == 0)
				zoom = Input.GetAxisRaw ("Mouse ScrollWheel") * cameraSpeed/3;
			rotate = -Input.GetAxisRaw ("Horizontal");
		}
		else {
			if (Input.GetMouseButton (2)) {
				moveX = -Input.GetAxis ("Mouse X") * 2;
				moveZ = -Input.GetAxis ("Mouse Y") * 2;
			}
			zoom = Input.GetAxisRaw ("Mouse ScrollWheel") * cameraSpeed;
			if(Input.GetMouseButton(1))
				rotate = Input.GetAxis("Mouse X") * 2;
			else 
				rotate = 0;
		}
		moveX *= Time.deltaTime * cameraSpeed;
		moveZ *= Time.deltaTime * cameraSpeed;
		Quaternion rot = transform.rotation;
		transform.Rotate (new Vector3 (-40, 0, 0));
		transform.Translate (moveX, 0, moveZ, Space.Self);
		transform.rotation = rot;
		Vector3 center = new Vector3 (0, 0);
		Ray ray1 = Camera.main.ScreenPointToRay (new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2));
		RaycastHit hit1;
		if (Physics.Raycast (ray1, out hit1, 100, 1 << 8)) {
			center = hit1.point;
		}
		transform.RotateAround (center, new Vector3(0,1,0), rotate);
		if((zoom > 0 && transform.position.y >= 7) || (zoom < 0 && transform.position.y <= 25))
			transform.Translate (0, 0, zoom, Space.Self);

		//KEY INPUT
		if(Input.GetKeyDown(KeyCode.Escape))
			quitMenu.enabled = true;
		if (Input.GetKeyDown (KeyCode.B)) {
			SetTool (1);
		}
		if (Input.GetKeyDown (KeyCode.N)) {
			SetTool (2);
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			SetTool (3);
		}


		if (!isBattle) {
			if (dragging)
				UpdateDrag ();
			//MOUSE CLICK
			if (Input.GetMouseButtonDown (0)) {
				if (dragging) {
					dragging = false;
				} else {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) {
						Transform objectHit = hit.transform;
						GameObject obj = objectHit.gameObject;
						if (tool == 1) {
							if (!buildPrefab) {
								return;
							}
							else {
								Deselect ();
								if (hit.collider.tag == groundTag)
									BuildObject (hit.point);
							}
						}
						if (tool == 2 && obj.tag == playerTag)
							Destroy (obj);
						if (tool == 3 && obj.tag == playerTag) {
							Deselect ();
							Select (obj);
						}

					}
				}
			}
			if (selected) {
				if (selected.GetComponent<CastlePart> ()) {
					hp.text = "Health: " + selected.GetComponent<CastlePart> ().GetHealth ();
					adjustMenu.SetActive (true);
				}
				if (selected.GetComponent<Troops> ()) {
					troopHP.text = selected.GetComponent<Troops> ().GetHP ();
					troopView.SetActive (true);
				}
			} else {
				adjustMenu.SetActive (false);
				troopView.SetActive (false);
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					Transform objectHit = hit.transform;
					GameObject obj = objectHit.gameObject;
					if (obj.tag == playerTag) {
						Deselect ();
						Select (obj);
					} else if (troopSend) {
						selected.GetComponent<Troops> ().SetWayPoint (hit.point);
					} else if (obj.tag == "Player2")
						obj.GetComponent<CastlePart> ().TakeDamage (15);
				}
			}

			if (selected) {
				if (selected.GetComponent<CastlePart> ()) {
					troopHP.text = "Health: " + selected.GetComponent<CastlePart> ().GetHealth ();
					troopView.SetActive (true);

				}
				if (selected.GetComponent<Troops> ()) {
					troopHP.text = selected.GetComponent<Troops> ().GetHP ();
					troopView.SetActive (true);
				}
			} else {
				troopView.SetActive (false);
			}
		}
	}

	private void Select(GameObject target) {
		if (target.layer == 9) {
			defaultColor = target.GetComponent<Renderer> ().material.color;
			selected = target;
			selected.GetComponent<Renderer> ().material.color = Color.cyan;
			dragging = true;
			troopSend = false;
		}
		if (target.layer == 10) {
			defaultColor = target.GetComponentInChildren<Renderer> ().material.color;
			selected = target;
			foreach (Renderer r in target.GetComponentsInChildren<Renderer>())
				r.material.color = Color.cyan;
			dragging = true;
			troopSend = true;
		}
	}

	public void Deselect() {
		if (selected) {
			if (selected.layer == 9) {
				selected.GetComponent<Renderer> ().material.color = defaultColor;
				dragging = false;
				troopSend = false;
				selected = null;
			}
			else if (selected.layer == 10) {
				foreach (Renderer r in selected.GetComponentsInChildren<Renderer>())
					r.material.color = defaultColor;
				dragging = false;
				troopSend = false;
				selected = null;
			}
		} else
			return;
	}

	public void SetTool(int tool) {
		Deselect ();
		this.tool = tool;
		if (tool == 1)
			buildTypes.SetActive (true);
		else
			buildTypes.SetActive (false);
	}

	public void SetBuildObject(GameObject obj) {
		buildPrefab = obj;
	}

	private void BuildObject(Vector3 pos) {
		Debug.Log ("attempting build");
		GameObject obj = (GameObject) Instantiate (buildPrefab, pos, new Quaternion(0,0,0,0));
		obj.tag = playerTag;
		if (obj.layer == 10 && playerTag == "Player1")
			foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
				r.material.color = Color.green;
		if (obj.layer == 10 && playerTag == "Player2")
			foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
				r.material.color = Color.blue;
		Select (obj);
		obj.GetComponent<Rigidbody> ().isKinematic = true;
		SetBuildObject (null);
	}

	public void UpdateDrag() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		float height = (selected.layer == 9 ? 3 : 0);
		if (Physics.Raycast (ray, out hit, 200.0f, 1 << 8)) {
			if (hit.collider.tag == groundTag) {
				selected.transform.position = new Vector3 (hit.point.x, height, hit.point.z);
				if (selected.layer == 10)
					selected.GetComponent<Troops> ().SetWayPoint (hit.point);
			}
		}

	}

	public void SetPlayerTag(string s) {
		this.playerTag = s;
		groundTag = (s == "Player1" ? "P1Zone" : "P2Zone");
	}

	public void ResizeSelected(float x) {
		if(selected)
			selected.GetComponent<CastlePart> ().ChangeSize (x);
	}

	public void RotateSelected(float x) {
		if (selected)
			selected.GetComponent<CastlePart> ().ChangeRotate (x);
	}

	public void ExitPress() {
		quitMenu.enabled = true;
	}

	public void NoPress() {
		quitMenu.enabled = false;
	}
	public void ExitGame() {
		SceneManager.LoadScene ("Menu");
	}

	public void StartBattle() {
		isBattle = true;
		GameObject.Find ("BuildMenu").SetActive (false);
	}

}
