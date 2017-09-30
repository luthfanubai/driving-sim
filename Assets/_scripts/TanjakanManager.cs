using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;
using System.Linq;

public class TanjakanManager : MonoBehaviour, ExamController.IExam
{

    [SerializeField] Text objectiveText;
    [SerializeField] Text objectiveText2;
    [SerializeField] Text countPatokText;
    [SerializeField] string[] guideStrings;

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
    void SetObjectiveText()
    {
        objectiveText.gameObject.SetActive(true);
        objectiveText.text = "Next Objective : " + guideStrings[CheckpointIndex];
    }

    public void SetObjectiveText(string s)
    {
        objectiveText.gameObject.SetActive(true);
        objectiveText.text = s;
    }

	void Start () 
    {
        //countPatokText.gameObject.SetActive(false);
        ResetCheckpoints();
	}

    private void ResetCheckpoints()
    {
        //memuat semua gameobject yang memiliki tag checkpoint
        checkpoints = transform.OfType<Transform>()
            .Where(x => x.tag == "Checkpoint")
            .Select(x => x.gameObject).ToArray();

        CheckpointIndex = 0;
        foreach (var cp in checkpoints)
        {
            cp.SetActive(false);
        }
        checkpoints[CheckpointIndex].SetActive(true);
    }
	
	void Update () 
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
    private void ExamFinish()
    {
        status = ExamController.Status.Passed;
        objectiveText.gameObject.SetActive(false);
    }

    private ExamController.Status _status;
    public ExamController.Status status
    {
        get
        {
            return _status;
        }
        set
        {
            //mencegah perubahan status dari failed ke passed atau sebaliknya
            if (_status == ExamController.Status.Ongoing || value == ExamController.Status.Ongoing)
            {
                _status = value;
            }
        }
    }

    void ExamController.IExam.Restart()
    {

        ResetCheckpoints();
        

        if (GetComponentInChildren<CheckpointTanjakan>())
            GetComponentInChildren<CheckpointTanjakan>().hasStopped = false;
            
        //reset patok
        foreach (var item in GetComponentsInChildren<ObjectResetter>())
        {
            item.DelayedReset(0f);
        }

        status = ExamController.Status.Ongoing;
    }
}
