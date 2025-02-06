using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        AssignPlayer();
    }

    void AssignPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            virtualCamera.Follow = player.transform;
            //virtualCamera.LookAt = player.transform;
        }
        else
        {
            Debug.LogWarning("Kein Spieler gefunden! Stelle sicher, dass der Spieler ein 'Player'-Tag hat.");
        }
    }
}

