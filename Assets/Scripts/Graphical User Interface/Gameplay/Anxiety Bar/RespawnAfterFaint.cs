using UnityEngine;

public class RespawnAfterFaint : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private GettingOutOfSafeSpace gettingOutOfSafeSpace;
    
    private void Update()
    {
        if (anxietyBar.currentAnxiety == anxietyBar.maxAnxiety)
        {
            anxietyBar.ResetAnxiety();
            this.transform.position = new Vector3(27.53f,3.79f,-301.08f);
            anxietyBar.currentAnxiety = 0f;
            anxietyBar.state.fillAmount = 0f;
            anxietyBar.dOF.focusDistance.value = 3.8f;
            anxietyBar.cA.intensity.value = 0.3f;
        }
    }
}