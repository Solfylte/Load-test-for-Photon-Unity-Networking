using System;
using UnityEngine;

namespace PunLoadTest
{
    /// <summary>
    /// Linear looped moving
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class MovementController : MonoBehaviour, IMovementController
    {
        public event Action<PhotonView> OnArrivedToDestination;

        [SerializeField] private float maxMoveDistance = 50f;
        [SerializeField] private float speed = 2f;

        private Vector3 startPosition;

        private PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            startPosition = transform.position;
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
                SetToStartPoint();
                OnArrivedToDestination?.Invoke(photonView);
            }
            else
                MoveUp();
        }

        private void MoveUp() => transform.position += transform.up * speed * Time.deltaTime;

        private void SetToStartPoint() => transform.position = startPosition;

        private bool IsArrived() => transform.position.y - startPosition.y > maxMoveDistance;
    }
}
