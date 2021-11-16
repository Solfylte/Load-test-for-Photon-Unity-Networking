using System;
using UnityEngine;

namespace PunLoadTest
{
    /// <summary>
    /// Linear looped moving
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class MovementController : MonoBehaviour
    {
        event Action<Transform> OnArrivedToDestination;

        private const float DESTINATION_TRESHOLD = 0.2f;

        [SerializeField] private float maxMoveDistance = 20f;
        [SerializeField] private float speed = 2f;

        private Vector3 startPosition;
        private Vector3 destination;

        private PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();

            startPosition = transform.position;
            destination = startPosition + transform.forward * maxMoveDistance; 
        }

        private void Update()
        {
            if (PhotonNetworkFacade.IsMine(photonView))
                UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (IsArrived())
            {
                OnArrivedToDestination?.Invoke(transform);
                SetToStartPoint();
            }
            else
                MoveForward();
        }

        private void MoveForward() => transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        private void SetToStartPoint() => transform.position = startPosition;

        private bool IsArrived() => Vector3.Distance(transform.position, destination) < DESTINATION_TRESHOLD;
    }
}
