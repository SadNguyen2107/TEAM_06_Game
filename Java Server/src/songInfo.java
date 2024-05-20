import java.nio.charset.StandardCharsets;

class songInfo {
    private int id;
    private String name;

    public songInfo(int id, String name) {

        this.id = id;
        byte[] s = name.getBytes();
        this.name = new String(s,StandardCharsets.UTF_8);
    }
    public String toString(){
        return String.format("{\"ID\":\"%s\",\"Name\":\"%s\"}", this.id, this.name);
    }
    public String getSong(){
        return this.name;
    }
    public int getID(){
        return this.id;
    }
}