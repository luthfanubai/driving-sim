using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Car;

public class ExamController : MonoBehaviour
{
    public interface IExam
    {
        Status status { get; }
        void Restart();

        Vector3 startingPosition { get; }
        Quaternion startingRotation { get; }
    }

    public enum Status
    {
        Ongoing,
        Failed,
        Passed,
    }

    IExam[] allExams;
    IExam currentExam;
    int examIndex;

    [SerializeField]
    Text examResultText;
    [SerializeField]
    Text resultStatsText;
    [SerializeField]
    CarUserControl carUC;
    [SerializeField]
    GameObject sim;

    Color redColor;
    Color greenColor;
    Rigidbody carRB;
    Camera[] cameras;
    int cameraIndex = 0;
    float[] timer = new float[4];
    int[] attemptNo = new int[4];

    void Awake()
    {
        //memuat semua gameoject yang meng-inherit interface IExam
        allExams = GetComponentsInChildren<MonoBehaviour>()
            .Where(x => x is IExam).Cast<IExam>().ToArray();

        //mematikan semua IExam pada allExams
        foreach (MonoBehaviour exam in allExams)
        {
            exam.enabled = false;
        }

        carRB = carUC.GetComponent<Rigidbody>();

        redColor = new Color(255, 0, 0);
        greenColor = new Color(0, 208, 0);
    }

    void Start()
    {
        LogitechGSDK.LogiSteeringInitialize(false);
        cameras = carRB.gameObject.GetComponentsInChildren<Camera>().Where(x => x.tag == "MainCamera").ToArray();
        cameras[cameraIndex].enabled = true;
        for (int i = 1; i < cameras.Length; i++)
            cameras[i].enabled = false;

        examIndex = -1;
        Proceed();



#if !UNITY_EDITOR
        ResetCar();
#endif
    }

    /// <summary>
    /// Mengembalikan nilai true apabila masih ada exam yang harus dilakukan
    /// </summary>
    /// <returns></returns>
    private bool Proceed()
    {
        examIndex++;
        if (examIndex < allExams.Length)
        {
            currentExam = allExams[examIndex];
            ((MonoBehaviour)currentExam).enabled = true;
            //set torsi stage 4
            if (currentExam == allExams[3])
            {
                carRB.GetComponent<TruckController>().m_FullTorqueOverAllWheels = 3500;
            }
            else
            {
                carRB.GetComponent<TruckController>().m_FullTorqueOverAllWheels = 1600f;
            }
            return true;
        }
        return false;
    }

    void Update()
    {
        //print(currentExam.status);
        if (currentExam.status == Status.Failed)
        {
            carUC.CarDisabled = true;
            SetExamResultText("GAME OVER \nTekan (R) untuk Restart", redColor);
            timer[examIndex] = 0f;
            if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
            {
                var rec = LogitechGSDK.LogiGetStateUnity(0);
                if (rec.rgbButtons[2] == 128 || rec.rgbButtons[4] == 128 || rec.rgbButtons[6] == 128 || Input.GetKeyDown(KeyCode.R))
                {
                    ResetCar();
                    currentExam.Restart();
                    attemptNo[examIndex] += 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetCar();
                currentExam.Restart();
                attemptNo[examIndex] += 1;
            }
            //print((attemptNo[0] + 1) + " " + (attemptNo[1] + 1) + " " + (attemptNo[2] + 1) + " " + (attemptNo[3] + 1));
        }
        else if (currentExam.status == Status.Passed)
        {
            currentExam.Restart(); //restart exam setelah exam selesai
            carUC.CarDisabled = true;

            SetExamResultText("SELAMAT!\nStage " + (examIndex + 1) + " telah selesai", greenColor);

            ((MonoBehaviour)currentExam).enabled = false;
            if (Proceed())
            {
                Invoke("ResetCar", 3f);
            }
            else
            {

                // ... selamat anda telah lulus ujian praktik 1
                //SetExamResultText("Selamat anda telah lulus\nUjian Praktik 1", greenColor);
                sim.SetActive(true);
                Invoke("ShowResultStats", 3f);
            }
        }
        else
        {
            if(examIndex<timer.Length)
                timer[examIndex] += Time.deltaTime;
            //print(timer[0] + " " + timer[1] + " " + timer[2] + " " + timer[3]);
        }

        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            var rec = LogitechGSDK.LogiGetStateUnity(0);
            if (rec.rgbButtons[3] == 128 && cameraIndex != 0)
            {
                cameras[cameraIndex].enabled = false;
                cameraIndex = 0;
                cameras[cameraIndex].enabled = true;
            }
            if (rec.rgbButtons[5] == 128 && cameraIndex != 1)
            {
                cameras[cameraIndex].enabled = false;
                cameraIndex = 1;
                cameras[cameraIndex].enabled = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            cameras[cameraIndex].enabled = false;
            cameraIndex++;
            if (cameraIndex < cameras.Length)
            {
                cameras[cameraIndex].enabled = true;
            }
            else
            {
                cameraIndex = 0;
                cameras[cameraIndex].enabled = true;
            }
        }


        //cheat
        //#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            examIndex = -1;
            if (Proceed())
            {
                Invoke("ResetCar", 0f);
                currentExam.Restart();
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            examIndex = 0;
            if (Proceed())
            {
                Invoke("ResetCar", 0f);
                currentExam.Restart();
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            examIndex = 1;
            if (Proceed())
            {
                Invoke("ResetCar", 0f);
                currentExam.Restart();
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            examIndex = 2;
            if (Proceed())
            {
                Invoke("ResetCar", 0f);
                currentExam.Restart();
            }
        }

        //#endif
    }

    void ResetCar()
    {
        examResultText.transform.parent.gameObject.SetActive(false);

        carUC.CarDisabled = false;
        carUC.transform.position = currentExam.startingPosition;
        carUC.transform.rotation = currentExam.startingRotation;
        if (carRB)
        {
            carRB.velocity = Vector3.zero;
            carRB.angularVelocity = Vector3.zero;
            carRB.GetComponent<TruckController>().GearType = GearType.Forward;
        }
    }

    void SetExamResultText(string str, Color color)
    {
        examResultText.color = color;
        examResultText.text = str;
        examResultText.transform.parent.gameObject.SetActive(true);
    }

    void ShowResultStats()
    {
        sim.SetActive(false);
        examResultText.gameObject.SetActive(false);
        resultStatsText.transform.parent.gameObject.SetActive(true);
        resultStatsText.text = string.Format("Waktu : {0:d2}:{1:d2}:{2:d2}\nJumlah Percobaan : {3}\n\nWaktu : {4:d2}:{5:d2}:{6:d2}\nJumlah Percobaan : {7}\n\nWaktu : {8:d2}:{9:d2}:{10:d2}\nJumlah Percobaan : {11}\n\nWaktu : {12:d2}:{13:d2}:{14:d2}\nJumlah Percobaan : {15}\n\n", (int)timer[0] / 60, (int)timer[0]%60, (int)(timer[0] * 100) % 100, attemptNo[0] + 1, (int)timer[1] / 60, (int)timer[1]%60, (int)(timer[1] * 100) % 100, attemptNo[1] + 1, (int)timer[2] / 60, (int)timer[2]%60, (int)(timer[2] * 100) % 100, attemptNo[2] + 1, (int)timer[3] / 60, (int)timer[3]%60, (int)(timer[3] * 100) % 100, attemptNo[3] + 1);
    }
}
