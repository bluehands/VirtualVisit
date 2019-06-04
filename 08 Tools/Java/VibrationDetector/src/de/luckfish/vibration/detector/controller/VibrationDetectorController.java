package de.luckfish.vibration.detector.controller;

import de.luckfish.vibration.detector.model.Model;
import de.luckfish.vibration.detector.view.ControlPanelView;
import de.luckfish.vibration.detector.view.VibrationDetectorView;

import javax.swing.*;
import java.awt.*;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;

public class VibrationDetectorController extends JFrame implements Runnable {

    private static final int WIDTH = 1800;
    private static final int HEIGHT = 1200;

    private Thread thread;

    private VibrationDetectorView m_View;
    private ControlPanelView m_ControlPanel;
    private Model m_Model;

    private boolean isRunning;

    public VibrationDetectorController() {

        m_Model = new Model();

        m_View = new VibrationDetectorView(m_Model);
        m_ControlPanel = new ControlPanelView(m_Model);

        init();

        isRunning = true;
        thread = new Thread(this);
        thread.start();
    }

    private void init() {
        setSize(WIDTH, HEIGHT);
        setTitle("VibrationDetector");
        setDefaultLookAndFeelDecorated(true);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                m_Model.stop();
                isRunning = false;
                System.exit(1);
            }
        });

        setLayout(null);

        m_View.setBounds(0, 0, 1200, HEIGHT);
        m_ControlPanel.setBounds(1200, 0, 600, HEIGHT);
        add(m_View, BorderLayout.CENTER);
        add(m_ControlPanel, BorderLayout.CENTER);

        setVisible(true);
        setFocusable(true);

    }

    public void run() {
        while(isRunning) {
            m_Model.grabImage();

            m_View.repaint();
            //m_ControlPanel.repaint();
            try {
                Thread.sleep(17);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
