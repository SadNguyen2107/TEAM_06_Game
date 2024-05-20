import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;

public class Server {
    private int port;
    private String dir;

    public Server(int port, String dir) {
        this.port = port;
        this.dir = dir;
    }

    public void setPort(int port) {
        this.port = port;
    }

    public void setDir(String dir) {
        this.dir = dir;
    }

    public int getPort() {
        return this.port;
    }

    public String getDir() {
        return this.dir;
    }

    public void Serve() throws IOException {
        ServerSocket s = new ServerSocket(8080);
        System.out.println("Listening at: " + s.getLocalPort());
        try {
            while (true) {
                Socket newSocket = s.accept();
                Thread t = new Thread(new Conn(newSocket, this.dir,
                        new BufferedReader(new InputStreamReader(newSocket.getInputStream())),
                        new DataOutputStream(newSocket.getOutputStream())));
                t.start();
            }
        } finally {
            s.close();
        }
    }
}
