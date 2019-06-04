package de.luckfish.vibration.detector.model;

import org.opencv.core.Mat;

/**
 * Created by marcel on 10/13/2016.
 */
public interface FrameListener {

    void receive(Mat frame);

}
