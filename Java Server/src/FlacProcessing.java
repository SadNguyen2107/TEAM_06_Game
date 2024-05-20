import java.io.ByteArrayOutputStream;
import org.kc7bfi.jflac.*;
import org.kc7bfi.jflac.metadata.StreamInfo;
import org.kc7bfi.jflac.util.ByteData;

class FlacProcessing implements PCMProcessor {
    ByteArrayOutputStream out;

    public FlacProcessing(ByteArrayOutputStream out) {
        this.out = out;
    }

    @Override
    public void processPCM(ByteData data) {
        
        out.write(data.getData(), 0, data.getLen());
    }

    @Override
    public void processStreamInfo(StreamInfo arg0) {
        // nothing too see here!!!
    }
}