using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IpConfig : MonoBehaviour
{
    [Header("IP Config")]
    [SerializeField] MusicClient _musicClient;
    [SerializeField] TMP_InputField _hostAddrInputField;
    [SerializeField] TMP_InputField _hostPortInputField;

    public void OnPlayButton(int mainSceneBuildIndex)
    {
        // Get the Host Addr Text
        string hostAddr = _hostAddrInputField.text;

        // Get the Host Port Text
        if (!int.TryParse(_hostPortInputField.text, out int port))
        {
            // Re-Enter the Port
            _hostPortInputField.text = "Re-Enter the Port";
        } 

        // Connect to the Server
        _musicClient.ConnectToServer(hostAddr, port);

        // Change to the next Scene
        SceneManager.LoadSceneAsync(mainSceneBuildIndex);
    }
}
