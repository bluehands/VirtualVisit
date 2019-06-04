package de.luckfish.vibration.detector.main;

import de.luckfish.vibration.detector.controller.VibrationDetectorController;
import java.math.*;

import javax.swing.*;

public class Main {

    public static void main(String[] args) throws ClassNotFoundException, InstantiationException, IllegalAccessException, UnsupportedLookAndFeelException {

        UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());

        new VibrationDetectorController();
        
        double pi = Math.PI;

    }
}
