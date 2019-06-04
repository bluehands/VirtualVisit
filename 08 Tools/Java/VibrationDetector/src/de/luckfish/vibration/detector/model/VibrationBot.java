package de.luckfish.vibration.detector.model;

import org.opencv.core.Mat;
import org.opencv.core.MatOfInt;
import org.opencv.imgcodecs.Imgcodecs;

/**
 * Created by marcel on 10/14/2016.
 */
public class VibrationBot implements Runnable, FrameListener {

    private ControlModel m_MovementModel;

    private boolean m_IsRunning;

    private int m_FrameCount;

    private boolean m_Record;

    public VibrationBot(ControlModel movementModel) {
        m_MovementModel = movementModel;
    }

    @Override
    public void run() {
        while(m_IsRunning) {
            try {
                System.out.println("VibrationBot started!");
                Thread.sleep(1000);

                m_MovementModel.setServoValue(4500);
                Thread.sleep(1000);

                m_Record = true;
                m_MovementModel.setServoValue(2500);


                while (m_FrameCount < 10) {
                    Thread.sleep(100);
                }
                m_Record = false;

                m_IsRunning = false;
            } catch(Exception e) {
                System.out.println(e.getMessage());
            }
        }
    }

    @Override
    public void receive(Mat frame) {
        if(m_Record) {
            MatOfInt mPngCompressionRate = new MatOfInt(9);

            Imgcodecs.imwrite(String.format("img/frame_%d.png", m_FrameCount++), frame, mPngCompressionRate);
        }
    }

    public void setRunning(boolean running) {
        m_IsRunning = running;
    }
}
