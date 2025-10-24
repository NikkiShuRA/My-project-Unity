using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DriftManager : MonoBehaviour
{
    public Rigidbody playerRB;
    public TMP_Text totalScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text factorText;
    public TMP_Text driftAngleText;
    public Image WheelImage;

    private float speed = 0;
    private float driftAngle = 0;
    private float driftFactor = 1;
    private float currentScore;
    private float totalScore;

    private bool isDrifting = false;

    private float points;
    public float purposeScore = 30;

    public float minimumSpeed = 5;
    public float minimumAngle = 5;
    public float driftingDelay = 0.2f;
    public GameObject driftingObject;
    public Color normalDriftColor;
    public Color nearStopColor;
    public Color driftEndedColor;

    private IEnumerator stopDriftingCoroutine = null;

    void Start()
    {
        driftingObject.SetActive(false);
    }


    void Update()
    {
        ManagerDrift();
        ManageUI();
    }

    void ManagerDrift()
    {
        speed = playerRB.linearVelocity.magnitude;
        driftAngle = Vector3.Angle(playerRB.transform.forward, (playerRB.linearVelocity + playerRB.transform.forward).normalized);
        if (driftAngle > 120)
        {
            driftAngle = 0;
        }
        if (driftAngle >= minimumAngle && speed > minimumSpeed)
        {
            if(!isDrifting || stopDriftingCoroutine != null)
            {
                StartDrift();
            }
        }
        else
        {
            if (isDrifting && stopDriftingCoroutine == null)
            {
                StopDrift();
            }
        }
        if(isDrifting)
        {
            points += Time.deltaTime * driftAngle * driftFactor;
            if (WheelImage.fillAmount >= 1)
            {
                currentScore += Time.deltaTime * driftAngle * driftFactor;
                driftFactor += Time.deltaTime;
            }

            driftingObject.SetActive(true);
        }
    }

    async void StartDrift()
    {
        if (!isDrifting)
        {
            await Task.Delay(Mathf.RoundToInt(1000 * driftingDelay));
            driftFactor = 1;
        }
        if (stopDriftingCoroutine != null)
        {
            StopCoroutine(stopDriftingCoroutine);
            stopDriftingCoroutine = null;
        }
        currentScoreText.color = normalDriftColor;
        isDrifting = true;
    }

    void StopDrift()
    {
        stopDriftingCoroutine = StoppingDrift();
        StartCoroutine(stopDriftingCoroutine);
    }

    private IEnumerator StoppingDrift()
    {
        yield return new WaitForSeconds(0.1f);
        currentScoreText.color = nearStopColor;
        yield return new WaitForSeconds(driftingDelay * 4);
        totalScore += currentScore;
        isDrifting = false;
        currentScoreText.color = driftEndedColor;
        yield return new WaitForSeconds(0.5f);
        currentScore = 0;
        points = 0;
        driftingObject.SetActive(false);
    }

    void ManageUI()
    {
        totalScoreText.text = "Total: " + (totalScore).ToString("###,###,000");
        factorText.text = driftFactor.ToString("###,###,##0.0") + "X";
        currentScoreText.text = currentScore.ToString("###,###,000");
        driftAngleText.text = driftAngle.ToString("###,##0") + "°";

        WheelImage.fillAmount = points / purposeScore;
    }
}
