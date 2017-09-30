using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(TruckController))]
    public class CarUserControl : MonoBehaviour
    {
        private TruckController m_Car; 
        [HideInInspector]
        public bool CarDisabled;

        private void Awake()
        {
            m_Car = GetComponent<TruckController>();
        }

        void Start()
        {
            LogitechGSDK.LogiSteeringInitialize(false);
        }

        void FixedUpdate()
        {
            if (!CarDisabled)
            {
                if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
                {
                    var rec = LogitechGSDK.LogiGetStateUnity(0);

                    var h = Mathf.Lerp(-1, 1, Mathf.InverseLerp(short.MinValue, short.MaxValue, rec.lX));
                    var v = Mathf.Lerp(0, 1, Mathf.InverseLerp(short.MaxValue, short.MinValue, rec.lY));
                    var b = Mathf.Lerp(0, -1, Mathf.InverseLerp(short.MaxValue, short.MinValue, rec.lRz));
                    v = v > 0.05f ? v : 0f;
                    b = b < -0.3f ? b : 0f;
                    var hb = (rec.rgbButtons[0] == 128 || rec.rgbButtons[1] == 128) ? 1 : 0;
                    m_Car.Move(h, v, b, hb);
                    if (rec.rgbButtons[8] == 128)
                    {
                        m_Car.GearType = GearType.Reverse;
                    }
                    else if (rec.rgbButtons[9] == 128)
                    {
                        m_Car.GearType = GearType.Forward;
                    }
                }
                else
                {
                    var h = Input.GetAxis("Horizontal");
                    var v = Input.GetAxis("Vertical");
#if !MOBILE_INPUT
                    if (Input.GetKeyUp(KeyCode.PageUp))// && rb.velocity.magnitude <= 0.1f)
                        m_Car.GearType = GearType.Forward;
                    if (Input.GetKeyUp(KeyCode.PageDown))// && rb.velocity.magnitude <= 0.1f)
                        m_Car.GearType = GearType.Reverse;
                    float handbrake = Input.GetAxis("Jump");
                    m_Car.Move(h, v, v, handbrake);
#else
                                m_Car.Move(h, v, v, 0f);
#endif
                }

            }
            else //menghilangkan user control setelah gameover
            {
                m_Car.Move(0, 0, 0.7f, 0.01f);
            }
        }
    }
}