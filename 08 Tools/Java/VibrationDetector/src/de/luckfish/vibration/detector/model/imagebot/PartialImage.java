package de.luckfish.vibration.detector.model.imagebot;

import java.awt.image.BufferedImage;

/**
 * Created by marcel on 10/22/2016.
 */
public class PartialImage {

    /**
     * vertical direction
     */
    private int m_Longitude;

    /**
     * horizontal direction
     */
    private int m_Latitude;

    private int m_ServoPos;

    private int m_MotorPos;

    private BufferedImage m_ImageLeft;
    private BufferedImage m_ImageRight;

    private boolean m_IsSet;

    public PartialImage(int motorPos, int servoPos) {
        m_MotorPos = motorPos;
        m_ServoPos = servoPos;

        m_IsSet = false;
    }

    public int getLongitude() {
        return m_Longitude;
    }

    public int getLatitude() {
        return m_Latitude;
    }

    public int getServoPos() {
        return m_ServoPos;
    }

    public int getMotorPos() {
        return m_MotorPos;
    }

    public BufferedImage getImageLeft() {
        return m_ImageLeft;
    }

    public BufferedImage getImageRight() {
        return m_ImageRight;
    }

    public void setImages(BufferedImage imageLeft, BufferedImage imageRight) {
        m_ImageLeft = imageLeft;
        m_ImageRight = imageRight;
        m_IsSet = true;
    }

    public boolean isSet() {
        return m_IsSet;
    }
}
