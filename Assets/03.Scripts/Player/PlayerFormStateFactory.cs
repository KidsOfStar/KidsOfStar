using System.Collections;
using System.Collections.Generic;

public class PlayerFormStateFactory
{
    private readonly PlayerStateContext context;
    private readonly Dictionary<string, IFormState> states;

    public PlayerFormStateFactory(PlayerStateContext context, PlayerFormData data)
    {
        this.context = context;
        states = new Dictionary<string, IFormState>()
        {
            {"Stone", new StoneFormState(context, data.PlayerFromDataList[0]) },
            {"Human", new HumanFormState(context, data.PlayerFromDataList[1]) },
            {"Squirrel", new SquirrelFormState(context, data.PlayerFromDataList[2]) },
            {"Dog", new DogFormState(context, data.PlayerFromDataList[3]) },
            {"Cat", new CatFormState(context, data.PlayerFromDataList[4]) },
            {"Hide", new HideFormState(context, data.PlayerFromDataList[5]) }
        };
    }

    public IFormState GetFormState(string formName)
    {
        if(states.TryGetValue(formName, out IFormState state))
        {
            return state;
        }
        else
        {
            return null;
        }
    }
}
