using System;

public class LowPassFilter
{
    public float[] CurrentData { get; private set; }
    public bool DataValid { get; private set; }

    private float[][] m_Data;
    private int m_DataSize;
    private int m_Order;
    private float m_Factors;
    private int m_Count;

    public LowPassFilter(int dataSize, int order)
    {
        CurrentData = new float[dataSize];
        m_DataSize = dataSize;
        m_Data = new float[order][];
        m_Order = order;
        m_Factors = 1f / order;
        DataValid = false;
    }

    public void Push(float[] data)
    {
        if (data.Length != m_DataSize)
        {
            throw new ArgumentException("Invalid Data Size, should be " + m_DataSize);
        }

        m_Data[m_Count % m_Order] = data;

        if (m_Count >= m_Order - 2)
        {
            if (!DataValid)
            {
                CurrentData = m_Data[(m_Count + 2) % m_Order];
            }
            var tempData = copyData();

            tempData[(m_Count + 1) % m_Order] = CurrentData;

            for (int row = 0; row < m_Order; row++)
            {
                for (int column = 0; column < m_DataSize; column++)
                {
                    tempData[row][column] *= m_Factors;
                }
            }
            float[] tempCurrentData = new float[3];
            for (int column = 0; column < m_DataSize; column++)
            {
                for (int row = 0; row < m_Order; row++)
                {
                    tempCurrentData[column] += tempData[row][column];
                }
            }
            CurrentData = tempCurrentData;

            DataValid = true;
        }
        m_Count++;
    }

    private float[][] copyData()
    {
        var tempData = new float[m_Order][];
        for (int i = 0; i < m_Order; i++)
        {
            if (m_Data[i] != null)
            {
                tempData[i] = new float[m_DataSize];
                for (int k = 0; k < m_DataSize; k++)
                {
                    tempData[i][k] = m_Data[i][k];
                }
            }
        }
        return tempData;
    }

}
