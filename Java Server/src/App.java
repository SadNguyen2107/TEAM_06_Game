

public class App {
    public static void main(String[] args) throws Exception {
        Server s = new Server(8080, "./songlists");
        s.Serve();
    }
}
