using UnityEngine;
using Unity.Netcode;

public class SimpleConnectionDebug : MonoBehaviour
{
    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 22;
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.UpperLeft;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);

        float x = 10f;
        float y = 10f;
        float w = 420f;
        float h = 220f;

        string text = "No NetworkManager";

        if (NetworkManager.Singleton != null)
        {
            string mode = "Offline";

            if (NetworkManager.Singleton.IsHost)
                mode = "Host";
            else if (NetworkManager.Singleton.IsClient)
                mode = "Client";

            int players = -1;

            if (GameNetworkController.Instance != null)
                players = GameNetworkController.Instance.connectedPlayers.Value;

            text =
                "NETWORK DEBUG\n\n" +
                "Mode: " + mode + "\n" +
                "Listening: " + NetworkManager.Singleton.IsListening + "\n" +
                "Connected: " + NetworkManager.Singleton.IsConnectedClient + "\n" +
                "Local Client ID: " + NetworkManager.Singleton.LocalClientId + "\n" +
                "Players: " + players;
        }

        GUI.Box(new Rect(x, y, w, h), "", boxStyle);
        GUI.Label(new Rect(x + 10f, y + 10f, w - 20f, h - 20f), text, labelStyle);
    }
}