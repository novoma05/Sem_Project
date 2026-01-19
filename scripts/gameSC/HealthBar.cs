using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public static HealthBar main;
    [SerializeField] private Slider healtbar;

    public void UpdatehealthBar(float currentHealt, float maxHealt)
    {
        healtbar.value = currentHealt / maxHealt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
