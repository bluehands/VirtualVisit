package de.luckfish.vibration.detector.model;

import java.awt.image.BufferedImage;

/**
 * Created by marcel on 10/14/2016.
 */
public interface ModelFacade {

    BufferedImage getCurrentImageLeft();

    BufferedImage getCurrentImageRight();

    BufferedImage getDetectorImage();

    double getExposure();

    void setExposure(double exposure);

    double getGain();

    void setGain(double gain);

    void startBot(int minExposure, int maxExposure);

    ImageTakingBot.BotState getBotState();

    void setBotState(ImageTakingBot.BotState state);

    void takeOneHDRPicture(int minExposure, int maxExposure);

    void setServo(int value);

    int getServo();

    void setMotor(int value);

    int getMotor();

    int getWhiteCountInDetection();

    int getCurrentMotorValue();

    void resetCameras();
}
