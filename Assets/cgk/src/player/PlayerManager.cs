using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * If the splitScreen option is checked, it's assumed that players
 * prefabs will provide there own camera and ui, so no UI's will be
 * created.
 */

/// <summary>
/// Script for managing multiple players in a couch co-op game.
/// </summary>
public class PlayerManager  : MonoBehaviour {

    // Exposed in inspector.
    [SerializeField]
    [Tooltip("The Maximum number of Players that the game can have.")]
    [Range(1, 4)]
    private int maxPlayers = 1;
    [SerializeField]
    [Tooltip("The Player prefab.  It must have a component that implements IPlayer.")]
    private GameObject playerPrefab = null;

    [Space]
    [Tooltip("If the splitScreen option is checked, it's assumed that players prefabs will provide there own camera and ui, so no UI's will be created.")]
    [SerializeField]
    private bool splitScreen = false;

    [Space]
    [SerializeField]
    [Tooltip("The Player UI Prefab must have a component that implements IPlayerUI.")]
    private GameObject uiPrefab = null;
    [SerializeField]
    [Tooltip("The location of the Player UIs.  There must be the same or more than the number of max players.")]
    private RectTransform[] uiLocations = null;

    // Internal
    private IPlayerUI[] playerUIs;
    private IPlayer[] players;

    private void Awake() {
        this.playerUIs = new IPlayerUI[this.maxPlayers];
        this.players = new IPlayer[this.maxPlayers];

        // Validate Player Prefab.
        if(this.playerPrefab.GetComponentInChildren<IPlayer>() == null) {
            throw new Exception("Player Prefab must have a component that implements IPlayer!");
        }

        if(!this.splitScreen) {
            // Validate Player UI prefab.
            if(this.uiPrefab.GetComponent<IPlayerUI>() == null) {
                throw new Exception("Player UI Prefab must have a component that implements IPlayerUI");
            }

            if(this.maxPlayers > this.uiLocations.Length) {
                throw new Exception("Can not have more players than UI locations!  Please add more.");
            }
        }
    }

    /// <summary>
    /// Returns the number of players in the game.
    /// </summary>
    public int getPlayerCount() {
        return this.getAllPlayers<IPlayer>().Count;
    }

    /// <summary>
    /// Returns a list of player that can be used for iterating through.
    /// </summary>
    public List<T> getAllPlayers<T>() where T : IPlayer {
        List<T> list = new List<T>();
        for(int i = 0; i < this.maxPlayers; i++) {
            if(this.players[i] != null) {
                list.Add((T)this.players[i]);
            }
        }
        return list;
    }

    /// <summary>
    /// Returns the Player with the passed id.  May return null.
    /// </summary>
    public T getPlayer<T>(int playerId) where T : IPlayer {
        return (T)this.players[playerId];
    }

    /// <summary>
    /// Returns the maximum number of Players that the game can have.
    /// </summary>
    public int getMaxPlayers() {
        return this.maxPlayers;
    }

    /// <summary>
    /// Creates a new Player and then returnes them.
    /// </summary>
    public T createPlayer<T>(Vector3 playerStartPos) where T : IPlayer {
        if(this.getPlayerCount() >= this.maxPlayers) {
            throw new Exception("Can not have more than " +  this.maxPlayers + " players!");
        }

        T newPlayer = (T)GameObject.Instantiate(this.playerPrefab, playerStartPos, Quaternion.identity).GetComponent<IPlayer>();
        newPlayer.getTransform().position = playerStartPos;

        // Locate an id to use for the player.
        int freeID = -1;
        for(int i = 0; i < this.getMaxPlayers(); i++) {
            if(this.players[i] == null) {
                freeID = i;
                break;
            }
        }

        this.players[freeID] = newPlayer;

        if(this.splitScreen) {
            this.updateSplitScreenCameraSetup();
        } else {
            // Create the UI for the player.
            GameObject obj = GameObject.Instantiate(this.uiPrefab, this.uiLocations[freeID]);
            IPlayerUI ui = obj.GetComponent<IPlayerUI>();
            obj.transform.position = this.uiLocations[freeID].position;
            ui.setPlayer(newPlayer);

            this.playerUIs[freeID] = null;
        }

        return newPlayer;
    }

    /// <summary>
    /// Returns a location centered between all of the players.
    /// </summary>
    public Vector3 getPlayerCenterpoint() {
        List<IPlayer> pList = this.getAllPlayers<IPlayer>();

        Vector3 centerPoint = Vector3.zero;
        foreach(IPlayer p in pList) {
            centerPoint += p.getTransform().position;
        }
        centerPoint = centerPoint / pList.Count;

        return centerPoint;
    }

    /// <summary>
    /// Removes a player from the game and destroys both the Player Object and their UI, if they have one.
    /// </summary>
    public void removePlayer(IPlayer player) {
        int id = -1;
        for(int i = 0; i < this.maxPlayers; i++) {
            if(this.players[i] == player) {
                id = i;
                break;
            }
        }

        // Destory the Player GameObject.
        GameObject.Destroy(this.players[id].getTransform().gameObject);
        this.players[id] = null;

        if(this.splitScreen) {
            // Redo the screens.
            this.updateSplitScreenCameraSetup();
        }
        else {
            // Destroy the UI.
            GameObject.Destroy(this.playerUIs[id].getTransform().gameObject);
            this.playerUIs[id] = null;
        }
    }

    /// <summary>
    /// Returns true if the PlayerManager is set up to use split screen.
    /// </summary>
    public bool isSplitScreen() {
        return this.splitScreen;
    }

    /// <summary> Updates the cameras on the screen. </summary>
    private void updateSplitScreenCameraSetup() {
        int pCount = this.getPlayerCount();

        if(pCount == 1) {
            this.getAllPlayers<IPlayer>()[0].getCamera().rect = new Rect(0, 0, 1, 1);
        }
        else if(pCount > 1 && pCount <= 4) {
            if(pCount == 2) {
                this.getAllPlayers<IPlayer>()[0].getCamera().rect = new Rect(0, 0, 0.5f, 1); // (P2) Left
                this.getAllPlayers<IPlayer>()[1].getCamera().rect = new Rect(0.5f, 0, 0.5f, 1); // (P1) Right
            }
            else { // Player count == 3 or 4
                float camSize = 0.5f;
                this.getAllPlayers<IPlayer>()[0].getCamera().rect = new Rect(0, 0.5f, camSize, camSize); // P1 (top left)
                this.getAllPlayers<IPlayer>()[1].getCamera().rect = new Rect(0.5f, 0.5f, camSize, camSize); // P2 (top right)
                this.getAllPlayers<IPlayer>()[2].getCamera().rect = new Rect(0, 0, camSize, camSize); // P3 (bottom left)
                this.getAllPlayers<IPlayer>()[3].getCamera().rect = new Rect(0.5f, 0, camSize, camSize); // P4 (bottom right)
            }
        }
        else {
            // Error!  0 Players?
        }
    }
}
