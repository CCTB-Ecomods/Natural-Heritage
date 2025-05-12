using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentApprovalUi : MonoBehaviour
{
    public GameObject barUiPrefab;

    private WorldData world;
    private static List<ApprovalBar> approvalBars = new List<ApprovalBar>();

    private List<Color> colors = new List<Color>
    {
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    private void Start()
    {
        world = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<GameLogic>().GetWorld();

        int i = 0;
        foreach (var group in world.residentGroups)
        {
            approvalBars.Add(new ApprovalBar(group, colors[i], this, barUiPrefab));
            
            if (i < colors.Count)
                i++;
            else
                i = 0;
        }

        onUpdate();
    }

    public static void onUpdate()
    {
        foreach (var bar in approvalBars)
        {
            bar.Update();
        }
    }
}

