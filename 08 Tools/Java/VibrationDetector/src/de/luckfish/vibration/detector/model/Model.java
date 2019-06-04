package de.luckfish.vibration.detector.model;

import java.awt.image.BufferedImage;

/**
 * Created by marcel on 10/14/2016.
 */
public class Model implements ModelFacade {

    private Camera m_ImageGrabberLeft;
    private Camera m_ImageGrabberRight;
    private ControlModel m_MovementModel;
    private VibrationDetector m_VibrationDetector;
    private HDRImageCreator m_HDHdrImageCreatorLeft;
    private HDRImageCreator m_HDHdrImageCreatorRight;

    private Thread m_BotThread;

    //private VibrationBot m_VibrationBot;
    private ImageTakingBot m_ImageTakingBot;

    public Model() {
        m_ImageGrabberLeft = new Camera(0, Camera.Orientation.ORIENTATION_90);
        m_ImageGrabberRight = new Camera(2, Camera.Orientation.ORIENTATION_90);
        m_HDHdrImageCreatorLeft = new HDRImageCreator(m_ImageGrabberLeft);
        m_HDHdrImageCreatorRight = new HDRImageCreator(m_ImageGrabberRight);
        m_MovementModel = new ControlModel();
        m_VibrationDetector = new VibrationDetector();

        //m_VibrationBot = new VibrationBot(m_MovementModel);
        m_ImageTakingBot = new ImageTakingBot(m_MovementModel, m_VibrationDetector, m_HDHdrImageCreatorLeft, m_HDHdrImageCreatorRight);

        //m_ImageGrabberLeft.addListener(m_VibrationDetector);
        //m_WebcamImageGrabber.addListener(m_VibrationBot);

    }

    @Override
    public BufferedImage getCurrentImageLeft() {
        return m_ImageGrabberLeft.getImage();
    }

    @Override
    public BufferedImage getCurrentImageRight() {
        return m_ImageGrabberRight.getImage();
    }

    @Override
    public BufferedImage getDetectorImage() {
        return m_VibrationDetector.getImage();
    }

    @Override
    public double getExposure() {
        return m_ImageGrabberLeft.getExposure();
    }

    @Override
    public void setExposure(double exposure) {
        m_ImageGrabberLeft.setExposure(exposure);
        m_ImageGrabberRight.setExposure(exposure);
    }

    @Override
    public double getGain() {
        return m_ImageGrabberLeft.getGain();
    }

    @Override
    public void setGain(double gain) {
        m_ImageGrabberLeft.setGain(gain);
        m_ImageGrabberRight.setGain(gain);
    }

    @Override
    public void startBot(int minExposure, int maxExposure) {
        //m_VibrationBot.setRunning(true);

        //(new Thread(m_VibrationBot)).start();
        if(m_ImageTakingBot.getState() == ImageTakingBot.BotState.Running) {
            return;
        }

        m_ImageTakingBot.setExposureRange(minExposure, maxExposure);

        m_ImageTakingBot.setState(ImageTakingBot.BotState.Running);

        if(m_BotThread == null) {
            m_BotThread = new Thread(m_ImageTakingBot);
            m_BotThread.start();
        }
    }

    @Override
    public ImageTakingBot.BotState getBotState() {
        return m_ImageTakingBot.getState();
    }

    @Override
    public void setBotState(ImageTakingBot.BotState state) {
        m_ImageTakingBot.setState(state);
    }

    public void takeOneHDRPicture(int minExposure, int maxExposure) {
        m_ImageTakingBot.setExposureRange(minExposure, maxExposure);

        m_ImageTakingBot.takeAndSaveHDRImage();
    }

    @Override
    public void setServo(int value) {
        m_MovementModel.setServoValue(value);
    }

    @Override
    public int getServo() {
        return m_MovementModel.getSetServoValue();
    }

    @Override
    public void setMotor(int value) {
        m_MovementModel.setMotorValue(value);
    }

    @Override
    public int getMotor() {
        return m_MovementModel.getMotorValue();
    }

    @Override
    public int getWhiteCountInDetection() {
        return m_VibrationDetector.getVibrationDection();
    }

    @Override
    public int getCurrentMotorValue() {
        return m_MovementModel.getMotorValue();
    }

    @Override
    public void resetCameras() {
        m_ImageGrabberLeft.setConfig();
        m_ImageGrabberRight.setConfig();
    }

    public void stop() {
        m_ImageGrabberLeft.stop();
        m_ImageGrabberRight.stop();
    }

    public void grabImage() {
        m_ImageGrabberLeft.grabImage();
        m_ImageGrabberRight.grabImage();
    }
}
