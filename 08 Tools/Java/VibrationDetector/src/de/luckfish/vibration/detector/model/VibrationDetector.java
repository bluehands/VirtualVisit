package de.luckfish.vibration.detector.model;

import org.opencv.core.*;
import org.opencv.imgproc.Imgproc;
import org.opencv.photo.Photo;

import java.awt.image.BufferedImage;

import static org.opencv.core.Core.absdiff;
import static org.opencv.imgproc.Imgproc.INTER_CUBIC;

/**
 * Created by marcel on 10/13/2016.
 */
public class VibrationDetector implements ImageReceiver, FrameListener{

    private BufferedImage m_Image;
    private int m_WhitePixelInImage;
    private int m_FrameCounter;
    private int m_CurrentFrame;

    private int i = 0;

    private Mat outerBox;
    private Mat diff_frame = null;
    private Mat tempon_frame = null;

    private int[][] vibrationMesser = new int[120][100];
    private int messCounter = 0;

    @Override
    public void receive(Mat frame) {
        outerBox = new Mat(frame.size(), CvType.CV_8UC1);

        Imgproc.resize(frame, frame, new Size(frame.size().width/5, frame.size().height/5), 0, 0, INTER_CUBIC);

        Imgproc.cvtColor(frame, outerBox, Imgproc.COLOR_BGR2GRAY);
        Imgproc.GaussianBlur(outerBox, outerBox, new Size(3, 3), 0);

        Photo.fastNlMeansDenoising(outerBox,outerBox, 3, 7, 21);

        if (i == 0) {
            diff_frame = new Mat(outerBox.size(), CvType.CV_8UC1);
            tempon_frame = new Mat(outerBox.size(), CvType.CV_8UC1);
            diff_frame = outerBox.clone();
        }

        if (i == 1) {
            Core.subtract(outerBox, tempon_frame, diff_frame);
            Imgproc.adaptiveThreshold(diff_frame, diff_frame, 255,
                    Imgproc.ADAPTIVE_THRESH_MEAN_C,
                    Imgproc.THRESH_BINARY_INV, 5, 2);
        }

        i = 1;

        m_Image = MatConverter.toBufferedImage(diff_frame);

        countWhitePixel(diff_frame);

        tempon_frame = outerBox.clone();

        m_FrameCounter++;
    }

    private void countWhitePixel(Mat mat) {
        int count_white = 0;
        for( int y = 0; y < mat.rows(); y++ ) {
            for( int x = 0; x < mat.cols(); x++ ) {
                double[] values = mat.get(y,x);
                if(values[0] > 200) {
                    count_white++;
                }
            }
        }
        m_WhitePixelInImage = count_white;
    }

    @Override
    public BufferedImage getImage() {
        return m_Image;
    }

    public int getVibrationDection() {
        return m_WhitePixelInImage;
    }

    public void waitUntilFinishedVibration() {
        m_FrameCounter = 0;
        m_CurrentFrame = 0;
        int frameWithoutVibration = 10;
        int timeOut = 20;
        while(timeOut > 0) {
            while(m_FrameCounter == m_CurrentFrame) {
                try {
                    Thread.sleep(10);
                } catch (InterruptedException e) {
                }
            }
            int vibrationValue = getVibrationDection();
            vibrationMesser[m_CurrentFrame][messCounter] = vibrationValue;

            if(isVibrated(vibrationValue)) {
                frameWithoutVibration = 10;
            } else {
                frameWithoutVibration--;
            }

            if(frameWithoutVibration <= 0) {
                timeOut = 0;
                if(m_CurrentFrame != 19) {
                    vibrationMesser[m_CurrentFrame + 1][messCounter] = -500;
                }
            }


            timeOut--;
            m_CurrentFrame = m_FrameCounter;

        }
        messCounter++;
        //print();
    }

    private void print() {
        for(int i=0; i<vibrationMesser.length; i++) {
            System.out.print(i);
            for(int k=0; k<messCounter; k++) {
                System.out.print(", " + vibrationMesser[i][k]);
            }
            System.out.println();
        }
    }

    private boolean isVibrated(int vibrationValue) {
        return vibrationValue > 500;
    }

}
