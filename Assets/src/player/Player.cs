using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public static Player localPlayer;

    public Text timerText;
    public SetupPhaseUI setupPhaseUI;

    public Transform bulletSpawn;
    public Team team;
    public GameObject cameraObj;
    private NetworkClient client;

    public override void OnStartLocalPlayer() {
        Player.localPlayer = this;

        // Create the camera
        this.cameraObj.SetActive(true);

        // TODO make better
        this.client = GameObject.FindObjectOfType<NetworkManager>().client;
    }

    public override void OnStartServer() {
        base.OnStartServer();

        NetworkServer.RegisterHandler(messageID, OnServerReadyToBeginMessage);

        //NetworkManager netManager = GameObject.FindObjectOfType<NetworkManager>();
        //NetworkClient client = netManager.client;
        //client.RegisterHandler(MyBeginMsg, OnServerReadyToBeginMessage);
    }

    private void Update() {
        // Only handle input if this is a local player.
        if (this.isLocalPlayer) {
            const float sensitivity = 40f;
            float x = Input.GetAxis("Vertical") * Time.deltaTime * sensitivity;
            float z = Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity;

            this.transform.Translate(x, 0, z * -1);

            bool leftBtnDown = Input.GetMouseButtonDown(0);
            bool rightBtnDown = Input.GetMouseButtonDown(1);

            if (!EventSystem.current.IsPointerOverGameObject()) {
                if (leftBtnDown || rightBtnDown) {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity)) {
                        if (leftBtnDown) {
                            TeamGround tg = hit.transform.GetComponent<TeamGround>();
                            if (tg != null) {
                                if (this.team == tg.getTeam()) {
                                    Team t = Input.GetKey(KeyCode.LeftShift) ? Team.BLUE : this.team;
                                    this.sendMessage(new MessageSpawnEntity(Registry.unitSoldier, hit.point, Vector3.zero, t));
                                }
                            }
                        }
                        if (rightBtnDown) {
                            SidedEntity entity = hit.transform.GetComponent<SidedEntity>();
                            if (entity != null) {
                                this.sendMessage(new MessageRemoveEntity(entity));
                            }
                        }
                    }
                }
            }
        }
    }

    [ClientRpc]
    public void RpcSetTeam(int newTeamId) {
        this.team = Team.getTeamFromId(newTeamId);
        Color color = this.team.getTeamColor();
        this.GetComponent<MeshRenderer>().material.color = color;
    }

    public Team getTeam() {
        return this.team;
    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    private void CmdFire() {
        // Create the Bullet from the Bullet Prefab
        GameObject bullet = GameObject.Instantiate(
            Registry.projectileBullet.getPrefab(),
            this.bulletSpawn.position,
            this.bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 60;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        GameObject.Destroy(bullet, 2.0f);
    }

    /// <summary>
    /// Sends a message to the server.
    /// </summary>
    private void sendMessage(AbstractMessage message) {
        this.client.Send(message.getID(), message);
    }



    const short messageID = 1002;

    public void SendReadyToBeginMessage(string myId) {
        NetworkManager netManager = GameObject.FindObjectOfType<NetworkManager>();
        NetworkClient client = netManager.client;
        client.Send(messageID, new StringMessage(myId));
    }

    void OnServerReadyToBeginMessage(NetworkMessage netMsg) {
        StringMessage beginMessage = netMsg.ReadMessage<StringMessage>();
        Debug.Log("received OnServerReadyToBeginMessage " + beginMessage.value);
    }
}
