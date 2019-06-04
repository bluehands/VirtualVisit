package de.luckfish.vibration.detector.model;

import de.luckfish.vibration.detector.model.serialport.TwoWaySerialComm;

import java.io.IOException;

/**
 * Created by marcel on 10/14/2016.
 */
public class ControlModel {

    private TwoWaySerialComm m_TwoWaySerialComm;

    private boolean m_IsComConnected;

    private int m_SetServerValue;
    private int m_SetMotorValue;

    public ControlModel() {

        initSerialComm();

        initServo();
        initMotor();
    }

    private void initMotor() {
        m_SetMotorValue = getMotorValue();
    }

    private void initServo() {
        setServoValue(2500);
    }

    private void initSerialComm() {
        m_TwoWaySerialComm = new TwoWaySerialComm();

        try {
            m_TwoWaySerialComm.connect("COM3");
            m_IsComConnected = true;

            System.out.println("Serial port COM3 is OK!");
        } catch (Exception e) {
            System.out.println("Serial port COM3 is failed to connect!");
        }
    }

    public void setServoValue(int value) {
        m_SetServerValue = value;
        sendMessage("b" + value);
    }

    public int getSetServoValue() {
        return m_SetServerValue;
    }

    public void setMotorValue(int value) {
        m_SetMotorValue = value;
        sendMessage("a" + value);
    }

    public int getSetMotorValue() {
        return m_SetMotorValue;
    }

    public int getMotorValue() {
        if(m_IsComConnected != true) {
            return -1;
        }
        sendMessage("e");

        String line = TwoWaySerialComm.SerialReader.readLine;

        if(line == null || line.isEmpty()) {
            return -1;
        }
        return Integer.parseInt(line.substring(1, line.length()));
    }

    private void sendMessage(String message) {
        if(m_IsComConnected != true) {
            return;
        }
        message = message + " ";
        byte[] massageBytes = message.getBytes();
        massageBytes[massageBytes.length-1] = 0;
        try {
            TwoWaySerialComm.SerialWriter.out.write(massageBytes);
        } catch (IOException e) {
            System.out.println("Could not send message: " + message);
        }
    }
}
