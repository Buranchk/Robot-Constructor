using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotDynamicUI : MonoBehaviour
{
    [SerializeField] private RobotController robotController;
    [SerializeField] private TextMeshProUGUI legsName;
    [SerializeField] private TextMeshProUGUI torsoName;
    [SerializeField] private TextMeshProUGUI headName;
    [SerializeField] private TextMeshProUGUI heightValue;
    [SerializeField] private TextMeshProUGUI weightValue;
    [SerializeField] private TextMeshProUGUI energyValue;
    [SerializeField] private Slider weightSlider;
    [SerializeField] private Slider energySlider;

    private float currentWeight;
    private float currentEnergy;
    private float desiredWeight;
    private float desiredEnergy;
    private Tween weightTween;
    private Tween energyTween;

    private void OnEnable()
    {
        robotController.RobotChanged += UpdateText;
    }

    private void Start()
    {
        UpdateText(robotController.Robot);
    }

    private void OnDisable()
    {
        robotController.RobotChanged -= UpdateText;
        weightTween?.Kill();
        energyTween?.Kill();
    }

    private void UpdateText(RobotBuild robot)
    {
        legsName.text = robot.Legs.PartId;
        torsoName.text = robot.Torso.PartId;
        headName.text = robot.Head.PartId;

        float height = robot.Legs.Height + robot.Torso.Height + robot.Head.Height;
        heightValue.text = $"{height:0.00} meters";
        weightValue.text = $"{robot.TotalWeight:0}/50";
        energyValue.text = $"{robot.TotalPower}/50";

        UpdateSliders(robot.TotalWeight, robot.TotalPower);
    }

    private void UpdateSliders(float desiredWeight, float desiredEnergy)
    {
        this.desiredWeight = desiredWeight;
        this.desiredEnergy = desiredEnergy;
        weightTween?.Kill();
        energyTween?.Kill();

        weightTween = DOTween.To(() => currentWeight, value =>
        {
            currentWeight = value;
            weightSlider.value = value;
        }, this.desiredWeight, 0.25f);

        energyTween = DOTween.To(() => currentEnergy, value =>
        {
            currentEnergy = value;
            energySlider.value = value;
        }, this.desiredEnergy, 0.25f);
    }
}
