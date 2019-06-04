package de.luckfish.vibration.detector.model;

import org.opencv.core.Mat;
import org.opencv.core.MatOfByte;
import org.opencv.imgcodecs.Imgcodecs;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;

/**
 * Created by marcel on 10/13/2016.
 */
public class MatConverter {

    public static BufferedImage toBufferedImage(Mat matrix)
    {
        BufferedImage bimg;

        if ( matrix != null ) {
            int cols = matrix.cols();
            int rows = matrix.rows();
            int elemSize = (int)matrix.elemSize();
            byte[] data = new byte[cols * rows * elemSize];
            int type;
            matrix.get(0, 0, data);
            switch (matrix.channels()) {
                case 1:
                    type = BufferedImage.TYPE_BYTE_GRAY;
                    break;
                case 3:
                    type = BufferedImage.TYPE_3BYTE_BGR;
                    // bgr to rgb
                    byte b;
                    for(int i=0; i<data.length; i=i+3) {
                        b = data[i];
                        data[i] = data[i+2];
                        data[i+2] = b;
                    }
                    break;
                default:
                    return null;
            }
            if(cols != 0 && rows != 0) {
                bimg = new BufferedImage(cols, rows, type);
                bimg.getRaster().setDataElements(0, 0, cols, rows, data);
            } else {
                bimg = null; // no image was captured
            }
        } else { // mat was null
            bimg = null;
        }
        return bimg;
    }

    public static BufferedImage Mat2bufferedImage(Mat image) {
        MatOfByte bytemat = new MatOfByte();
        Imgcodecs.imencode(".jpg", image, bytemat);
        byte[] bytes = bytemat.toArray();
        InputStream in = new ByteArrayInputStream(bytes);
        BufferedImage img = null;
        try {
            img = ImageIO.read(in);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return img;
    }
}
