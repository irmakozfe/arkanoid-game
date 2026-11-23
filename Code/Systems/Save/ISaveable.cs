using Godot;
using Godot.Collections;

namespace Garkanoid.Save{
public interface ISaveable
{

    string ID
        {
            get;
            set;
        }

        
    bool Save(ref Dictionary<string, Variant> saveData);
    bool Load(Dictionary<string, Variant> saveData);
    
}

}