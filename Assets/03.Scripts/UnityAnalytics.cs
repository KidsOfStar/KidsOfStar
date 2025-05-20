using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Analytics : MonoBehaviour
{
    async void Start()
    {
        try
        {
            //Unity Services 초기화
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();

        }
        catch (System.Exception error)
        {
            EditorLog.Log($"Unity Services failed to + {error}");
        }
    }

    public static void Chapter1Choice(string choice)
    {
        var choiceEvent = new CustomEvent("Chapter1_Choice");
        choiceEvent["choice"] = choice;

        AnalyticsService.Instance.RecordEvent(choiceEvent);
    }

    public static void Chapter1ChaseClear(int clearNumber)
    {
        var clearEvent = new CustomEvent("Chapter1_Chase_Clear");
        clearEvent["clear_number"] = clearNumber;

        AnalyticsService.Instance.RecordEvent(clearEvent);
    }

    public static void Chapter2PopUpPuzzle1(int clearNumber, int time) 
    {
        var puzzleEvent = new CustomEvent("Chapter2_PopUp_Puzzle1");
        puzzleEvent["Clear_Number"] = clearNumber;
        puzzleEvent["Time"] = time;

        AnalyticsService.Instance.RecordEvent(puzzleEvent);
    }

    public static void Chapter2MapPuzzle1(int count)
    {
        var chpater2Event1 = new CustomEvent("Chapter2_Map_Puzzle1");
        chpater2Event1["Count"] = count;

        AnalyticsService.Instance.RecordEvent(chpater2Event1);
    }

    public static void Chapter2PopUpPuzzle2(int clearNumber, int time)
    {
        var puzzle2Event = new CustomEvent("Chapter2_PopUp_Puzzle2");
        puzzle2Event["clear_Number"] = clearNumber;
        puzzle2Event["time"] = time;

        AnalyticsService.Instance.RecordEvent(puzzle2Event);
    }

    public static void Chapter2MapPuzzle2AgainTime(int againCount)
    {
        var chapter2Event2 = new CustomEvent("Chapter2_Map_Puzzle2_AgainTime");
        chapter2Event2["again_Count"] = againCount;

        AnalyticsService.Instance.RecordEvent(chapter2Event2);
    }

    public static void Chapter2Choice(string choice)
    {
        var choiceEvent2 = new CustomEvent("Chapter2_Choice");
        choiceEvent2["choice"] = choice;

        AnalyticsService.Instance.RecordEvent(choiceEvent2);
    }

    public static void Chapter3RunningPuzzleClear(int clearTime, int clear, int failPosition)
    {
        var clearEvent3 = new CustomEvent("Chapter3_RunningPuzzle_Clear");
        clearEvent3["clearTime"] = clearTime;
        clearEvent3["clear"] = clear;
        clearEvent3["failPosition"] = failPosition;

        AnalyticsService.Instance.RecordEvent(clearEvent3);
    }

    public static void Chapter3Choice(string choice)
    {
        var choiceEvent3 = new CustomEvent("Chapter3_Choice");
        choiceEvent3["choice"] = choice;

        AnalyticsService.Instance.RecordEvent(choiceEvent3);
    }

    public static void Chapter4PopUpPuzzle101Clear(int choice, int clearTime)
    {
        var clearEvent4 = new CustomEvent("Chapter4_PopUpPuzzle101_Clear");
        clearEvent4["choice"] = choice;
        clearEvent4["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent4);
    }


    ////TODO: 기획자님께 물어보기 변수명 같음.
    //public static void Chapter4PopUpPuzzle101Clear(int clearNumber, int clearTime)
    //{
    //    var clearEvent4 = new CustomEvent("Chapter4PopUpPuzzle101Clear");
    //    clearEvent4["clearNumber"] = clearNumber;
    //    clearEvent4["clearTime"] = clearTime;

    //    AnalyticsService.Instance.RecordEvent(clearEvent4);
    //}
    public static void Chapter4PopUpPuzzle103Clear(int clearNumber, int clearTime)
    {
        var clearEvent5 = new CustomEvent("Chapter4_PopUpPuzzle103_Clear");
        clearEvent5["clearNumber"] = clearNumber;
        clearEvent5["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent5);
    }

    public static void Chapter4MapPuzzle1AgainCount(int againCount)
    {
        var chapter4Event1 = new CustomEvent("Chapter4_Map_Puzzle1_AgainCount");
        chapter4Event1["againCount"] = againCount;

        AnalyticsService.Instance.RecordEvent(chapter4Event1);
    }

    public static void Chapter4PopUpPuzzle201Clear(int clearNumber, int clearTime)
    {
        var clearEvent6 = new CustomEvent("Chapter4_PopUp_Puzzle201_Clear");
        clearEvent6["clearNumber"] = clearNumber;
        clearEvent6["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent6);
    }

    public static void Chapter4PopUpPuzzle202Clear(int clearNumber, int clearTime)
    {
        var clearEvent7 = new CustomEvent("Chapter4_PopUp_Puzzle202_Clear");
        clearEvent7["clearNumber"] = clearNumber;
        clearEvent7["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent7);
    }

    public static void Chapter4PopUpPuzzle203Clear(int clearNumber, int clearTime)
    {
        var clearEvent8 = new CustomEvent("Chapter4_PopUp_Puzzle203_Clear");
        clearEvent8["clearNumber"] = clearNumber;
        clearEvent8["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent8);
    }

    //TODO: 현아님께 여쭤보기 위의 메서드랑 겹치는 부분 존재, 61번째, 45번째
    public static void Chapter4MapPuzzle1AgainCount(int againCount)
    {
        var chapter4Event1 = new CustomEvent("Chapter4_Map_Puzzle1_AgainCount");
        chapter4Event1["againCount"] = againCount;

        AnalyticsService.Instance.RecordEvent(chapter4Event1);
    }

    public static void Chapter4Choice(string choice)
    {
        var choiceEvent4 = new CustomEvent("Chapter4_Choice");
        choiceEvent4["choice"] = choice;

        AnalyticsService.Instance.RecordEvent(choiceEvent4);
    }

    public static void Chapter5PopUpPuzzle1(int clearNumber, int clearTime)
    {
        var clearEvent9 = new CustomEvent("Chapter5_PopUp_Puzzle1");
        clearEvent9["clearNumber"] = clearNumber;
        clearEvent9["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent9);
    }

    public static void Chapter5PopUpPuzzle2(int clearNumber, int clearTime)
    {
        var clearEvent10 = new CustomEvent("Chapter5_PopUp_Puzzle2");
        clearEvent10["clearNumber"] = clearNumber;
        clearEvent10["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent10);
    }

    public static void Chapter5PopUpPuzzle3(int clearNumber, int clearTime)
    {
        var clearEvent11 = new CustomEvent("Chapter5_PopUp_Puzzle3");
        clearEvent11["clearNumber"] = clearNumber;
        clearEvent11["clearTime"] = clearTime;

        AnalyticsService.Instance.RecordEvent(clearEvent11);
    }

    public static void Chapter5Choice(string choice)
    {
        var choiceEvent5 = new CustomEvent("Chapter5Choice");
        choiceEvent5["choice"] = choice;

        AnalyticsService.Instance.RecordEvent(choiceEvent5);
    }

}


