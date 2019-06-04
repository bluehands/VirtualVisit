package de.luckfish.vibration.detector.model;

import org.opencv.core.Core;
import org.opencv.core.Mat;
import org.opencv.videoio.VideoCapture;
import org.opencv.videoio.Videoio;

import java.awt.image.BufferedImage;
import java.util.ArrayList;
import java.util.List;

import static org.opencv.core.Core.flip;
import static org.opencv.core.Core.transpose;

public class Camera implements ImageReceiver {

    public enum Orientation {
        ORIENTATION_0,
        ORIENTATION_90,
        ORIENTATION_180,
        ORIENTATION_270
    }

    private Orientation m_Orientation = Orientation.ORIENTATION_0;

    private VideoCapture m_Camera;

    private Mat m_Frame;

    private BufferedImage m_Image;

    private List<FrameListener> m_FrameListeners;

    private int m_WebcamId;

    public Camera(int webcamId, Orientation orientation) {
        System.loadLibrary(Core.NATIVE_LIBRARY_NAME);

        m_WebcamId = webcamId;
        m_Orientation = orientation;
        m_FrameListeners = new ArrayList<>();

        initCamera();
    }

    private void initCamera()  {
        m_Camera = new VideoCapture(m_WebcamId);

        m_Frame = new Mat();

        m_Image = null;

        m_Camera.open(m_WebcamId);

        setConfig();

        try {
            Thread.sleep(2);
        } catch (InterruptedException e) {
        }

        if(!m_Camera.isOpened()){
            System.out.println("Camera with id: " + m_WebcamId + " have a error!");
        }
        else{
            System.out.println("Camera with id: " + m_WebcamId + " is OK!");
        }
    }

    public void setConfig() {
        //m_Camera.set(Videoio.CV_CAP_PROP_FRAME_WIDTH, 1920);
        //m_Camera.set(Videoio.CV_CAP_PROP_FRAME_HEIGHT, 1080);
        m_Camera.set(Videoio.CV_CAP_PROP_FRAME_WIDTH, 1280);
        m_Camera.set(Videoio.CV_CAP_PROP_FRAME_HEIGHT, 960);
        m_Camera.set(Videoio.CAP_PROP_BRIGHTNESS, 125);
        m_Camera.set(Videoio.CAP_PROP_CONTRAST, 25);
        m_Camera.set(Videoio.CAP_PROP_SATURATION, 50);
        m_Camera.set(Videoio.CAP_PROP_GAIN, 0);  
        m_Camera.set(Videoio.CAP_PROP_BACKLIGHT, 0);
    }

    public void stop() {
        if(m_Camera.isOpened()){
            m_Camera.release();
            System.out.println("Camera with id: " + m_WebcamId + " closed!");
        }
    }

    public void grabImage() {
        m_Camera.read(m_Frame);

        flipFrame();

        informListeners(m_Frame.clone());

        m_Image = MatConverter.toBufferedImage(m_Frame);
    }

    private void flipFrame() {
        switch (m_Orientation) {
            case  ORIENTATION_90:
                transpose(m_Frame, m_Frame);
                flip(m_Frame, m_Frame, 1);
                break;
            case ORIENTATION_180:
                flip(m_Frame, m_Frame, -1);
                break;
            case  ORIENTATION_270:
                transpose(m_Frame, m_Frame);
                flip(m_Frame, m_Frame, 0);
                break;
        }
    }

    private void informListeners(Mat left) {
        for(FrameListener listener : m_FrameListeners) {
            listener.receive(left.clone());
        }
    }

    public void addListener(FrameListener frameListener) {
        m_FrameListeners.add(frameListener);
    }

    public BufferedImage getImage() {
        return m_Image;
    }

    public double getExposure() {
        return m_Camera.get(Videoio.CAP_PROP_EXPOSURE) * -1;
    }

    public void setExposure(double exposure) {
        m_Camera.set(Videoio.CAP_PROP_EXPOSURE, exposure * -1);
    }

    public double getGain() {
        return m_Camera.get(Videoio.CAP_PROP_GAIN);
    }

    public void setGain(double gain) {
        m_Camera.set(Videoio.CAP_PROP_GAIN, gain);
    }
}
