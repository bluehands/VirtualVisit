package de.luckfish.vibration.detector.model.imagebot;

import de.luckfish.vibration.detector.model.*;
import org.opencv.core.Mat;
import org.opencv.core.MatOfInt;
import org.opencv.imgcodecs.Imgcodecs;

import java.awt.geom.AffineTransform;
import java.awt.image.AffineTransformOp;
import java.awt.image.BufferedImage;

/**
 * Created by marcel on 10/22/2016.
 */
public class PanoramaCreatingBot implements Runnable {

    private enum BotState {
        Stopped,
        Running,
        Paused
    }

    private enum RunningState {
        MoveMotor,
        WaitForMotor,
        MoveServo,
        TakePicture,
        CheckVibrating,
        PickNextImage,
        ResetServoAndMotor,
    }

    private enum RunningMode {
        Single,
        Partially,
        Full
    }

    private enum RunThroughMode {
        Vertical,
        Horizontal
    }

    private final int ROWS = 20;
    private final int[] SERVO_VALUES = {1400, 1950, 2500, 3050, 3600, 4400};

    private BotState m_BotState;
    private RunningState m_RunningState;
    private RunningMode m_RunningMode;
    private RunThroughMode m_RunThroughMode;

    public PartialImage[][] m_PartialImages;

    private int m_CurrentX;
    private int m_CurrentY;
    private boolean m_Forward;

    private MatOfInt m_PngCompressionRate = new MatOfInt(9);

    private HDRImageCreator m_HdrImageCreatorLeft;
    private HDRImageCreator m_HdrImageCreatorRight;

    private ControlModel m_ControlModel;
    private VibrationDetector m_VibrationDetector;

    private int m_MinExposure = 0;
    private int m_MaxExposure = 5;

    public PanoramaCreatingBot(ControlModel controlModel, VibrationDetector vibrationDetector, HDRImageCreator hdrImageCreatorLeft, HDRImageCreator hdrImageCreatorRight) {
        m_ControlModel = controlModel;
        m_VibrationDetector = vibrationDetector;
        m_HdrImageCreatorLeft = hdrImageCreatorLeft;
        m_HdrImageCreatorRight = hdrImageCreatorRight;

        initPartialImages();
        initStates();
        initModes();
    }

    private void initModes() {
        m_RunningMode = RunningMode.Full;
        m_RunThroughMode = RunThroughMode.Vertical;
    }

    private void initStates() {
        m_BotState = BotState.Stopped;
        m_RunningState = RunningState.MoveMotor;
    }

    private void initPartialImages() {
        int motorValue = 0;
        int servoValue;

        m_PartialImages = new PartialImage[ROWS][SERVO_VALUES.length];
        for(int x=0; x<ROWS; x++) {
            for(int y=0; y<SERVO_VALUES.length; y++) {
                if((y != SERVO_VALUES.length-1) || (x == (ROWS/2)-1 || x == ROWS-1)) {
                    servoValue = SERVO_VALUES[y];
                    m_PartialImages[x][y] = new PartialImage(motorValue, servoValue);
                }
            }
            motorValue += 5;
        }
    }

    public void setExposureRange(int minExposure, int maxExposure) {
        m_MinExposure = minExposure;
        m_MaxExposure = maxExposure;
    }

    public void startFull() {
        m_RunningMode = RunningMode.Full;
        m_BotState = BotState.Running;
    }

    public void startSingle(int row, int column) {
        m_CurrentX = row;
        m_CurrentY = column;
        m_RunningMode = RunningMode.Single;
        m_BotState = BotState.Running;
    }

    public void stop() {
        m_BotState = BotState.Stopped;
    }

    public void paused() {
        m_BotState = BotState.Paused;
    }

    public void resume() {
        m_BotState = BotState.Running;
    }

    @Override
    public void run() {
        while(true) {
            try {
                switch (m_BotState) {
                    case Stopped:
                        rest();
                        break;
                    case Paused:
                        break;
                    case Running:
                        DoStep();
                        break;
                }
            } catch(Exception e) {
                System.out.println(e.getMessage());
            }
        }
    }

    private void rest() {
        m_CurrentX = 0;
        m_CurrentY = 0;

        m_RunningState = RunningState.MoveMotor;
    }

    private void DoStep() {
        switch (m_RunningState) {
            case MoveMotor:
                moveMotorToPosition();
                break;
            case WaitForMotor:
                checkIfMotorReachedPosition();
                break;
            case MoveServo:
                moveServerToPosition();
                break;
            case CheckVibrating:
                waitUntilCameraStopVibrating();
                break;
            case TakePicture:
                takeAndSaveHDRImage();
                break;
            case PickNextImage:
                pickNextPartialImage();
                break;
            case ResetServoAndMotor:
                restMotorAndServo();
                break;
        }
    }

    private PartialImage getCurrentPartialImage() {
        return m_PartialImages[m_CurrentX][m_CurrentY];
    }

    private void moveMotorToPosition() {
        m_ControlModel.setMotorValue(getCurrentPartialImage().getMotorPos());

        m_RunningState = RunningState.WaitForMotor;
    }

    private void checkIfMotorReachedPosition() {
        if (m_ControlModel.getMotorValue() == getCurrentPartialImage().getMotorPos()) {
            m_RunningState = RunningState.MoveServo;
        }
    }

    private void moveServerToPosition() {
        m_ControlModel.setServoValue(getCurrentPartialImage().getServoPos());

        m_RunningState = RunningState.CheckVibrating;
    }

    private void waitUntilCameraStopVibrating() {
        int averageExposure = m_MinExposure + (m_MaxExposure - m_MinExposure)/2;
        m_HdrImageCreatorLeft.setExposure(averageExposure);
        m_HdrImageCreatorRight.setExposure(averageExposure);
        m_VibrationDetector.waitUntilFinishedVibration();

        m_RunningState = RunningState.TakePicture;
    }

    private void takeAndSaveHDRImage() {
        m_HdrImageCreatorLeft.resetFrames();
        m_HdrImageCreatorRight.resetFrames();

        for(float e=m_MinExposure; e<=m_MaxExposure; e++) {
            m_HdrImageCreatorLeft.addFrame(e);
            m_HdrImageCreatorRight.addFrame(e);
        }

        Mat leftMat = m_HdrImageCreatorLeft.computeFrame();
        Mat rightMat = m_HdrImageCreatorRight.computeFrame();

        BufferedImage imageLeft = MatConverter.toBufferedImage(leftMat);
        BufferedImage imageRight = MatConverter.toBufferedImage(rightMat);

        getCurrentPartialImage().setImages(scaleImage(imageLeft), scaleImage(imageRight));

        Imgcodecs.imwrite(String.format("img/%s/img_%d_%d_%d_%d.png", "left", m_ControlModel.getSetMotorValue(), m_ControlModel.getSetServoValue(), m_MinExposure, m_MaxExposure), leftMat, m_PngCompressionRate);
        Imgcodecs.imwrite(String.format("img/%s/img_%d_%d_%d_%d.png", "right", m_ControlModel.getSetMotorValue(), m_ControlModel.getSetServoValue(), m_MinExposure, m_MaxExposure), rightMat, m_PngCompressionRate);

        m_RunningState = RunningState.PickNextImage;
    }

    private void pickNextPartialImage() {
        if(m_RunningMode == RunningMode.Full) {
            if(m_RunThroughMode == RunThroughMode.Vertical) {
                verticalRunThrough();
            } else if(m_RunThroughMode == RunThroughMode.Horizontal) {
                m_BotState = BotState.Stopped;
                m_RunningState = RunningState.MoveMotor;
            }
        } else if(m_RunningMode == RunningMode.Single) {
            m_CurrentX = 0;
            m_CurrentY = 0;
            m_BotState = BotState.Stopped;
            m_RunningState = RunningState.MoveMotor;
        } else if(m_RunningMode == RunningMode.Partially) {
            m_BotState = BotState.Stopped;
            m_RunningState = RunningState.MoveMotor;
        }
    }

    private void verticalRunThrough() {
        m_CurrentX++;
        if(m_CurrentX <  ROWS) {
            if(m_Forward) {
                m_CurrentY++;
            } else {
                m_CurrentY--;
            }
            if((m_Forward && m_CurrentY < SERVO_VALUES.length)
                    || (!m_Forward && m_CurrentY > 0)) {
                m_Forward = !m_Forward;
            }
            if(getCurrentPartialImage() == null) {
                pickNextPartialImage();
            }
        } else {
            m_CurrentX = 0;
            m_CurrentY = 0;
            m_BotState = BotState.Stopped;
        }
        m_RunningState = RunningState.ResetServoAndMotor;
    }

    private void restMotorAndServo() {
        m_ControlModel.setServoValue(2500);
        m_ControlModel.setMotorValue(0);

        m_RunningState = RunningState.MoveMotor;
    }

    private BufferedImage scaleImage(BufferedImage image) {
        if(image == null) {
            return null;
        }
        BufferedImage before = image;
        int w = before.getWidth();
        int h = before.getHeight();
        BufferedImage after = new BufferedImage(w, h, BufferedImage.TYPE_INT_ARGB);
        AffineTransform at = new AffineTransform();
        at.scale(0.1, 0.1);
        AffineTransformOp scaleOp =
                new AffineTransformOp(at, AffineTransformOp.TYPE_BILINEAR);
        after = scaleOp.filter(before, after);
        return after;
    }

}
