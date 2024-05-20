import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;
import java.util.stream.Stream;

import org.kc7bfi.jflac.*;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import javax.sound.sampled.*;

public class Conn implements Runnable {
    private String dir;
    private Socket s;
    private BufferedReader br;
    private DataOutputStream out;

    public Conn(Socket s, String dir, BufferedReader br, DataOutputStream out) {
        this.s = s;
        this.dir = dir;
        this.br = br;
        this.out = out;
    }

    static String processJSON(List<songInfo> s) {
        String res = "";
        for (int i = 0; i < s.size() - 1; i++) {
            res += String.format("%s,", s.get(i).toString());
        }
        if (s.size() > 0) {
            res += s.get(s.size() - 1).toString();
        }

        return String.format("[%s]", res);
    }

    @SuppressWarnings("finally")
    private ByteArrayOutputStream processFile(String song_name) throws IOException, UnsupportedAudioFileException {
        // buffer for prebuffering music data....
        ByteArrayOutputStream bo = new ByteArrayOutputStream();

        // preparing for processing
        String song_name_remaster = new String(song_name.getBytes(), StandardCharsets.UTF_8);
        FlacProcessing flac_processor = new FlacProcessing(bo);
        System.out.println(this.dir + "/" + song_name);
        InputStream fs = new FileInputStream(new File(this.dir + "/" + song_name_remaster));
        // if (fs == null) {
        // System.out.println("sdfsdfsderewrs");
        // }
        FLACDecoder decoder = new FLACDecoder(fs);
        // decoder.getStreamInfo()
        // start prebuffer....
        try {
            decoder.addPCMProcessor(flac_processor);
            decoder.decode();
        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("Error encountered: " + e.getCause());
        } finally {
            fs.close();
            return bo;
        }
        // ByteArrayOutputStream b = new ByteArrayOutputStream().
    }

    static List<songInfo> getSongs(String dir) {

        //getting the songs in the directory and return a list
        AtomicInteger counter = new AtomicInteger(0);
        File f = new File(dir);
        List<songInfo> list_songs = Stream.of(f.listFiles()).filter(x -> x.getName().contains(".flac"))
                .map(x -> x.getName().split(".flac")[0]).map(x -> new songInfo(counter.incrementAndGet(), x))
                .collect(Collectors.toList());
        return list_songs;

    }

    private void send(String text, DataOutputStream out) throws Exception {
        InputStream is = new ByteArrayInputStream(text.getBytes(StandardCharsets.UTF_8));
        // int byte_read =0;
        is.transferTo(out);
        out.flush();
        // while()
    }

    public void run() {
        List<songInfo> list_songs = getSongs(this.dir);
        // Gson gson = new Gson();
        try {
            // java.io.BufferedReader
            // out.flush();
            String x;
            while ((x = this.br.readLine()) != null) {
                // this.out = new DataOutputStream(this.s.getOutputStream());
                // String x = bf.readLine();
                System.out.println(x);
                // DataOutputStream dout = new DataOutputStream(out);
                if (x.equals("all")) {
                    // System.out.println("...");
                    // out.writeUTF(gson.toJson(list_songs));
                    // System.out.println(processJSON(list_songs));

                    // out.write(processJSON(list_songs).getBytes());
                    this.send(processJSON(list_songs), out);
                    out.write("\r\n".getBytes());
                    out.flush();

                    // out.write(13)
                    // out.write(10);
                    // out.flush();
                    // out.close();
                    System.out.println("Send complete!!!!");
                } else {

                    //get the song id
                    int z = Integer.valueOf(x);


                    //get the buffer....
                    ByteArrayInputStream r = new ByteArrayInputStream(
                            this.processFile(list_songs.get((z - 1)).getSong() + ".flac").toByteArray());

                    //start getting from chunk, read it....
                    byte[] data = new byte[8192];

                    int c = data.length;

                    while ((c = r.read(data, 0, c)) != -1) {
                        out.write(data, 0, c);
                    }
                    // buff.close();
                    out.write("\r\n".getBytes());
                    out.flush();
                    // out.close();
                    // dout.close();
                }
            }
        } catch (Exception e) {
            System.out.println("Conn.run()");
            System.out.printf("%s: %s \n", this.s.getRemoteSocketAddress().toString(), e.getMessage());
        }

    }
}
