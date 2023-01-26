namespace Core.Event
{
    public enum GameEventType : ushort
    {
        //Menu
        MenuPlayButtonClicked,
        MenuLeaderBoardButtonClicked,
        
        //Game
        GameStart,
        GameEnd,

        UpdateScore,
        TimeIsUp,
        OptionSelected,
        
        //Other
        EnableDimmer,
        DisableDimmer,
    }
}