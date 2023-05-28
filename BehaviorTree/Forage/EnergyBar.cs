using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    private bool alive;
    private Slider _energyBar;
      
    /// <summary>
    /// Aid in the translation between coordinate spaces.
    /// </summary>
    private Camera mainCamera;
    
    /// <summary>
    /// Transform of the energy bar.
    /// </summary>
    private RectTransform _energyBarTransform;
    
    public GameObject grizzly;
    public int startingEnergy;
    public int energyDecayRate;
    
    private void Start()
    {
        _energyBar = GetComponent<Slider>();
        _energyBar.value = startingEnergy;
        
        _energyBarTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
        
        StartCoroutine(DecayTimer());

    }

    private void Update()
    {
        var targetScreenPosition = mainCamera.WorldToScreenPoint(grizzly.transform.position);
        targetScreenPosition += Vector3.up * 15;
        _energyBarTransform.position = targetScreenPosition;
    }
    
    private IEnumerator DecayTimer()
    {
        alive = true;
        while (alive)
        {
            yield return new WaitForSeconds(1);
            DecayEnergy();
        }
    }
    
    private void DecayEnergy()
    {
        _energyBar.value -= energyDecayRate;
        if (!(_energyBar.value <= 0)) return;
        
        alive = false;
        StopCoroutine(DecayTimer());
        grizzly.SetActive(false);
    }
}

