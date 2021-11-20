using System;
using System.Collections;
using UnityEngine;
#if IS_PUN2
using Photon.Pun;
using Photon.Realtime;
#endif

namespace PunLoadTest
{
    /// <summary>
    /// Position synchronization via RPC. Just for check RPC traffic
    /// </summary>
    public class RPCTransformSync : Photon.MonoBehaviour
    {
        [SerializeField] private float lerpSpeed = 6f;

        private WaitForSeconds sendTimeout;

        private void Start()
        {
            sendTimeout = new WaitForSeconds(1f / PhotonNetworkFacade.SendRate);
            StartCoroutine(SyncPosition());
        }

        private IEnumerator SyncPosition()
        {
            while (true)
            {
                if (PhotonNetworkFacade.IsMine(photonView))
                    PhotonNetworkFacade.RPC(photonView, nameof(SyncPosition),
                                                PunLoadTest.RpcTarget.Others,
                                                transform.position);

                yield return sendTimeout;
            }
        }

        [PunRPC]
        public void SyncPosition(Vector3 position) => transform.position = position;
    }
}
