using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameObject groundPlane;
    [SerializeField] private GameObject city;
    [SerializeField] private GameObject sky;

    [Header("Settings")]
    [SerializeField] private bool disableGround;
    [SerializeField] private bool disableCity;
    [SerializeField] private bool disableSky;
    [SerializeField] private int disableGroundThreshhold;
    [SerializeField] private int disableCityThreshhold;
    [SerializeField] private int disableSkyThreshhold;

    private bool groundDisabled = false;
    private bool cityDisabled = false;
    private bool skyDisabled = false;

    private void FixedUpdate()
    {
        CheckForDisables();
    }

    private void CheckForDisables()
    {
        if (disableGround)
        {
            if (groundDisabled == false && ScoreManager.Instance.Score >= disableGroundThreshhold)
            {
                groundDisabled = true;
                groundPlane.SetActive(false);
                Debug.Log("Ground disabled", this);
            }
        }
        if (disableCity)
        {
            if (cityDisabled == false && ScoreManager.Instance.Score >= disableCityThreshhold)
            {
                cityDisabled = true;
                city.SetActive(false);
                Debug.Log("City disabled", this);
            }
        }
        if (disableSky)
        {
            if (skyDisabled == false && ScoreManager.Instance.Score >= disableSkyThreshhold)
            {
                skyDisabled = true;
                GameObject[] children = new GameObject[sky.transform.childCount];

                foreach (Transform child in sky.transform)
                {
                    child.gameObject.SetActive(false);
                }

                Debug.Log("Sky disabled", this);
            }
        }
    }
}
