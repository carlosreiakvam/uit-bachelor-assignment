//using Unity.Netcode;
//using Unity.Netcode.Transports.UTP;
//using Unity.Networking.Transport.Relay;
//using Unity.Services.Relay.Models;
//using UnityEngine;
//using HeroNetworkManager;

//public class InitRelay : MonoBehaviour
//{
//    Allocation allocation;
//    //NetworkManager relayConnector;

//    void Start()
//    {
//        //relayConnector = FindObjectOfType<NetworkManager>();
//        //allocation = relayConnector.allocation;
//        allocation = HeroNetworkManager.NetworkManager.Instance.allocation;
//        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
//        Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
//        Unity.Netcode.NetworkManager.Singleton.StartHost();
//    }
//}
