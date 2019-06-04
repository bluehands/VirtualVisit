package de.luckfish.vibration.detector.model.serialport;

/**
 * Created by marcel on 10/14/2016.
 */
import gnu.io.CommPort;
import gnu.io.CommPortIdentifier;
import gnu.io.SerialPort;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

public class TwoWaySerialComm
{
    public TwoWaySerialComm()
    {

    }

    public void connect ( String portName ) throws Exception
    {
        CommPortIdentifier portIdentifier = CommPortIdentifier.getPortIdentifier(portName);
        if ( portIdentifier.isCurrentlyOwned() )
        {
            System.out.println("Error: Port is currently in use");
        }
        else
        {
            CommPort commPort = portIdentifier.open(this.getClass().getName(),2000);

            if ( commPort instanceof SerialPort )
            {
                SerialPort serialPort = (SerialPort) commPort;
                serialPort.setSerialPortParams(9600,SerialPort.DATABITS_8,SerialPort.STOPBITS_1,SerialPort.PARITY_NONE);

                InputStream in = serialPort.getInputStream();
                OutputStream out = serialPort.getOutputStream();

                (new Thread(new SerialReader(in))).start();
                (new Thread(new SerialWriter(out))).start();

            }
            else
            {
                System.out.println("Error: Only serial ports are handled by this example.");
            }
        }
    }

    /** */
    public static class SerialReader implements Runnable
    {
        public static InputStream in;

        public static String readLine;

        public SerialReader ( InputStream in )
        {
            this.in = in;
        }

        public void run ()
        {

            byte[] buffer = new byte[1024];
            int len = -1;
            StringBuilder sb = new StringBuilder();
            try
            {
                while ( ( len = this.in.read(buffer)) > -1 )
                {
                    //System.out.print(new String(buffer,0,len));
                    for(int i=0; i<len; i++) {
                        if(buffer[i] == 0) {
                            readLine = sb.toString();
                            sb.setLength(0);
                        } else {
                            sb.append((char)buffer[i]);
                        }
                    }

                }
            }
            catch ( IOException e )
            {
                e.printStackTrace();
            }
        }
    }

    /** */
    public static class SerialWriter implements Runnable
    {
        public static OutputStream out;

        public SerialWriter ( OutputStream out )
        {
            this.out = out;
        }

        public void run ()
        {
            try
            {
                int c = 0;
                while ( ( c = System.in.read()) > -1 )
                {
                    this.out.write(c);
                }
            }
            catch ( IOException e )
            {
                e.printStackTrace();
            }
        }
    }
}
