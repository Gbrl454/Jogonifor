using UnityEngine;
using System.IO.Ports;
using StarterAssets;
using UnityEngine.InputSystem;

public class ESP32Communication : MonoBehaviour
{
    // 0,0,0,0,0,0
    public string serialPortName = "COM5";
    public int baudRate = 115200;

    public StarterAssetsInputs starterAssetsInputs;
    private SerialPort serialPort;
    [SerializeField] UnityEngine.Events.UnityEvent InteractEvent;

    private void Start()
    {
        try
        {
            serialPort = new SerialPort(serialPortName, baudRate);
            serialPort.Open();
        }
        catch (System.Exception)
        {
            Debug.Log("ESP não iniciado");
        }
    }

    bool interacting;

    private void Update()
    {
        if (serialPort.IsOpen)
        {
            string[] values = serialPort.ReadLine().Split(",");
            float[] vector = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                vector[i] = (float.TryParse(values[i], out float floatValue)) ? floatValue : 0;
            }

            if (vector.Length == 6)
            {
                starterAssetsInputs.SprintInput((vector[0] == 2 || vector[0] == -2 | vector[1] == 2 || vector[1] == -2) ? true : false);
                starterAssetsInputs.MoveInput(new Vector2(vector[0], vector[1]));
                starterAssetsInputs.LookInput(new Vector2(vector[3] * 5, vector[4] * 5));
                starterAssetsInputs.JumpInput((vector[2] == 1) ? true : false);
                starterAssetsInputs.InteractInput((vector[5] == 1) ? true : false);
                starterAssetsInputs.ClimbInput((vector[5] == 1) ? true : false);
            }
        }
    }
}

