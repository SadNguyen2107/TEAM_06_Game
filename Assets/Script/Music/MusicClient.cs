using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicClient : MonoBehaviour
{
    private TcpClient tcpClient = null;
    private NetworkStream networkStream = null;

    void OnDestroy()
    {
        Close();
    }

    public void ConnectToServer(string host, int port)
    {
        // If Has Already a Connection
        if (tcpClient != null) return;

        // Create a TCP/IP socket
        IPAddress iPAddress = IPAddress.Parse(host);

        // Create a TCP/IP socket
        tcpClient = new TcpClient();

        // Connect to the Server
        try
        {
            tcpClient.Connect(iPAddress, port);
            // Debug.Log($"Connect to Server: {host}:{port}");
        }
        catch (System.Exception)
        {
            //! If Error Return to the main Screen immediately
            SceneManager.LoadSceneAsync(sceneBuildIndex: 0);
        }
    }

    public void Close()
    {
        // If Did Not Create A TCP Connection
        if (tcpClient == null || !tcpClient.Connected) return;

        // Debug.Log("Closed connection with Server!");

        // Free Resource
        networkStream?.Close();
        tcpClient?.Close();
    }

    public void SendMessageToServer(string message)
    {
        if (tcpClient == null || !tcpClient.Connected) return;

        // Make networkStream
        networkStream = tcpClient.GetStream();

        // Get Network Stream 
        byte[] data = Encoding.ASCII.GetBytes(message);
        networkStream.Write(data, 0, data.Length);

        // Send "\r\n" to mark end of the Stream
        byte[] endOfStream = { 13, 10 };
        networkStream.Write(endOfStream, 0, endOfStream.Length);

        // Debug.Log($"Sent: [{message}]");
    }

    public string ReceiveMessage()
    {
        if (tcpClient == null || !tcpClient.Connected) return null;

        // Make networkStream
        networkStream = tcpClient.GetStream();

        StringBuilder messageBuilder = new StringBuilder();
        byte[] buffer = new byte[1];    // Read Byte by byte
        byte lastChar = 0;

        while (networkStream.Read(buffer, 0, buffer.Length) > 0)
        {
            byte currentChar = buffer[0];
            messageBuilder.Append((char)currentChar);

            // Debug.Log((char)currentChar);
            if (lastChar == '\r' && currentChar == '\n')
            {
                // Found the end of the message
                break;
            }

            lastChar = currentChar;
        }

        string message = messageBuilder.ToString().TrimEnd('\r', '\n');    // Remove leading/trailing whitespace
        // Debug.Log($"Received: [{message}]");
        return message;
    }

    // TODO: Prototype For Receive each Chunk From Server
    // public int ReceiveChunkOfBytes(byte[] buffer) => networkStream.Read(buffer, 0, buffer.Length);

    public IEnumerator ReceiveFile(string filePath)
    {
        // float startTime = Time.time;

        // Get Network Stream 
        networkStream = tcpClient.GetStream();

        // Open the File
        using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
            // Read Every Byte From the networkStream -> Write to file
            byte[] bytes = new byte[65536];
            int count;

            bool hasFoundCarriageReturn = false;
            bool hasFoundNewLine = false;

            while ((count = networkStream.Read(bytes, 0, bytes.Length)) > 0)
            {
                // Debug.Log(count);   // TODO: Debug

                int lastIndex = count - 1;

                // If Found the '\r' but not the '\n'
                if (hasFoundCarriageReturn && !hasFoundNewLine)
                {
                    hasFoundNewLine = bytes[lastIndex] == '\n';

                    if (!hasFoundNewLine)
                    {
                        // Reset Everything
                        hasFoundCarriageReturn = false;
                    }
                }

                // 3 conditions must required '\r\n' + nothing behind
                hasFoundCarriageReturn = bytes[lastIndex - 1] == '\r';
                hasFoundNewLine = bytes[lastIndex] == '\n';

                if (hasFoundCarriageReturn && hasFoundNewLine)
                {
                    fileStream.Write(bytes, 0, lastIndex);
                    break;
                }

                fileStream.Write(bytes, 0, count);
                yield return null;
            }
        }

        // float elapsedTime = Time.time - startTime;
        // Debug.Log($"Successfully wrote to {filePath} take {elapsedTime} seconds");
    }

    public bool IsConnected() => tcpClient != null && tcpClient.Connected;
}
