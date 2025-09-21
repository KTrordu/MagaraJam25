using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public GameObject spawnPoint;

    [SerializeField] private List<Door> resetDoorList = new List<Door>();
    [SerializeField] private List<Trap> resetSpikeList = new List<Trap>();

    private void Start()
    {
        GameInput.Instance.OnReset += GameInput_OnReset;
    }

    private void GameInput_OnReset(object sender, GameInput.OnResetEventArgs e)
    {
        Room currentRoom = e.currentRoom;
        ResetState(currentRoom);
    }

    public void ResetState(Room currentRoom)
    {
        Player.Instance.ResetPlayer();
        Corpse.Instance.ResetCorpse();

        foreach (Door door in resetDoorList)
        {
            door.ResetDoor();
        }
    }
}
