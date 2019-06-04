package de.luckfish.vibration.detector.view;

import de.luckfish.vibration.detector.model.ImageTakingBot;
import de.luckfish.vibration.detector.model.ModelFacade;

import javax.swing.*;
import java.awt.*;

public class ControlPanelView extends JPanel {

    private JLabel m_WhiteCountValue;
    private JLabel m_CurrentMotorValue;

    private ModelFacade m_ModelFacade;

    private int[] columnPosition = {1400, 1950, 2500, 3050, 3600, 4500};

    public ControlPanelView(ModelFacade modelFacade) {
        m_ModelFacade = modelFacade;

        init();
    }

    private void init() {
        setLayout(null);

        JLabel label = new JLabel("Exposure");
        label.setBounds(10, 10, 200, 25);
        add(label);

        double exposure = m_ModelFacade.getExposure();
        JLabel exposureValue = new JLabel(Integer.toString((int)exposure));
        exposureValue.setBounds(420, 10, 30, 25);
        add(exposureValue);

        JSlider exposureSlider = new JSlider();
        exposureSlider.setMinimum(0);
        exposureSlider.setMaximum(11);
        exposureSlider.setPaintTicks(true);
        exposureSlider.setBounds(100, 10, 300, 25);
        exposureSlider.setValue((int)exposure);
        exposureSlider.addChangeListener(ce -> exposureValue.setText(Integer.toString(exposureSlider.getValue())));
        exposureSlider.addChangeListener(ce -> {
            exposureValue.setText(Integer.toString(exposureSlider.getValue()));
            m_ModelFacade.setExposure(exposureSlider.getValue());
        });
        add(exposureSlider);

        JLabel labelGain = new JLabel("Gain");
        labelGain.setBounds(10, 70, 200, 25);
        add(labelGain);

        double gain = m_ModelFacade.getGain();
        JLabel gainValue = new JLabel(Integer.toString((int)gain));
        gainValue.setBounds(420, 70, 30, 25);
        add(gainValue);


        JSlider gainSlider = new JSlider();
        gainSlider.setMinimum(0);
        gainSlider.setMaximum(255);
        gainSlider.setPaintTicks(true);
        gainSlider.setBounds(100, 70, 300, 25);
        gainSlider.setValue((int)gain);
        gainSlider.addChangeListener(ce -> {
            gainValue.setText(Integer.toString(gainSlider.getValue()));
            m_ModelFacade.setGain(gainSlider.getValue());
        });
        add(gainSlider);

        JTextField minExposureInput = new JTextField("0");
        minExposureInput.setBounds(160, 160, 50, 30);
        add(minExposureInput);

        JTextField maxExposureInput = new JTextField("0");
        maxExposureInput.setBounds(210, 160, 50, 30);
        add(maxExposureInput);

        JButton mStartBotBtn = new JButton("Start Bot");
        mStartBotBtn.setBounds(30, 120, 120, 30);
        mStartBotBtn.addActionListener(e ->
        {
            if(m_ModelFacade.getBotState() == ImageTakingBot.BotState.Stopped) {
                m_ModelFacade.startBot(Integer.decode(minExposureInput.getText()), Integer.decode(maxExposureInput.getText()));
                mStartBotBtn.setText("Stop Bot");
            } else {
                m_ModelFacade.setBotState(ImageTakingBot.BotState.Running);
                mStartBotBtn.setText("Start Bot");
            }
        }
        );
        add(mStartBotBtn);

        JButton mPausBotBtn = new JButton("Pause Bot");
        mPausBotBtn.setBounds(180, 120, 120, 30);
        mPausBotBtn.addActionListener(e ->
                {
                    if(m_ModelFacade.getBotState() == ImageTakingBot.BotState.Running) {
                        m_ModelFacade.setBotState(ImageTakingBot.BotState.Paused);
                        mPausBotBtn.setText("Resume Bot");
                    } else {
                        m_ModelFacade.setBotState(ImageTakingBot.BotState.Running);
                        mPausBotBtn.setText("Pause Bot");
                    }
                }
        );
        add(mPausBotBtn);

        JButton mTakeBotBtn = new JButton("Take Picture");
        mTakeBotBtn.setBounds(30, 160, 120, 30);
        mTakeBotBtn.addActionListener(e -> m_ModelFacade.takeOneHDRPicture(Integer.decode(minExposureInput.getText()), Integer.decode(maxExposureInput.getText())));
        add(mTakeBotBtn);

        JLabel labelS = new JLabel("Servo");
        labelS.setBounds(10, 200, 200, 25);
        add(labelS);

        JLabel servoValue = new JLabel("0");
        servoValue.setBounds(420, 200, 60, 25);
        add(servoValue);

        /*JSlider mServoSlider = new JSlider();
        mServoSlider.setBounds(100, 200, 300, 25);
        mServoSlider.setMinimum(1000);
        mServoSlider.setMaximum(4500);
        mServoSlider.setPaintTicks(true);
        mServoSlider.setMajorTickSpacing(100);
        mServoSlider.addChangeListener(ce -> {
            servoValue.setText(Integer.toString(mServoSlider.getValue()));
            m_ModelFacade.setServo(mServoSlider.getValue());
        });
        mServoSlider.setValue(m_ModelFacade.getServo());
        add(mServoSlider);*/


        JSlider mServoSlider = new JSlider();
        mServoSlider.setBounds(100, 200, 300, 25);
        mServoSlider.setMinimum(0);
        mServoSlider.setMaximum(5);
        mServoSlider.setPaintTicks(true);
        mServoSlider.setMajorTickSpacing(100);
        mServoSlider.addChangeListener(ce -> {
            servoValue.setText(Integer.toString(columnPosition[mServoSlider.getValue()]));
            m_ModelFacade.setServo(columnPosition[mServoSlider.getValue()]);
        });
        mServoSlider.setValue(m_ModelFacade.getServo());
        add(mServoSlider);

        JLabel labelM = new JLabel("Motor");
        labelM.setBounds(10, 260, 200, 25);
        add(labelM);

        JLabel motorValue = new JLabel("0");
        motorValue.setBounds(420, 260, 60, 25);
        add(motorValue);

        JSlider mMotorSlider = new JSlider();
        mMotorSlider.setBounds(100, 260, 300, 25);
        mMotorSlider.setMinimum(0);
        mMotorSlider.setMaximum(19);
        mMotorSlider.setMajorTickSpacing(5);
        mMotorSlider.setMinorTickSpacing(5);
        mMotorSlider.setPaintTicks(true);
        mMotorSlider.addChangeListener(ce -> {
            motorValue.setText(Integer.toString(mMotorSlider.getValue()*5));
            m_ModelFacade.setMotor(mMotorSlider.getValue()*5);
        });
        mMotorSlider.setValue(m_ModelFacade.getMotor());
        add(mMotorSlider);

        JLabel labelWC = new JLabel("WhiteCount");
        labelWC.setBounds(10, 300, 90, 25);
        add(labelWC);

        m_WhiteCountValue = new JLabel("0");
        m_WhiteCountValue.setBounds(100, 300, 100, 25);
        add(m_WhiteCountValue);

        JLabel labelCM = new JLabel("CurrentMotor");
        labelCM.setBounds(10, 330, 90, 25);
        add(labelCM);

        m_CurrentMotorValue = new JLabel("0");
        m_CurrentMotorValue.setBounds(100, 330, 100, 25);
        add(m_CurrentMotorValue);

        JButton mResetCamBtn = new JButton("Reset");
        mResetCamBtn.setBounds(100, 400, 120, 30);
        mResetCamBtn.addActionListener(e ->
                {
                    m_ModelFacade.resetCameras();
                }
        );
        add(mResetCamBtn);
    }

    public void paint(Graphics g) {
        super.paint(g);

        m_WhiteCountValue.setText(Integer.toString(m_ModelFacade.getWhiteCountInDetection()));

        m_CurrentMotorValue.setText(Integer.toString(m_ModelFacade.getCurrentMotorValue()));
    }
}
