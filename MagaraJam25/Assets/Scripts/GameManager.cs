using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Light2D globalLight;
    [SerializeField] private float waitTimeForTransitionEffect;

    [SerializeField] public List<Room> rooms;

    private void Awake()
    {
        Instance = this;

        globalLight.enabled = false;
    }

    private void Start()
    {
        RoomExit.OnRoomExitTriggered += RoomExit_OnRoomExitTriggered;
    }

    private void RoomExit_OnRoomExitTriggered(object sender, System.EventArgs e)
    {
        RoomExit roomExit = sender as RoomExit;
        StartCoroutine(DelayedRoomChange(roomExit));
    }

    public void ChangeRoom(Room roomToEnter, RoomTransitable roomTransitable)
    {
        roomTransitable.transform.position = roomToEnter.spawnPoint.transform.position;
    }

    private IEnumerator DelayedRoomChange(RoomExit roomExit)
    {
        yield return new WaitForSeconds(waitTimeForTransitionEffect);
        ChangeRoom(roomExit.roomToGo, Player.Instance);
    }
}
