using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.Utility;

/// <summary>
/// Script untuk mengatur sekumpulan patok dalam sebuah stage
/// </summary>
    public class PatokManager : MonoBehaviour, ExamController.IExam
    {
        [SerializeField] Text countPatokText;
        [SerializeField] Text objectiveText;
        [SerializeField] Text objectiveText2;
        [SerializeField] string[] guideStrings;

        GameObject[] checkpoints;
        int _checkpointIndex;
        int CheckpointIndex
        {
            get
            {
                return _checkpointIndex;
            }
            set
            {
                _checkpointIndex = value;
                StartCoroutine("PopUpObjectiveText");
                SetObjectiveText();         
            }
        }

        IEnumerator PopUpObjectiveText()
        {
            if (CheckpointIndex != checkpoints.Length && CheckpointIndex != 0)
            {
                objectiveText2.gameObject.SetActive(true);
                yield return new WaitForSeconds(2);
                objectiveText2.gameObject.SetActive(false);
            }
        }

        [SerializeField] Vector3 _startingPosition;
        [SerializeField] Vector3 _startingRotation;

        public Vector3 startingPosition
        {
            get { return _startingPosition; }
        }

        public Quaternion startingRotation
        {
            get { return Quaternion.Euler( _startingRotation); }

        }

        private int _jmlPatokJatuh;
        public int PatokCount
        {
            get
            {
                return _jmlPatokJatuh;
            }
            set
            {
                _jmlPatokJatuh = value;
                SetPatokText();
                if (_jmlPatokJatuh >=2) //jumlah maksimal patok yang boleh jatuh
                {
                    status = ExamController.Status.Failed;
                    
                }
            }
        }

        void Start()
        {
            SetPatokText();    
            ResetCheckpoints();
            objectiveText2.gameObject.SetActive(false);
        }

        private void ResetCheckpoints()
        {
            //memuat semua gameoject yang memiliki tag checkpoint
            checkpoints = checkpoints ?? transform.OfType<Transform>()
                .Where(x => x.tag == "Checkpoint")
                .Select(x => x.gameObject).ToArray();

            CheckpointIndex = 0;
            foreach (var cp in checkpoints)
            {
                cp.SetActive(false);
            }
            checkpoints[CheckpointIndex].SetActive(true);
        }


        public void AddPatokCount()
        {
            PatokCount += 1;
        }
		
		
        void SetPatokText()
        {
            countPatokText.text = string.Format("Jumlah Patok Jatuh : {0}/2", PatokCount);
        }

        void SetObjectiveText()
        {
            objectiveText.gameObject.SetActive(true);
            objectiveText.text = "Next Objective : " + guideStrings[CheckpointIndex];
        }

		void Update()
        {

            //checkpoint checking
            if (checkpoints[CheckpointIndex].activeSelf == false)
            {
                CheckpointIndex++;
                if (CheckpointIndex < checkpoints.Length)
                {
                    checkpoints[CheckpointIndex].SetActive(true);
                }
                else
                {
                    ExamFinish();
                }
            }
        }

        private ExamController.Status _status;
        public ExamController.Status status
        {
            get 
            {
                return _status;
            }
            private set 
            {
                //mencegah perubahan status dari failed ke passed atau sebaliknya
                if(_status == ExamController.Status.Ongoing || value == ExamController.Status.Ongoing)
                {
                    _status = value;
                }
            }
        }

        void ExamController.IExam.Restart()
        {
            PatokCount = 0;
            ResetCheckpoints();
            
            //reset patok
            foreach (var item in GetComponentsInChildren<ObjectResetter>())
            {
                item.DelayedReset(0f);
            }

            status = ExamController.Status.Ongoing;
        }

        private void ExamFinish()
        {
            status = ExamController.Status.Passed;
                      
        }

    }

