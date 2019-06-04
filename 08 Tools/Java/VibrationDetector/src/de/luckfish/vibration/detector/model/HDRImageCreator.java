package de.luckfish.vibration.detector.model;

import org.opencv.core.Core;
import org.opencv.core.Mat;
import org.opencv.core.Scalar;
import org.opencv.photo.MergeMertens;

import java.util.ArrayList;
import java.util.List;

import static org.opencv.photo.Photo.createMergeMertens;

/**
 * Created by marcel on 10/14/2016.
 */
public class HDRImageCreator implements FrameListener {

    private Camera m_Camera;

    private boolean m_GrabFrame;

    private Mat m_Frame;

    List<Mat> m_Frames = new ArrayList<>();

    public HDRImageCreator(Camera camera) {
        m_Camera = camera;

        m_Frames = new ArrayList<>();

        camera.addListener(this);
    }

    public void addFrame(float exposure){
        grabAndAddFrame(m_Frames, exposure);
    }

    public Mat computeFrame() {
        Mat hdrMertens = new Mat();
        MergeMertens mergeMertens = createMergeMertens();
        mergeMertens.process(m_Frames, hdrMertens);

        Mat hdrMertensResult = new Mat();
        Core.multiply(hdrMertens, new Scalar(255, 255, 255), hdrMertensResult);

        if(!this.isFilled(hdrMertensResult)) {
        	hdrMertensResult = this.computeFrame();
        }
        
        return hdrMertensResult;
    }
    
    private boolean isFilled(Mat mat) {
    	for(int r=0; r<mat.rows(); r++) {
    		for(int c=0; c<mat.cols(); c++) {
    			if(((int)mat.get(r, c)[0]) != 255
    					&& ((int)mat.get(r, c)[1]) != 255
    					&& ((int)mat.get(r, c)[2]) != 255) {
    				return true;
    			}
    		}
    	}
    	return false;
    }

    public void resetFrames() {
        m_Frames.clear();
    }

    private void grabAndAddFrame(List<Mat> frames, float exposure)  {
        m_Camera.setExposure(exposure);
        grabOneFrame();
        frames.add(m_Frame);
    }

    public void setExposure(float exposure) {
        m_Camera.setExposure(exposure);
    }

    private void grabOneFrame() {
        m_GrabFrame = true;
        while(m_GrabFrame) {
            try {
                Thread.sleep(10);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    @Override
    public void receive(Mat frame) {
        if(m_GrabFrame) {
            m_Frame = frame;
            m_GrabFrame = false;
        }
    }
}
