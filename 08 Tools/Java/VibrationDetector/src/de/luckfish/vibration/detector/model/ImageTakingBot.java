package de.luckfish.vibration.detector.model;

import org.opencv.core.MatOfInt;
import org.opencv.imgcodecs.Imgcodecs;

import java.io.File;

/**
 * Created by marcel on 10/14/2016.
 */
public class ImageTakingBot implements Runnable {

    public enum BotState {
        Stopped,
        Running,
        Paused
    }
    private BotState m_State = ImageTakingBot.BotState.Stopped;

    private int[] columnPosition = {1500, 1950, 2500, 3050, 3600};
    private boolean m_Direction;

    private MatOfInt m_PngCompressionRate = new MatOfInt(9);

    private HDRImageCreator m_HdrImageCreatorLeft;
    private HDRImageCreator m_HdrImageCreatorRight;

    private ControlModel m_ControlModel;
    private VibrationDetector m_VibrationDetector;

    private int m_MinExposure = 0;
    private int m_MaxExposure = 5;

    public ImageTakingBot(ControlModel controlModel, VibrationDetector vibrationDetector, HDRImageCreator hdrImageCreatorLeft, HDRImageCreator hdrImageCreatorRight) {
        m_ControlModel = controlModel;
        m_VibrationDetector = vibrationDetector;
        m_HdrImageCreatorLeft = hdrImageCreatorLeft;
        m_HdrImageCreatorRight = hdrImageCreatorRight;
    }

    @Override
    public void run() {
        while(true) {
            try {
                if(m_State == BotState.Running) {
                    runBot();
                } else {
                    Thread.sleep(100);
                    System.out.println("Stopped!");
                }
            } catch(Exception e) {
                System.out.println(e.getMessage());
            }
        }
    }

    private void runBot() throws InterruptedException {
        System.out.println("ImageTakingBot started!");

        Thread.sleep(1000);

        int rowValue = 0;
        int columnValue;
        for (int row=0; row<20; row++) {
            moveMotorToPosition(rowValue);

            for (int column = 0; column < columnPosition.length; column++) {
                while(m_State == BotState.Paused) {
                    Thread.sleep(100);
                    System.out.println("Pause!");
                }
                /*if(m_State == BotState.Stopped) {
                    return;
                }*/
                System.out.println("Running!");
                columnValue = columnPosition[m_Direction ? column : (columnPosition.length-1)-column];
                if (!isServerOutOfBounds(columnValue)) {
                    moveServerToPosition(columnValue);

                    int averageExposure = m_MinExposure + (m_MaxExposure - m_MinExposure)/2;
                    m_HdrImageCreatorLeft.setExposure(averageExposure);
                    m_HdrImageCreatorRight.setExposure(averageExposure);
                    //m_VibrationDetector.waitUntilFinishedVibration();
                    Thread.sleep(1000);

                    takeAndSaveHDRImage();
                }
            }

            m_Direction = !m_Direction;
            rowValue += 5;
        }

        moveServerToPosition(4500);

        Thread.sleep(500);
        takeAndSaveHDRImage();
        Thread.sleep(100);

        moveMotorToPosition(50);

        Thread.sleep(500);
        takeAndSaveHDRImage();
        Thread.sleep(1000);

        restMotorAndServo();

        System.out.println("ImageBot finished!");

        m_State = BotState.Stopped;

    }

    public BotState getState() {
        return m_State;
    }

    public void setState(BotState state) {
        m_State = state;
    }

    private void moveMotorToPosition(int rowValue) throws InterruptedException {
        m_ControlModel.setMotorValue(rowValue);
        while (m_ControlModel.getMotorValue() != rowValue) {
            Thread.sleep(100);
        }
    }

    private void moveServerToPosition(int columnValue) throws InterruptedException {
        m_ControlModel.setServoValue(columnValue);
    }

    private boolean isServerOutOfBounds(int column) {
        return column < 1000 || 4500 < column;
    }

    public void takeAndSaveHDRImage() {
        m_HdrImageCreatorLeft.resetFrames();
        m_HdrImageCreatorRight.resetFrames();

        for(float e=m_MinExposure; e<=m_MaxExposure; e++) {
            m_HdrImageCreatorLeft.addFrame(e);
            m_HdrImageCreatorRight.addFrame(e);
        }


        checkOrCreateIfFoldersExists();

        Imgcodecs.imwrite(String.format("img/%s/img_%d_%d_%d_%d.png", "left", m_ControlModel.getSetMotorValue(), m_ControlModel.getSetServoValue(), m_MinExposure, m_MaxExposure), m_HdrImageCreatorLeft.computeFrame(), m_PngCompressionRate);
        Imgcodecs.imwrite(String.format("img/%s/img_%d_%d_%d_%d.png", "right", m_ControlModel.getSetMotorValue(), m_ControlModel.getSetServoValue(), m_MinExposure, m_MaxExposure), m_HdrImageCreatorRight.computeFrame(), m_PngCompressionRate);

    }

    private boolean checkOrCreateIfFoldersExists()
    {
        boolean successLeft = (new File(String.format("img/%s", "left"))).mkdirs();
        boolean successRight = (new File(String.format("img/%s", "right"))).mkdirs();

        return successLeft && successRight;
    }

    private void restMotorAndServo() {
        m_ControlModel.setServoValue(2500);
        m_ControlModel.setMotorValue(0);
    }

    public void setExposureRange(int minExposure, int maxExposure) {
        m_MinExposure = minExposure;
        m_MaxExposure = maxExposure;
    }
}
