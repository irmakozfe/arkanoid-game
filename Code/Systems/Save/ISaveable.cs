namespace Garkanoid.Save{
public interface ISaveable
{

    string ID
        {
            get;
            set;
        }

    // TODO: design what kind of parameter these methods should accept
    bool Save();
    bool Load();
    
}

}