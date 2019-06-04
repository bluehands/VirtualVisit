package de.luckfish.vibration.detector.view;

import de.luckfish.vibration.detector.model.ModelFacade;

import javax.swing.*;
import java.awt.*;
import java.awt.geom.AffineTransform;
import java.awt.image.AffineTransformOp;
import java.awt.image.BufferedImage;

/**
 * Created by marcel on 10/13/2016.
 */
public class VibrationDetectorView extends JPanel {

    private ModelFacade m_ModelFacade;

    public VibrationDetectorView(ModelFacade modelFacade) {
        m_ModelFacade = modelFacade;
    }

    public void paint(Graphics g) {
        Graphics2D g2 = (Graphics2D) g;

        g2.drawImage(scaleImage(m_ModelFacade.getCurrentImageLeft()), null , 0, 0);
        g2.drawImage(scaleImage(m_ModelFacade.getCurrentImageRight()), null , 600, 0);
        g2.drawImage(m_ModelFacade.getDetectorImage(), null , 0, 700);
    }

    private BufferedImage scaleImage(BufferedImage image) {
        if(image == null) {
            return null;
        }
        BufferedImage before = image;
        int w = before.getWidth();
        int h = before.getHeight();
        BufferedImage after = new BufferedImage(w, h, BufferedImage.TYPE_INT_ARGB);
        AffineTransform at = new AffineTransform();
        at.scale(0.5, 0.5);
        AffineTransformOp scaleOp =
                new AffineTransformOp(at, AffineTransformOp.TYPE_BILINEAR);
        after = scaleOp.filter(before, after);
        return after;
    }
}
